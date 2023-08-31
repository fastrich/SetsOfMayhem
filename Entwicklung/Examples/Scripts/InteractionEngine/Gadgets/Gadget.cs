using JsonSubTypes;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;

/// <summary>
/// Base class for all Gadgets to derive from.
/// A Gadget is a tool for the player (and level editor) to interact with the GameWorld.
/// </summary>
[JsonConverter(typeof(JsonSubtypes), "s_type")]
[JsonSubtypes.KnownSubType(typeof(Pointer), "Pointer")]
[JsonSubtypes.KnownSubType(typeof(Tape), "Tape")]
[JsonSubtypes.KnownSubType(typeof(LineTool), "LineTool")]
[JsonSubtypes.KnownSubType(typeof(LotTool), "LotTool")]
[JsonSubtypes.KnownSubType(typeof(AngleTool), "AngleTool")]
[JsonSubtypes.KnownSubType(typeof(Pendulum), "Pendulum")]
[JsonSubtypes.KnownSubType(typeof(PoleTool), "PoleTool")]
[JsonSubtypes.KnownSubType(typeof(Remover), "Remover")]
[JsonSubtypes.KnownSubType(typeof(EqualCircleGadget), "EqualCircles")]
[JsonSubtypes.FallBackSubType(typeof(UndefinedGadget))]
public abstract class Gadget
{
    /// <value>
    /// [ClassName] for JSON de-/serialization.
    /// Set in every non-abstract subclass of Gadget.
    /// Also add JsonSubtypes.KnownSubType attribute for deserialization to Gadget!
    /// </value>
    [JsonProperty]
    protected static /*new*/ string s_type = "ERROR: set s_type in T:Gadget"; // In the subtype! NOT here!

    /// <summary>Used to map to a T:Gadget </summary>
    /// <remarks>Do NOT rename elements! Do NOT change values! Deserialization relies on it!</remarks>
    public enum GadgetIDs
    {
        Undefined = -1,
        Pointer = 0,
        Tape = 1,
        AngleTool = 2,
        LineTool = 3,
        LotTool = 4,
        Pendulum = 5,
        PoleTool = 6,
        Remover = 7,
        EqualCircles = 8,
        MiddlePoint = 9,
    }

    public static Dictionary<Type, GadgetIDs> GadgetTypeToIDs = new(){
        {typeof(UndefinedGadget)    , GadgetIDs.Undefined },
        {typeof(Pointer)            , GadgetIDs.Pointer },
        {typeof(Tape)               , GadgetIDs.Tape },
        {typeof(AngleTool)          , GadgetIDs.AngleTool },
        {typeof(LineTool)           , GadgetIDs.LineTool },
        {typeof(LotTool)            , GadgetIDs.LotTool },
        {typeof(Pendulum)           , GadgetIDs.Pendulum },
        {typeof(PoleTool)           , GadgetIDs.PoleTool },
        {typeof(Remover)            , GadgetIDs.Remover },
        {typeof(EqualCircleGadget)  , GadgetIDs.EqualCircles },
        {typeof(TestMiddlePoint), GadgetIDs.MiddlePoint },
        };

    /// <summary> Position in tool belt. </summary>
    /// <remarks>Set in Inspector or <see cref="Awake"/></remarks>
    public int Rank = int.MinValue;
    /// <summary> Tool Name </summary>
    /// <remarks>Set in Inspector or <see cref="Awake"/></remarks>
    public string UiName = null;
    /// <summary> Maximum range for this Tool. For consistency use GadgetDistances in <see cref="GlobalBehaviour"/>.</summary>
    /// <remarks>Set in Inspector or <see cref="Awake"/></remarks>
    public float MaxRange = float.NegativeInfinity;
    public float MaxHeight = float.NegativeInfinity;
    private float NewMaxRange = float.NegativeInfinity;

    /// <summary>Which sprite to use</summary>
    public int ButtonIndx = -1;
    public int MaterialIndx = -1;
    /// <summary>Layers to ignore for this gadget by default.</summary>
    /// <remarks>Set in Inspector</remarks>
    public LayerMask IgnoreLayerMask = -1;
    public LayerMask SecondaryLayerMask = -1;

    private bool init_success = false;

    /// <summary>Keeps track of selected <see cref="Fact.Id"/>s the Gadget used to produce a single <see cref="Fact"/>.</summary>
    public List<string> Workflow = new();

    /// <summary>
    /// Collection of <c>Type</c>s of *all* available <see cref="Gadget"/>s to choose from.
    /// </summary>
    [JsonIgnore]
    public static readonly IEnumerable<Type> GadgetTypes = Assembly.GetExecutingAssembly().GetTypes().Where(t => t != typeof(Gadget) && typeof(Gadget).IsAssignableFrom(t));

    public Gadget()
    {
        Init(true);
    }

    public static bool Equals(Gadget a, Gadget b)
        => (a == null && b == null) || (a != null && b != null
        && a.GetType() == b.GetType()
        && a.Rank == b.Rank
        && a.UiName == b.UiName
        && a.MaxRange == b.MaxRange
        && a.MaxHeight == b.MaxHeight
        && a.ButtonIndx == b.ButtonIndx
        && a.MaterialIndx == b.MaterialIndx
        && a.IgnoreLayerMask == b.IgnoreLayerMask
        && a.SecondaryLayerMask == b.SecondaryLayerMask);

    public void Init(bool overrite)
    {
        if ( init_success
          || GadgetBehaviour.DataContainerGadgetDict == null)
            return;

        Type this_type = this.GetType();
        if (!GadgetTypeToIDs.ContainsKey(this_type))
        {
            Debug.LogError("No " + this_type.ToString() + "in Dictionary<Type, GadgetIDs> GadgetTypeToIDs!");
            return;
        }
        var GadgetID = GadgetTypeToIDs[this_type];
        
        if (!GadgetBehaviour.DataContainerGadgetDict.ContainsKey(GadgetID))
        {
            Debug.LogError("No " + GadgetID.ToString() + "in assigned " + typeof(DataContainerGadgetCollection).Name + "!");
            return;
        }
        var data_cache = GadgetBehaviour.DataContainerGadgetDict[GadgetID];

        if (overrite || Rank == int.MinValue)
            Rank = data_cache.Rank;
        if (overrite || UiName == null)
            UiName = data_cache.UiName;
        if (overrite || MaxRange == float.NegativeInfinity)
            MaxRange = data_cache.MaxRange;
        if (overrite || MaxHeight == float.NegativeInfinity)
            MaxHeight = data_cache.MaxHeight;
        if (overrite || IgnoreLayerMask == -1)
            IgnoreLayerMask = data_cache.IgnoreLayerMask;
        if (overrite || SecondaryLayerMask == -1)
            SecondaryLayerMask = data_cache.SecondaryLayerMask;
        if (overrite || ButtonIndx < 0)
            ButtonIndx = 
                data_cache.ButtonIndx < GadgetBehaviour.ButtonSprites.Length
             && data_cache.ButtonIndx >= 0
                ? data_cache.ButtonIndx : 0;
        if (overrite || MaterialIndx < 0)
            MaterialIndx = 
                data_cache.MaterialIndx < GadgetBehaviour.Materials.Length 
             && data_cache.MaterialIndx >= 0
                ? data_cache.MaterialIndx : 0;

        init_success = true;
    }

    public void Awake()
    {
        Init(false);
        _Awake();
    }

    public void Enable()
    {
        GadgetBehaviour.Cursor.setLayerMask(~IgnoreLayerMask.value);
        _Update_Range();
        ResetGadget();
        _Enable();
    }

    public void Disable()
    {
        ResetGadget();
        _Disable();
    }

    public void Update()
    {
        if (!CommunicationEvents.GadgetCanBeUsed)
            return;

        if (GadgetBehaviour.LineRenderer.enabled)
            UpdateLineDrawing();

        _Update();
        
}

    public void Hit(RaycastHit[] hit)
    {    
        if (!CommunicationEvents.GadgetCanBeUsed
            //TODO: We should probably check all hits and sort out the "bad" ones
            || hit[0].transform.position.y > MaxHeight)
            return;

        _Hit(hit);
    }

    public virtual void _Awake() { }
    public virtual void _Enable() { }
    public virtual void _Disable() { }
    public virtual void _Update() { }

    /// <summary>
    /// Called when <see cref="CommunicationEvents.TriggerEvent"/> is invoked, a.k.a. when Player clicks in GameWorld.
    /// </summary>
    /// <param name="hit">the position where it was clicked</param>
    public virtual void _Hit(RaycastHit[] hit) { }

    protected void ActivateLineDrawing()
    {
        GadgetBehaviour.LineRenderer.enabled = true;
        GadgetBehaviour.LineRenderer.material = GadgetBehaviour.Materials[MaterialIndx];

        _ActivateLineDrawing();
    }

    protected void DeactivateLineDrawing()
    {
        GadgetBehaviour.LineRenderer.positionCount = 0;
        GadgetBehaviour.LineRenderer.enabled = false;

        _DeactivateLineDrawing();
    }

    protected void UpdateLineDrawing()
    {
        _UpdateLineDrawing();
    }

    protected virtual void _ActivateLineDrawing() { }
    protected virtual void _DeactivateLineDrawing() { }
    protected virtual void _UpdateLineDrawing() { }

    protected Vector3 GetPosition(int i)
        => GadgetBehaviour.LineRenderer.GetPosition(i);
    protected void SetPosition(int i, Vector3 v)
        => GadgetBehaviour.LineRenderer.SetPosition(i, v);
    protected void SetPositions(Vector3[] v)
        => GadgetBehaviour.LineRenderer.SetPositions(v);

    public void ResetGadget()
    {
        DeactivateLineDrawing();
        Workflow = new();
        _ResetGadget();
    }

    protected virtual void _ResetGadget() { }

    protected virtual void _Update_Range() { NewMaxRange = MaxRange; GadgetBehaviour.Cursor.MaxRange = NewMaxRange; }

    public class UndefinedGadget : Gadget { }
}
