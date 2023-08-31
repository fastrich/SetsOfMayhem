using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; //andr
using UnityEngine.SceneManagement;
using System.IO; //
using UnityEngine.Video;//streaming
using UnityEngine.Networking;
//using static StreamingAssetLoader;
//using static CheckServer;
//using static CommunicationEvents;
using static UIconfig;
using static StreamingAssetLoader;
using static CommunicationEvents;
using System;

public class StartMenue_mobile : MonoBehaviour
{


    //public int myUI_ID;
    public GameObject myself_GObj;
    //public GameObject parent_GObj;
    //public int backUI_ID;
    //public int optionsUI_ID;
    //public int failedUI_ID;
    public GameObject child1_GObj;

    public Text GObj_text;


    private void Awake()
    {
        ScreenOptimization();
        //GObj_text.text = "1: "+  Application.streamingAssetsPath + " " + Application.persistentDataPath + " " + Application.dataPath;
        toChild1();
        if (checkOperationSystemAlreadyDone == false || checkOperationSystemAlreadyDone_check == false)
        {
            start2_CheckOS_CheckConfig();
            checkOperationSystemAlreadyDone = true;
        }
        GObj_text.text = Application.platform + " -> " + CommunicationEvents.Opsys + "";
        //GObj_text.text = "2: " + Application.streamingAssetsPath + " " + Application.persistentDataPath + " " + Application.dataPath;
        //Debug.Log(Application.streamingAssetsPath);
        CheckServerA[1] = 1;
        CheckServerA[2] = 1;
        CheckServerA[3] = 1;

    }

    void Start()
    {
       
    }

    void start2_CheckOS_CheckConfig()
    {
        //Try to find existing Config:
        int configExists = 0;
        //Debug.Log("Load Streaming Asset");
        try { NetworkJSON_Load_0(); configExists=1; }
        catch (Exception e)
        {
            Debug.Log(e);   
        }
        //Debug.Log("Load Streaming Asset finished");
        //Debug.Log(checkPersistentDataPath());
        try
        {
            NetworkJSON_Load();
            configExists = 2;
        }
        catch(Exception e)
        {
            Debug.Log(e);
        }

        
        if (configExists == 0)
        {
            Debug.Log(configExists + "No Config found");
            checkOS2();
            try
            {
                ResetPlayerConfig();
                NetworkJSON_Load();
                configExists = 3;
            }
            catch (Exception e)
            {
                Debug.Log(e);
            }
        }
        else
        {
            //Debug.Log(configExists + "Config found");
            checkOS();
        }
        //Entpacken
        if (!checkPersistentDataPath() || configExists<1)
        {
            //Debug.Log("initialReset_PDP");
            ResetPersistentDataPath();
            //Debug.Log("Reset_PDP_finished");
            NetworkJSON_Load();
            checkOS();
            changeSettingsToOS();
            NetworkJSON_Save();
        }
        //Save CheckOS
        NetworkJSON_Save();
        
        //Debug.Log("StartAdaption");
        if (autoSettingsAdaption)
        {
            changeSettingsToOS();
            NetworkJSON_Save();
        }
        //Debug.Log("EndAdaption");
        NetworkJSON_Load();
        
        if (!checkDataPath() || true)
        {
            ResetDataPath();
            //Debug.Log("DataPath new finished");
        };
        //Debug.Log("DataPath finished");
        setMouse();

        if (UIconfig.MouseKeepingInWindow == true)
        {
            Cursor.lockState = CursorLockMode.Confined;
        }
        else
        {
            Cursor.lockState = CursorLockMode.None;
        }
        
    }

    private void Update()
    {
        
    }

    void checkOS()
    {
        if (CommunicationEvents.autoOSrecognition == true) {
            checkOS2();
        }
        else
        {
            //CommunicationEvents.Opsys = CommunicationEvents.Opsys_Default;
        }
        if(Opsys == OperationSystem.Android)
        {
            ServerAutoStart = false;
        }
    }

    void checkOS2()
    {
        //https://docs.unity3d.com/ScriptReference/RuntimePlatform.html
        

        if (Application.platform == RuntimePlatform.WindowsPlayer)
        {
            
            //Debug.Log("Windows OS detected");
            CommunicationEvents.Opsys = OperationSystem.Windows;
            
            return;
        }
        if (Application.platform == RuntimePlatform.Android)
        {
            //Debug.Log("Android OS detected");
            CommunicationEvents.Opsys = OperationSystem.Android;
            
            return;
        }
        if (Application.platform == RuntimePlatform.WindowsEditor)
        {
            CommunicationEvents.Opsys = OperationSystem.Windows;
            return;
        }
        Debug.Log("Detecting OS: " + Application.platform + " -> " + CommunicationEvents.Opsys);

        //Default:
        //CommunicationEvents.Opsys = CommunicationEvents.Opsys_Default;
        return;
    }


    void changeSettingsToOS()
    {
        switch (Opsys)
        {
            case OperationSystem.Windows:
                UIconfig.controlMode = ControlMode.Keyboard;
                UIconfig.FrameITUIversion = 1;
                break;
            case OperationSystem.Android:
                UIconfig.controlMode = ControlMode.Mobile;
                UIconfig.FrameITUIversion = 2;
                break;
            default:
                break;
        }
    }


public void setMouse()
    {
        updateMouseCursor.setMouse();          
    }


    void ScreenOptimization()
    {
        Screen.orientation = ScreenOrientation.LandscapeLeft;
        
        UIconfig.screHeight = Screen.height;
        UIconfig.screWidth = Screen.width;
        CommunicationEvents.lastIP = CommunicationEvents.selecIP;


        /* //ScreenMatchMode.MatchWidthOrHeight:
            // If one axis has twice resolution and the other has half, it should even out if widthOrHeight value is at 0.5.
            // In normal space the average would be (0.5 + 2) / 2 = 1.25
            // In logarithmic space the average is (-1 + 1) / 2 = 0
            float scaleFactor = Mathf.Max(screenSize.x / m_ReferenceResolution.x, screenSize.y / m_ReferenceResolution.y);

             float logWidth = Mathf.Log(screenSize.x / m_ReferenceResolution.x, kLogBase);
             float logHeight = Mathf.Log(screenSize.y / m_ReferenceResolution.y, kLogBase);
             float logWeightedAverage = Mathf.Lerp(logWidth, logHeight, m_MatchWidthOrHeight);
             scaleFactor = Mathf.Pow(kLogBase, logWeightedAverage);


            //GameObject.Find("ASMenue").GetComponent;
            //Camera;


            //mainInputField.text = "Enter IP Here...";

        */

        UnityEngine.UI.CanvasScaler c = myself_GObj.GetComponent<UnityEngine.UI.CanvasScaler>();
        c.uiScaleMode = UnityEngine.UI.CanvasScaler.ScaleMode.ScaleWithScreenSize;

        UIconfig.refWidth = (int)Mathf.Round(c.referenceResolution[0]);
        UIconfig.refHeight = (int)Mathf.Round(c.referenceResolution[1]);
        //CommunicationEvents.scaleMatch = 0.5f;

        /*
            //float kLogBase=10;
            //CommunicationEvents.scaleMatch = Mathf.Max(CommunicationEvents.screWidth / CommunicationEvents.refWidth, CommunicationEvents.screHeight / CommunicationEvents.refHeight);
            //float logWidth = Mathf.Log(CommunicationEvents.screWidth / CommunicationEvents.refWidth, kLogBase);
            //float logHeight = Mathf.Log(CommunicationEvents.screHeight / CommunicationEvents.refHeight, kLogBase);
            //float logWeightedAverage = Mathf.Lerp(logWidth, logHeight, 0.5f);
            //CommunicationEvents.scaleMatch = Mathf.Pow(kLogBase, logWeightedAverage);

            //c.matchWidthOrHeight = CommunicationEvents.scaleMatch;
         /*
                RectTransform rt = GetComponent<RectTransform>();

                Vector3 screenSize = Camera.main.ViewportToWorldPoint(Vector3.up + Vector3.right);

                screenSize *= 02;

                float sizeY = screenSize.y / rt.rect.height;
                float sizeX = screenSize.x / rt.rect.width;

                rt.localScale = new Vector3(sizeX, sizeY, 1);
        */
        

    }


    public void toChild1()
    {
        ClearUIC();
        UIconfig.Andr_Start_menue_counter = 1;
        child1_GObj.SetActive(true); ;


    }
    
    /// <summary>
    /// Deactivates all Pages.
    /// </summary>
    private void ClearUIC()
    {
        
        for (int i = 0; i < myself_GObj.transform.childCount; i++)
        {
            myself_GObj.transform.GetChild(i).gameObject.SetActive(false);
        }
    }




}


