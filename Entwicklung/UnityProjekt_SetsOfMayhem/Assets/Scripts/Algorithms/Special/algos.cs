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
                
        for (int i = 0; i < Max_Anzahl_katProKarte; i++)
        {
            fieldOfCards[pos, i] = (int)UnityEngine.Random.Range(0, numberofUnitsPerKat_max - 1);
            //fieldOfCards[pos, i] = ;
        }

    }
    public static void checkForPlayerSetSelection()
    {

        scanForSelected();
        if (numberOfSelected >= numberOfSelected_soll)
        {
            for (int i = 0; i < array_cards_status.Length; i++)
            {
                bool cfs = checkForSetinSelected();
                if (cfs == true)
                {
                    //ebug.Log(array_cards_selected + " ; " + array_cards_status + " ; " + array_cards_used_with_id);
                    if (array_cards_status[i] == 2)
                    {
                        array_cards_status[i] = 0;
                    }
                }
                else
                {
                    array_cards_status[i] = 1;
                }
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
        for (int j = 0; j < numberofUnitsPerKat_max; j++)
        {
            if (werte_n[kat_id, j] == wertname)
            {
                return j;
            }
        }
        //wenn nicht gefunden, finde platz
        for (int j = 0; j < numberofUnitsPerKat_max; j++)
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
}
