using System.Collections.Generic;
using System.Reflection;
using System.IO;
using Newtonsoft.Json;
using System.Collections;
using UnityEngine;
using System.Linq;
using System;
using static CommunicationEvents;

//TODO: MMT: move some functionality there
//TODO: consequent!= samestep != dependent
//TODO: support renamne functionality

//PERF: avoid string as key (general: allocations & dict: hash -> colission? -> strcmp[!])

/// <summary>
/// Organizes (insertion/ deletion / etc. operations) and sepperates <see cref="Fact">Fact</see> spaces.
/// Keeps track of insertion/ deletion actions for <see cref="undo"/> and <see cref="redo"/>.
/// </summary>
public class FactOrganizer : IJSONsavable<FactOrganizer>
{
    /// <summary>
    /// - <c>Key</c>: <see cref="Gadget"/> Used Gadget
    /// - <c>Value</c>: <see cref="int"/> First occurence of gadget in Workflow
    /// </summary>
    protected Dictionary<Gadget, (int first_occurrence, int last_occurrence)> GadgetWorkflowDict = new();
    /// <summary>
    /// - <c>Key</c>: <see cref="int"/> First occurence of gadget in Workflow
    /// - <c>Value</c>: <see cref="Gadget"/> Used Gadget
    /// </summary>
    [JsonProperty]
    protected Dictionary<int, Gadget> WorkflowGadgetDict = new() { {-1, null } };

    /// <summary>
    /// - <c>Key</c>: <see cref="Fact.Id"/>
    /// - <c>Value</c>: <see cref="Fact"/>
    /// </summary>
    [JsonProperty]
    protected Dictionary<string, Fact> FactDict;

    /// <summary>
    /// - <c>Key</c>: <see cref="Fact.Id"/>
    /// - <c>Value</c>: <see cref="meta"/>
    /// </summary>
    [JsonProperty]
    protected Dictionary<string, meta> MetaInf = new();

    /// <summary>
    /// Keeps track of insertion/ deletion/ etc. operations for <see cref="undo"/> and <see cref="redo"/>
    /// </summary>
    [JsonProperty]
    protected List<stepnote> Workflow = new();

    /// <summary>
    /// Notes position in <see cref="Workflow"/> for <see cref="undo"/> and <see cref="redo"/>; the pointed to element is non-acitve
    /// </summary>
    [JsonProperty]
    protected int marker = 0;

    /// <summary>
    /// Backlock logic redundant - for convinience.
    /// Keeps track of number of steps in <see cref="Workflow"/>.
    /// One step can consist of multiple operations.
    /// <seealso cref="stepnote"/>
    /// </summary>
    [JsonProperty]
    protected int worksteps = 0;
    /// <summary>
    /// Backlock logic redundant - for convinience.
    /// Keeps track of number of steps in <see cref="Workflow"/>, which are not set active.
    /// One step can consist of multiple operations.
    /// <seealso cref="stepnote"/>
    /// </summary>
    [JsonProperty]
    protected int backlog = 0;

    /// <summary>
    /// Set to <c>true</c> if recently been resetted.
    /// </summary>
    [JsonProperty]
    protected bool soft_resetted = false;

    /// <summary>
    /// If set to <c>true</c>, <see cref="Remove(string, bool)"/> and <see cref="Add(Fact, out bool, bool)"/> will invoke <see cref="CommunicationEvents.RemoveFactEvent"/> and <see cref="CommunicationEvents.AddFactEvent"/> respectively.
    /// </summary>
    [JsonProperty]
    public bool invoke;

    // TODO? SE: better seperation
    /// <summary>
    /// Keeps track of maximum <see cref="Fact.LabelId"/> for <see cref="Fact.generateLabel"/>.
    /// </summary>
    [JsonProperty]
    protected internal int MaxLabelId = 0;
    /// <summary>
    /// Stores unused <see cref="Fact.LabelId"/> for <see cref="Fact.generateLabel"/>, wich were freed in <see cref="Fact.freeAutoLabel"/> for later reuse to keep naming space compact.
    /// </summary>
    [JsonProperty]
    protected internal SortedSet<int> UnusedLabelIds = new();

    // TODO: put this stuff in Interface
    /// @{ <summary>
    /// For <see cref="store(string, List<Directories>, bool, bool)"/> and <see cref="load(ref FactOrganizer, bool, string, List<Directories>, bool, out Dictionary<string, string>)"/>
    /// </summary>
    public string name { get; set; } = null;
    public string path { get; set; } = null;
    /// @}


    /// <summary>
    /// Keeps track of insertion/ deletion/ etc. operations for <see cref="undo"/> and <see cref="redo"/>
    /// Used Terminology
    /// ================
    /// - steproot: elements where <see cref="samestep"/> == <c>true</c>
    /// - steptail: elements where <see cref="samestep"/> == <c>false</c>
    /// <seealso cref="Workflow"/>
    /// </summary>
    protected internal struct stepnote
    {
        /// <summary> <see cref="Fact.Id"/> </summary>
        public string Id;

        /// <summary>
        /// <c>true</c> if this Fact has been created in the same step as the last one 
        ///      steproot[false] (=> steptail[true])*
        /// </summary>
        public bool samestep;

        /// <summary>
        /// For fast access of beginning and end of steps.
        /// Reference to position in <see cref="Workflow"/> of:
        /// - steproot: for all elements in steptail
        /// - after steptail-end: for steproot
        /// </summary>
        public int steplink;

        /// <summary> distincts creation and deletion </summary>
        public bool creation;

        /// <summary>
        /// keeps track with wich <see cref="Gadget"/> the <see cref="Fact"/> is created
        /// <c>-1</c> iff its not the case
        /// </summary>
        public int gadget_rank;

        /// <summary>
        /// keeps track with wich <see cref="Scroll"/> the <see cref="Fact"/> is created
        /// <c>null</c> iff its not the case
        /// </summary>
        public string scroll_label;

        /// <summary>Loggs <see cref="Fact"/>s used by <see cref="Gadget"/></summary>
        public string[] GadgetFlow;

        /// <summary>Loggs time, when <see cref="Fact"/> was manipulated</summary>
        public double GadgetTime;


        /// <summary>
        /// Initiator
        /// </summary>
        /// <param name="Id"><see cref="Fact.Id"/></param>
        /// <param name="samestep">sets <see cref="samestep"/></param>
        /// <param name="creation">sets <see cref="creation"/></param>
        /// <param name="that"><see cref="FactOrganizer"/> of which <c>this</c> will be added in its <see cref="FactOrganizer.Workflow"/></param>
        public stepnote(FactOrganizer that, string Id, bool samestep, bool creation, Gadget gadget, string scroll_label)
        {
            this.Id = Id;
            this.samestep = samestep;
            this.creation = creation;
            this.scroll_label = scroll_label;
            this.GadgetTime = StageStatic.stage_time;

            stepnote prev = default;

            if (samestep) {
                prev = that.Workflow[that.marker - 1];
                // steplink = !first_steptail ? previous.steplink : steproot
                this.steplink = prev.samestep ? prev.steplink : that.marker - 1;
            }
            else
                // steproot sets steplink after itself (end of steptail)
                this.steplink = that.marker + 1;

            this.GadgetFlow = new string[0];
            this.gadget_rank = -1;
            if (gadget != null) 
            {
                bool new_gadget;
                if (new_gadget = !that.GadgetWorkflowDict.ContainsKey(gadget)) {
                    that.GadgetWorkflowDict.Add(gadget, (that.marker, that.marker));
                    that.WorkflowGadgetDict.Add(that.marker, gadget);
                }
                var gadget_entree = that.GadgetWorkflowDict[gadget];

                this.gadget_rank = gadget_entree.first_occurrence;


                // check for new USE of gadget
                bool set_workflow = !samestep || new_gadget || prev.gadget_rank != this.gadget_rank
                  || gadget_entree.last_occurrence >= that.marker;

                stepnote gadget_prev = set_workflow ? default /*unused then*/
                    : that.Workflow[gadget_entree.last_occurrence];

                if ( set_workflow || gadget_prev.GadgetFlow == null
                  || !gadget_prev.GadgetFlow.SequenceEqual(gadget.Workflow.ToArray()))
                {
                    this.GadgetFlow = gadget.Workflow.ToArray();
                    that.GadgetWorkflowDict[gadget] = (gadget_entree.first_occurrence, that.marker);
                }
            }
        }
    }

    /// <summary>
    /// Each <see cref="Fact"/> entry in <see cref="FactDict"/> has a corresponding <see cref="meta"/> entry in <see cref="MetaInf"/>.
    /// The <see cref="meta"/> struct is a collection of %meta-variables.
    /// <seealsocref="PruneWorkflow"/>
    /// </summary>
    protected internal struct meta
    {
        // TODO? -> public int last_occurence; // for safe_dependencies
        /// <summary>
        /// position of first occurrence in <see cref="Workflow"/>
        /// </summary>
        public int workflow_id;

        /// <summary>
        /// keeps track wether <see cref="Fact"/> is currently in Scene
        /// </summary>
        public bool active;

        /// <summary>
        /// Initiator
        /// </summary>
        /// <param name="workflow_id">sets <see cref="workflow_id"/></param>
        /// <param name="active">sets <see cref="active"/></param>
        public meta(int workflow_id, bool active)
        {
            this.workflow_id = workflow_id;
            this.active = active;
        }
    }


    static FactOrganizer()
    {
        IJSONsavable<FactOrganizer>.hierarchie = new List<Directories> { Directories.FactStateMachines };
    }

    /// <summary>
    /// Only used by <see cref="JsonConverter"/> to initiate empty instance.
    /// </summary>
    public FactOrganizer()
    {
        FactDict = new Dictionary<string, Fact>();
        this.invoke = false;
    }

    /// <summary>
    /// Standard Constructor for empty, ready to use <see cref="FactOrganizer"/>
    /// </summary>
    /// <param name="invoke">sets <see cref="invoke"/>.</param>
    public FactOrganizer(bool invoke = false)
    {
        FactDict = new Dictionary<string, Fact>();
        this.invoke = invoke;
    }

    /// <summary>
    /// Used to parse read-in <see cref="FactOrganizer"/> by <see cref="JsonReader"/> and make <see cref="Fact.Id"/> conform.
    /// </summary>
    /// <param name="target">to be parsed into, will be overwritten. 
    /// If <c><paramref name="invoke"/> = true</c>, <paramref name="target"/> should be <see cref="StageStatic.stage.factState"/>, outherwise <see cref="InvokeFactEvent(bool, string)"/> will cause <see cref="Exception">Exceptions</see> when it invokes Events of <see cref="CommunicationEvents"/></param>
    /// <param name="source">instance to be parsed</param>
    /// <param name="invoke">see <see cref="invoke"/></param>
    /// <param name="old_to_new">generated to map <c>Key</c> <see cref="Fact.Id"/> of <paramref name="source"/> to corresponding <c>Value</c> <see cref="Fact.Id"/> of <paramref name="target"/></param>.
    public static T ReInitializeFactOrganizer<T>
        (FactOrganizer source, bool invoke, out Dictionary<string, string> old_to_new)
         where T : FactOrganizer, new()
    {
        // TODO: other strategy needed when MMT save/load supported
        // map old URIs to new ones
        Dictionary<string, string>  _old_to_new = new();

        // initiate
        T target = new()
        {
            invoke = invoke,
            MaxLabelId = source.MaxLabelId,
            UnusedLabelIds = source.UnusedLabelIds,
            FactDict = new Dictionary<string, Fact>(),
        };

        // work Workflow
        for(int i = 0; i < source.Workflow.Count; i++)
        {
            stepnote s_step = source.Workflow[i];
            Gadget used_gadget = source.WorkflowGadgetDict[s_step.gadget_rank];

            if (used_gadget != null)
            {
                Gadget init_gadget = GadgetBehaviour.gadgets?.FirstOrDefault(g => Gadget.Equals(g, used_gadget));
                if (init_gadget != null && init_gadget != default(Gadget))
                    used_gadget = init_gadget;

                used_gadget.Workflow = s_step.GadgetFlow?.Select(uri => _old_to_new[uri]).ToList() ?? new();
            }

            if (s_step.creation)
            // Add
            {
                Fact add;
                if (_old_to_new.ContainsKey(s_step.Id))
                    add = target.FactDict[_old_to_new[s_step.Id]];
                else
                {
                    Fact old_Fact = source.FactDict[s_step.Id];

                    add = old_Fact.GetType()
                        .GetConstructor(new Type[] { old_Fact.GetType(), _old_to_new.GetType(), typeof(FactOrganizer) })
                        .Invoke(new object[] { old_Fact, _old_to_new, target })
                        as Fact;

                    _old_to_new.Add(s_step.Id, add.Id);
                }

                target.Add(add, out _, s_step.samestep, used_gadget, s_step.scroll_label);
            }
            else if (_old_to_new.ContainsKey(s_step.Id))
            // Remove
            {
                Fact remove = target.FactDict[_old_to_new[s_step.Id]];
                target.Remove(remove, s_step.samestep, used_gadget);
            }

            stepnote t_step = target.Workflow[i];
            t_step.GadgetTime = s_step.GadgetTime;
            target.Workflow[i] = t_step;
        }

        // set un-redo state
        while (target.backlog < source.backlog)
            target.undo();

        target.soft_resetted = source.soft_resetted;
        old_to_new = _old_to_new;
        return target;
    }


    /// <summary>
    /// wrappes <c><see cref="FactDict"/>[<paramref name="id"/>]</c>
    /// <seealso cref="ContainsKey(string)"/>
    /// </summary>
    /// <param name="id">a <see cref="Fact.Id"/> in <see cref="FactDict"/></param>
    /// <returns><c><see cref="FactDict"/>[<paramref name="id"/>]</c></returns>
    public Fact this[string id] { get => FactDict[id]; }

    /// <summary>
    /// wrappes <c><see cref="FactDict"/>.ContainsKey(<paramref name="id"/>)</c>
    /// </summary>
    /// <param name="id">a <see cref="Fact.Id"/></param>
    /// <returns><c><see cref="FactDict"/>.ContainsKey(<paramref name="id"/>)</c></returns>
    public bool ContainsKey(string id) => FactDict.ContainsKey(id);

    /// <summary>
    /// Looks up if there is a <paramref name="label"/> <see cref="Fact.Label"/> in <see cref="FactDict"/>.Values
    /// </summary>
    /// <param name="label">supposed <see cref="Fact.Label"/> to be checked</param>
    /// <returns><c>true</c> iff <see cref="FactDict"/> conatains a <c>Value</c> <see cref="Fact"/>, where <see cref="Fact.Label"/> == <paramref name="label"/>.</returns>
    public bool ContainsLabel(string label)
    {
        if (string.IsNullOrEmpty(label))
            return false;

        var hit = FactDict.FirstOrDefault(e => e.Value.Label == label);
        return !hit.Equals(System.Activator.CreateInstance(hit.GetType()));
    }

    //TODO? MMT? PERF: O(n), every Fact-insertion
    /// <summary>
    /// Looks for existent <see cref="Fact"/> (<paramref name="found"/>) which is very similar or identical (<paramref name="exact"/>) to prposed <see cref="Fact"/> (<paramref name="search"/>)
    /// <remarks>does not check active state</remarks>
    /// </summary>
    /// <param name="search">to be searched for</param>
    /// <param name="found"><see cref="Fact.Id"/> if return value is <c>true</c></param>
    /// <param name="exact"><c>true</c> iff <paramref name="found"/> == <paramref name="search"/><see cref="Fact.Id">.Id</see></param>
    /// <returns><c>true</c> iff the exact same or an equivalent <see cref="Fact"/> to <paramref name="search"/> was found in <see cref="FactDict"/></returns>
    private bool FindEquivalent(Fact search, out string found, out bool exact)
    {
        if (exact = FactDict.ContainsKey(search.Id))
        {
            found = search.Id;
            return true;
        }

        foreach (var entry in FactDict)
        {
            if (entry.Value.Equivalent(search))
            {
                found = entry.Key;
                return true;
            }
        }

        found = null;
        return false;
    }

    /// <summary>
    /// <see cref="PruneWorkflow">prunes</see> & adds <paramref name="note"/> to <see cref="Workflow"/>; <see cref="InvokeFactEvent(bool, string)">Invokes Events</see>
    /// </summary>
    /// <param name="note">to be added</param>
    private void WorkflowAdd(stepnote note)
    {
        PruneWorkflow(note);

        if (note.samestep)
        // update steplink of steproot
        {
            stepnote tmp = Workflow[note.steplink];
            tmp.steplink = Workflow.Count + 1;
            Workflow[note.steplink] = tmp;
        }
        else
            worksteps++;

        Workflow.Add(note);
        marker = Workflow.Count;

        InvokeFactEvent(note.creation, note.Id);
    }

    /// <summary>
    /// set current (displayed) state in stone, a.k.a. <see cref="Fact.delete(bool)">delete</see> non <see cref="meta.active"/> <see cref="Fact">Facts</see> for good;
    /// resets <see cref="undo">un</see>-<see cref="redo"/> parameters
    /// </summary>
    private void PruneWorkflow(stepnote not_me)
    {
        /*if (soft_resetted)
            this.hardreset(false); // musn't clear

        else*/
        if (backlog > 0)
        {
            worksteps -= backlog;
            backlog = 0;

            for (int i = Workflow.Count - 1; i >= marker; i--)
            // cleanup now obsolete Facts
            {
                stepnote last = Workflow[i];

                if (last.gadget_rank == MetaInf[last.Id].workflow_id
                 && last.gadget_rank != not_me.gadget_rank) 
                {   // Remove Gadget, if its the first time it's beeing used
                    GadgetWorkflowDict.Remove(WorkflowGadgetDict[last.gadget_rank]);
                    WorkflowGadgetDict.Remove(last.gadget_rank);
                }

                if (last.Id != not_me.Id // may be zombie
                 && MetaInf[last.Id].workflow_id == i)
                // remove for good, if original creation gets pruned
                {
                    this[last.Id].delete();
                    FactDict.Remove(last.Id);
                    MetaInf.Remove(last.Id);
                }
            }

            // prune Worklfow down to marker
            Workflow.RemoveRange(marker, Workflow.Count - marker);
        }
    }

    /// <summary>
    /// Call this to Add a <see cref="Fact"/> to <see cref="FactOrganizer">this</see> instance.
    /// <remarks>*Warning*: If return_value != <paramref name="value"/><see cref="Fact.Id">.Id</see>, <paramref name="value"/> will be <see cref="Fact.delete(bool)">deleted</see> for good to reduce ressource usage!</remarks>
    /// </summary>
    /// <param name="value">to be added</param>
    /// <param name="exists"><c>true</c> iff <paramref name="value"/> already exists (may be <see cref="meta.active">inactive</see> before opreation)</param>
    /// <param name="samestep">set <c>true</c> if <see cref="Fact"/> creation happens as a subsequent/ consequent step of multiple <see cref="Fact"/> creations and/or deletions, 
    /// and you whish that these are affected by a single <see cref="undo"/>/ <see cref="redo"/> step</param>
    /// <returns><see cref="Fact.Id"/> of <paramref name="value"/> or <see cref="FindEquivalent(Fact, out string, out bool)">found</see> <see cref="Fact"/> iff <paramref name="exists"/>==<c>true</c></returns>
    public string Add(Fact value, out bool exists, bool samestep, Gadget gadget, string scroll_label)
    {
        soft_resetted = false;
#pragma warning disable IDE0018 // Inlinevariablendeklaration
        string key;
#pragma warning restore IDE0018 // Inlinevariablendeklaration

        if (exists = FindEquivalent(value, out key, out bool exact))
        {
            if (!exact)
                // no longer needed
                value.delete();

            if (exists = MetaInf[key].active) //Fact in Scene?
                // desired outcome already achieved
                return key;

            if (MetaInf[key].workflow_id > marker)
                // update meta data: everything >= marker will be pruned (except this Fact)
                MetaInf[key] = new meta(marker, true);

        }
        else
        // brand new Fact
        {
            key = value.Id;
            FactDict.Add(key, value);
            MetaInf.Add(key, new meta(marker, true));
        }

        WorkflowAdd(new stepnote(this, key, samestep, true, gadget, scroll_label));
        return key;
    }

    /// <summary>
    /// Call this to Remove a <see cref="Fact"/> from <see cref="FactOrganizer">this</see> instance.
    /// If other <see cref="Fact">Facts</see> depend on <paramref name="value"/> <see cref="Remove(Fact, bool)">Remove(/<depending Fact/>, <c>true</c>)</see> will be called recursively/ cascadingly.
    /// </summary>
    /// <remarks>this will not <see cref="Fact.delete(bool)">delete</see> a <see cref="Fact"/>, but sets it <see cref="meta.active">inactive</see> for later <see cref="Fact.delete(bool)">deletion</see> when <see cref="PruneWorkflow">pruned</see>.</remarks>
    /// <param name="value">to be removed</param>
    /// <param name="samestep">set <c>true</c> if <see cref="Fact"/> deletion happens as a subsequent/ consequent step of multiple <see cref="Fact"/> creations and/or deletions, 
    /// and you whish that these are affected by a single <see cref="undo"/>/ <see cref="redo"/> step</param>
    /// <returns><c>true</c> iff <paramref name="value"/><see cref="Fact.Id">.Id</see> was found.</returns>
    public bool Remove(Fact value, bool samestep, Gadget gadget)
        => this.Remove(value.Id, samestep, gadget);

    /// \copybrief Remove(Fact, bool)
    /// <remarks>this will not <see cref="Fact.delete(bool)">delete</see> a <see cref="Fact"/>, but sets it <see cref="meta.active">inactive</see> for later <see cref="Fact.delete(bool)">deletion</see> when <see cref="PruneWorkflow">pruned</see>.</remarks>
    /// <param name="key">to be removed</param>
    /// <param name="samestep">set <c>true</c> if <see cref="Fact"/> deletion happens as a subsequent/ consequent step of multiple <see cref="Fact"/> creations and/or deletions, 
    /// and you whish that these are affected by a single <see cref="undo"/>/ <see cref="redo"/> step</param>
    /// <returns><c>true</c> iff <paramref name="value"/> was found.</returns>
    public bool Remove(string key, bool samestep, Gadget gadget)
    //no reset check needed (impossible state)
    {
        if (!FactDict.ContainsKey(key))
            return false;

        if (!MetaInf[key].active)
            // desiered outcome reality
            return true;

        //TODO: see issue #58

        safe_dependencies(key, out List<string> deletethis);

        if (deletethis.Count > 0)
        {
            yeetusdeletus(deletethis, samestep, gadget);
        }

        return true;
    }

    // TODO: MMT: decide dependencies there (remember virtual deletions in Unity (un-redo)!)
    // TODO? decrease runtime from amorised? O((n/2)^2)
    /// <summary>
    /// searches recursively for <see cref="Fact">Facts</see> where <see cref="Fact.getDependentFactIds"/> includes <paramref name="key"/>/ found dependencies
    /// </summary>
    /// <param name="key">to be cross referenced</param>
    /// <param name="dependencies">all <see cref="Fact">Facts</see> where <see cref="Fact.getDependentFactIds"/> includes <paramref name="key"/>/ found dependencies</param>
    /// <returns><c>false</c> if any dependencies are <see cref="stepnote">steproots</see></returns>
    public bool safe_dependencies(string key, out List<string> dependencies)
    {
        dependencies = new List<string>();
        int c_unsafe = 0;

        int pos = MetaInf[key].workflow_id;
        dependencies.Add(key);

        // accumulate facts that are dependent of dependencies
        for (int i = pos; i < marker; i++)
        {
            // TODO: consequent != samestep != dependent (want !consequent)
            if (!Workflow[i].creation && Workflow[i].Id != key)
            {
                // just try
                if (dependencies.Remove(Workflow[i].Id) && !Workflow[i].samestep)
                    c_unsafe--;
            }
            else if (this[Workflow[i].Id].getDependentFactIds().Intersect(dependencies).Any() && Workflow[i].Id != key)
            {
                dependencies.Add(Workflow[i].Id);
                if (!Workflow[i].samestep)
                    c_unsafe++;
            }
        }

        return c_unsafe == 0;
    }

    /// <summary>
    /// Turns every <see cref="Fact"/> in <paramref name="deletereverse"/> (in reverse order) <see cref="meta.active">inactive</see>, as it would be <see cref="Remove(string, bool)">removed</see>, but without checking for (recursive) dependencies.
    /// </summary>
    /// <param name="deletereverse">to be <see cref="Remove(string, bool)">removed</see>, but without checking for (recursive) dependencies</param>
    /// <param name="samestep">see <see cref="Remove(string, bool).samestep"/>. Only applies to last (first iteration) element of <paramref name="deletereverse"/>; for everything else <paramref name="samestep"/> will be set to <c>true</c>.</param>
    private void yeetusdeletus(List<string> deletereverse, bool samestep, Gadget gadget)
    {
        for (int i = deletereverse.Count - 1; i >= 0; i--, samestep = true)
        {
            WorkflowAdd(new stepnote(this, deletereverse[i], samestep, false, gadget, null));
        }
    }

    /// <summary>
    /// reverses any entire step; adds process to Workflow!
    /// </summary>
    /// <remarks>*Warning*: unused therefore untested and unmaintained.</remarks>
    /// <param name="pos">position after <see cref="stepnote">steptail-end</see> of the step to be reversed</param>
    /// <param name="samestep">see <see cref="yeetusdeletus(List<string>, bool).samestep"/></param>
    private void reversestep(int pos, bool samestep = false)
    {
        pos--;

        if (pos >= marker)
            // check for valid step (implicit reset check)
            return;

        for (int i = pos, stop = Workflow[pos].samestep ? Workflow[pos].steplink : pos;
            i >= stop; i--, samestep = true)
        {
            if (Workflow[i].creation)
                Remove(Workflow[i].Id, samestep, null);
            else if (!MetaInf[Workflow[i].Id].active)
                WorkflowAdd(new stepnote(this, Workflow[i].Id, samestep, true, null, null));
        }
    }

    /// <summary>
    /// Undoes an entire <see cref="stepnote">step</see> or last <see cref="softreset"/> .
    /// No <see cref="Fact"/> will be actually <see cref="Add(Fact, out bool, bool)">added</see>, <see cref="Remove(Fact, bool)">removed</see> or <see cref="Fact.delete(bool)">deleted</see>; only its visablity and <see cref="meta.active"/> changes.
    /// <seealso cref="marker"/>
    /// <seealso cref="worksteps"/>
    /// <seealso cref="backlog"/>
    /// </summary>
    public void undo()
    {
        if (soft_resetted)
            fastforward(); // revert softreset

        else if (backlog < worksteps) {
            backlog++;

            stepnote last = Workflow[--marker];
            int stop = last.samestep ? last.steplink : marker;
            for (int i = marker; i >= stop; i--)
            {
                last = Workflow[i];
                InvokeFactEvent(!last.creation, last.Id);
            }

            marker = stop;
        }
    }

    /// <summary>
    /// Redoes an entire <see cref="stepnote">step</see> .
    /// No <see cref="Fact"/> will be actually <see cref="Add(Fact, out bool, bool)">added</see>, <see cref="Remove(Fact, bool)">removed</see> or <see cref="Fact.delete(bool)">deleted</see>; only its visablity and <see cref="meta.active"/> changes.
    /// <seealso cref="marker"/>
    /// <seealso cref="worksteps"/>
    /// <seealso cref="backlog"/>
    /// </summary>
    public void redo()
    {
        soft_resetted = false;

        if (backlog > 0)
        {
            backlog--;

            stepnote last = Workflow[marker];
            int stop = last.samestep ? Workflow[last.steplink].steplink : last.steplink;
            for (int i = marker; i < stop; i++)
            {
                last = Workflow[i];
                InvokeFactEvent(last.creation, last.Id);
            }

            marker = stop;
        }
    }

    /// <summary>
    /// Resets to "factory conditions".
    /// Neither <see cref="Fact.delete(bool)">deletes</see> <see cref="Fact">Facts</see> nor invokes <see cref="CommunicationEvents.RemoveFactEvent"/>
    /// </summary>
    /// <seealso cref="hardreset(bool)"/>
    public void Clear()
    {
        FactDict.Clear();
        MetaInf.Clear();
        Workflow.Clear();
        GadgetWorkflowDict.Clear();

        marker = 0;
        worksteps = 0;
        backlog = 0;
        soft_resetted = false;
    }

    /// <summary>
    /// Resets to "factory conditions".
    /// <see cref="Fact.delete(bool)">deletes</see> <see cref="Fact">Facts</see> and invokes <see cref="CommunicationEvents.RemoveFactEvent"/> iff <paramref name="invoke_event"/> && <see cref="invoke"/>.
    /// </summary>
    /// <seealso cref="Clear"/>
    /// <param name="invoke_event">if set to <c>true</c> *and* <see cref="invoke"/> set to <c>true</c> will invoke <see cref="CommunicationEvents.RemoveFactEvent"/></param>
    public void hardreset(bool invoke_event = true)
    {
        foreach (var entry in FactDict)
        {
            if (invoke_event && invoke && MetaInf[entry.Key].active)
                CommunicationEvents.RemoveFactEvent.Invoke(entry.Value);
            entry.Value.delete();
        }
        this.Clear();
    }

    /// <summary>
    /// <see cref="undo">Undoes</see> *all* <see cref="worksteps"/> (since <see cref="marker"/>) and sets <see cref="soft_resetted"/> to <c>true</c>.
    /// </summary>
    public void softreset()
    {
        if (soft_resetted)
        {
            fastforward();
            return;
        }

        while (marker > 0)
            undo();
        // marker = 0; backlog = worksteps;

        soft_resetted = true;
    }

    /// <summary>
    /// <see cref="redo">Redoes</see> *all* <see cref="worksteps"/> (from <see cref="marker"/> onwards) and sets <see cref="soft_resetted"/> to <c>false</c>.
    /// </summary>
    public void fastforward()
    {
        while (backlog > 0)
            // also sets resetted = false;
            redo();
    }

    FactOrganizer IJSONsavable<FactOrganizer>._IJPostProcess(FactOrganizer raw_payload)
        => raw_payload == null ? raw_payload : ReInitializeFactOrganizer<FactOrganizer>(raw_payload, false, out _);

    /// <summary>
    /// Call this after assigning a stored instance in an empty world, that was not drawn.
    /// <see cref="redo">Redoes</see>/ draws everything from <see cref="marker"/> = 0 to <paramref name="draw_all"/><c> ? worksteps : backlog</c>
    /// </summary>
    /// <remarks>Does not invoke <see cref="softreset"/> or <see cref="undo"/> in any way and thus may trigger <see cref="Exception">Exceptions</see> or undefined behaviour if any <see cref="Fact"/> in <see cref="FactDict"/> is already drawn.</remarks>
    public void Draw(bool draw_all = false)
    {
        // TODO: see issue #58
        // TODO: communication with MMT

        foreach (var key in FactDict.Keys)
        {
            // update active info if needed
            meta info = MetaInf[key];
            if (info.active)
            {
                info.active = false;
                MetaInf[key] = info;
            }
        }

        marker = 0;
        var stop = draw_all ? worksteps : backlog;
        backlog = worksteps;

        while(backlog > stop)
            redo();
    }

    /// <summary>
    /// Undraws everything by invoking <see cref="CommunicationEvents.RemoveFactEvent"/>, that is <see cref="meta.active"/>, but does not change that satus.
    /// </summary>
    /// <param name="force_invoke">if set <c>true</c>, invokes <see cref="CommunicationEvents.RemoveFactEvent"/> for every <see cref="Fact"/> regardles of <see cref="meta.active"/> status or <see cref="invoke"/></param>
    public void Undraw(bool force_invoke = false)
    {
        foreach (var entry in FactDict)
        {
            if (force_invoke || (invoke && MetaInf[entry.Key].active))
                CommunicationEvents.RemoveFactEvent.Invoke(entry.Value);
        }
    }

    /// <summary>
    /// Updates <see cref="MetaInf"/>, <see cref="Fact.Label"/> and invokes <see cref="CommunicationEvents"/> (latter iff <see cref="invoke"/> is set)
    /// </summary>
    /// <param name="creation">wether <see cref="Fact"/> is created or removed</param>
    /// <param name="Id"><see cref="Fact.Id"/></param>
    private void InvokeFactEvent(bool creation, string Id)
    {
        // update meta struct
        meta info = MetaInf[Id];
        info.active = creation;
        MetaInf[Id] = info;

        if (invoke)
            if (creation)
                CommunicationEvents.AddFactEvent.Invoke(this[Id]);
            else
                CommunicationEvents.RemoveFactEvent.Invoke(this[Id]);

        if (creation)
        // undo freeLabel()
            _ = FactDict[Id].Label;
        else
            FactDict[Id].freeAutoLabel();
    }

    /// <summary>
    /// Used to check wether <see cref="FactOrganizer">this</see> satisfies the constrains of an <see cref="SolutionOrganizer">Solution</see>.
    /// Only <see cref="meta.active"/> are accounted for.
    /// </summary>
    /// <param name="MinimalSolution">describes constrains</param>
    /// <param name="MissingElements">elements which were *not* found in <see cref="SolutionOrganizer.ValidationSet"/> in a format reflecting that of <see cref="SolutionOrganizer.ValidationSet"/></param>
    /// <param name="Solutions">elements which *were* found in <see cref="SolutionOrganizer.ValidationSet"/> in a format reflecting that of <see cref="SolutionOrganizer.ValidationSet"/></param>
    /// <returns><c>true</c> iff *all* constrains set by <paramref name="MinimalSolution"/> are met</returns>
    public bool DynamiclySolved(
        SolutionOrganizer MinimalSolution,
        out List<List<string>> MissingElements,
        out List<List<string>> Solutions)
    {
        MissingElements = new List<List<string>>();
        // need to work not on ref/out
        List<List<string>> Solution_L = new();

        int MissingElementsCount = 0;
        var activeList = FactDict.Values.Where(f => MetaInf[f.Id].active);

        foreach (var ValidationSet in MinimalSolution.ValidationSet)
        {
            // List to relato to. Either all active facts or those defined in RelationIndex if not empty
            var relateList = ValidationSet.RelationIndex.Count == 0 ? activeList :
                ValidationSet.RelationIndex.Select(i => Solution_L[i]) // Select by Index
                .SelectMany(i => i) // Flatten structure
                .Select(URI => this[URI]); // Get Facts

            // check by MasterIds
            // ALL Masters must relate
            var part_minimal = 
                ValidationSet.MasterIDs.Select(URI => MinimalSolution[URI]);

            var part_solution =
                relateList.Where(active => part_minimal.Contains(active, ValidationSet.Comparer.SetSearchRight()))
                .ToList(); // needed for some reason
            
            var part_missing =
                part_minimal.Except(part_solution, ValidationSet.Comparer.SetSearchLeft());

            // SolutionIndex may include current index
            Solution_L.Add(part_solution.Select(fact => fact.Id).ToList());
            MissingElements.Add(part_missing.Select(fact => fact.Id).ToList());
            MissingElementsCount += part_missing.Count();

            // check by previous solutions
            // at least ONE member must relate
            var part_consequential_minimal =
                ValidationSet.SolutionIndex.Select(i => Solution_L[i]) // Select by Index
                .SelectMany(i => i) // Flatten structure
                .Select(URI => this[URI]); // Get Facts

            var part_consequential_solution =
                relateList.Where(active => part_consequential_minimal.Contains(active, ValidationSet.Comparer.SetSearchRight()));

            Solution_L.Last().AddRange(part_consequential_solution.Select(fact => fact.Id).ToList());
            MissingElementsCount += Convert.ToInt32(
                part_consequential_solution.Count() == 0 && part_consequential_minimal.Count() != 0);
        }

        Solutions = Solution_L;
        return MissingElementsCount == 0;
    }

    public IEnumerable<Gadget> GetUsedGadgets() => GadgetWorkflowDict.Keys;

    public int GetNumberOfGadgets() => GadgetWorkflowDict.Count;
    
    public IEnumerable<string> GetUsedScrolls()
        => Workflow.Where(sn => MetaInf[sn.Id].active && sn.scroll_label != null).Select(sn => sn.scroll_label).Distinct();

    public int GetNumberOfScrolls() => GetUsedScrolls().Count();

    public int GetNumberOfFacts() => FactDict.Count;

}