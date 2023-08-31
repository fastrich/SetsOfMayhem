    using UnityEngine;
using UnityEngine.SceneManagement;
using static CommunicationEvents;
using static UIconfig;
using static Restart;
using System.Collections;
using UnityEngine.InputSystem;

public class HideUI_mobile : MonoBehaviour
{

    //public KeyCode Key = KeyCode.F1;
    //public KeyCode visMouse = KeyCode.LeftControl;
    //public KeyCode ScreenshotKey = KeyCode.F12;

    public string
        modifier,
        modundo,
        modredo,
        modreset,
        modsave,
        modload;
        
    public string toolMode_keyBind;
    public string MathMode_keyBind;
    public string cancel_keyBind;
    public float waitingBetweenInputs = 0.2f;

    public UnityStandardAssets.Characters.FirstPerson.FirstPersonController CamControl_StdAsset;
    public Characters.FirstPerson.FirstPersonController1 CamControl_ScriptChar;
    
    public bool LockOnly = true;
    public MeshRenderer CursorRenderer;
    private double numinputtrigger=0;
    internal Canvas UICanvas;

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
        if (UIconfig.FrameITUIversion == 2) // 1= FrameITUI; 2= FrameITUI_mobil
        {
            Start2();
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
            camActive = false;
            UICanvas.enabled = camActive;
            //CamControl.enabled = false;
            //CursorRenderer.enabled = true;
            SetCamControl123(false);
            SetCursorRenderer123(true);

        }
        //}
        //Start3();
        //CamControl.enabled = true;

        StartCoroutine(slowInput());

    }

    void Start3()
    {
        print("Start3");
        UIconfig.CanvasOnOff_Array[14] = 1;
        UIconfig.CanvasOnOff_Array[20] = 0;
        //setUI_Vis_walk(0);
        //UIconfig.CanvasOnOff_Array[20] = 0;
        Update();
        //CheckUI_Vis_walk();
        UIconfig.CanvasOnOff_Array[14] = 0;
        UIconfig.CanvasOnOff_Array[20] = 1;
        UIconfig.CanvasOnOff_Array[10] = 1;
        
        UIconfig.CanvasOnOff_Array[3] = 1;
        SetCamControl123(false);

        

    }

    // Update is called once per frame
    void Update()
    {
        
        if (UIconfig.FrameITUIversion == 2)
        {
            Update3();
        }
        
        //print("dada" + UIconfig.CanvasOnOff_Array[4]);
    }


    public void HudButton()
    {
        Update3();
    }


    void Update3()
    {
        CheckUI_Vis_walk();
        CheckIf();

        
        Update2();
        
        CheckUI_Vis();
        
    }

    void CheckUI_Vis_walk()
    {

        int uiccm=0;
        switch (UIconfig.controlMode)
        {
            case ControlMode.Keyboard:
                uiccm = 0;
                break;
            case ControlMode.Mobile:
                uiccm = 1;
                break;
            default:    
                uiccm = 0;
                break;
        }

        switch(GameplayMode){
            case 2:
                UIconfig.CanvasOnOff_Array[11] = 0;
                UIconfig.CanvasOnOff_Array[19] = 1;
                break;
            default:
                UIconfig.CanvasOnOff_Array[11] = uiccm;
                UIconfig.CanvasOnOff_Array[12] = uiccm;
                UIconfig.CanvasOnOff_Array[13] = uiccm;
                UIconfig.CanvasOnOff_Array[15] = uiccm;
                UIconfig.CanvasOnOff_Array[17] = uiccm;
                UIconfig.CanvasOnOff_Array[19] = 0;
                UIconfig.CanvasOnOff_Array[18] = uiccm;

                break;


        }

    }
    void setUI_Vis_walk(int a)
    {
        int uiccm = a;
        UIconfig.CanvasOnOff_Array[11] = uiccm;
        UIconfig.CanvasOnOff_Array[12] = uiccm;
        UIconfig.CanvasOnOff_Array[13] = uiccm;
        UIconfig.CanvasOnOff_Array[15] = uiccm;
        UIconfig.CanvasOnOff_Array[17] = uiccm;
        UIconfig.CanvasOnOff_Array[18] = uiccm;
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



        //walking
        if (UIconfig.CanvasOnOff_Array[10] == 1 && UIconfig.CanvasOnOff_Array[20] == 1 && UIconfig.CanvasOnOff_Array[14] == 0)
        {
            if (UIconfig.InputManagerVersion == 1)
            {
                if (Input.GetButtonDown(toolMode_keyBind))
                {
                    UIconfig.CanvasOnOff_Array[14] = 1;
                    UIconfig.CanvasOnOff_Array[20] = 0;
                    return;
                }
                if (Input.GetButtonDown(MathMode_keyBind))
                {

                    UIconfig.CanvasOnOff_Array[16] = 1;
                    UIconfig.CanvasOnOff_Array[20] = 0;
                    return;
                }

                if (Input.GetButtonDown(cancel_keyBind))
                {
                    UIconfig.CanvasOnOff_Array[02] = 1;
                    UIconfig.CanvasOnOff_Array[10] = 0;
                    return;
                }
                return;
            }
            if (UIconfig.InputManagerVersion == 2 && numinputtrigger==0)
            {
                if (action_ToolM.ReadValue<float>()!=0 || action_ToolM_int!=0)//input_ControlMapping.Actionmap1.ToolMode.ReadValue<float>() != 0)
                {
                    UIconfig.CanvasOnOff_Array[14] = 1;
                    UIconfig.CanvasOnOff_Array[20] = 0;
                    numinputtrigger++;
                action_ToolM_int = 0;
                    StartCoroutine(slowInput());
                    return;
                }
                if (action_MathM.ReadValue<float>() != 0 || action_MathM_int!=0)//input_ControlMapping.Actionmap1.MathMode.ReadValue<float>() != 0)
                {

                    UIconfig.CanvasOnOff_Array[16] = 1;
                    UIconfig.CanvasOnOff_Array[20] = 0;
                    numinputtrigger++;
                    action_MathM_int = 0;
                    StartCoroutine(slowInput());
                    return;
                }

                if (action_Cancel_PM.ReadValue<float>() != 0 || action_Cancel_PM_int!=0)//input_ControlMapping.Actionmap1.Cancel_or_PauseMenue.ReadValue<float>() != 0)
                    {
                    UIconfig.CanvasOnOff_Array[02] = 1;
                    UIconfig.CanvasOnOff_Array[10] = 0;
                    numinputtrigger++;
                        action_Cancel_PM_int = 0;
                        StartCoroutine(slowInput());
                    return;
                }
                return;
            }


        }


        //Toolmode
        if (UIconfig.CanvasOnOff_Array[10] == 1 && UIconfig.CanvasOnOff_Array[20] == 0 && UIconfig.CanvasOnOff_Array[14] == 1)
        {
            if (UIconfig.InputManagerVersion == 1)
            {
                if (Input.GetButtonDown(toolMode_keyBind))
                {
                    UIconfig.CanvasOnOff_Array[14] = 0;
                    UIconfig.CanvasOnOff_Array[20] = 1;
                    return;
                }
                if (Input.GetButtonDown(MathMode_keyBind))
                {
                    UIconfig.CanvasOnOff_Array[14] = 0;
                    UIconfig.CanvasOnOff_Array[16] = 1;
                    return;
                }
                if (Input.GetButtonDown(cancel_keyBind))
                {
                    UIconfig.CanvasOnOff_Array[02] = 1;
                    UIconfig.CanvasOnOff_Array[10] = 0;
                    return;
                }
                return;
            }
            if (UIconfig.InputManagerVersion == 2 && numinputtrigger == 0)
            {
                if (action_ToolM.ReadValue<float>() != 0 || action_ToolM_int!=0)//input_ControlMapping.Actionmap1.ToolMode.ReadValue<float>() != 0)
                {
                    UIconfig.CanvasOnOff_Array[14] = 0;
                    UIconfig.CanvasOnOff_Array[20] = 1;
                    numinputtrigger++;
                    action_ToolM_int = 0;
                    StartCoroutine(slowInput());
                    return;
                }
                if (action_MathM.ReadValue<float>() != 0 || action_MathM_int!=0)//input_ControlMapping.Actionmap1.MathMode.ReadValue<float>() != 0)
                {

                    UIconfig.CanvasOnOff_Array[14] = 0;
                    UIconfig.CanvasOnOff_Array[16] = 1;
                    numinputtrigger++;
                    action_MathM_int = 0;
                    StartCoroutine(slowInput());
                    return;
                }

                if (action_Cancel_PM.ReadValue<float>() != 0 || action_Cancel_PM_int!=0)//input_ControlMapping.Actionmap1.Cancel_or_PauseMenue.ReadValue<float>() != 0)
                {
                    UIconfig.CanvasOnOff_Array[02] = 1;
                    UIconfig.CanvasOnOff_Array[10] = 0;
                    numinputtrigger++;
                    action_Cancel_PM_int = 0;
                    StartCoroutine(slowInput());
                    return;
                }
                return;
            }
        }
        //PauseMenue

        //MathMenue
        if (UIconfig.InputManagerVersion == 1)
        {
                if (Input.GetButtonDown(MathMode_keyBind))
                {

                    UIconfig.CanvasOnOff_Array[16] = 0;
                    UIconfig.CanvasOnOff_Array[20] = 1;
                    return;
                }
                if (Input.GetButtonDown(cancel_keyBind))
                {

                    UIconfig.CanvasOnOff_Array[02] = 1;
                    UIconfig.CanvasOnOff_Array[10] = 0;
                    return;
                }
                return;
        }
        if (UIconfig.InputManagerVersion == 2 && numinputtrigger == 0)
        {
                if (action_MathM.ReadValue<float>() != 0f || action_MathM_int!=0)//input_ControlMapping.Actionmap1.MathMode.ReadValue<float>() != 0)
                {

                    UIconfig.CanvasOnOff_Array[16] = 0;
                    UIconfig.CanvasOnOff_Array[20] = 1;
                    numinputtrigger++;
                        action_MathM_int = 0;
                        StartCoroutine(slowInput()); 
                    return;
                }

                if (action_Cancel_PM.ReadValue<float>() != 0f || action_Cancel_PM_int!=0)//input_ControlMapping.Actionmap1.Cancel_or_PauseMenue.ReadValue<float>() != 0)
                {
                    UIconfig.CanvasOnOff_Array[02] = 1;
                    UIconfig.CanvasOnOff_Array[10] = 0;
                    numinputtrigger++;
                        action_Cancel_PM_int = 0;
                    StartCoroutine(slowInput()); 
                    return;
                }
                return;
            

        }
    }







    void CheckUI_Vis() 
    {
        //Toolmode
        if (UIconfig.CanvasOnOff_Array[14] == 1 && UIconfig.CanvasOnOff_Array[10] == 1)
        {
            if (LockOnly)
            {
                CamControl_StdAsset.enabled = !CamControl_StdAsset.enabled;
                SetCursorRenderer123(CamControl_StdAsset.enabled);
                SetCamControl123(CamControl_StdAsset.enabled);
            }
            else
            {
                Cursor.visible = true;
                SetCursorRenderer123(true);
                //SetCamControl123(false); 
                SetCamControl123(true);
                GadgetCanBeUsed = true;

                UICanvas.enabled = false;
            }
            return;

        }

        //Walkingmode
        if (UIconfig.CanvasOnOff_Array[20] == 1 && UIconfig.CanvasOnOff_Array[10] == 1)
        {
            if (LockOnly)
            {
                CamControl_StdAsset.enabled = !CamControl_StdAsset.enabled;
                SetCursorRenderer123(CamControl_StdAsset.enabled);
                SetCamControl123(CamControl_StdAsset.enabled);
            }
            else
            {
                Cursor.visible = false;
                SetCursorRenderer123(false);
                SetCamControl123(true);
                GadgetCanBeUsed = false;

                UICanvas.enabled = false;
            }
            return;

        }
        //Mathmode
        if (UIconfig.CanvasOnOff_Array[16] == 1 && UIconfig.CanvasOnOff_Array[10] == 1)
        {
            if (LockOnly)
            {
                CamControl_StdAsset.enabled = !CamControl_StdAsset.enabled;
                SetCursorRenderer123(CamControl_StdAsset.enabled);
                SetCamControl123(CamControl_StdAsset.enabled);
            }
            else
            {
                  
                Cursor.visible = true;
                SetCursorRenderer123(false);
                SetCamControl123(false);
                UICanvas.enabled = true;
                GadgetCanBeUsed = false;


            }
            return;
        }
        //PauseMenue
        if (UIconfig.CanvasOnOff_Array[2] == 1)
        {
            if (LockOnly)
            {
                CamControl_StdAsset.enabled = !CamControl_StdAsset.enabled;
                SetCursorRenderer123(CamControl_StdAsset.enabled);
                SetCamControl123(CamControl_StdAsset.enabled);
            }
            else
            {
                
                Cursor.visible = true;
                SetCursorRenderer123(false);
                SetCamControl123(false);
                UICanvas.enabled = false;
                GadgetCanBeUsed = false;
            }
            return;

        }
        //Startmenue
        if (UIconfig.CanvasOnOff_Array[3] == 1)
        {
            if (LockOnly)
            {
                CamControl_StdAsset.enabled = !CamControl_StdAsset.enabled;
                SetCursorRenderer123(CamControl_StdAsset.enabled);
                SetCamControl123(CamControl_StdAsset.enabled);
            }
            else
            {
                UICanvas.enabled = false;
                Cursor.visible = true;
                SetCursorRenderer123(false);
                SetCamControl123(false);
                GadgetCanBeUsed = false;


            }
            return;

        }
        if (UIconfig.CanvasOnOff_Array[20] != 1 && UIconfig.CanvasOnOff_Array[14] != 1 && UIconfig.CanvasOnOff_Array[16] != 1)
        {
            //print("CheckHideUI_mobile");
            
            if (LockOnly)
            {
                CamControl_StdAsset.enabled = !CamControl_StdAsset.enabled;
                SetCursorRenderer123(CamControl_StdAsset.enabled);
                SetCamControl123(CamControl_StdAsset.enabled);
            }
            else
            {
                UICanvas.enabled = false;
                
                
                CursorRenderer.enabled = false;
                Cursor.visible = true;
                SetCamControl123(false);
                SetCursorRenderer123(false);
                UICanvas.enabled = !UICanvas.enabled;


            }
            return;
        }
    }


   
    void Update2()
    {
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
            if (action_modifier.ReadValue<float>() != 0 || action_modifier_int != 0)//input_ControlMapping.Actionmap1.Modifier.ReadValue<float>() != 0)
            {
                action_modifier_int = 0;
                if (numinputtrigger == 0 && (action_undo.ReadValue<float>() != 0 || action_undo_int != 0))//input_ControlMapping.Actionmap1.Undo.ReadValue<float>() != 0 )
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
