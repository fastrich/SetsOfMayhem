using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class config_parameters {

    //================================
    // Intro
    public static int Intro_waitTime = 3;


    //================================
    //ImSpiel
    public static int KartenInfosAnzeigen = 0;
    public static int Set_InfosAnzeigen_Anzahl = 0;
    public static int Set_InfosAnzeigen_Anzahl_Classic = 0;

    //================================
    // Describing Field 
    public static int Game_numberOfCardsOnDeck_Classic = 9;
    public static int Game_numberOfCardsOnDeck = 9;
    public static int Game_numberOfCardsOnDeck_SLIDER_MIN = 4;
    public static int Game_numberOfCardsOnDeck_SLIDER_MAX = 30;


    //================================
    // Rahmenbedingungen f�r Karten

    //Eigenschaften
    public static int Max_Anzahl_katProKarte_classic = 4;
    public static int Max_Anzahl_katProKarte = 4; //Pr�fe kartenGobj_nachEigenschaften
    public static int Max_Anzahl_katProKarte_SLIDER_MIN = 1;
    public static int Max_Anzahl_katProKarte_SLIDER_MAX = 9;

    //Werte
    public static int numberofUnitsPerKat_max = 3;
    public static int numberofUnitsPerKat_max_classic = 3;
    public static int numberofUnitsPerKat_max_SLIDER_MIN = 1;
    public static int numberofUnitsPerKat_max_SLIDER_MAX = 35; //35 default werte f�r winkel und anzahl.


    //================================
    // SET
    public static int numberOfSelected_soll = 3;
    public static int numberOfSelected_soll_Classic = 3;
    public static int numberOfSelected_soll_SLIDER_MIN = 3;
    public static int numberOfSelected_soll_SLIDER_MAX = 6;
    public static int numberOfSelected_soll_gen = 1;
    public static int numberOfSelected_soll_gen_Classic = 1;
    public static int numberOfSelected_soll_gen_SLIDER_MIN = 0;
    public static int numberOfSelected_soll_gen_SLIDER_MAX = 15;



    //=================================
    // EDITOR
    public static string Editor_NameCardslots = "Kartenplatz_0000";
    public static int Editor_NameCardslots_howmany0 = 4;
    public static string Editor_NameCardslots_mu = "KarteMultiplier_0000";
    public static int Editor_NameCardslots_mu_howmany0 = 4;
    public static string Text_Kat_UT = "Verf�gbare Werte: ";
    public static int Max_Anzahl_Versuche_KartePlatzieren = 1000;


    //================================
    // Farben
    //Rot, Gr�n, Blau
    public static Color colOnline = new Color(148f / 255f, 229f / 255f, 156f / 255f, 1f);
    public static Color colOffline = new Color(255f / 255f, 255f / 255f, 255f / 255f, 1f);
    public static Color colPressed = new Color(133f / 255f, 140f / 255f, 107f / 255f, 1f);
    public static Color colSelect = new Color(133f / 255f, 125f / 255f, 107f / 255f, 1f);
    public static Color colClear = new Color(133f / 255f, 125f / 255f, 107f / 255f, 0f);
    public static Color colOptionEnabled = new Color(148f / 255f, 229f / 255f, 156f / 255f, 0.3f);
    //-----------------------------------------------------------------------------------------
    public static Color cred;

    //================================
    //Vorbelegungen
    public static string[] classicSET = new string[] { "F�llung","Form", "Farbe_", "Anzahl_"  };
    public static string[] classicSET_1 = new string[] { "Leer", "Voll", "Streifen" };
    public static string[] classicSET_2 = new string[] { "Kreis", "Raute", "S" };

    //Farben
    public static string farbe_gen = "Farbe_";
    public static Color[] dyn_farbe = new Color[9] {Color.red, Color.green, Color.blue, Color.yellow, Color.white, Color.magenta, Color.cyan, Color.gray, Color.black};
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
