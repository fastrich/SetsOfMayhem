using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class config_parameters {

    //================================
    // Intro
    public static int Intro_waitTime = 3;//3
    


    //================================
    //ImSpiel
    public static int KartenInfosAnzeigen = 0;
    public static int Set_InfosAnzeigen_Anzahl = 0;
    public static int Set_InfosAnzeigen_Anzahl_Classic = 0;
    public static bool VetoFuerKarten = false;
    public static int SETsBisZurWertung=10;
    public static float Wertung_Strafzeit_inSekunden = 20;
    public static float Wertung_Strafzeit_NeuesFeld_inSekunden = 10;

    //================================
    // Describing Field 
    public static int Game_numberOfCardsOnDeck_Classic = 9;
    public static int Game_numberOfCardsOnDeck = 9;
    public static int Game_numberOfCardsOnDeck_Intro = 3;
    public static int Game_numberOfCardsOnDeck_SLIDER_MIN = 4;
    public static int Game_numberOfCardsOnDeck_SLIDER_MAX = 30;


    //================================
    // Rahmenbedingungen für Karten

    //Eigenschaften
    public static int Max_Anzahl_katProKarte_classic = 4;
    public static int Max_Anzahl_katProKarte = 4; //Prüfe kartenGobj_nachEigenschaften
    public static int Max_Anzahl_katProKarte_SLIDER_MIN = 1;
    public static int Max_Anzahl_katProKarte_SLIDER_MAX = 9; //aufräumen
    public static int Max_Anzahl_katProKarte_Classic =4;

    //Werte
    public static int numberofUnitsPerKat_max = 3;
    public static int numberofUnitsPerKat_max_classic = 3;
    public static int numberofUnitsPerKat_max_SLIDER_MIN = 1;
    public static int numberofUnitsPerKat_max_SLIDER_MAX = 35; //35 default werte für winkel und anzahl.

    //
    // 0=aus, 1=Immer, 2=wenn anvisiert,
    public static int Element1_visible = 1;
    public static int Element1_visible_classic = 1;

    //================================
    // SET
    public static int numberOfSelected_soll = 3;
    public static int numberOfSelected_soll_Classic = 3;
    public static int numberOfSelected_soll_SLIDER_MIN = 3;
    public static int numberOfSelected_soll_SLIDER_MAX = 6;
    public static int numberOfSelected_soll_gen = 1;
    public static int numberOfSelected_soll_gen_Íntro = 1;
    public static int numberOfSelected_soll_gen_Classic = 1;
    public static int numberOfSelected_soll_gen_SLIDER_MIN = 0;
    public static int numberOfSelected_soll_gen_SLIDER_MAX = 15;

    //================================
    // Kostueme und Kartenaussehen
    //Karten Inhalt
    // 0=Neon3x3, 1=Neon, 2=Pfeile
    public static int kartenKostuem_ID = 2;
    public static int kartenKostuem_Pointer = 1;
    public static int kartenKostuem_Pointer_ClassicDefault = 0;
    public static int kartenKostuem_Anzahl = 4;
    public const int kartenKostuem_Anzahl_const = 4;
    public static int[]     arr_Kostuem_ID =    new int[kartenKostuem_Anzahl_const] { 0, 1, 2, 3 };
    public static string[] arr_Kostuem_Name = new string[kartenKostuem_Anzahl_const] { "Neon", "Neon erweitert", "Pfeile", "Gestein" };
    public static bool[] arr_Kostuem_klassisch = new bool[kartenKostuem_Anzahl_const] { true, true, false, false};
    public static string kostuemTyp_klassisch = "Klassisch";


    //Karten Hintergrund
    
    public static int kartenKostuem_HG_ID = 4;
    public static int[] arr_kartenKostuem_HG_ID = new int[kartenKostuem_HG_Kat_Anzahl_const] { 0, 0, 0, 0 };
    public static int[] arr_kartenKostuem_HG_Pointer = new int[kartenKostuem_HG_Kat_Anzahl_const] {0,5,0,0 };
    public static int[] arr_kartenKostuem_HG_Pointer_Default = new int[kartenKostuem_HG_Kat_Anzahl_const] { 0, 0, 0, 0 };


    public static int kartenKostuem_HG_Kat_ID = 1;
    public static int kartenKostuem_HG_Kat_Pointer = 1;
    public static int kartenKostuem_HG_Kat_Pointer_Default = 1;
    public static int kartenKostuem_HG_Kat_Anzahl = 4;
    public const int kartenKostuem_HG_Kat_Anzahl_const = 4;
    public static int[] arr_Kostuem_HG_Kat_ID = new int[kartenKostuem_HG_Kat_Anzahl_const] { 0, 1, 2, 3 };
    public static string[] arr_Kostuem_HG_Kat_Name = new string[kartenKostuem_HG_Kat_Anzahl_const] { "Andere", "Abstrakt", "Rock Raiders", "Fotos" };

    public const int _0_kartenKostuem_HG_Anzahl_const = 1;
    public static int[] _0_arr_Kostuem_HG_ID = new int[_0_kartenKostuem_HG_Anzahl_const] {0 };
public const int _1_kartenKostuem_HG_Anzahl_const = 7;
    public static int[] _1_arr_Kostuem_HG_ID = new int[_1_kartenKostuem_HG_Anzahl_const] { 0, 1, 6, 2, 3, 4, 5 };
public const int _2_kartenKostuem_HG_Anzahl_const = 22;
    public static int[] _2_arr_Kostuem_HG_ID = new int[_2_kartenKostuem_HG_Anzahl_const] { 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20, 21, 22, 23, 24, 25, 26, 27, 28 };
    public const int _3_kartenKostuem_HG_Anzahl_const = 1;
    public static int[] _3_arr_Kostuem_HG_ID = new int[_3_kartenKostuem_HG_Anzahl_const] {0 };




    //=================================
    // EDITOR
    public static string Editor_NameCardslots = "Kartenplatz_00_0000";
    public static int Editor_NameCardslots_fieldID_howmany0 = 2;
    public static int Editor_NameCardslots_fieldID_HowManyCharsBetween = 1;
    //public static string Editor_NameCardslots_fieldID_whichCharsBetween = "_";
    public static int Editor_NameCardslots_howmany0 = 4;
    public static string Editor_NameCardslots_mu = "KarteMultiplier_00_0000";
    public static int Editor_NameCardslots_mu_howmany0 = 4;
    public static string Text_Kat_UT = "Werte: "; //"Verfügbare Werte: ";
    public static int Max_Anzahl_Versuche_KartePlatzieren = 10000;


    //================================
    // Farben
    //Rot, Grün, Blau
    public static Color colOnline = new Color(148f / 255f, 229f / 255f, 156f / 255f, 1f);
    public static Color colOffline = new Color(255f / 255f, 255f / 255f, 255f / 255f, 1f);
    public static Color colPressed = new Color(133f / 255f, 140f / 255f, 107f / 255f, 1f);
    public static Color colSelect = new Color(133f / 255f, 125f / 255f, 107f / 255f, 1f);
    public static Color colClear = new Color(133f / 255f, 125f / 255f, 107f / 255f, 0f);
    public static Color colOptionEnabled = new Color(148f / 255f, 229f / 255f, 156f / 255f, 0.3f);
    public static Color colOptionEnabled2 = new Color(148f / 255f, 229f / 255f, 156f / 255f, 0.6f);
    //-----------------------------------------------------------------------------------------
    public static Color cred;

    //================================
    //Vorbelegungen
    public static string[] classicSET = new string[] { "Füllung","Form", "Farbe_", "Anzahl_"  };
    public static string[] classicSET_1 = new string[] { "Leer", "Ganz", "Streifen" };
    public static string[] classicSET_2 = new string[] { "Oval", "Raute", "S" };

    //Farben
    public static string farbe_gen = "Farbe_";
    public static Color[] dyn_farbe = new Color[9] {Color.green, Color.red, Color.blue, Color.yellow, Color.white, Color.magenta, Color.cyan, Color.gray, Color.black};
    //Anzahl
    public static string anzahl_gen = "Anzahl_";
    public static int[] dyn_anzahl = new int[35] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20, 21, 22, 23, 24, 25, 26, 27, 28, 29, 30, 31, 32, 33, 34, 35};
    //Ausrichtung
    public static string ausrichtung_gen = "Ausrichtung_";
    public static int[] dyn_ausrichtung = new int[35] { 0, 40, -40, 60, -60, 20, -20, 80, -80, 10, -10, 30, -30, 50, -50, 70, -70, 86, -86, 5, -5, 15, -15, 25, -25, 35, -35, 45, -45, 55, -55, 65, -65, 75, -75 };

    //================================
    //Weitere


    public static int Max_Anzahl_Karten = 8;

}
