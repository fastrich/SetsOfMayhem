using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; //andr
using UnityEngine.SceneManagement;
using System.IO; //
using UnityEngine.Video;//streaming
using UnityEngine.Networking;
//using static CommunicationEvents;
//using static UIconfig;
//using static StreamingAssetLoader;
using static config_parameters;
using static medium;
using static methods;
using static methods_unity;
using TMPro;
using static algos;

public class gameoptions2_main : MonoBehaviour
{


    public GameObject Text_WarnungElemente;
    public GameObject Toogle_gen_SETanz_info;
    public GameObject Toogle_gen_SETanz_info_t;
    public GameObject Toogle_gen_SETanz_info_ut;

    public GameObject SET1_nbrOfCards_Slider;
    public GameObject SET1_nbrOfCards_SliderT;

    public GameObject SETX_onField_Slider;
    public GameObject SETX_onField_SliderT;

    public GameObject nbrOfCardsInField_Slider;
    public GameObject nbrOfCardsInField_SliderT;





    private Color colChangeable = new Color(1f, 1f, 1f, 0.5f);
    private Color colChangeable2 = new Color(1f, 1f, 1f, 0.5f);
    private float transCol;
    private ColorBlock[] tempColB = new ColorBlock[2];



    void Start()
    {
        tempColB[0] = Toogle_gen_SETanz_info.GetComponent<Button>().colors;
        tempColB[0].normalColor = colClear;
        tempColB[0].selectedColor = colClear;
        tempColB[1] = Toogle_gen_SETanz_info.GetComponent<Button>().colors;
        tempColB[1].normalColor = colOptionEnabled;
        tempColB[1].selectedColor = colOptionEnabled;
    }

    private void Update()
    {

        SET1_nbrOfCards_Slider.GetComponent<Slider>().value = (float)(((float)numberOfSelected_soll - (float)numberOfSelected_soll_SLIDER_MIN) / ((float)numberOfSelected_soll_SLIDER_MAX - (float)numberOfSelected_soll_SLIDER_MIN));
        numOfCardsInASET_Bttn();

        SETX_onField_Slider.GetComponent<Slider>().value = (float)(((float)numberOfSelected_soll_gen - (float)numberOfSelected_soll_gen_SLIDER_MIN) / ((float)numberOfSelected_soll_gen_SLIDER_MAX - (float)numberOfSelected_soll_gen_SLIDER_MIN));
        SETX_onField_Bttn();

        nbrOfCardsInField_Slider.GetComponent<Slider>().value = (float)(((float)Game_numberOfCardsOnDeck - (float)Game_numberOfCardsOnDeck_SLIDER_MIN) / ((float)Game_numberOfCardsOnDeck_SLIDER_MAX - (float)Game_numberOfCardsOnDeck_SLIDER_MIN));
        numOfCardsInField_Bttn();

        string nun = "";

        Text_WarnungElemente.GetComponent<TMP_Text>().text = nun;

        string titel1 = "Aufzählung der SETs im Feld";
        string titelUT = "AUS";
        if (Set_InfosAnzeigen_Anzahl==1) { titel1 = "Aufzählung der SETs im Feld"; titelUT = "AN"; } 
        Toogle_gen_SETanz_info_ut.GetComponent<TMP_Text>().text = titelUT;
        Toogle_gen_SETanz_info_t.GetComponent<TMP_Text>().text = titel1;
        Toogle_gen_SETanz_info.GetComponent<Button>().colors = tempColB[Set_InfosAnzeigen_Anzahl];

    }



    public void numOfCardsInASET_Bttn()
    {
        numberOfSelected_soll = (int)(SET1_nbrOfCards_Slider.GetComponent<Slider>().value * ((float)numberOfSelected_soll_SLIDER_MAX - (float)numberOfSelected_soll_SLIDER_MIN) + (float)numberOfSelected_soll_SLIDER_MIN);
        double zwischenRechn = numberOfSelected_soll;//100 * (UIconfig.camRotatingSensitivity); // /(UIconfig.camRotatingSensitivity_default);
        SET1_nbrOfCards_SliderT.GetComponent<TMP_Text>().text = "Karten für ein SET: " + (int)(zwischenRechn) + " ";
    }

    public void numOfCardsInField_Bttn()
    {
        Game_numberOfCardsOnDeck = (int)(nbrOfCardsInField_Slider.GetComponent<Slider>().value * ((float)Game_numberOfCardsOnDeck_SLIDER_MAX - (float)Game_numberOfCardsOnDeck_SLIDER_MIN) + (float)Game_numberOfCardsOnDeck_SLIDER_MIN);
        double zwischenRechn = Game_numberOfCardsOnDeck;//100 * (UIconfig.camRotatingSensitivity); // /(UIconfig.camRotatingSensitivity_default);
        nbrOfCardsInField_SliderT.GetComponent<TMP_Text>().text = "Karten auf dem Feld: " + (int)(zwischenRechn) + " ";
    }
    public void SETX_onField_Bttn()
    {
        numberOfSelected_soll_gen = (int)(SETX_onField_Slider.GetComponent<Slider>().value * ((float)numberOfSelected_soll_gen_SLIDER_MAX - (float)numberOfSelected_soll_gen_SLIDER_MIN) + (float)numberOfSelected_soll_gen_SLIDER_MIN);
        double zwischenRechn = numberOfSelected_soll_gen;//100 * (UIconfig.camRotatingSensitivity); // /(UIconfig.camRotatingSensitivity_default);
        SETX_onField_SliderT.GetComponent<TMP_Text>().text = "SETs auf dem Feld: " + (int)(zwischenRechn) + " ";
    }



    public void OnKlickBttn_InfoShowSETAnz() { Set_InfosAnzeigen_Anzahl = 1 - Set_InfosAnzeigen_Anzahl;}



}
