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


public class GameplayOptionsMenue_mobile : MonoBehaviour
{

    public GameObject cllscaleAll_Slider;
    public GameObject cllscaleAll_SliderT;

    public GameObject CamSens_Slider;
    public GameObject CamSens_SliderT;





    private Color colChangeable = new Color(1f, 1f, 1f, 0.5f);
    private Color colChangeable2 = new Color(1f, 1f, 1f, 0.5f);



    void Start()
    {
        
    }

    private void Update()
    {
        cllscaleAll_Slider.GetComponent<Slider>().value = (float)(UIconfig.colliderScale_all / (UIconfig.colliderScale_all_default * UIconfig.colliderScale_all_SliderMax));
        ScaleColliderAllBttn();

       

        CamSens_Slider.GetComponent<Slider>().value = (float)((UIconfig.camRotatingSensitivity) / (UIconfig.camRotatingSensitivity_default * UIconfig.camRotatingSensitivity_sliderMax));
        CamSensitivityBttn();

    }



    



    public void ScaleColliderAllBttn()
    {
        UIconfig.colliderScale_all = cllscaleAll_Slider.GetComponent<Slider>().value* UIconfig.colliderScale_all_SliderMax* UIconfig.colliderScale_all_default;
        cllscaleAll_SliderT.GetComponent<Text>().text = "Scale of Hitbox for MouseClicks is " + (int)(100 * UIconfig.colliderScale_all/UIconfig.colliderScale_all_default) + "%";

        //updateUIpreview();

    }
    

    public void CamSensitivityBttn()
    {
        UIconfig.camRotatingSensitivity = CamSens_Slider.GetComponent<Slider>().value * UIconfig.camRotatingSensitivity_sliderMax * UIconfig.camRotatingSensitivity_default;
        double zwischenRechn = 100 * (UIconfig.camRotatingSensitivity); // /(UIconfig.camRotatingSensitivity_default);
        CamSens_SliderT.GetComponent<Text>().text = "Sensitivity of Camera is " + (int)(zwischenRechn) + "%";
        
    }

   


    

}
