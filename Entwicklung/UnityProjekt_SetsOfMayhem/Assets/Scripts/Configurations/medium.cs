using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static config_parameters;
using static bruecke;

public static class medium {


    public static int numberOfSelected = 2;
    //Version1
    //public static int numberofSlots_max = 12;
    
    public static int[] array_cards_used_with_id = new int[Game_numberOfCardsOnDeck]; //Kartenabbild_ID

    
    public static int[] array_cards_selected_temp_for_gen_field = new int[Game_numberOfCardsOnDeck];//verweise auf array

    public static string[,] KS_kat = new string[Max_Anzahl_Karten, Max_Anzahl_katProKarte];
    public static string[,] KS_wert = new string[Max_Anzahl_Karten, Max_Anzahl_katProKarte];

    public static int SetsFoundInField_gen = 0;


    public static GameObject[,] array = new GameObject[10, 10];

    //Version 2
    //public GameObject[,,,,,,,,] kartenGobj_nachEigenschaften = new GameObject[upkm, upkm, upkm, upkm, upkm, upkm, upkm, upkm, upkm]; //OutOfMemory
    //public static GameObject[,,,,,,,,,,] kartenGobj_nachEigenschaften_roh = new GameObject[Game_numberOfCardsOnDeck, numberofUnitsPerKat_max, numberofUnitsPerKat_max, numberofUnitsPerKat_max, numberofUnitsPerKat_max, numberofUnitsPerKat_max, numberofUnitsPerKat_max, numberofUnitsPerKat_max, numberofUnitsPerKat_max, numberofUnitsPerKat_max, numberofUnitsPerKat_max];


    //Version 3
    //Matrix mit welche Karten liegen auf dem Tisch

   

    public static int[,] fieldOfCards_backup = new int[Game_numberOfCardsOnDeck, Max_Anzahl_katProKarte];
    //Enum 
    public static string[] kategorien_n = new string[Max_Anzahl_katProKarte_SLIDER_MAX + 1];
    public static string[,] werte_n = new string[Max_Anzahl_katProKarte_SLIDER_MAX + 1, numberofUnitsPerKat_max_SLIDER_MAX + 1];
    public static int[] werte_n_length = new int[Max_Anzahl_katProKarte_SLIDER_MAX + 1];
    public static int[] ChoosenKats = new int[100];

    public static string[] kategorien_n2 = new string[Max_Anzahl_katProKarte_SLIDER_MAX + 1];
    public static int[,] werte_n2 = new int[Max_Anzahl_katProKarte_SLIDER_MAX + 1, numberofUnitsPerKat_max_SLIDER_MAX + 1];
    public static int[] werte_n2_length = new int[Max_Anzahl_katProKarte_SLIDER_MAX + 1];
    public static int[] ChoosenKats2 = new int[100];

    public static int numberOfKatsOnCardsNeeded = Max_Anzahl_katProKarte;


    public static string[] kategorien_n_sorted = new string[Max_Anzahl_katProKarte_SLIDER_MAX + 10];
    public static string[,] werte_n_sorted = new string[Max_Anzahl_katProKarte_SLIDER_MAX + 10, numberofUnitsPerKat_max_SLIDER_MAX + 1];


    public static int[] ueberschuessig = new int[Max_Anzahl_katProKarte_SLIDER_MAX];
    public static int AnazhlEintraege = 10;
    public static int AnazhlEintraege_nurManuell = 10;

    public static int gefundeneSets = 0;

    public static float geschwin_zuLetztemSet = 0;
    public static int zuLetztemSet_AnzahlSET = 0;

    public static bool inSettings = false;
    public static bool problemInGO_Kat = false;

    public static string karteHG_Beschriftung = "";
    public static int[] range1 = new int[arr_Kostuem_HG_Kat_ID.Length]; //Saved array ranges from Background images

    //__________________________

    public static float[,,,,] bestzeiten = new float[Game_numberOfCardsOnDeck_SLIDER_MAX, Max_Anzahl_katProKarte_SLIDER_MAX + 10, numberofUnitsPerKat_max_SLIDER_MAX, numberOfSelected_soll_SLIDER_MAX, numberOfSelected_soll_gen_SLIDER_MAX];//
    public static int arr_listeUberSetZeitenInSpiel_pointer_alt = 0;
    public static int arr_listeUberSetZeitenInSpiel_pointer_neu = 0;
    public static System.DateTime[] arr_listeUberSetZeitenInSpiel = new System.DateTime[SETsBisZurWertung+1];
    public static float[] arr_listeUberSetZeitenInSpiel_korrektur = new float[SETsBisZurWertung + 1];
    public static bool neuerRekord = false;
    public static bool neuerRekord_lokal_temp = false;
    public static float bestzeiten_lokal_temp = 0f;
    public static float korrekturZeit = 0;
    //__________________________
    //public static 




}
