using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static medium;
using static config_parameters;

public static class algos
{




    public static void scanForSelected()
    {
        numberOfSelected = 0;

        for (int i = 0; i < array_cards_status.Length; i++)
        {
            if (array_cards_status[i] == 2)
            {
                //Debug.Log("scan: " + array_cards_used_with_id[i]);
                array_cards_selected[numberOfSelected] = array_cards_used_with_id[i];
                numberOfSelected++;

            }
            else
            {
                array_cards_selected[numberOfSelected] = -1;
            }
        }
    }

    public static bool checkForSet()
    {



        return true;
    }
    public static bool checkForSetinSelected()
    {
        scanForSelected();
        for (int i = 0; i < numberOfSelected; i++)
        {
            if (array_cards_selected[i] >= 0) { } else { break; }
            //Debug.Log("Selected: " + array_cards_selected[i]);

            for (int ik = 0; ik < Max_Anzahl_katProKarte; ik++)
            {
                //Debug.Log("IKwert:" + KS_wert[array_cards_selected[i], ik]);
                if (string.IsNullOrEmpty(KS_kat[array_cards_selected[i], ik])) { break; }
                if (string.IsNullOrEmpty(KS_wert[array_cards_selected[i], ik])) { break; }
                //Debug.Log("wert:" + KS_wert[array_cards_selected[i], ik]);
                int temp_a = 0;
                for (int j = 0; j < numberOfSelected; j++)
                {
                    if (array_cards_selected[j] >= 0) { } else { break; }
                    if (i != j)
                    {
                        for (int jk = 0; jk < Max_Anzahl_katProKarte; jk++)
                        {
                            //Debug.Log("hihi");
                            if (string.IsNullOrEmpty(KS_kat[array_cards_selected[j], jk])) { break; }
                            if (string.IsNullOrEmpty(KS_wert[array_cards_selected[j], jk])) { break; }
                            //Debug.Log("OB_wert:" + KS_wert[array_cards_selected[i], ik] + "wert:" + KS_wert[array_cards_selected[j], jk]);
                            if (KS_kat[array_cards_selected[i], ik].CompareTo(KS_kat[array_cards_selected[j], jk]) == 0)
                            {
                                if (KS_wert[array_cards_selected[i], ik].CompareTo(KS_wert[array_cards_selected[j], jk]) == 0)
                                {
                                    //Debug.Log("IN_wert:" + KS_wert[array_cards_selected[i], ik] + "wert:" + KS_wert[array_cards_selected[j], jk]);
                                    temp_a++;
                                }

                            }

                        }

                    }

                }
                //Debug.Log(KS_kat[array_cards_selected[i], ik] + ": temp:" + temp_a);
                if (temp_a == 0 || temp_a == numberOfSelected_soll - 1)
                {

                }
                else
                {
                    Debug.Log("Kein_Set");
                    return false;
                }





            }


        }






        Debug.Log("Ein_Set");

        return true;
    }

    public static void CheckAndFillCardField()
    {


        for (int i = 0; i < array_cards_status.Length; i++)
        {
            if (array_cards_status[i] == 0)
            {

                FillCardInField(i);
                array_cards_status[i] = 3;

            }
        }
    }

    public static void FillCardInField(int pos)
    {
        for (int i2 = 0; i2 < Max_Anzahl_Versuche_KartePlatzieren ; i2++)
        {
            for (int i = 0; i < Max_Anzahl_katProKarte; i++)
            {
                fieldOfCards[pos, i] = (int)UnityEngine.Random.Range(0, numberofUnitsPerKat_max);
                //fieldOfCards[pos, i] = ;
            }
            if (!checkForDoubles_rel(pos)) { return ; }
        }
        Debug.Log("MaxVersuche erreicht");
    }

    public static bool checkForDoubles()
    {
        int d;
        //int z = 0 ;
        for (int i = 0; i < array_cards_status.Length; i++)
        {
           
            for (int i2 = 0; i2 < array_cards_status.Length; i2++)
            {

                d = 0;
                for (int ij = 0; ij < Max_Anzahl_katProKarte; ij++)
                {

                    if (i != i2 && fieldOfCards[i, ij] == 0 && fieldOfCards[i, ij] == fieldOfCards[i2, ij])
                        {
                            d++;
                        }

                    
                }
                //Debug.Log("M" + d + "  "+ Max_Anzahl_katProKarte);
                if (d >= Max_Anzahl_katProKarte) { return true; }
            }

        }
        return false;
    }

    public static bool checkForDoubles_rel(int k)
    {
        int d;
        //int z = 0 ;

            for (int i2 = 0; i2 < array_cards_status.Length; i2++)
            {

                d = 0;
                for (int ij = 0; ij < Max_Anzahl_katProKarte; ij++)
                {

                    if (k != i2 && fieldOfCards[k, ij] == fieldOfCards[i2, ij])
                    {
                        d++;
                    }


                }
                //Debug.Log("M" + d + "  "+ Max_Anzahl_katProKarte);
                if (d >= Max_Anzahl_katProKarte) { return true; }
            }

        
        return false;
    }






    public static void checkForPlayerSetSelection()
    {

        scanForSelected();
        if (numberOfSelected >= numberOfSelected_soll)
        {
            bool cfs = checkForSetinSelected();
            
               
                if (cfs == true)
                {
                     gefundeneSets++;
                    for (int i = 0; i < array_cards_status.Length; i++)
                    {
                        //ebug.Log(array_cards_selected + " ; " + array_cards_status + " ; " + array_cards_used_with_id);
                        if (array_cards_status[i] == 2)
                        {
                            array_cards_status[i] = 0;
                        }
                    }

                }
                else
                {
                    //array_cards_status[i] = 1;
                }
            


        }


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

    public static void update_arrays()
    {
        fieldOfCards = new int[Game_numberOfCardsOnDeck, Max_Anzahl_katProKarte];
        array_cards_status = new int[Game_numberOfCardsOnDeck];//0=nothing, 1=updated, 2=selected, 3=update

        array_cards_used_with_id = new int[Game_numberOfCardsOnDeck]; //Kartenabbild_ID

        array_cards_selected = new int[Game_numberOfCardsOnDeck];//verweise auf array

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


    public static void check_werte_n_n2_length()
    {
        for (int i=0; i< AnazhlEintraege_nurManuell; i++) {
            //werte_n_length[i]= checklength(werte_n, i);

            int k = 0;
            for (int ij = 0; ij < numberofUnitsPerKat_max_SLIDER_MAX; ij++)
            {
                if (!string.IsNullOrEmpty(werte_n[i,ij]))
                {
                    k++;
                }
            }
            werte_n_length[i]= k;


        }
       // Debug.Log("checkW " + werte_n[0, 0] + "," + werte_n[0, 1] + "," + werte_n[0, 2] + "," + werte_n[0,3] + "," + werte_n[0, 4]+ ","+ werte_n[0, 5]);


    }

    public static int checklength(string[,] strgar, int einschr)
    {
        int k = 0;
        for (int i = 0; i < strgar.Length; i++){
            if (!string.IsNullOrEmpty(strgar[einschr,i]))
            {
                k++;
            }
        }
        return k;
    }
}
