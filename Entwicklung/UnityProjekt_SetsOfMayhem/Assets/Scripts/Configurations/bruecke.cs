using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static config_parameters;
using static medium;

public static class bruecke {

    private static int anzahlVersucheDatenEinzutragen = 2;

    public static int howManyExtraDecks_withSizeOfSETS = 5;


    //------------------------------------------------------------------------------------------------------------
    // FieldOfCards
    //------------------------------------------------------------------------------------------------------------
    // KnownFields: 0=MainGame, 1=Intro, 2=MainMenue, 3=LastSET, 4=Player2, 
    public static int[,] fieldOfCards_Field = new int[Game_numberOfCardsOnDeck, Max_Anzahl_katProKarte];
    public static int[,,] fieldOfCards_SETs = new int[howManyExtraDecks_withSizeOfSETS, numberOfSelected_soll, Max_Anzahl_katProKarte];
   

    public static void fieldOfCards_SetIt(int WhichField, int ID_OfCardInField, int ID_OfCatOfCard, int neuerWert)
    {
        for (int i = 0; i < anzahlVersucheDatenEinzutragen; i++) {
            try {
                if (WhichField == 0) { fieldOfCards_Field[ID_OfCardInField, ID_OfCatOfCard] = neuerWert; return; }
                if (WhichField > 0) { fieldOfCards_SETs[WhichField - 1, ID_OfCardInField, ID_OfCatOfCard] = neuerWert; return; }
            }  catch  {  fieldOfCards_SizeIt(WhichField);  }
        }
    }
    public static int fieldOfCards_GetIt(int WhichField, int ID_OfCardInField, int ID_OfCatOfCard)
    {
        for (int i = 0; i < anzahlVersucheDatenEinzutragen; i++)   {
            try
            {
                if (WhichField == 0) { return fieldOfCards_Field[ID_OfCardInField, ID_OfCatOfCard]; }
                if (WhichField > 0) { return fieldOfCards_SETs[WhichField - 1, ID_OfCardInField, ID_OfCatOfCard]; }
            } catch { fieldOfCards_SizeIt(WhichField); }
        }
        return -1;
    }
    public static int fieldOfCards_LengthIt(int WhichField, int dim)

    {
        if (WhichField == 0) { if (dim == 0) { return 1; } if (dim == 1) { return Game_numberOfCardsOnDeck; } if (dim == 2) { return Max_Anzahl_katProKarte; } }
        if (WhichField > 0) { if (dim == 0) { return howManyExtraDecks_withSizeOfSETS; } if (dim == 1) { return numberOfSelected_soll; } if (dim == 2) { return Max_Anzahl_katProKarte; } }

        return -1;
    }
    public static void fieldOfCards_SizeIt(int WhichField)
    {
        fieldOfCards_RenewIt();
    }
    public static void fieldOfCards_RenewIt()
    {
        fieldOfCards_Field = new int[Game_numberOfCardsOnDeck, Max_Anzahl_katProKarte];
        fieldOfCards_SETs = new int[howManyExtraDecks_withSizeOfSETS, numberOfSelected_soll, Max_Anzahl_katProKarte];
    }


    //------------------------------------------------------------------------------------------------------------
    // Array_cards_status
    //------------------------------------------------------------------------------------------------------------
    //0=nothing, 1=updated, 2=selected, 3=update, 4=update2, 5=OhneSelection_Neu, 6=OhneSelection2, 
    public static int[] array_cards_status_Field = new int[Game_numberOfCardsOnDeck];
    public static int[,] array_cards_status_SETs = new int[howManyExtraDecks_withSizeOfSETS, numberOfSelected_soll];

    public static void array_cards_status_SetIt(int WhichField, int ID_OfCardInField, int neuerWert)
    {
        for (int i = 0; i < anzahlVersucheDatenEinzutragen; i++)
        {
            try { 
            if (WhichField == 0) { array_cards_status_Field[ID_OfCardInField] = neuerWert; }
                if (WhichField > 0) { array_cards_status_SETs[WhichField - 1, ID_OfCardInField] = neuerWert; return; }
            }
            catch { fieldOfCards_SizeIt(WhichField); }
        }

    }
    public static int array_cards_status_GetIt(int WhichField, int ID_OfCardInField)
    {
        for (int i = 0; i < anzahlVersucheDatenEinzutragen; i++)
        {
            try{
                if (WhichField == 0) { return array_cards_status_Field[ID_OfCardInField]; }
                if (WhichField > 0) { return array_cards_status_SETs[WhichField - 1, ID_OfCardInField]; }
            }
            catch { fieldOfCards_SizeIt(WhichField); }
        }


        return -1;
    }
    public static int array_cards_status_LengthIt(int WhichField, int dim)
    {
        if (WhichField == 0) { if (dim == 0) { return 1; } if (dim == 1) { return Game_numberOfCardsOnDeck; } }
        if (WhichField > 0) { if (dim == 0) { return howManyExtraDecks_withSizeOfSETS; } if (dim == 1) { return numberOfSelected_soll; } }
        
        return -1;
    }
    public static void array_cards_status_SizeIt(int WhichField)
    {
        array_cards_status_RenewIt();
    }
    public static void array_cards_status_RenewIt()
    {
        array_cards_status_Field = new int[Game_numberOfCardsOnDeck];
        array_cards_status_SETs = new int[howManyExtraDecks_withSizeOfSETS, numberOfSelected_soll];
    }

    //------------------------------------------------------------------------------------------------------------
    // Array_cards_selected
    //------------------------------------------------------------------------------------------------------------
    //0=Player1, 1=LastSet, 2=Player2, 3=SetFound
    public static int[,] array_cards_selected = new int[howManyExtraDecks_withSizeOfSETS, numberOfSelected_soll];
    public static void array_cards_selected_SetIt(int WhichField, int ID_OfCardInField, int neuerWert)
    {
        for (int i = 0; i < anzahlVersucheDatenEinzutragen; i++)
        {
            try
            {
                array_cards_selected[WhichField, ID_OfCardInField] = neuerWert; return;
            }
            catch { array_cards_selected_SizeIt(WhichField); }
        }

    }
    public static int array_cards_selected_GetIt(int WhichField, int ID_OfCardInField)
    {
        for (int i = 0; i < anzahlVersucheDatenEinzutragen; i++)
        {
            try
            {
                return array_cards_selected[WhichField, ID_OfCardInField]; 
            }
            catch { array_cards_selected_SizeIt(WhichField); }
        }


        return -1;
    }
    public static int array_cards_selected_LengthIt(int WhichField, int dim)
    {
        if (dim == 0) { return howManyExtraDecks_withSizeOfSETS; } if (dim == 1) { return numberOfSelected_soll; } 

        return -1;
    }
    public static void array_cards_selected_SizeIt(int WhichField)
    {
        array_cards_selected_RenewIt();
    }
    public static void array_cards_selected_RenewIt()
    {
        array_cards_selected = new int[howManyExtraDecks_withSizeOfSETS, numberOfSelected_soll];
    }

}
