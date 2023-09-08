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

public class gameoptions_main : MonoBehaviour
{

    //public GameObject kindsOfOpt_Slider;
    public GameObject kindsOfOpt_SliderT;
    public GameObject Text_WarnungElemente;
    public GameObject Toogle_opt01;
    public GameObject Toogle_opt01_t;
    public GameObject Toogle_opt01_ut;
    public GameObject Toogle_opt02;
    public GameObject Toogle_opt02_t;
    public GameObject Toogle_opt02_ut;
    public GameObject Toogle_opt03;
    public GameObject Toogle_opt03_t;
    public GameObject Toogle_opt03_ut;
    public GameObject Toogle_opt04;
    public GameObject Toogle_opt04_t;
    public GameObject Toogle_opt04_ut;
    public GameObject Toogle_opt05;
    public GameObject Toogle_opt05_t;
    public GameObject Toogle_opt05_ut;
    public GameObject Toogle_opt06;
    public GameObject Toogle_opt06_t;
    public GameObject Toogle_opt06_ut;
    public GameObject Toogle_opt07;
    public GameObject Toogle_opt07_t;
    public GameObject Toogle_opt07_ut;
    public GameObject Toogle_opt08;
    public GameObject Toogle_opt08_t;
    public GameObject Toogle_opt08_ut;
    public GameObject Toogle_opt09;
    public GameObject Toogle_opt09_t;
    public GameObject Toogle_opt09_ut;

    public GameObject Toogle_gen_rotation;
    public GameObject Toogle_gen_rotation_ut;
    public GameObject Toogle_gen_childs;
    public GameObject Toogle_gen_childs_ut;
    public GameObject Toogle_gen_colour;
    public GameObject Toogle_gen_colour_ut;
    //public GameObject Toogle_gen_01;


    public GameObject numberOf1Opt_Slider;
    public GameObject numberOf1Opt_SliderT;







    private Color colChangeable = new Color(1f, 1f, 1f, 0.5f);
    private Color colChangeable2 = new Color(1f, 1f, 1f, 0.5f);
    private float transCol;
    private ColorBlock[] tempColB = new ColorBlock[2];



    void Start()
    {
        tempColB[0]= Toogle_gen_colour.GetComponent<Button>().colors;
        tempColB[0].normalColor = colClear;
        tempColB[0].selectedColor = colClear;
        tempColB[1] = Toogle_gen_colour.GetComponent<Button>().colors;
        tempColB[1].normalColor = colOptionEnabled;
        tempColB[1].selectedColor = colOptionEnabled;
        getToSetKatNum();

    }

    private void Update()
    {

        //kindsOfOpt_Slider.GetComponent<Slider>().value = (float)(((float)Max_Anzahl_katProKarte - (float)Max_Anzahl_katProKarte_SLIDER_MIN) / ((float)Max_Anzahl_katProKarte_SLIDER_MAX - (float)Max_Anzahl_katProKarte_SLIDER_MIN)); 
        kindsOfOptions_Bttn();



        numberOf1Opt_Slider.GetComponent<Slider>().value = (float)(((float)numberofUnitsPerKat_max - (float)numberofUnitsPerKat_max_SLIDER_MIN) / ((float)numberofUnitsPerKat_max_SLIDER_MAX - (float)numberofUnitsPerKat_max_SLIDER_MIN));
        numOfUperKat_Bttn();



        Toogle_opt01.GetComponent<Button>().colors = tempColB[(int)ChoosenKats[0]];
        Toogle_opt02.GetComponent<Button>().colors = tempColB[(int)ChoosenKats[1]];
        Toogle_opt03.GetComponent<Button>().colors = tempColB[(int)ChoosenKats[2]];
        Toogle_opt04.GetComponent<Button>().colors = tempColB[(int)ChoosenKats[3]];
        Toogle_opt05.GetComponent<Button>().colors = tempColB[(int)ChoosenKats[4]];
        Toogle_opt06.GetComponent<Button>().colors = tempColB[(int)ChoosenKats[5]];
        Toogle_opt07.GetComponent<Button>().colors = tempColB[(int)ChoosenKats[6]];
        Toogle_opt08.GetComponent<Button>().colors = tempColB[(int)ChoosenKats[7]];
        Toogle_opt09.GetComponent<Button>().colors = tempColB[(int)ChoosenKats[8]];


        Toogle_gen_colour.GetComponent<Button>().colors = tempColB[(int)ChoosenKats2[0]];
        Toogle_gen_rotation.GetComponent<Button>().colors = tempColB[(int)ChoosenKats2[1]];
        Toogle_gen_childs.GetComponent<Button>().colors = tempColB[(int)ChoosenKats2[2]];

        Toogle_opt01_t.GetComponent<TMP_Text>().text = kategorien_n[0];
        Toogle_opt02_t.GetComponent<TMP_Text>().text = kategorien_n[1];
        Toogle_opt03_t.GetComponent<TMP_Text>().text = kategorien_n[2];
        Toogle_opt04_t.GetComponent<TMP_Text>().text = kategorien_n[3];
        Toogle_opt05_t.GetComponent<TMP_Text>().text = kategorien_n[4];
        Toogle_opt06_t.GetComponent<TMP_Text>().text = kategorien_n[5];
        Toogle_opt07_t.GetComponent<TMP_Text>().text = kategorien_n[6];
        Toogle_opt08_t.GetComponent<TMP_Text>().text = kategorien_n[7];
        Toogle_opt09_t.GetComponent<TMP_Text>().text = kategorien_n[8];



        check_werte_n_n2_length();
        //Debug.Log("checkW " + ArrayToString(werte_n_length));
       

        if (werte_n_length[0] != 0) {Toogle_opt01_ut.GetComponent<TMP_Text>().text = Text_Kat_UT +werte_n_length[0].ToString(); } else { Toogle_opt01_ut.GetComponent<TMP_Text>().text = " "; }
        if (werte_n_length[1] != 0) { Toogle_opt02_ut.GetComponent<TMP_Text>().text = Text_Kat_UT +werte_n_length[1].ToString(); } else { Toogle_opt02_ut.GetComponent<TMP_Text>().text = " "; }
        if (werte_n_length[2] != 0) { Toogle_opt03_ut.GetComponent<TMP_Text>().text = Text_Kat_UT +werte_n_length[2].ToString(); } else { Toogle_opt03_ut.GetComponent<TMP_Text>().text = " "; }
        if (werte_n_length[3] != 0) { Toogle_opt04_ut.GetComponent<TMP_Text>().text = Text_Kat_UT +werte_n_length[3].ToString(); } else { Toogle_opt04_ut.GetComponent<TMP_Text>().text = " "; }
        if (werte_n_length[4] != 0) { Toogle_opt05_ut.GetComponent<TMP_Text>().text = Text_Kat_UT +werte_n_length[4].ToString(); } else { Toogle_opt05_ut.GetComponent<TMP_Text>().text = " "; }
        if (werte_n_length[5] != 0) { Toogle_opt06_ut.GetComponent<TMP_Text>().text = Text_Kat_UT +werte_n_length[5].ToString(); } else { Toogle_opt06_ut.GetComponent<TMP_Text>().text = " "; }
        if (werte_n_length[6] != 0) { Toogle_opt07_ut.GetComponent<TMP_Text>().text = Text_Kat_UT +werte_n_length[6].ToString(); } else { Toogle_opt07_ut.GetComponent<TMP_Text>().text = " "; }
        if (werte_n_length[7] != 0) { Toogle_opt08_ut.GetComponent<TMP_Text>().text = Text_Kat_UT +werte_n_length[7].ToString(); } else { Toogle_opt08_ut.GetComponent<TMP_Text>().text = " "; }
        if (werte_n_length[8] != 0) { Toogle_opt09_ut.GetComponent<TMP_Text>().text = Text_Kat_UT +werte_n_length[8].ToString(); } else { Toogle_opt09_ut.GetComponent<TMP_Text>().text = " "; }


        Toogle_gen_rotation_ut.GetComponent<TMP_Text>().text = Text_Kat_UT + dyn_ausrichtung.Length.ToString();
        Toogle_gen_childs_ut.GetComponent<TMP_Text>().text = Text_Kat_UT + dyn_anzahl.Length.ToString();
        Toogle_gen_colour_ut.GetComponent<TMP_Text>().text= Text_Kat_UT + dyn_farbe.Length.ToString();

        int druber = 0;
        string nun = " ";
        for(int i=0; i < werte_n_length.Length; i++)
        {
            if (ChoosenKats[i]==1 && werte_n_length[i]< numberofUnitsPerKat_max){ druber++; }
            if (ChoosenKats2[0] == 1 && dyn_farbe.Length < numberofUnitsPerKat_max) { druber++; }
            if (ChoosenKats2[1] == 1 && dyn_anzahl.Length < numberofUnitsPerKat_max) { druber++; }
            if (ChoosenKats2[2] == 1 && dyn_ausrichtung.Length < numberofUnitsPerKat_max) { druber++; }


        }
        if (druber > 0) { nun = " Warnung: Nicht genügend Karten für die aktuellen Einstellungen."; }
        if (Max_Anzahl_katProKarte==0) { nun = " Bitte wählen sie Kategorien aus."; }
        Text_WarnungElemente.GetComponent<TMP_Text>().text = nun;
        //getToSetKatNum();
    }




    public void kindsOfOptions_Bttn()
    {
        //Max_Anzahl_katProKarte = (int) (kindsOfOpt_Slider.GetComponent<Slider>().value * ((float)Max_Anzahl_katProKarte_SLIDER_MAX - (float)Max_Anzahl_katProKarte_SLIDER_MIN) + (float)Max_Anzahl_katProKarte_SLIDER_MIN);
        double zwischenRechn = Max_Anzahl_katProKarte;//100 * (UIconfig.camRotatingSensitivity); // /(UIconfig.camRotatingSensitivity_default);
        kindsOfOpt_SliderT.GetComponent<TMP_Text>().text = "Kategorien je Karte: " + (int)(zwischenRechn) + " "; 
    }

    public void numOfUperKat_Bttn()
    {
        numberofUnitsPerKat_max = (int)(numberOf1Opt_Slider.GetComponent<Slider>().value * ((float)numberofUnitsPerKat_max_SLIDER_MAX - (float)numberofUnitsPerKat_max_SLIDER_MIN) + (float)numberofUnitsPerKat_max_SLIDER_MIN);
        double zwischenRechn = numberofUnitsPerKat_max;//100 * (UIconfig.camRotatingSensitivity); // /(UIconfig.camRotatingSensitivity_default);
        numberOf1Opt_SliderT.GetComponent<TMP_Text>().text = "Anzahl der Werte je Kategorie: " + (int)(zwischenRechn) + " ";
    }




    public void OnKlickBttn01(){ int i = 0; if (string.IsNullOrEmpty(kategorien_n[i])) { return; } ChoosenKats[i] = 1-ChoosenKats[i]; getToSetKatNum(); }
    public void OnKlickBttn02() { int i = 1; if (string.IsNullOrEmpty(kategorien_n[i])) { return; } ChoosenKats[i] = 1-ChoosenKats[i]; getToSetKatNum(); }
    public void OnKlickBttn03() { int i = 2; if (string.IsNullOrEmpty(kategorien_n[i])) { return; } ChoosenKats[i] = 1-ChoosenKats[i]; getToSetKatNum(); }
    public void OnKlickBttn04() { int i = 3; if (string.IsNullOrEmpty(kategorien_n[i])) { return; } ChoosenKats[i] = 1-ChoosenKats[i]; getToSetKatNum(); }
    public void OnKlickBttn05() { int i = 4; if (string.IsNullOrEmpty(kategorien_n[i])) { return; } ChoosenKats[i] = 1-ChoosenKats[i]; getToSetKatNum(); }
    public void OnKlickBttn06() { int i = 5; if (string.IsNullOrEmpty(kategorien_n[i])) { return; } ChoosenKats[i] = 1-ChoosenKats[i]; getToSetKatNum(); }
    public void OnKlickBttn07() { int i = 6; if (string.IsNullOrEmpty(kategorien_n[i])) { return; } ChoosenKats[i] = 1-ChoosenKats[i]; getToSetKatNum(); }
    public void OnKlickBttn08() { int i = 7; if (string.IsNullOrEmpty(kategorien_n[i])) { return; } ChoosenKats[i] = 1-ChoosenKats[i]; getToSetKatNum(); }
    public void OnKlickBttn09() { int i = 8; if (string.IsNullOrEmpty(kategorien_n[i])) { return; } ChoosenKats[i] = 1-ChoosenKats[i]; getToSetKatNum(); }
    public void OnKlickBttn_GenColor() { int i = 0; ChoosenKats2[i] = 1 - ChoosenKats2[i]; getToSetKatNum(); }
    public void OnKlickBttn_GenRot() { int i = 1; ChoosenKats2[i] = 1-ChoosenKats2[i]; getToSetKatNum(); }
    public void OnKlickBttn_GenChilds() { int i = 2; ChoosenKats2[i] = 1-ChoosenKats2[i]; getToSetKatNum(); }
    

    

}
