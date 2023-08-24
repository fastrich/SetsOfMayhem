using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Medium { 
    public static int numberOfSelected = 2;
    public static int numberOfSelected_soll = 3;
    public static int numberofSlots_max = 12;
    public static int[] array_cards_status = new int[numberofSlots_max];//0=renew, 1=renewed, 2=selected
    public static int[] array_cards_used_with_id = new int[numberofSlots_max]; //

    public static int[] array_cards_selected = new int[numberofSlots_max];

    public static int Max_Anzahl_Karten = 100;
    public static int Max_Anzahl_katProKarte = 10;
    public static string[,] KS_kat = new string[Max_Anzahl_Karten, Max_Anzahl_katProKarte];
    public static string[,] KS_wert = new string[Max_Anzahl_Karten, Max_Anzahl_katProKarte];


    public static GameObject[,] array = new GameObject[10,10]; //


    public static void scanForSelected()
    {
        numberOfSelected = 0;
        for (int i = 0; i < Medium.array_cards_status.Length; i++)
        {
            if (Medium.array_cards_status[i] == 2)
            {
                Debug.Log("scan: " + array_cards_used_with_id[i]);
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
        for (int i = 0; i < numberOfSelected ; i++)
        {
            if (array_cards_selected[i] >= 0) { }else{break; }
            Debug.Log("Selected: " + array_cards_selected[i]);

            for (int ik = 0; ik < Max_Anzahl_katProKarte; ik++)
            {
                Debug.Log("IKwert:" + KS_wert[array_cards_selected[i], ik]);
                if (string.IsNullOrEmpty(KS_kat[array_cards_selected[i], ik])) { break; }
                if (string.IsNullOrEmpty(KS_wert[array_cards_selected[i], ik])) { break; }
                Debug.Log("wert:" + KS_wert[array_cards_selected[i], ik]);
                int temp_a = 0;
                    for (int j = 0; j < numberOfSelected; j++)
                    {
                        if (array_cards_selected[j] >= 0) { } else { break; }
                        if (i != j)
                        {
                            for (int jk = 0; jk < Max_Anzahl_katProKarte; jk++)
                            {
                            Debug.Log("hihi");
                                if (string.IsNullOrEmpty(KS_kat[array_cards_selected[j], jk])) { break; }
                                if (string.IsNullOrEmpty(KS_wert[array_cards_selected[j], jk])) { break; }
                            Debug.Log("OB_wert:" + KS_wert[array_cards_selected[i], ik] + "wert:" + KS_wert[array_cards_selected[j], jk]);
                                if (KS_kat[array_cards_selected[i], ik].CompareTo(KS_kat[array_cards_selected[j], jk])==0)
                                {
                                    if (KS_wert[array_cards_selected[i], ik].CompareTo(KS_wert[array_cards_selected[j], jk])==0)
                                    {
                                    Debug.Log("IN_wert:" + KS_wert[array_cards_selected[i], ik]+ "wert:" + KS_wert[array_cards_selected[j], jk] );
                                        temp_a++;
                                    }
                                    
                                }

                            }

                        }

                    }
                    Debug.Log(KS_kat[array_cards_selected[i],ik] + ": temp:" + temp_a);
                    if (temp_a == 0 || temp_a == numberOfSelected_soll-1)
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






}
