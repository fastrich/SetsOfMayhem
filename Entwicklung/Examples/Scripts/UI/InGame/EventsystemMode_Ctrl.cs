using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; 
using UnityEngine.SceneManagement;
using System.IO; 
using UnityEngine.Video;//streaming
using UnityEngine.Networking;
//using static StreamingAssetLoader;
//using static CheckServer;
//using static CommunicationEvents;
using static UIconfig;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem.UI;

public class EventsystemMode_Ctrl : MonoBehaviour
{

    //public GameObject myself_GObj;
    //public GameObject StandaloneInputModule_GObj;
    //public GameObject InputSystem_GObj;
    public StandaloneInputModule StandaloneInputModule_script;
    public InputSystemUIInputModule InputSystem_script;

    private int GpMode_before=-99;




    void Start()
    {
        
        Update2(); 
        
        
    }

    private void Update()
    {
        /* 
        Update3();
        */
    }
    private void Update3()
    {
        if (GpMode_before != UIconfig.InputManagerVersion)
        {
            Update2();
            EventSystemModule = InputManagerVersion;

        }
        if (GpMode_before != UIconfig.EventSystemModule)
        {
            Update2();

        }
    }


    private void Update2(){

        
        //Todo Eventbased
        //ClearUIC();
        switch (UIconfig.EventSystemModule)
        {
            case 0:
                break;
            case 1:

                //StandaloneInputModule_script.ActivateModule();
                //InputSystem_script.DeactivateModule();

                gameObject.GetComponent<StandaloneInputModule>().enabled=true;
                gameObject.GetComponent<InputSystemUIInputModule>().enabled = false;


                break;
            case 2:
                //InputSystem_script.ActivateModule();
                //StandaloneInputModule_script.DeactivateModule();

                gameObject.GetComponent<StandaloneInputModule>().enabled = false;
                gameObject.GetComponent<InputSystemUIInputModule>().enabled = true;
                break;
            case 3:

                gameObject.GetComponent<StandaloneInputModule>().enabled = false;
                gameObject.GetComponent<InputSystemUIInputModule>().enabled = true;
                break;

            default:
                gameObject.GetComponent<StandaloneInputModule>().enabled = false;
                gameObject.GetComponent<InputSystemUIInputModule>().enabled = true;
                break;
        }
        GpMode_before = UIconfig.GameplayMode;

    }

   
    /*
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
    */


    }