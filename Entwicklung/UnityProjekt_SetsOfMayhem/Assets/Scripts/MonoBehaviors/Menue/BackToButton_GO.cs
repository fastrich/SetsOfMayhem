using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; //andr
using UnityEngine.SceneManagement;
using System.IO; //
using UnityEngine.Video;//streaming
using UnityEngine.Networking;
//using static CommunicationEvents;
using static UIconfig;
using static StreamingAssetLoader;
using static algos;
using static medium;
public class BackToButton_GO : MonoBehaviour
{
    public GameObject backTo_GObj;
    public GameObject parentM_GObj;

    public void goBackButtonOPTM()
    {
        //NetworkJSON_Save();
        if (checkParameterForToGo() && !problemInGO_Kat)
        {
            if (parentM_GObj != null) 
            { 
                //parentM_GObj.SetActiveAllChildren(false);
                for (int i = 0; i < parentM_GObj.transform.childCount; i++)
                {
                    parentM_GObj.transform.GetChild(i).gameObject.SetActive(false);

                }
            backTo_GObj.SetActive(true);
             }

        }
        
    }
}