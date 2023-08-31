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


public class GameObj_OnOff : MonoBehaviour
{
    public GameObject Target_GObj;
    public int myUI_ID;
    public int default_value;


    void Start()
    {
        if (CheckArray())
        {
            if (UIconfig.CanvasOnOff_Array[myUI_ID] != 2 || UIconfig.CanvasOnOff_Array[myUI_ID] != 3) {
                UIconfig.CanvasOnOff_Array[myUI_ID] = default_value;
                //Update();
            }
        }
    }

    private void Update()
    {
        if (CheckArray())
        {
            var activate = UIconfig.CanvasOnOff_Array[myUI_ID] switch
            {
                1 or 3 => true,
                0 or 2 or _ => false,
            };

            Target_GObj.SetActive(activate);
        }
    }
    
    private bool CheckArray() 
        => myUI_ID >= 0 && myUI_ID < UIconfig.CanvasOnOff_Array.Length;
}