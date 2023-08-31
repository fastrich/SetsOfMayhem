using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;


public class GadgetBehaviour : MonoBehaviour, ISerializationCallbackReceiver
{
    // could be worked around of. But as long as not needed: this is better
    public static GadgetBehaviour Singelton = null;

    /// <summary>Which cursor to use</summary>
    /// <remarks>When not set in Inspector, will be searching for any <see cref="WorldCursor"/>.</remarks>
    public static WorldCursor Cursor;
    public static LineRenderer LineRenderer;
    public static GameObject GadgetButton;
    public static GameObject GadgetName;
    public static GameObject ParentMe;
    public static Material[] Materials;
    public static Sprite[] ButtonSprites;
    public static String[] ButtonNames;

    public static Dictionary<Gadget.GadgetIDs, DataContainerGadgetInit> DataContainerGadgetDict;

    public static float ActiveGadgetScaleFactor = 1.5f;

    public static Gadget ActiveGadget { get => gadgets[ActiveGadgetInd]; }
    public static int ActiveGadgetInd;
    public static Gadget[] gadgets;
    public static Button[] buttons;

    private static UnityAction<RaycastHit[]> OnHit;
    private static RectTransform updateRect;

    #region Unity Serialization
    [SerializeField] private float _ActiveGadgetScaleFactor;

    [SerializeField] private GameObject _GadgetButton;
    [SerializeReference] private GameObject _GadgetName;
    [SerializeField] private GameObject _ParentMe;
    [SerializeField] private WorldCursor _Cursor;
    [SerializeField] private LineRenderer _LineRenderer;

    [SerializeField] private DataContainerGadgetCollection _DataContainerGadgetDict;

    public void OnBeforeSerialize() { }

    public void OnAfterDeserialize()
    {
        Cursor = _Cursor;
        LineRenderer = _LineRenderer;
        GadgetButton = _GadgetButton;
        GadgetName = _GadgetName;
        ParentMe = _ParentMe;

#pragma warning disable UNT0008 // Null propagation on Unity objects
        Materials = _DataContainerGadgetDict?.Materials ?? new Material[0];
        ButtonSprites = _DataContainerGadgetDict?.ButtonSprites ?? new Sprite[0];
        ButtonNames = _DataContainerGadgetDict?.GadgetNames ?? new string[0];
        DataContainerGadgetDict = _DataContainerGadgetDict?.DataContainerGadgetDict ?? new();
#pragma warning restore UNT0008 // Null propagation on Unity objects
    }
    #endregion

    private void OnDestroy()
    {
        Singelton = null;
        CommunicationEvents.TriggerEvent.RemoveListener(OnHitCallback);
    }

    private void Awake()
    {
        Singelton ??= this;
        if (this != Singelton) { 
            Debug.LogError("Only one Instance of GadgetManager allowed!");
            return;
        }

        gameObject.DestroyAllChildren();

        if (Cursor == null)
            Cursor = GameObject.FindObjectOfType<WorldCursor>();
        if (LineRenderer == null)
            LineRenderer = Cursor.GetComponent<LineRenderer>();
        if (ParentMe == null)
            ParentMe = this.gameObject;

        updateRect = this.gameObject.transform.parent as RectTransform;

        CommunicationEvents.TriggerEvent.AddListener(OnHitCallback);

        gadgets = (
            StageStatic.stage.AllowedGadgets == null
            || StageStatic.stage.AllowedGadgets.Count == 0
            ? Gadget.GadgetTypes
                .Where(t => t != typeof(Gadget.UndefinedGadget))
                .Select(t => (Gadget)Activator.CreateInstance(t))
            : StageStatic.stage.AllowedGadgets
            ).OrderBy(g => g.Rank).ToArray();
    }

    void Start()
    {
        void CreateButton(int gid)
        {
            GameObject button = GameObject.Instantiate(GadgetButton, parent: ParentMe.transform);
            button.GetComponent<Image>().sprite = ButtonSprites[gadgets[gid].ButtonIndx];
            var cache = gameObject.transform.parent.parent as RectTransform;
            (buttons[gid] = button.GetComponent<Button>())
                .onClick.AddListener(() => ActivateGadget(gid));
        }

        buttons = new Button[gadgets.Length];

        //Debug.Log("GadjetsNr: " + gadgets.Length);

        for (int i = 0; i < gadgets.Length; i++)
        {
            gadgets[i].Awake();
            //gadgets[i].Start();
            CreateButton(i);
        }

        ActiveGadgetInd = 0;
        buttons[0].transform.localScale *= ActiveGadgetScaleFactor;
        GadgetName = _GadgetName;

        ActivateGadget(0);
    }

    void Update()
    {
        if (this != Singelton) return;

        if (!ParentMe.activeInHierarchy)
            return;

        int offset = 0;

        if (Input.GetButtonDown("ToolMode"))
            offset = 1;
        else if (Input.GetAxis("Mouse ScrollWheel") != 0)
            offset = (int) Mathf.Sign(Input.GetAxis("Mouse ScrollWheel"));

        if (offset != 0)
            ActivateGadget(ActiveGadgetInd + offset);

        gadgets[ActiveGadgetInd].Update();
    }

    void OnDisable()
    {
        if (this != Singelton) return;

        gadgets[ActiveGadgetInd].Disable();
    }

    void OnEnable()
    {
        if (this != Singelton) return;

        gadgets[ActiveGadgetInd].Enable();
    }

    public static void ActivateGadget(int gid)
    {
        ParentMe.SetActive(true);
        gid += gid >= gadgets.Length 
            ? -gadgets.Length 
            : gid < 0 ? +gadgets.Length : 0;

        //buttons[activeGadget].animator.StopPlayback();
        buttons[ActiveGadgetInd].transform.localScale /= ActiveGadgetScaleFactor;
        gadgets[ActiveGadgetInd].Disable();


        ActiveGadgetInd = gid;
        //buttons[gid].animator.StartPlayback();
        buttons[gid].transform.localScale *= ActiveGadgetScaleFactor;
        GadgetName.GetComponent<TMP_Text>().text = ButtonNames[gadgets[gid].ButtonIndx];
        gadgets[gid].Enable();

        OnHit = gadgets[gid].Hit;

        CommunicationEvents.ToolModeChangedEvent.Invoke(gid);
        LayoutRebuilder.ForceRebuildLayoutImmediate(updateRect);
    }

    public static void OnHitCallback(RaycastHit[] hit) => OnHit.Invoke(hit);
}
