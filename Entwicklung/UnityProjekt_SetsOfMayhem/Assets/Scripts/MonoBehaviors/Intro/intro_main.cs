using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using static methods_unity;
using UnityEngine.SceneManagement;
using static config_parameters;
using static algos;
using static medium;
public class intro_main : MonoBehaviour
{

    public GameObject Karte1;
    public GameObject Karte2;
    public GameObject Karte3;
    private int backup_numberField=0;
    private int backup_genSet = 0;
    private int backup_kartenKostuem_HG_Kat_Pointer = 0;
    private int backup_arr_kartenKostuem_HG_Pointer=0;

    public bool weiter=true;

    private bool BeispielBehalten = false;

    // Start is called before the first frame update
    void Start()
    {
        inSettings = true;
        backup_numberField = Game_numberOfCardsOnDeck;
        Game_numberOfCardsOnDeck = Game_numberOfCardsOnDeck_Intro;
        backup_genSet = numberOfSelected_soll_gen;
        numberOfSelected_soll_gen = numberOfSelected_soll_gen_Íntro;
        Karte1.SetActive(false);
        Karte2.SetActive(false); 
        Karte3.SetActive(false);
        backup_kartenKostuem_HG_Kat_Pointer = kartenKostuem_HG_Kat_Pointer;
        backup_arr_kartenKostuem_HG_Pointer = arr_kartenKostuem_HG_Pointer[kartenKostuem_HG_Kat_ID];
        kartenKostuem_HG_Kat_Pointer = (int)Random.Range(0, arr_Kostuem_HG_Kat_ID.Length - 1);
        kartenKostuem_HG_Kat_ID = arr_Kostuem_HG_Kat_ID[kartenKostuem_HG_Kat_Pointer];
        //Debug.Log(Random.Range(0, arr_Kostuem_HG_Kat_ID.Length - 1) + " "+ Random.Range(0, 3));
        UIKHG_Kaz_update(true);
        kartenKostuem_HG_ID = arr_kartenKostuem_HG_ID[kartenKostuem_HG_Kat_ID];
        //kartenKostuem_Pointer = (int)Random.Range(0, arr_Kostuem_ID.Length - 1); resettonew();
        //lade123(classicSET);
        update_arrays();
        StartCoroutine(Waiting(Intro_waitTime));
        
    }

    // Update is called once per frame
    void Update()
    {
       
    }

    public void onClickSetBackground()
    {
        BeispielBehalten = true;
        if (weiter)
        {
            //SceneManager.LoadScene(sceneName: "MainMenue");
            SceneManager.LoadScene(sceneName: "Hauptspiel");
        }
    }


    IEnumerator Waiting(int t)
    {

        float t2 = t;
        t2=   (float)t / (float)5;

        yield return new WaitForSeconds(t2);
        Karte1.SetActive(true);
        yield return new WaitForSeconds(t2);
        Karte2.SetActive(true);
        yield return new WaitForSeconds(t2);
        Karte3.SetActive(true); 
        yield return new WaitForSeconds(t2*2);
        if (!BeispielBehalten) {
            Game_numberOfCardsOnDeck = backup_numberField;
            numberOfSelected_soll_gen = backup_genSet;
            kartenKostuem_HG_Kat_Pointer = backup_kartenKostuem_HG_Kat_Pointer;
            kartenKostuem_HG_Kat_ID = arr_Kostuem_HG_Kat_ID[kartenKostuem_HG_Kat_Pointer];
            UIKHG_Kaz_update(false);
            arr_kartenKostuem_HG_Pointer[kartenKostuem_HG_Kat_ID] = backup_arr_kartenKostuem_HG_Pointer;
            kartenKostuem_HG_ID = arr_kartenKostuem_HG_ID[kartenKostuem_HG_Kat_ID];
        }
        if (weiter) { 
            //SceneManager.LoadScene(sceneName: "MainMenue");
            SceneManager.LoadScene(sceneName: "Hauptspiel");
        }
    }

    private void resettonew()
    {
        kategorien_n = new string[Max_Anzahl_katProKarte_SLIDER_MAX + 1];
        werte_n = new string[Max_Anzahl_katProKarte_SLIDER_MAX + 1, numberofUnitsPerKat_max_SLIDER_MAX + 1];
        werte_n_length = new int[Max_Anzahl_katProKarte_SLIDER_MAX + 1];
        //Debug.Log(kategorien_n[0]);
        ChoosenKats = new int[100];
        kategorien_n_sorted = new string[Max_Anzahl_katProKarte_SLIDER_MAX + 10];
        werte_n_sorted = new string[Max_Anzahl_katProKarte_SLIDER_MAX + 10, numberofUnitsPerKat_max_SLIDER_MAX + 1];
        StartCoroutine(Waiting2());
    }
    IEnumerator Waiting2()
    {
        float wait = 0.4f;
        float wait2 = 0.4f;
        //KartenReboot.SetActive(false);
        yield return new WaitForSeconds(wait2);
        //KartenReboot.SetActive(true);
        /*
         * yield return new WaitForSeconds(wait);
        KartenReboot.SetActive(false);
        yield return new WaitForSeconds(wait2);
        KartenReboot.SetActive(true);
        //yield return new WaitForSeconds(wait);
        //KartenReboot.SetActive(false);
        */
    }




}
