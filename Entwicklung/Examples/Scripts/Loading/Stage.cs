using System.IO;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Newtonsoft.Json;
using static CommunicationEvents;
using System;

public class Stage: IJSONsavable<Stage>
{
    #region metadata

    /// <summary> Which category this <see cref="Stage"/> should be displayed in. </summary>
    public string category = null;

    /// <summary> Where to display this <see cref="Stage"/> within a <see cref="category"/> relative to others. </summary>
    public int number = -1;

    /// <summary>
    /// The name this <see cref="Stage"/> will be displayed with.
    /// Also defines names of save files of <see cref="player_record_list">stage progress</see>, <see cref="solution"/>
    /// </summary>
    public string name { set; get; } = null;
    public string path { set; get; } = null;

    /// <summary> The description this <see cref="Stage"/> will be displayed with.</summary>
    public string description = null;

    /// <summary> The name of a <see cref="UnityEngine.SceneManagement.Scene"/> that this <see cref="Stage"/> takes place in.</summary>
    public string scene = null;

    /// <summary> Wether this <see cref="Stage"/> is located in installation folder or user data (a.k.a. !local).</summary>
    public bool use_install_folder = false;

    #endregion metadata

    /// <summary>
    /// Defining when this <see cref="Stage.player_record"/> is considered as solved.
    /// <seealso cref="FactOrganizer.DynamiclySolved(SolutionOrganizer, out List{List{string}}, out List{List{string}})"/>
    /// </summary>
    [JSONsavable.JsonAutoPreProcess, JSONsavable.JsonAutoPostProcess]
    public SolutionOrganizer solution = null;

    /// <summary>
    /// A single class containing all savegame-data.
    /// Stored seperately.
    /// </summary>
    [JsonIgnore, JSONsavable.JsonSeparate]
    public SaveGame savegame = null;

    public List<PlayerRecord> solution_approches = new();
    public List<string> AllowedScrolls = null;
    public List<Gadget> AllowedGadgets = null;

    #region makros/shortcuts

    /// <summary>
    /// <see langword="true"/> iff there is at least one element in <see cref="player_record_list"/> where <see cref="PlayerRecord.solved"/> == <see langword="true"/>.
    /// </summary>
    [JsonIgnore]
    //TODO? update color if changed
    public bool completed_once { get => player_record_list != null && player_record_list.Values.Any(s => s.solved == true); }

    /// <summary> Current Stage progress.</summary>
    [JsonIgnore]
    public PlayerRecord player_record { 
        get => savegame.player_record ??= new(record_name);
        set => (savegame ??= new())
            .player_record = value;
    }

    /// <summary>
    /// A list containing all saved player progress. <br/>
    /// - Key: name of file
    /// - Value: <see cref="PlayerRecord"/>
    /// </summary>
    [JsonIgnore]
    public Dictionary<string, PlayerRecord> player_record_list {
        get => savegame?.player_record_list;
        set => (savegame ??= new())
            .player_record_list = value;
    }

    /// <summary>
    /// A wrapper returning (or setting) <see cref="player_record.factState"/>. <br/>
    /// When <see cref="player_record"/> == <see langword="null"/>:
    /// - <c>get</c> returns <see langword="null"/>
    /// - <c>set</c> initiates <see cref="player_record"/>
    /// </summary>
    [JsonIgnore]
    public FactOrganizer factState { 
        get => player_record?.factState;
        set => (player_record ??= new(record_name))
            .factState = value;
    }

    #endregion makros/shortcuts

    /// <summary> Returns a name for <see cref="player_record.name"/> which needs to be uniquified once put into <see cref="player_record_list"/> (e.g. by <see cref="push_record(double, bool) adding '_i'"/>).</summary>
    private string record_name { get => name + "_save"; }

    /// <summary> Wether <see cref="player_record.factState"/> (<see langword="false"/>) or <see cref="solution"/> (<see langword="true"/>) is exposed and drawn.</summary>
    [JsonIgnore]
    public bool creatorMode = false;

    /// <summary> Tempory variable storing <see cref="factState"/> when <see cref="creatorMode"/> == <see langword="true"/>. </summary>
    private FactOrganizer hiddenState;

    static Stage()
    {
        IJSONsavable<Stage>.hierarchie = new List<Directories> { Directories.Stages };
    }

    /// <summary>
    /// Initiates all parameterless members. <br/>
    /// Used by <see cref="JsonConverter"/> to initate empty <c>class</c>.
    /// <seealso cref="InitOOP"/>
    /// </summary>
    public Stage()
    {
        InitOOP();
    }

    /// <summary>
    /// Standard Constructor. <br/>
    /// Initiates all members.
    /// <seealso cref="InitOOP"/>
    /// <seealso cref="InitFields(string, int, string, string, string, bool)"/>
    /// </summary>
    /// <param name="category">sets <see cref="category"/></param>
    /// <param name="number">sets <see cref="number"/></param>
    /// <param name="name">sets <see cref="name"/></param>
    /// <param name="description">sets <see cref="description"/></param>
    /// <param name="scene">sets <see cref="scene"/></param>
    /// <param name="local">sets !<see cref="use_install_folder"/></param>
    public Stage(string category, int number, string name, string description, string scene, bool local = true)
    {
        InitFields(category, number, name, description, scene, local);
        InitOOP();
    }


    /// <summary>
    /// Sets members which are primitives.
    /// </summary>
    /// <param name="category">sets <see cref="category"/></param>
    /// <param name="number">sets <see cref="number"/></param>
    /// <param name="name">sets <see cref="name"/></param>
    /// <param name="description">sets <see cref="description"/></param>
    /// <param name="scene">sets <see cref="scene"/></param>
    /// <param name="local">sets !<see cref="use_install_folder"/></param>
    public void InitFields(string category, int number, string name, string description, string scene, bool local)
    {
        this.category = category;
        this.number = number;
        this.name = name;
        this.description = description;
        this.scene = scene;
        this.use_install_folder = !local;
    }

    /// <summary>
    /// Initiates members which are non primitives.
    /// </summary>
    private void InitOOP()
    {
        solution = new SolutionOrganizer();
        savegame = new();
        player_record = new PlayerRecord(record_name);
    }

    /// <summary>
    /// Resets to factory condition.
    /// <see cref="ClearSolution"/>
    /// <see cref="ClearPlay"/>
    /// <see cref="ClearALLRecords"/>
    /// </summary>
    public void ClearAll()
    {
        ClearSolution();
        ClearPlay();
        ClearALLRecords();
    }

    /// <summary>
    /// Resets <see cref="solution"/> and calling <see cref="solution.hardreset(bool)"/>.
    /// <seealso cref="FactOrganizer.hardreset(bool)"/>
    /// </summary>
    public void ClearSolution()
    {
        solution.hardreset(false);
        solution = new SolutionOrganizer();
    }

    /// <summary>
    /// Resets current <see cref="player_record"/> and calling <see cref="player_record.factState.hardreset(bool)"/>.
    /// <seealso cref="FactOrganizer.hardreset(bool)"/>
    /// </summary>
    public void ClearPlay()
    {
        player_record.factState.hardreset(false);
        player_record = new PlayerRecord(record_name);
    }

    /// <summary>
    /// Resets and <see cref="deletet_record(PlayerRecord, bool)">deletes</see> all members of <see cref="player_record_list"/>.
    /// <seealso cref="PlayerRecord.delete(List<Directories>)"/>
    /// </summary>
    public void ClearALLRecords()
    {
        foreach (var record in player_record_list.Values)
            deletet_record(record, false);

        player_record_list = new Dictionary<string, PlayerRecord>();
    }

    /// <summary>
    /// <see cref="PlayerRecord.delete(List<Directories>)">Deletes</see> <paramref name="record"/> and calls <see cref="PlayerRecord.factState.hardreset()"/>.
    /// <seealso cref="PlayerRecord.delete(List<Directories>)"/>
    /// <seealso cref="FactOrganizer.hardreset(bool)"/>
    /// </summary>
    /// <param name="record">to be deleted</param>
    /// <param name="b_store">iff <see langword="true"/> <see cref="store(bool)">stores</see> changes made to this <see cref="Stage"/></param>
    public void deletet_record(PlayerRecord record, bool b_store = true)
    {
        record.factState.hardreset();

        if (b_store) {
            player_record_list.Remove(record.name);
            store();
        }
    }

    /// <summary>
    /// Clones <paramref name="record"/> to <see cref="player_record"/> iff found in <see cref="player_record_list"/> <br/>
    /// or initiates new <see cref="player_record"/> iff <paramref name="record"/>==<see langword="null"/>.
    /// </summary>
    /// <param name="record">to be set or <see langword="null"/></param>
    /// <returns><see langword="false"/> iff <paramref name="record"/> not found in <see cref="player_record_list"/> <br/>
    /// or <see cref="PlayerRecord.load(List<Directories>)"/> fails.</returns>
    public bool set_record(string record_index)
    {
        if (record_index == null)
            return false;

        if (!player_record_list.ContainsKey(record_index))
            return false;

        player_record = player_record_list[record_index].Clone();
        player_record.name = record_name;

        player_record.solved = false;
        store(false);
        return true;
    }

    /// <summary>
    /// Adds current <see cref="player_record"/> to <see cref="player_record_list"/> incrementing <see cref="PlayerRecord.seconds"/> beforehand.
    /// <remarks> IMPACTS SERVER PERFORMANCE: Increases number of <see cref="Fact"/>s at the Server for every <see cref="Fact"/> in current <see cref="player_record"/> </remarks>
    /// </summary>
    /// <param name="seconds_s">time in seconds to be added to <see cref="player_record.seconds"/> before pushing. <br/>
    /// Iff set to <c>-1</c> <see cref="Time.timeSinceLevelLoadAsDouble"/> will be used.</param>
    /// <param name="force_push">iff set <see langword="true"/> && <see cref="StageStatic.mode"/> == <see cref="StageStatic.Mode.Create"/> && <see cref="creatorMode"/> <br/>
    /// current displayed <see cref="solution"/> in <see cref="factState"/> will be pushed into <see cref="player_record_list"/></param>
    public void push_record(double seconds_s = -1, bool force_push = false)
    {
        if(!force_push && StageStatic.mode == StageStatic.Mode.Create && creatorMode)
        // store solution space
        {
            store(false);
            return;
        }

        player_record.seconds += seconds_s != -1 ? seconds_s : Time.timeSinceLevelLoadAsDouble;
        var push = player_record.Clone();

        int i = 1;
        push.name = record_name + "_" + i.ToString();
        for (; player_record_list.ContainsKey(push.name); i++)
            push.name = record_name + "_" + i.ToString();

        player_record_list.Add(push.name, push);

        player_record.solved = false;
        store(false);
    }

    /// <summary>
    /// Switches between <see cref="player_record.factState"/> (<see langword="false"/>) and <see cref="solution"/> (<see langword="true"/>) to display in GameWorld.
    /// </summary>
    /// <param name="create">sets <see cref="creatorMode"/></param>
    public void SetMode(bool create)
    {
        if (create == creatorMode)
            return;
        
        creatorMode = create;

        if (create)
        {
            hiddenState = factState;
            factState.Undraw();

            factState = solution as FactOrganizer;
            factState.invoke = true;
            factState.Draw();
        }
        else
        {
            solution = factState as SolutionOrganizer;
            factState.Undraw();
            //solution.invoke = false;

            factState = hiddenState;
            factState.Draw();
        }

    }

    /// <summary>
    /// Clears and deletes all files associated with this <see cref="Stage"/>.
    /// </summary>
    /// <param name="player_record_list_too">iff set <see langword="false"/>, all files regarding <see cref="player_record_list"/> will be spared.</param>
    public void delete()
    {
        ClearAll();
        IJSONsavable<Stage>.delete(null, name);
    }

    /// <summary>
    /// Stores and overwrites this <see cref="Stage"/>, <see cref="player_record"/>, every element in <see cref="player_record_list"/> and <see cref="solution"/> (no overwrite for latter if empty).
    /// </summary>
    /// <param name="reset_player">wether to clear current <see cref="player_record"/></param>
    public void store(bool reset_player = false, bool force_stage_file = false)
    {
        player_record.name = record_name;
        if (reset_player)
            player_record = new PlayerRecord(record_name);

        if (force_stage_file || StageStatic.mode == StageStatic.Mode.Create)
            (this as IJSONsavable<Stage>).store         (null, name, use_install_folder, deep_store: false);
        else
            (this as IJSONsavable<Stage>).store_children(null, name, false             , deep_store: true );
    }

    /// <summary>
    /// Reads File given by <paramref name="path"/> and writes its contents into <paramref name="set"/>.
    /// <remarks>Will not read members decorated with <see cref="JsonIgnoreAttribute"/>: <see cref="solution"/>, <see cref="player_record"/>.</remarks>
    /// </summary>
    /// <param name="set">to be written in</param>
    /// <param name="path">file location</param>
    /// <returns><see langword="true"/> iff succeeded</returns>
    public static bool ShallowLoad(out Stage set, string path)
    {
        if (!IJSONsavable<Stage>.Instance._IJGetRawObject(out set, path))
            return false;

        set = IJSONsavable<Stage>.postprocess(set);
        set.path = path;
        IJSONsavable<Stage>.load_children(null /*hierarchie*/, set.name, ref set, post_process: false);

        return true;
    }

    /// <summary>
    /// Loads every member decorated with <see cref="JsonIgnoreAttribute"/>: <see cref="solution"/>, <see cref="player_record"/>.
    /// </summary>
    /// <returns><see langword="false"/> iff <see cref="solution"/> could not be <see cref="SolutionOrganizer.load(ref SolutionOrganizer, bool, string, List<Directories>, bool)">loaded</see>.</returns>
    public bool DeepLoad()
    {
        Stage cpy = this;
        IJSONsavable<Stage>.load_children(null /*hierarchie*/, name, ref cpy);
        return true;
    }

    /// <summary>
    /// Looks for saved <see cref="Stage">Stages</see> in parametised directories and calls on them <see cref="ShallowLoad(out Stage, string)"/>.
    /// </summary>
    /// <param name="hierarchie">see <see cref="hierarchie"/> //TODO? Interface</param>
    /// <param name="use_install_folder">see <see cref="use_install_folder"/></param>
    /// <returns>contians all <see cref="Stage">Stages</see> found given parameters.</returns>
    public static Dictionary<string, Stage> Grup(List<Directories> hierarchie = null, bool use_install_folder = false)
    {
        Dictionary<string, Stage> ret = new();

        var new_hierarchie = IJSONsavable<Stage>.Instance._IJGetHierarchie(hierarchie);
        string path = CreatePathToFile(out _, "", "", new_hierarchie, use_install_folder);

        foreach(var file in new DirectoryInfo(path).GetFiles())
        {
            if (file.Extension != ".JSON")
                continue;

            if (ShallowLoad(out Stage tmp, file.FullName))
                ret.Add(tmp.name, tmp);
        }

        return ret;
    }

    /// <summary>
    /// Calls <see cref="ClearPlay"/> and <see cref="store(bool)">store(true)</see>.
    /// </summary>
    public void ResetPlay()
    {
        ClearPlay();
        store(true);
    }

    /// <summary>
    /// Calls <see cref="ClearPlay"/>, <see cref="ClearALLRecords"/> and <see cref="store(bool)">store(true)</see>.
    /// </summary>
    public void ResetSaves()
    {
        ClearPlay();
        ClearALLRecords();
        store(true);
    }

    /// <summary>
    /// Checks if current <see cref="player_record"/> is solved. <br/>
    /// Iff return value <see langword="true"/>:
    /// - Highlites all <see cref="Fact">Facts</see> in <see cref="factState"/> beeing found in <see cref="solution"/>
    /// - Iff <see cref="player_record.seconds"/> > 0: 
    /// <see cref="push_record(double, bool)">Pushes</see> current <see cref="player_record"/> to <see cref="player_record_list"/> and sets <see cref="PlayerRecord.solved"/> to <see langword="true"/>.
    /// </summary>
    /// <returns><see langword="true"/> iff current <see cref="player_record"/> is solved.</returns>
    /// <seealso cref="FactOrganizer.DynamiclySolved(SolutionOrganizer, out List<List<string>>, out List<List<string>>)"/>
    public bool CheckSolved()
    {
        double time_s = Time.timeSinceLevelLoadAsDouble;

        bool solved =
            factState.DynamiclySolved(solution, out _, out List<List<string>> hits);

        if (solved)
        {
            foreach (var hitlist in hits)
                foreach (var hit in hitlist)
                    AnimateExistingAsSolutionEvent.Invoke(factState[hit], FactObject.FactMaterials.Solution);

            player_record.solved = true;
            push_record(time_s);
            store(false); // keep player_record
            player_record.solved = false;
        }

        return solved;
    }
}


public class SaveGame : IJSONsavable<SaveGame>
{
    public string name { get; set; }
    public string path { get; set; }

    [JSONsavable.JsonAutoPreProcess, JSONsavable.JsonAutoPostProcess]
    public PlayerRecord player_record = null;

    public Dictionary<string, PlayerRecord> player_record_list = new(); //entries are "PostProcess"ed when accessed/Cloned

    static SaveGame() 
    {
        IJSONsavable<SaveGame>.hierarchie = new() { Directories.SaveGames };
    }
    public SaveGame() { }

    string IJSONsavable<SaveGame>._IJGetName(string name) => name + "_save";
    SaveGame IJSONsavable<SaveGame>._IJPostProcess(SaveGame payload)
    {
        if ((payload.player_record_list ??= new()).Count == 0)
            payload.player_record = null;
        return payload;
    }
}


/// <summary>
/// Represents a save slot.
/// </summary>
public class PlayerRecord: IJSONsavable<PlayerRecord>
{
    /// <summary> Wether this save has solved the <see cref="Stage"/> which contains it. </summary>
    public bool solved = false;
    /// <summary> When this save was created (not modified!). </summary>
    public long date = System.DateTime.Now.ToBinary();
    /// <summary> The time spent within this save since creation. </summary>
    public double seconds = 0;

    /// <summary> Stage progress. </summary>
    [JSONsavable.JsonAutoPreProcess, JSONsavable.JsonAutoPostProcess]
    public FactOrganizer factState = new();
    /// <summary> save game file name </summary>
    public string name { get; set; } = null;
    public string path { get; set; } = null;

    static PlayerRecord(/*static!*/)
    {
        IJSONsavable<PlayerRecord>.hierarchie = new List<Directories> { /*Directories.FactStateMachines*/ };
    }

    /// <summary>
    /// Empty constructor for <see cref="JsonConverter"/>
    /// </summary>
    public PlayerRecord() { }

    /// <summary>
    /// Standard Constructor.
    /// </summary>
    /// <param name="name">sets <see cref="name"/></param>
    public PlayerRecord(string name) {
        this.name = name;
        factState = new FactOrganizer() { invoke = true };
    }

    /// <summary>
    /// Copies a specified <see cref="PlayerRecord"/>
    /// </summary>
    /// <param name="hierarchie">// TODO:</param>
    /// <returns>a copied <see cref="PlayerRecord"/></returns>
    public PlayerRecord Clone()
    {
        var ret = new PlayerRecord(this.name)
        {
            solved = this.solved,
            seconds = this.seconds
        };
        ret.factState = IJSONsavable<FactOrganizer>.postprocess(this.factState);

        return ret;
    }
}