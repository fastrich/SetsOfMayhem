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


public class UI_OnOff_InputModule : MonoBehaviour
{

    public GameObject myself_GObj;
    public int myInputModule_ID;
    //public int default_value;
    private bool cA=true;
    private int before = -99;


    void Start()
    {

    }

    private void Update()
    {
        if (cA && before!= UIconfig.InputManagerVersion)
        {
            if (UIconfig.InputManagerVersion==myInputModule_ID)
            {
                ActivateUIC();
            }
            else
            {
                
                ClearUIC();
 
            }
            before = UIconfig.InputManagerVersion;
        }
    }
    



    /// <summary>
    /// Activates all Pages.
    /// </summary>
    private void ActivateUIC()
    {

        for (int i = 0; i < myself_GObj.transform.childCount; i++)
        {
            myself_GObj.transform.GetChild(i).gameObject.SetActive(true);
        }
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