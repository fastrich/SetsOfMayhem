using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static methods_unity;
using UnityEngine.SceneManagement;
using static config_parameters;
using static medium;
using static algos;
using static methods;
public class mainmenue_main : MonoBehaviour
{
   public GameObject KartenReboot;


    // Start is called before the first frame update
    void Start()
    {
        //StartCoroutine(Waiting(3));
        
        inSettings = true;
        kategorien_n2[0] = farbe_gen;
        kategorien_n2[1] = ausrichtung_gen;
        kategorien_n2[2] = anzahl_gen;
        for (int i =0; i < ueberschuessig.Length; i++) {
            ueberschuessig[i] =0;
                }

        KartenReboot.SetActive(false);

     
        StartCoroutine(Waiting2());




    }

    private void OnEnable()
    {
        update_arrays();
        resettonew();
        StartCoroutine(Waiting2());

    }
    // Update is called once per frame
    void Update()
    {
        inSettings = true;
        //lade123(classicSET);
   
        kartenKostuem_ID = arr_Kostuem_ID[kartenKostuem_Pointer];
    }
    IEnumerator Waiting2()
    {
        float wait = 0.2f;
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
        kategorien_n = new string[Max_Anzahl_katProKarte_SLIDER_MAX + 1];
        werte_n = new string[Max_Anzahl_katProKarte_SLIDER_MAX + 1, numberofUnitsPerKat_max_SLIDER_MAX + 1];
        werte_n_length = new int[Max_Anzahl_katProKarte_SLIDER_MAX + 1];
        //Debug.Log(werte_n_length[1]);
        ChoosenKats = new int[100];
        //kategorien_n_sorted = new string[Max_Anzahl_katProKarte_SLIDER_MAX + 10];
        //werte_n_sorted = new string[Max_Anzahl_katProKarte_SLIDER_MAX + 10, numberofUnitsPerKat_max_SLIDER_MAX + 1];
        StartCoroutine(Waiting2());


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
        Debug.Log("Kartenwerte123 " + ArrayToString(kategorien_n_sorted));
        SceneManager.LoadScene(sceneName: "Stage");



    }
    public void StartGame_Fast_Classic()
    {
        
        Game_numberOfCardsOnDeck = Game_numberOfCardsOnDeck_Classic;
        numberOfSelected_soll = numberOfSelected_soll_Classic;
        numberofUnitsPerKat_max = numberofUnitsPerKat_max_classic;
        Max_Anzahl_katProKarte = Max_Anzahl_katProKarte_classic;
        numberOfSelected_soll_gen = numberOfSelected_soll_gen_Classic;
        Set_InfosAnzeigen_Anzahl = Set_InfosAnzeigen_Anzahl_Classic;
        kartenKostuem_Pointer = 0;
        kartenKostuem_ID = arr_Kostuem_ID[kartenKostuem_Pointer];
        lade123(classicSET);
        update_arrays();
        resettonew();
        //Debug.Log("Kartenwerte123 " + ArrayToString(kategorien_n_sorted));
        inSettings = false;
        SceneManager.LoadScene(sceneName: "Stage");
    }

    public void StartGame_withOptionsM()
    {
        update_arrays();
        if (Max_Anzahl_katProKarte < 1) { return; }
        if (numberofUnitsPerKat_max < 1) { return; }
        if (numberOfSelected_soll < 1) { return; }
        if (Game_numberOfCardsOnDeck < 1) { return; }
        if (numberOfSelected_soll > Game_numberOfCardsOnDeck) { return; }

        //if (numberOfSelected_soll_gen >= (Game_numberOfCardsOnDeck-numberOfSelected_soll)) { return; }
        inSettings = false;
        SceneManager.LoadScene(sceneName: "Stage");
    }

    public void StartGame_withOptionsM2()
    {
        update_arrays();
        inSettings = false;
        SceneManager.LoadScene(sceneName: "Stage");
    }


    public void lade123(string[] presetSET) {
        kategorien_n_sorted = new string[Max_Anzahl_katProKarte_SLIDER_MAX + 10];
        Max_Anzahl_katProKarte = 0;
        int k = 0;
        int z = 0;
        AnazhlEintraege_nurManuell = presetSET.Length;

        for (int i = 0; i < classicSET.Length; i++)
        {
            Max_Anzahl_katProKarte = Max_Anzahl_katProKarte + 1;
            kategorien_n_sorted[k] = presetSET[i]; k++;
        }
        
        for (int i = 0; i < kategorien_n.Length; i++)
        {
            z =0;
            for (int ii = 0; ii < kategorien_n_sorted.Length; ii++)
            {
                if (kategorien_n[i] == kategorien_n_sorted[ii]) { z++; }
            }
            if (!string.IsNullOrEmpty(kategorien_n[i]) && z==0 ) { kategorien_n_sorted[k] = kategorien_n[i]; k++; }
            
            
        }
        
        for (int i = 0; i < kategorien_n2.Length; i++)
        {
            z = 0;
            for (int ii = 0; ii < kategorien_n_sorted.Length; ii++)
            {
                if (kategorien_n2[i] == kategorien_n_sorted[ii]) { z++; }
            }
            if (!string.IsNullOrEmpty(kategorien_n2[i]) && z == 0) { kategorien_n_sorted[k] = kategorien_n2[i]; k++; }
         
        }

        AnazhlEintraege = k;
        AnazhlEintraege_nurManuell = AnazhlEintraege;

        for (int i = 0; i < kategorien_n_sorted.Length; i++)
        {
            for (int j = 0; j < kategorien_n2.Length; j++)
            {

                if ( !string.IsNullOrEmpty(kategorien_n2[j])   && kategorien_n_sorted[i] == kategorien_n2[j])
                {
                    AnazhlEintraege_nurManuell--;
                }

            }

        }
        numberOfKatsOnCardsNeeded = AnazhlEintraege_nurManuell;


        //=)====================================
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

            if (kategorien_n_sorted[katj]==classicSET[0])
            {
                for (int j = 0; j < classicSET_1.Length; j++)
                {
                    werte_n_sorted[katj, j] = classicSET_1[j];
                        }
            }
            if (kategorien_n_sorted[katj] == classicSET[1])
            {
                for (int j = 0; j < classicSET_2.Length; j++)
                {
                    werte_n_sorted[katj, j] = classicSET_2[j];
                }
            }



        }





    }


}
