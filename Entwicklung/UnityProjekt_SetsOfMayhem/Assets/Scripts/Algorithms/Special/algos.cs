using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static medium;
using static config_parameters;
using static bruecke;
using System;
using static methods_unity;
using static player2;

public static class algos
{

   

    public static void check_werte_n_n2_length()
    {
        for (int i = 0; i < AnazhlEintraege_nurManuell; i++)
        {
            //werte_n_length[i]= checklength(werte_n, i);

            int k = 0;
            for (int ij = 0; ij < numberofUnitsPerKat_max_SLIDER_MAX; ij++)
            {
                if (!string.IsNullOrEmpty(werte_n[i, ij]))
                {
                    k++;
                }
            }
            werte_n_length[i] = k;


        }
        // Debug.Log("checkW " + werte_n[0, 0] + "," + werte_n[0, 1] + "," + werte_n[0, 2] + "," + werte_n[0,3] + "," + werte_n[0, 4]+ ","+ werte_n[0, 5]);


    }

    public static int checklength(string[,] strgar, int einschr)
    {
        int k = 0;
        for (int i = 0; i < strgar.Length; i++)
        {
            if (!string.IsNullOrEmpty(strgar[einschr, i]))
            {
                k++;
            }
        }
        return k;
    }
    public static bool checkParameterForToGo()
    {

        if (Max_Anzahl_katProKarte < 1) { return false; }
        if (numberofUnitsPerKat_max < 1) { return false; }
        if (numberOfSelected_soll < 1) { return false; }
        if (Game_numberOfCardsOnDeck < 1) { return false; }
        if (numberOfSelected_soll > Game_numberOfCardsOnDeck) { return false; }
        return true;
    }
   
    


}
