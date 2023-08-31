using UnityEngine;
using UnityEngine.SceneManagement;
using static CommunicationEvents;
using static UIconfig;
using System.Collections;
using UnityEngine.InputSystem;
//using static CamControl_1;

public class HideUI : MonoBehaviour
{

    //public KeyCode Key = KeyCode.F1;
    //public KeyCode ScreenshotKey = KeyCode.F12;

    public string
        modifier,
        modundo,
        modredo,
        modreset,
        modsave,
        modload;
    public string cancel_keyBind;
    public string MathM_keyBind;
    public float waitingBetweenInputs = 0.2f;
    private double numinputtrigger = 0;


    public UnityStandardAssets.Characters.FirstPerson.FirstPersonController CamControl_StdAsset;
    public Characters.FirstPerson.FirstPersonController1 CamControl_ScriptChar;


    public bool LockOnly = true;
    public MeshRenderer CursorRenderer;
    //for Debug:
    //public MeshRenderer CursorRenderer_FirstP; 
    //public MeshRenderer CursorRenderer_FirstP_oldInpOrig;
    //public int whichCursor;

    private ControlMapping input_ControlMapping;
    private PlayerInput playerInput;
    //Store the controls
    private InputAction action_MathM;
    private InputAction action_ToolM;
    private InputAction action_Cancel_PM;
    private InputAction action_modifier;
    private InputAction action_load;
    private InputAction action_save;
    private InputAction action_reset;
    private InputAction action_undo;
    private InputAction action_redo;

    private int action_MathM_int = 0;
    private int action_ToolM_int = 0;
    private int action_Cancel_PM_int = 0;
    private int action_modifier_int = 0;
    private int action_load_int = 0;
    private int action_save_int = 0;
    private int action_reset_int = 0;
    private int action_undo_int = 0;
    private int action_redo_int = 0;





    internal Canvas UICanvas;

    private void Awake()
    {
        //New InputSystem
        input_ControlMapping = new ControlMapping();
            input_ControlMapping.Actionmap1.Cancel_or_PauseMenue.Enable();
            input_ControlMapping.Actionmap1.ToolMode.Enable();
            input_ControlMapping.Actionmap1.MathMode.Enable();
            input_ControlMapping.Actionmap1.Modifier.Enable();
            input_ControlMapping.Actionmap1.Load.Enable();
            input_ControlMapping.Actionmap1.Save.Enable();
            input_ControlMapping.Actionmap1.Reset.Enable();
            input_ControlMapping.Actionmap1.Undo.Enable();
            input_ControlMapping.Actionmap1.Redo.Enable();

        playerInput = GetComponent<PlayerInput>();
        action_MathM = playerInput.actions["MathMode"];
        action_ToolM = playerInput.actions["ToolMode"];
        action_Cancel_PM = playerInput.actions["Cancel_or_PauseMenue"];
        action_modifier = playerInput.actions["Modifier"];
        action_load = playerInput.actions["Load"];
        action_save = playerInput.actions["Save"];
        action_reset = playerInput.actions["Reset"];
        action_undo = playerInput.actions["Undo"];
        action_redo = playerInput.actions["Redo"];

}
    private void OnEnable()
    {
        input_ControlMapping.Actionmap1.Cancel_or_PauseMenue.Enable();
            input_ControlMapping.Actionmap1.ToolMode.Enable();
            input_ControlMapping.Actionmap1.MathMode.Enable();
            input_ControlMapping.Actionmap1.Modifier.Enable();
            input_ControlMapping.Actionmap1.Load.Enable();
            input_ControlMapping.Actionmap1.Save.Enable();
            input_ControlMapping.Actionmap1.Reset.Enable();
            input_ControlMapping.Actionmap1.Undo.Enable();
            input_ControlMapping.Actionmap1.Redo.Enable();
    }

    private void OnDisable()
    {
        input_ControlMapping.Actionmap1.Cancel_or_PauseMenue.Disable();
            input_ControlMapping.Actionmap1.ToolMode.Disable();
            input_ControlMapping.Actionmap1.MathMode.Disable();
            input_ControlMapping.Actionmap1.Modifier.Enable();
            input_ControlMapping.Actionmap1.Load.Disable();
            input_ControlMapping.Actionmap1.Save.Disable();
            input_ControlMapping.Actionmap1.Reset.Disable();
            input_ControlMapping.Actionmap1.Undo.Disable();
            input_ControlMapping.Actionmap1.Redo.Disable();
    }


    void Start()
    {
        if (UIconfig.FrameITUIversion == 1) // 1= FrameITUI; 2= FrameITUI_mobil
        {
            Start2();
            //Cursor.lockState = CursorLockMode.Confined;
        }
        else
        {
            if (UICanvas == null)
            {
                UICanvas = GetComponentInChildren<Canvas>();
            }
            UICanvas.enabled = false;
        }

    }
    void Start2()
    {

        if (!LockOnly)
        {
            if (UICanvas == null)
            {
                UICanvas = GetComponentInChildren<Canvas>();
            }
            bool camActive;
            camActive = !UICanvas.enabled;
            //camActive = false;
            SetCamControl123(camActive);
            SetCursorRenderer123(camActive);
        }

    }
    void Start3()
    {
        print("Start3");
        /*
        UIconfig.CanvasOnOff_Array[14] = 1;
        UIconfig.CanvasOnOff_Array[20] = 0;
        //setUI_Vis_walk(0);
        //UIconfig.CanvasOnOff_Array[20] = 0;
        */
        Update();
        /*
        //CheckUI_Vis_walk();
        UIconfig.CanvasOnOff_Array[14] = 0;
        UIconfig.CanvasOnOff_Array[20] = 1;
        UIconfig.CanvasOnOff_Array[10] = 1;

        UIconfig.CanvasOnOff_Array[3] = 1;
        */
        SetCamControl123(false);

        
       


    }

    // Update is called once per frame
    void Update()
    {

        if (UIconfig.FrameITUIversion == 1)
        {
            Update3();
        }
    }
    void Update3()
    {
        //CheckUI_Vis_walk();
        CheckIf();

        
        Update2();
        
        CheckUI_Vis();

    }

    IEnumerator slowInput()
    {

        yield return new WaitForSecondsRealtime(waitingBetweenInputs);

        numinputtrigger = 0;
        action_MathM_int = 0;
        action_ToolM_int = 0;
        action_Cancel_PM_int = 0;
        action_modifier_int = 0;
        action_load_int = 0;
        action_save_int = 0;
        action_reset_int = 0;
        action_undo_int = 0;
        action_redo_int = 0;


        yield return null;

    }
    void CheckIf()
    {

        if (UIconfig.InputManagerVersion == 1)
        {
            if (Input.GetButtonDown(cancel_keyBind))
            {

                UIconfig.CanvasOnOff_Array[02] = 1;
                //UIconfig.CanvasOnOff_Array[10] = 0;
                
                return;
            }
            
            if (Input.GetButtonDown(MathM_keyBind))
            {
                if (LockOnly)
                {
                    CamControl_StdAsset.enabled = !CamControl_StdAsset.enabled;
                    SetCursorRenderer123(CamControl_StdAsset.enabled);
                    SetCamControl123(CamControl_StdAsset.enabled);

                }
                else
                {
                    Cursor.visible = !UICanvas.enabled;
                    SetCamControl123(UICanvas.enabled);

                    SetCursorRenderer123(UICanvas.enabled);

                    UICanvas.enabled = !UICanvas.enabled;
                }

            }
        }


        if (UIconfig.InputManagerVersion == 2 && numinputtrigger == 0)
        {
            if (action_Cancel_PM.ReadValue<float>() != 0 || action_Cancel_PM_int !=0)//input_ControlMapping.Actionmap1.Cancel_or_PauseMenue.ReadValue<float>() != 0)
            {
                numinputtrigger++;
                action_Cancel_PM_int = 0;
                StartCoroutine(slowInput());

                UIconfig.CanvasOnOff_Array[02] = 1;
                //UIconfig.CanvasOnOff_Array[10] = 0;
                return;
            }

            if (action_MathM.ReadValue<float>() != 0 || action_MathM_int !=0)//(input_ControlMapping.Actionmap1.MathMode.ReadValue<float>() != 0)
            {
                numinputtrigger++;
                action_MathM_int = 0;
                StartCoroutine(slowInput());
                

                if (LockOnly)
                {
                    CamControl_StdAsset.enabled = !CamControl_StdAsset.enabled;
                    SetCursorRenderer123(CamControl_StdAsset.enabled);
                    SetCamControl123(CamControl_StdAsset.enabled);

                }
                else
                {
                    Cursor.visible = !UICanvas.enabled;
                    SetCamControl123(UICanvas.enabled);

                    SetCursorRenderer123(UICanvas.enabled);

                    UICanvas.enabled = !UICanvas.enabled;
                }

            }
        }


       
        


    }


    void CheckUI_Vis()
    {
        GadgetCanBeUsed = true;
    }



    void Update2() {
        if (UIconfig.InputManagerVersion == 1)
        {
            if (Input.GetButton(modifier))
            {
                if (Input.GetButtonDown(modundo))
                    StageStatic.stage.factState.undo();

                else if (Input.GetButtonDown(modredo))
                    StageStatic.stage.factState.redo();

                else if (Input.GetButtonDown(modreset))
                    StageStatic.stage.factState.softreset();

                else if (Input.GetButtonDown(modsave))
                    StageStatic.stage.push_record();

                else if (Input.GetButtonDown(modload))
                {
                    StageStatic.stage.factState.hardreset();
                    StageStatic.LoadInitStage(StageStatic.stage.name, !StageStatic.stage.use_install_folder);
                }
            }
        }
        if (UIconfig.InputManagerVersion == 2)
        {
            if (action_modifier.ReadValue<float>() != 0 || action_modifier_int!=0)//input_ControlMapping.Actionmap1.Modifier.ReadValue<float>() != 0)
            {
                action_modifier_int = 0;
                if (numinputtrigger == 0 && (action_undo.ReadValue<float>() != 0 || action_undo_int!=0))//input_ControlMapping.Actionmap1.Undo.ReadValue<float>() != 0 )
                {
                    StageStatic.stage.factState.undo();
                    numinputtrigger++;
                    action_undo_int = 0;
                    StartCoroutine(slowInput());
                }
                else if (numinputtrigger == 0 && (action_redo.ReadValue<float>() != 0 || action_redo_int != 0))//input_ControlMapping.Actionmap1.Redo.ReadValue<float>() != 0 )
                {
                    StageStatic.stage.factState.redo();
                    numinputtrigger++;
                    action_redo_int = 0;
                    StartCoroutine(slowInput());
                }
                else if (numinputtrigger == 0 && (action_reset.ReadValue<float>() != 0 || action_reset_int != 0))//input_ControlMapping.Actionmap1.Reset.ReadValue<float>() != 0 )
                {
                    StageStatic.stage.factState.softreset();
                    numinputtrigger++;
                    action_reset_int = 0;
                    StartCoroutine(slowInput());
                }
                else if (numinputtrigger < 0 && (action_save.ReadValue<float>() != 0 || action_save_int != 0))//input_ControlMapping.Actionmap1.Save.ReadValue<float>() != 0 ) 
                {
                    StageStatic.stage.push_record();
                    numinputtrigger = numinputtrigger + 10;
                    action_save_int = 0;
                    StartCoroutine(slowInput());
                }
                else if (numinputtrigger == 0 && (action_load.ReadValue<float>() != 0 || action_load_int != 0))//input_ControlMapping.Actionmap1.Load.ReadValue<float>() != 0 )
                {
                    StageStatic.stage.factState.hardreset();
                    StageStatic.LoadInitStage(StageStatic.stage.name, !StageStatic.stage.use_install_folder);
                    numinputtrigger++;
                    action_load_int = 0;
                    StartCoroutine(slowInput());
                }
            }
        }

        /*
        //Todo before capturing: Make directories "UFrameIT-Screenshots/Unity_ScreenCapture" in project folder
        else if (Input.GetKeyDown(ScreenshotKey)) {
            ScreenCapture.CaptureScreenshot("UFrameIT-Screenshots\\Unity_ScreenCapture\\Capture.png");
        }
        */
    }

    private void SetCursorRenderer123(bool opt)
    {
        CursorRenderer.enabled = opt;

        //multiple Cursor result in conflicts
        /*
        switch (whichCursor)
        //switch (UIconfig.GameplayMode)
        {
            case 0:
                CursorRenderer_FirstP_oldInpOrig.enabled = opt;
                break;

            case 5:
                CursorRenderer_FirstP.enabled = opt;
                break;
            case 6:
                CursorRenderer_FirstP_oldInpOrig.enabled = opt;
                break;
            default:
                CursorRenderer_FirstP.enabled = opt;
                break;
                
        }
        */
    }
    private void SetCamControl123(bool opt)
    {
        CamControl_StdAsset.enabled = opt;
        CamControl_ScriptChar.enabled = opt;
    }

    
    
    
    public void SetCancelOrPauseMenue()
    {
        action_Cancel_PM_int = 1;
    }
    public void SetMathM()
    {
        action_MathM_int = 1;
    }
    public void SetToolM()
    {
        action_ToolM_int = 1;
}
    public void SetModifier()
    {
        action_modifier_int = 1;
    }
    public void SetMLoad()
    {
        action_load_int = 1;
    }
    public void SetMSave()
    {
        action_save_int = 1;
    }
    public void SetMReset()
    {
        action_reset_int = 1;
    }
    public void SetMUndo()
    {
        action_undo_int = 1;
    }
    public void SetMRedo()
    {
        action_redo_int = 1;
    }



}
