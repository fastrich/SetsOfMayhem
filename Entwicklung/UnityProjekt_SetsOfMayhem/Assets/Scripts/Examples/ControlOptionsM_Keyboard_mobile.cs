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


public class ControlOptionsM_Keyboard_mobile : MonoBehaviour
{

    public GameObject MCiW_ButtonT;
    public GameObject MCiW_ButtonUT;



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
        switch (UIconfig.MouseKeepingInWindow)
        {
            case true:

                MCiW_ButtonT.GetComponent<Text>().text = "Mouse is hold in Window";
                MCiW_ButtonUT.GetComponent<Text>().text = "Press to change Mode ";
                Cursor.lockState = CursorLockMode.Confined;
                break;

            case false:

                MCiW_ButtonT.GetComponent<Text>().text = "Mouse can leave Window";
                MCiW_ButtonUT.GetComponent<Text>().text = "Press to change Mode";
                Cursor.lockState = CursorLockMode.None;
                break;

        }


    }

    public void ChangeMouseCaptureInWindow()
    {
        switch (UIconfig.MouseKeepingInWindow)
        {
            case false:
                UIconfig.MouseKeepingInWindow = true;
                Cursor.lockState = CursorLockMode.Confined;
                break;



            case true:
                UIconfig.MouseKeepingInWindow = false;
                Cursor.lockState = CursorLockMode.None;
                break;

        }
        
        //updateUIpreview();
        NetworkJSON_Save();
    }

    
}








