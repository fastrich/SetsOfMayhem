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



public class MainPlayerMode_Ctrl : MonoBehaviour
{
    //public TaskCharakterAnimation NPC1;

    //public int myUI_ID;
    public GameObject myself_GObj;
    //public GameObject parent_GObj;
    //public int backUI_ID;
    //public int optionsUI_ID;
    //public int failedUI_ID;
    public GameObject FirstPerson_GObj;
    public GameObject FirstPersonOldInpOrig_GObj;
    public GameObject ThirdPerson_Shoulder_GObj;
    public GameObject ThirdPerson_dampedCam_GObj;
    public GameObject ThirdPerson_manualCam_GObj;
    public GameObject Escaperoom_GObj;
    public GameObject Sidescroller_GObj;
    public GameObject Moorhuhn_GObj;
    public GameObject NoInput_GObj;
    //public GameObject Position_ofActivePlayer;
    public GameObject Camera_ofActivePlayer;

    private int GpMode_before = -99;




    void Start()
    {
        
        Update2(); 
        
        
    }

    private void Update()
    {
        if (InputDisable)
        {
            if (GpMode_before!= -100)
            {
                //Camera_ofActivePlayer.SetActive(false);
                //Camera_ofActivePlayer.SetActive(true);
                ClearUIC();
                GpMode_before = -100;
                NoInput_GObj.SetActive(true);
            }
        }
        if (!InputDisable && GpMode_before != UIconfig.GameplayMode)
        {
            Update2();
            //print(UIconfig.GameplayMode);

        }

    }

    private void Update2(){

        
        //Todo Eventbased
        ClearUIC();
        print("Active_Gameplaymode: "+ UIconfig.GameplayMode);
        switch (UIconfig.GameplayMode)
        {
            case 0:
                //otherSidescrolle
                break;
            case 1:
                Moorhuhn_GObj.SetActive(true);
                break;
            case 2:
                Escaperoom_GObj.SetActive(true);
                break;
            case 3:
                ThirdPerson_manualCam_GObj.SetActive(true);
                UIconfig.interactingRangeMode = InteractingRangeMode.fromObserverView;
                break;
            case 4:
                ThirdPerson_dampedCam_GObj.SetActive(true);
                UIconfig.interactingRangeMode = InteractingRangeMode.fromObserverView;
                break;
            case 5:
                FirstPerson_GObj.SetActive(true);
                //NPC1.setPlayer(FirstPerson_GObj.);
                break;
            case 6:
                FirstPersonOldInpOrig_GObj.SetActive(true);
                break;
            case 7:
                ThirdPerson_Shoulder_GObj.SetActive(true);
                UIconfig.interactingRangeMode = InteractingRangeMode.fromObserverView;
                break;
            case 8:
                Sidescroller_GObj.SetActive(true);
                break;

            default:
                
                break;
        }
        GpMode_before = UIconfig.GameplayMode;

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