using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; //andr
using UnityEngine.SceneManagement;
using System.IO; //
using UnityEngine.Video;//streaming
using UnityEngine.Networking;

using static UIconfig;
using static StreamingAssetLoader;
using static CommunicationEvents;

public class SystemOptionsMenue_mobile : MonoBehaviour
{

    public GameObject AutoOSreq_ButtonT;
    public GameObject AutoOSreq_ButtonUT;

    public GameObject Opsys_ButtonT;
    public GameObject Opsys_ButtonUT;

    public GameObject FrameITUI_ButtonT;
    public GameObject FrameITUI_ButtonUT;

    public GameObject AutoSettingsAdap_Button;
    public GameObject AutoSettingsAdap_ButtonUT;



    /*
    public GameObject TAV_Slider;
    public GameObject TAvisibilityT;
    */

    private Color colChangeable = new Color(1f, 1f, 1f, 0.5f);
    private Color colChangeable2 = new Color(1f, 1f, 1f, 0.5f);

    //public GameObject TouchModeButton;


    //public GameObject back_GObj;

    void Start()
    {
        UpdateUI_6();
    }

    private void Update()
    {
        UpdateUI_6();
    }

    void UpdateUI_6()
    {
        switch (CommunicationEvents.autoOSrecognition)
        {
            case false:

                AutoOSreq_ButtonT.GetComponent<Text>().text = "Operating System Recognition: OFF";
                AutoOSreq_ButtonUT.GetComponent<Text>().text = "Press for activating";
                break;

            case true:

                AutoOSreq_ButtonT.GetComponent<Text>().text = "Operating System Recognition: ON";
                AutoOSreq_ButtonUT.GetComponent<Text>().text = "Press for deactivating";
                break;

        }

       switch (UIconfig.autoSettingsAdaption)
       {
            case false:

                //GameObject.Find("TextSlotTOO").GetComponent<Text>().text = "Touch controls OFF";
                AutoSettingsAdap_Button.GetComponent<Text>().text = "Settings Adaption: OFF";
                AutoSettingsAdap_ButtonUT.GetComponentInChildren<Text>().text = "Press for activating";
                break;

            case true:

                AutoSettingsAdap_Button.GetComponent<Text>().text = "Settings Adaption: ON";
                AutoSettingsAdap_ButtonUT.GetComponentInChildren<Text>().text = "Press for deactivating";
                break; 


       }
 




        switch (CommunicationEvents.Opsys)
        {
            case OperationSystem.Windows:

                Opsys_ButtonT.GetComponent<Text>().text = "Windows optimized";
                Opsys_ButtonUT.GetComponent<Text>().text = "Press for changing optimzation";
                setMouse();
                break;

            case OperationSystem.Android:


                Opsys_ButtonT.GetComponent<Text>().text = "Android optimized: No mouse";
                Opsys_ButtonUT.GetComponent<Text>().text = "Press for changing optimzation";
                setMouse();
                break;

            default:

                Opsys_ButtonT.GetComponent<Text>().text = "Not optimized";
                Opsys_ButtonUT.GetComponent<Text>().text = "Press for changing optimzation";
                break;
        }
        switch (UIconfig.FrameITUIversion)
        {
            case 1:

                FrameITUI_ButtonT.GetComponent<Text>().text = "FrameITUI";
                FrameITUI_ButtonUT.GetComponent<Text>().text = "Press for changing UI";
                break;

            case 2:


                FrameITUI_ButtonT.GetComponent<Text>().text = "FrameITUI_mobile: Touchscreen";
                FrameITUI_ButtonUT.GetComponent<Text>().text = "Press for changing UI";
                break;

            default:

                FrameITUI_ButtonT.GetComponent<Text>().text = "Not optimized";
                FrameITUI_ButtonUT.GetComponent<Text>().text = "Press for changing UI";
                break;
        }
        /*
        TAV_Slider.GetComponent<Slider>().value = UIconfig.TAvisibility;
        TAvisibilityT.GetComponent<Text>().text = "Touch area visibility " + (int)(100 * UIconfig.TAvisibility) + "%";
        */
        //updateUIpreview();
    }



    public void ChangeAutoOSrecognition()
    {
        switch (CommunicationEvents.autoOSrecognition)
        {
            case false:
                CommunicationEvents.autoOSrecognition = true;
                AutoOSreq_ButtonT.GetComponent<Text>().text = "Operating System Recognition: ON";
                AutoOSreq_ButtonUT.GetComponentInChildren<Text>().text = "Press for deactivating";
                break;

            case true:
                CommunicationEvents.autoOSrecognition = false;
                //GameObject.Find("TextSlotTOO").GetComponent<Text>().text = "Touch controls OFF";
                AutoOSreq_ButtonT.GetComponent<Text>().text = "Operating System Recognition: OFF";
                AutoOSreq_ButtonUT.GetComponentInChildren<Text>().text = "Press for activating";
                break;


        }
        //updateUIpreview();
        NetworkJSON_Save();
    }

    public void ChangeAutoSettingsAdaption()
    {
        switch (UIconfig.autoSettingsAdaption)
        {
            case false:
                UIconfig.autoSettingsAdaption = true;
                AutoSettingsAdap_Button.GetComponent<Text>().text = "Settings Adaption: ON";
                AutoSettingsAdap_ButtonUT.GetComponentInChildren<Text>().text = "Press for deactivating";
                break;

            case true:
                UIconfig.autoSettingsAdaption = false;
                //GameObject.Find("TextSlotTOO").GetComponent<Text>().text = "Touch controls OFF";
                AutoSettingsAdap_Button.GetComponent<Text>().text = "Settings Adaption: OFF";
                AutoSettingsAdap_ButtonUT.GetComponentInChildren<Text>().text = "Press for activating";
                break;


        }
        //updateUIpreview();
        NetworkJSON_Save();
    }



    public void ChangeOpsysModes()
    {
        switch (CommunicationEvents.Opsys)
        {
            case OperationSystem.Windows:
                CommunicationEvents.Opsys = OperationSystem.Android;
                Opsys_ButtonT.GetComponent<Text>().text = "Android optimized: No Mouse";
                Opsys_ButtonUT.GetComponentInChildren<Text>().text = "Press for changing optimzation";

                break;

            case OperationSystem.Android:
                CommunicationEvents.Opsys = OperationSystem.Windows;
                Opsys_ButtonT.GetComponent<Text>().text = "Windows optimized";
                Opsys_ButtonUT.GetComponentInChildren<Text>().text = "Press for changing optimzation";
                if (UIconfig.MouseKeepingInWindow == true)
                {
                    Cursor.lockState = CursorLockMode.Confined;
                }
                else
                {
                    Cursor.lockState = CursorLockMode.None;
                }

                break;

            default:
                CommunicationEvents.Opsys = OperationSystem.Android;
                Opsys_ButtonT.GetComponent<Text>().text = "Not optimized";
                Opsys_ButtonUT.GetComponentInChildren<Text>().text = "Press for changing optimzation";
                break;

        }
        //updateUIpreview();
        NetworkJSON_Save();
    }

    public void ChangeFrameITUIvers()
    {
        switch (UIconfig.FrameITUIversion)
        {
            case 1:
                UIconfig.FrameITUIversion = 2;
                FrameITUI_ButtonT.GetComponent<Text>().text = "FrameITUI_mobile: Touchscreen";
                FrameITUI_ButtonUT.GetComponent<Text>().text = "Press for changing UI";
                break;
            case 2:
                UIconfig.FrameITUIversion = 1;
                FrameITUI_ButtonT.GetComponent<Text>().text = "FrameITUI";
                FrameITUI_ButtonUT.GetComponent<Text>().text = "Press for changing UI";
                break;

            default:
                UIconfig.FrameITUIversion = 1;
                FrameITUI_ButtonT.GetComponent<Text>().text = "Not optimized";
                FrameITUI_ButtonUT.GetComponent<Text>().text = "Press for changing UI";
                break;

        }
        //updateUIpreview();
        NetworkJSON_Save();
    }

    public void touchAreaVisibilityBttn()
    {
        /*
        UIconfig.TAvisibility = TAV_Slider.GetComponent<Slider>().value;
        TAvisibilityT.GetComponent<Text>().text = "Touch area visibility " + (int)(100 * UIconfig.TAvisibility) + "%";

        //updateUIpreview();
        */
    }


    public void setMouse()
    {
        updateMouseCursor.setMouse();

    }

}
