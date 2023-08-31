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
using TMPro;


public class gameoptions_main : MonoBehaviour
{

    //public GameObject kindsOfOpt_Slider;
    public GameObject kindsOfOpt_SliderT;

    public GameObject Toogle_opt01;
    public GameObject Toogle_opt01_t;
    public GameObject Toogle_opt02;
    public GameObject Toogle_opt02_t;
    public GameObject Toogle_opt03;
    public GameObject Toogle_opt03_t;
    public GameObject Toogle_opt04;
    public GameObject Toogle_opt04_t;
    public GameObject Toogle_opt05;
    public GameObject Toogle_opt05_t;
    public GameObject Toogle_opt06;
    public GameObject Toogle_opt06_t;
    public GameObject Toogle_opt07;
    public GameObject Toogle_opt07_t;
    public GameObject Toogle_opt08;
    public GameObject Toogle_opt08_t;
    public GameObject Toogle_opt09;
    public GameObject Toogle_opt09_t;

    public GameObject Toogle_gen_rotation;
    public GameObject Toogle_gen_childs;
    public GameObject Toogle_gen_colour;
    //public GameObject Toogle_gen_01;


    public GameObject numberOf1Opt_Slider;
    public GameObject numberOf1Opt_SliderT;

    public GameObject SET1_nbrOfCards_Slider;
    public GameObject SET1_nbrOfCards_SliderT;

    public GameObject nbrOfCardsInField_Slider;
    public GameObject nbrOfCardsInField_SliderT;





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

        SET1_nbrOfCards_Slider.GetComponent<Slider>().value = (float)(((float)numberOfSelected_soll - (float)numberOfSelected_soll_SLIDER_MIN) / ((float)numberOfSelected_soll_SLIDER_MAX - (float)numberOfSelected_soll_SLIDER_MIN));
        numOfCardsInASET_Bttn();

        nbrOfCardsInField_Slider.GetComponent<Slider>().value = (float)(((float)Game_numberOfCardsOnDeck - (float)Game_numberOfCardsOnDeck_SLIDER_MIN) / ((float)Game_numberOfCardsOnDeck_SLIDER_MAX - (float)Game_numberOfCardsOnDeck_SLIDER_MIN));
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
        numberOf1Opt_SliderT.GetComponent<TMP_Text>().text = "Arten je Kategorie: " + (int)(zwischenRechn) + " ";
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
    
    public void getToSetKatNum()
    {
        kategorien_n_sorted = new string[Max_Anzahl_katProKarte_SLIDER_MAX + 10];
        Max_Anzahl_katProKarte = 0;
        int k = 0;
        AnazhlEintraege_nurManuell = 0;
        for (int i=0; i<ChoosenKats.Length; i++ )
        {
            Max_Anzahl_katProKarte = Max_Anzahl_katProKarte+ChoosenKats[i];
            if (ChoosenKats[i] == 1) { kategorien_n_sorted[k]=kategorien_n[i]; k++; AnazhlEintraege_nurManuell++; }
        }
        numberOfKatsOnCardsNeeded = Max_Anzahl_katProKarte;
        for (int i = 0; i < ChoosenKats2.Length; i++)
        {
            Max_Anzahl_katProKarte = Max_Anzahl_katProKarte + ChoosenKats2[i];
            if (ChoosenKats2[i] == 1) { kategorien_n_sorted[k] = kategorien_n2[i];k++; }
        }

        for (int i = 0; i < kategorien_n.Length; i++)
        {
            if (ChoosenKats[i] == 0 && !string.IsNullOrEmpty(kategorien_n[i])) { kategorien_n_sorted[k] = kategorien_n[i]; k++; AnazhlEintraege_nurManuell++; }
        }
        numberOfKatsOnCardsNeeded = Max_Anzahl_katProKarte;
        for (int i = 0; i < kategorien_n2.Length; i++)
        {
            if (ChoosenKats2[i] == 0 && !string.IsNullOrEmpty(kategorien_n2[i])) { kategorien_n_sorted[k] = kategorien_n2[i]; k++; }
        }
        
       AnazhlEintraege =k;

        //=)=====================================
        werte_n_sorted = new string[Max_Anzahl_katProKarte_SLIDER_MAX + 10, numberofUnitsPerKat_max_SLIDER_MAX];
        for (int katj = 0; katj < kategorien_n_sorted.Length; katj++)
        {
            for (int katjv = 0; katjv < kategorien_n.Length; katjv++)
            {
                if (kategorien_n_sorted[katj] == kategorien_n[katjv])
                {
                    for (int j = 0; j < numberofUnitsPerKat_max; j++)
                    {
                        werte_n_sorted[katj, j] = werte_n[katjv, j];
                    }
                }
            }
            for (int katjv = 0; katjv < kategorien_n2.Length; katjv++)
            {
                if (kategorien_n_sorted[katj] == kategorien_n2[katjv])
                {
                    for (int j = 0; j < numberofUnitsPerKat_max; j++)
                    {
                        werte_n_sorted[katj, j] = werte_n2[katjv, j].ToString();
                    }
                }
            }
           


        }

       /*
        for(int i=0; i< numberofUnitsPerKat_max_SLIDER_MAX; i++)
        {

            for (int ii = 0; ii < Max_Anzahl_katProKarte_SLIDER_MAX + 10; ii++)
            {
                Debug.Log("hier "+ ii + " " + i + " "+werte_n_sorted[ii, i]);
            }
        }
       */
        
        
        //Debug.Log("kat "+kategorien_n[0]+ kategorien_n[1]);
    }
    

}
