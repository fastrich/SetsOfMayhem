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


public class GraphicOptionsMenue_mobile : MonoBehaviour
{

    public GameObject cursorScaler_Slider;
    public GameObject cursorScaler_SliderT;
    




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

    }

    private void Update()
    {
        cursorScaler_Slider.GetComponent<Slider>().value = (float)((UIconfig.cursorSize) / (UIconfig.cursorsize_default * UIconfig.cursorSize_SliderMax));
        mousePointerScaleBttn();
    }



    public void mousePointerScaleBttn()
    {
        UIconfig.cursorSize = cursorScaler_Slider.GetComponent<Slider>().value * UIconfig.cursorSize_SliderMax * UIconfig.cursorsize_default;
        double zwischenRechn = 100 * (UIconfig.cursorSize) / (UIconfig.cursorsize_default);
        cursorScaler_SliderT.GetComponent<Text>().text = "Size of MouseCursor is " + (int)(zwischenRechn) + "%";
        setMouse();
    }

    public void setMouse()
    {
        updateMouseCursor.setMouse();
        
    }

}