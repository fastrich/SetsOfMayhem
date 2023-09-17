using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static methods_unity;
using UnityEngine.SceneManagement;
using static config_parameters;
using static medium;
using static algos;
using static methods;
using TMPro;
using static setsUndFelder;
using static karteZuBild;
public class mainmenue_main : MonoBehaviour
{
   public GameObject KartenReboot;
    public GameObject IntSceneMng;
    public GameObject KlasUTZ;


    // Start is called before the first frame update
    void Start()
    {
        //newField()
        resettonew();
    }

    private void OnEnable()
    {
    //new arrays
    }
    // Update is called once per frame
    void Update()
    {
        //inSettings = true;
        //lade123(classicSET);
   
        kartenKostuem_ID = arr_Kostuem_ID[kartenKostuem_Pointer];
        kartenKostuem_HG_Kat_ID = arr_Kostuem_HG_Kat_ID[kartenKostuem_HG_Kat_Pointer];
        UIKHG_Kaz_update(false);
        kartenKostuem_HG_ID = arr_kartenKostuem_HG_ID[kartenKostuem_HG_Kat_ID];
        float i = bestzeiten[Game_numberOfCardsOnDeck_Classic, Max_Anzahl_katProKarte_Classic, numberofUnitsPerKat_max_classic, numberOfSelected_soll_Classic, numberOfSelected_soll_gen_Classic];//
        string s = "";
        if (i != 0) { s = "Schnellste Zeit: " + i.ToString(); }
        KlasUTZ.GetComponent<TMP_Text>().text =  s;
        //getToSetKatNum();
        resettonew();
    }
    IEnumerator Waiting2()
    {
        float wait = 0.4f;
        wait = 1;
        KartenReboot.SetActive(true);
        yield return new WaitForSeconds(wait);
        KartenReboot.SetActive(false);
        yield return new WaitForSeconds(wait);
        KartenReboot.SetActive(true);
        yield return new WaitForSeconds(wait);
            KartenReboot.SetActive(false);


    }

    IEnumerator Waiting(int t)
    {
        yield return new WaitForSeconds(t);
        SceneManager.LoadScene(sceneName: "Stage");
    }
    private void resettonew()
    {
        /*
        kategorien_n = new string[Max_Anzahl_katProKarte_SLIDER_MAX + 1];
        werte_n = new string[Max_Anzahl_katProKarte_SLIDER_MAX + 1, numberofUnitsPerKat_max_SLIDER_MAX + 1];
        werte_n_length = new int[Max_Anzahl_katProKarte_SLIDER_MAX + 1];
        //Debug.Log(werte_n_length[1]);
        ChoosenKats = new int[100];
        //kategorien_n_sorted = new string[Max_Anzahl_katProKarte_SLIDER_MAX + 10];
        //werte_n_sorted = new string[Max_Anzahl_katProKarte_SLIDER_MAX + 10, numberofUnitsPerKat_max_SLIDER_MAX + 1];
        StartCoroutine(Waiting2());
        */

    }

    public void StartGame_Fast_Random()
    {
        Game_numberOfCardsOnDeck = Game_numberOfCardsOnDeck_Classic;
        numberOfSelected_soll = numberOfSelected_soll_Classic;
        numberofUnitsPerKat_max = numberofUnitsPerKat_max_classic;
        Max_Anzahl_katProKarte = Max_Anzahl_katProKarte_classic;
        getToSetKatNum();
        lade123(classicSET);
        update_arrays();
        inSettings = false;
        Debug.Log("Kartenwerte123 " + ArrayToString(kategorien_n_sorted));
        if (IntSceneMng == null) { SceneManager.LoadScene(sceneName: "Stage"); }
        IntSceneMng.GetComponent<internalScenesManager>().ChangeInternalScene("Stage");



    }
    public void StartGame_Fast_Classic()
    {
        
        Game_numberOfCardsOnDeck = Game_numberOfCardsOnDeck_Classic;
        numberOfSelected_soll = numberOfSelected_soll_Classic;
        numberofUnitsPerKat_max = numberofUnitsPerKat_max_classic;
        Max_Anzahl_katProKarte = Max_Anzahl_katProKarte_classic;
        numberOfSelected_soll_gen = numberOfSelected_soll_gen_Classic;
        Set_InfosAnzeigen_Anzahl = Set_InfosAnzeigen_Anzahl_Classic;
        Element1_visible = Element1_visible_classic;
        if (!arr_Kostuem_klassisch[kartenKostuem_Pointer])
        {
            kartenKostuem_Pointer = kartenKostuem_Pointer_ClassicDefault;
            kartenKostuem_ID = arr_Kostuem_ID[kartenKostuem_Pointer]; 
           
        }
        lade123(classicSET);
        update_arrays();
        resettonew();
        lade123(classicSET);
        //Debug.Log("Kartenwerte123 " + ArrayToString(kategorien_n_sorted));
        inSettings = false;
        if (IntSceneMng == null) { SceneManager.LoadScene(sceneName: "Stage"); }
        IntSceneMng.GetComponent<internalScenesManager>().ChangeInternalScene("Stage");
    }

    public void StartGame_withOptionsM()
    {
        update_arrays();
        checkParameterForToGo();

        //if (numberOfSelected_soll_gen >= (Game_numberOfCardsOnDeck-numberOfSelected_soll)) { return; }
        inSettings = false;
        if (IntSceneMng == null) { SceneManager.LoadScene(sceneName: "Stage"); }
        IntSceneMng.GetComponent<internalScenesManager>().ChangeInternalScene("Stage");
    }

    public void StartGame_withOptionsM2()
    {
        update_arrays();
        inSettings = false;
        if (IntSceneMng == null) { SceneManager.LoadScene(sceneName: "Stage"); }
        IntSceneMng.GetComponent<internalScenesManager>().ChangeInternalScene("Stage");
    }


   


}
