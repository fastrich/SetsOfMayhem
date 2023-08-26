using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class medium { 
    public static int numberOfSelected = 2;
    public static int numberOfSelected_soll = 3;
    public static int numberofSlots_max = 12;
    public static int[] array_cards_status = new int[numberofSlots_max];//0=renew, 1=renewed, 2=selected
    public static int[] array_cards_used_with_id = new int[numberofSlots_max]; //Kartenabbild_ID

    public static int[] array_cards_selected = new int[numberofSlots_max];//verweise auf array

    public static int Max_Anzahl_Karten = 100;
    public static int Max_Anzahl_katProKarte = 10;
    public static string[,] KS_kat = new string[Max_Anzahl_Karten, Max_Anzahl_katProKarte];
    public static string[,] KS_wert = new string[Max_Anzahl_Karten, Max_Anzahl_katProKarte];


    public static GameObject[,] array = new GameObject[10,10]; 






}
