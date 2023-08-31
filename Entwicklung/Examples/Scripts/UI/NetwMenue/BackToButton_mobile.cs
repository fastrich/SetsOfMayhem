using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; //andr
using UnityEngine.SceneManagement;
using System.IO; //
using UnityEngine.Video;//streaming
using UnityEngine.Networking;
using static CommunicationEvents;
using static UIconfig;
using static StreamingAssetLoader;


public class BackToButton_mobile : MonoBehaviour
{
    public GameObject backTo_GObj;
    public GameObject parentM_GObj;

    public void goBackButtonOPTM()
    {
        NetworkJSON_Save();

        if (parentM_GObj != null)
            parentM_GObj.SetActiveAllChildren(false);

        backTo_GObj.SetActive(true);
    }
}