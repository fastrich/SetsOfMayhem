using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static medium;
using static config_parameters;
using static bruecke;
using System;
using static methods_unity;
using static algos;
using static setsUndFelder;
using static karteZuBild;

public static class player2
{
    public static void player2_main(){


    }

    public static void player2_thereIsNewField()
    {
        checkForSetInField(0,true);

        randomisePosOfMySelCards();
        loadMyField();



    }
    public static void randomisePosOfMySelCards()
    {
        int quelle = 3;
        int xtr = 0;//UnityEngine.Random.Range(0,4);
        int i2 = 0;
        int save1 = 0;
        for (int i = 0; i < array_cards_selected_LengthIt(2, 1); i++)
        {
            

            xtr = UnityEngine.Random.Range(0, array_cards_selected_LengthIt(2, 1));
            i2 = i + xtr;
            if (i2 >= array_cards_selected_LengthIt(2, 1))
            {
                i2 = i2 % array_cards_selected_LengthIt(2, 1);
            }
            save1 = array_cards_selected_GetIt(quelle, i2);
            array_cards_selected_SetIt(quelle, i2, array_cards_selected_GetIt(quelle, i));
            array_cards_selected_SetIt(quelle, i, save1);
        }
    }

    public static void loadMyField()
    {
        int quelle = 0;
        int quelle_arrsel = 3;
        int ziel = 4;
        int ziel_arrsel = 2;
        //Debug.Log("hoho");
        for (int i = 0; i < array_cards_selected_LengthIt(2, 1); i++)
        {
            array_cards_selected_SetIt(ziel_arrsel, i, array_cards_selected_GetIt(quelle_arrsel, i));
            array_cards_status_SetIt(ziel, i, 5);
            for (int ik = 0; ik < fieldOfCards_LengthIt(2, 2); ik++)
            {
                fieldOfCards_SetIt(ziel, i, ik, fieldOfCards_GetIt(quelle, array_cards_selected_GetIt(quelle_arrsel, i), ik));
            }

        }


    }




}
