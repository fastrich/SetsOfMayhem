using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static methods_unity;
using UnityEngine.SceneManagement;
using static config_parameters;
using static medium;
using static algos;
using static methods;
using static setsUndFelder;
using TMPro;

public static class karteZuBild
{
    public static void UIKHG_Kaz_update(bool random)
    {
        range1[0] = _0_arr_Kostuem_HG_ID.Length;
        range1[1] = _1_arr_Kostuem_HG_ID.Length;
        range1[2] = _2_arr_Kostuem_HG_ID.Length;
        range1[3] = _3_arr_Kostuem_HG_ID.Length;
        range1[4] = _4_arr_Kostuem_HG_ID.Length;

        if (random) { arr_kartenKostuem_HG_Pointer[kartenKostuem_HG_Kat_ID] = (int)UnityEngine.Random.Range(0, range1[kartenKostuem_HG_Kat_ID] - 1); }

        for (int i = 0; i < range1.Length; i++)
        {
            if (arr_kartenKostuem_HG_Pointer[i] < 0) { arr_kartenKostuem_HG_Pointer[i] = (range1[i] - 1); }
            if (arr_kartenKostuem_HG_Pointer[i] > (range1[i] - 1)) { arr_kartenKostuem_HG_Pointer[i] = 0; }
        }
        arr_kartenKostuem_HG_ID[0] = _0_arr_Kostuem_HG_ID[arr_kartenKostuem_HG_Pointer[0]];
        arr_kartenKostuem_HG_ID[1] = _1_arr_Kostuem_HG_ID[arr_kartenKostuem_HG_Pointer[1]];
        arr_kartenKostuem_HG_ID[2] = _2_arr_Kostuem_HG_ID[arr_kartenKostuem_HG_Pointer[2]];
        arr_kartenKostuem_HG_ID[3] = _3_arr_Kostuem_HG_ID[arr_kartenKostuem_HG_Pointer[3]];
        arr_kartenKostuem_HG_ID[4] = _4_arr_Kostuem_HG_ID[arr_kartenKostuem_HG_Pointer[4]];


    }

  
    public static int welcheNummerHatKat(string katname)
    {
        //kategorien_n[0] = "Andere";


        if (string.IsNullOrEmpty(katname))
        {
            return -1;
        }
        int i = 0;
        for (int j = 0; j < kategorien_n.Length; j++)
        {
            if (kategorien_n[j] == katname)
            {
                return j;
            }
        }
        //wenn nicht gefunden, finde platz
        for (int j = 0; j < kategorien_n.Length; j++)
        {
            if (string.IsNullOrEmpty(kategorien_n[j]))
            {
                kategorien_n[j] = katname;
                return j;
            }
        }

        Debug.Log("sollte nicht passieren");
        return i;
    }

    public static int welcheNummerHatWert(int kat_id, string wertname)
    {
        //werte_n[kat_id, 0] = "Unsortiert";

        int i = 0;
        if (string.IsNullOrEmpty(wertname))
        {
            return -1;
        }
        for (int j = 0; j < numberofUnitsPerKat_max_SLIDER_MAX; j++)
        {
            if (werte_n[kat_id, j] == wertname)
            {
                return j;
            }
        }
        //wenn nicht gefunden, finde platz
        for (int j = 0; j < numberofUnitsPerKat_max_SLIDER_MAX; j++)
        {
            if (string.IsNullOrEmpty(werte_n[kat_id, j]))
            {
                werte_n[kat_id, j] = wertname;
                return j;
            }
        }

        Debug.Log("sollte nicht passieren");
        return i;
    }
    public static void lade123(string[] presetSET)
    {
        //getToSetKatNum();
        resettonew();
        kategorien_n_sorted = new string[Max_Anzahl_katProKarte_SLIDER_MAX + 10];
        Max_Anzahl_katProKarte = 0;
        int k = 0;
        int z = 0;
        //AnazhlEintraege_nurManuell = presetSET.Length;

        for (int i = 0; i < classicSET.Length; i++)
        {
            Max_Anzahl_katProKarte = Max_Anzahl_katProKarte + 1;
            kategorien_n_sorted[k] = presetSET[i]; k++;
        }

        for (int i = 0; i < kategorien_n.Length; i++)
        {
            z = 0;
            for (int ii = 0; ii < kategorien_n_sorted.Length; ii++)
            {
                if (kategorien_n[i] == kategorien_n_sorted[ii]) { z++; }
            }
            if (!string.IsNullOrEmpty(kategorien_n[i]) && z == 0) { kategorien_n_sorted[k] = kategorien_n[i]; k++; }


        }

        AnazhlEintraege_nurManuell = AnazhlEintraege;
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
        update_arrays();

        for (int i = 0; i < kategorien_n_sorted.Length; i++)
        {
            for (int j = 0; j < kategorien_n2.Length; j++)
            {

                if (!string.IsNullOrEmpty(kategorien_n2[j]) && kategorien_n_sorted[i] == kategorien_n2[j])
                {
                    AnazhlEintraege_nurManuell--;
                }

            }

        }
        numberOfKatsOnCardsNeeded = AnazhlEintraege_nurManuell;
        Debug.Log("no " + numberOfKatsOnCardsNeeded + " "+ AnazhlEintraege);

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

            if (kategorien_n_sorted[katj] == classicSET[0])
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

    public static void getToSetKatNum()
    {
        kategorien_n_sorted = new string[Max_Anzahl_katProKarte_SLIDER_MAX + 10];
        Max_Anzahl_katProKarte = 0;
        int k = 0;
        AnazhlEintraege_nurManuell = 0;
        for (int i = 0; i < ChoosenKats.Length; i++)
        {
            Max_Anzahl_katProKarte = Max_Anzahl_katProKarte + ChoosenKats[i];
            if (ChoosenKats[i] == 1) { kategorien_n_sorted[k] = kategorien_n[i]; k++; AnazhlEintraege_nurManuell++; }
        }
        numberOfKatsOnCardsNeeded = Max_Anzahl_katProKarte;
        for (int i = 0; i < ChoosenKats2.Length; i++)
        {
            Max_Anzahl_katProKarte = Max_Anzahl_katProKarte + ChoosenKats2[i];
            if (ChoosenKats2[i] == 1) { kategorien_n_sorted[k] = kategorien_n2[i]; k++; }
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

        AnazhlEintraege = k;
        update_arrays();

        //=)=====================================
        werte_n_sorted = new string[Max_Anzahl_katProKarte_SLIDER_MAX + 10, numberofUnitsPerKat_max_SLIDER_MAX];
        for (int katj = 0; katj < kategorien_n_sorted.Length; katj++)
        {
            for (int katjv = 0; katjv < kategorien_n.Length; katjv++)
            {
                if (kategorien_n_sorted[katj] == kategorien_n[katjv])
                {
                    for (int j = 0; j < numberofUnitsPerKat_max_SLIDER_MAX; j++)
                    {
                        werte_n_sorted[katj, j] = werte_n[katjv, j];
                    }
                }
            }
            for (int katjv = 0; katjv < kategorien_n2.Length; katjv++)
            {
                if (kategorien_n_sorted[katj] == kategorien_n2[katjv])
                {
                    for (int j = 0; j < numberofUnitsPerKat_max_SLIDER_MAX; j++)
                    {
                        werte_n_sorted[katj, j] = werte_n2[katjv, j].ToString();
                    }
                }
            }



        }

        if (false)
        {
            for (int i = 0; i < 5; i++)
            {

                for (int ii = 0; ii < 2; ii++)
                {
                    Debug.Log("hier " + ii + " " + i + " " + werte_n_sorted[ii, i]);
                }
            }
        }



        //Debug.Log("kat "+kategorien_n[0]+ kategorien_n[1]);
    }


    public static void a2()
    {
        //inSettings = true;
        //update_arrays();
        //resettonew();
        //StartCoroutine(Waiting2());
    }

    public static IEnumerator Waiting2()
    {
        float wait = 0.4f;
        wait = 1;
        //KartenReboot.SetActive(true);
        yield return new WaitForSeconds(wait);
        //KartenReboot.SetActive(false);
        yield return new WaitForSeconds(wait);
        //KartenReboot.SetActive(true);
        yield return new WaitForSeconds(wait);
        //
        //KartenReboot.SetActive(false);


    }

    static public void resettonew()
    {
        /*
        //StartCoroutine(Waiting2());
        */
        //inSettings = true;
        kategorien_n2[0] = farbe_gen;
        kategorien_n2[1] = ausrichtung_gen;
        kategorien_n2[2] = anzahl_gen;
        for (int i = 0; i < ueberschuessig.Length; i++)
        {
            ueberschuessig[i] = 0;
        }

        kategorien_n = new string[Max_Anzahl_katProKarte_SLIDER_MAX + 1];
        werte_n = new string[Max_Anzahl_katProKarte_SLIDER_MAX + 1, numberofUnitsPerKat_max_SLIDER_MAX + 1];
        werte_n_length = new int[Max_Anzahl_katProKarte_SLIDER_MAX + 1];
        kategorien_n_sorted = new string[Max_Anzahl_katProKarte_SLIDER_MAX + 10];
        werte_n_sorted = new string[Max_Anzahl_katProKarte_SLIDER_MAX + 10, numberofUnitsPerKat_max_SLIDER_MAX + 1];
        ChoosenKats = new int[100];
        getToSetKatNum();
    }

}
