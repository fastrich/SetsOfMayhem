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




    public static void scanForSelected_withCardIDs(int whichField)
    {
        numberOfSelected = 0;

        for (int i = 0; i < array_cards_status_LengthIt(whichField,1); i++)
        {
            if (array_cards_status_GetIt(whichField,i) == 2)
            {
                Debug.Log("scan: " + array_cards_used_with_id[i]);
                array_cards_selected_SetIt(0, numberOfSelected, array_cards_used_with_id[i]);
                numberOfSelected++;

            }
            else
            {
                array_cards_selected_SetIt(0, numberOfSelected,-1);
            }
        }
    }
    public static void scanForSelected(int whichField)
    {
        numberOfSelected = 0;

        for (int i = 0; i < array_cards_status_LengthIt(whichField, 1); i++)
        {
            if (array_cards_status_GetIt(whichField, i) == 2)
            {
                array_cards_selected_SetIt(0, numberOfSelected, i);
                numberOfSelected++;
            }
        }
    }

    public static int checkForSet()
    {
        return 0;
    }

    public static int checkForSetInField(int whichField, bool saveFirstFoundSET)
    {
        int z = 0;
        int check1;
        int new1 = 0;
        //int[] intarr = new int[numberOfSelected_soll];
        

        if (numberOfSelected_soll > Game_numberOfCardsOnDeck) { return 0; }
        for (int j1 = numberOfSelected_soll - 1; j1 < Game_numberOfCardsOnDeck; j1++)
        {
            
            array_cards_selected_temp_for_gen_field[0] = j1;
            if (1 == numberOfSelected_soll) { if (checkForSetinSelected_gen(whichField)) { z++; if (saveFirstFoundSET) { array_cards_selected_SetIt(3, 0, j1); } } }
            for (int j2 = 0; j2 < j1; j2++)
            {
                
                array_cards_selected_temp_for_gen_field[1] = j2;
                if (2 == numberOfSelected_soll) { if (checkForSetinSelected_gen(whichField)) { z++; if (saveFirstFoundSET) { array_cards_selected_SetIt(3, 0, j1); array_cards_selected_SetIt(3, 1, j2); } } }
                for (int j3 = 0; j3 < j2; j3++)
                {
                        
                    array_cards_selected_temp_for_gen_field[2] = j3;
                    if (3 == numberOfSelected_soll) { if (checkForSetinSelected_gen(whichField)) { z++; if (saveFirstFoundSET) { array_cards_selected_SetIt(3, 0, j1); array_cards_selected_SetIt(3, 1, j2); array_cards_selected_SetIt(3, 2, j3); } } }
                    for (int j4 = 0; j4 < j3; j4++)
                    {
                        array_cards_selected_temp_for_gen_field[3] = j4;
                        if (4 == numberOfSelected_soll) { if (checkForSetinSelected_gen(whichField)) { z++; if (saveFirstFoundSET) { array_cards_selected_SetIt(3, 0, j1); array_cards_selected_SetIt(3, 1, j2); array_cards_selected_SetIt(3, 2, j3); array_cards_selected_SetIt(3, 3, j4); } } }
                        for (int j5 = 0; j5 < j4; j5++)
                        {
                            array_cards_selected_temp_for_gen_field[4] = j5;
                            if (5 == numberOfSelected_soll) { if (checkForSetinSelected_gen(whichField)) { z++; if (saveFirstFoundSET) { array_cards_selected_SetIt(3, 0, j1); array_cards_selected_SetIt(3, 1, j2); array_cards_selected_SetIt(3, 2, j3); array_cards_selected_SetIt(3, 3, j4); array_cards_selected_SetIt(3, 4, j5); } } }
                             
                             for (int j6 = 0; j6 < j5; j6++)
                             {
                                    array_cards_selected_temp_for_gen_field[5] = j6;
                                    if (6 == numberOfSelected_soll) { if (checkForSetinSelected_gen(whichField)) { z++; if (saveFirstFoundSET) { array_cards_selected_SetIt(3, 0, j1); array_cards_selected_SetIt(3, 1, j2); array_cards_selected_SetIt(3, 2, j3); array_cards_selected_SetIt(3, 3, j4); array_cards_selected_SetIt(3, 4, j5); array_cards_selected_SetIt(3, 5, j6); } } }
                             }
                             
                        }
                    }
                }
                
            }
            
        }
        
        return z;
    }

    

    public static bool checkForSetinSelected(int whichField)
    {
        return checkForSetinSelected_VersionWithField(whichField);
    }

    public static bool checkForSetinSelected_gen_3(int whichField, int pos)
    {
        for (int i2 = 0; i2 < Game_numberOfCardsOnDeck; i2++)
        {
            if (checkForSetinSelected_gen_2(whichField, pos))
            {
                return true;
            }
        }

        return false;
    }

    public static bool checkForSetinSelected_gen_2(int whichField, int pos)
    {
        array_cards_selected_temp_for_gen_field[0] = pos;
        for (int i = 1; i < numberOfSelected_soll; i++)
        {
            int new1 = (int)UnityEngine.Random.Range(0, Game_numberOfCardsOnDeck);
            int check1;
            for (int i2 = 0; i2 < Max_Anzahl_Versuche_KartePlatzieren; i2++)
            {
                check1 = 0;
                for (int hui = 0; hui < i; hui++)
                {
                    if (new1 == array_cards_selected_temp_for_gen_field[i] || array_cards_status_GetIt(whichField, new1) == 0)
                    {
                        check1++;
                    }
                }


                if (check1 == 0) { break; }


                new1 = (int)UnityEngine.Random.Range(0, Game_numberOfCardsOnDeck - 1);
            }
            array_cards_selected_temp_for_gen_field[i] = new1;
        }


        return checkForSetinSelected_gen(whichField);
    }

    public static bool checkForSetinSelected_gen(int whichField)
    {

        return checkForSetinSelected_gen_VersionWithField(whichField);
    }


    public static bool checkForSetinSelected_gen_VersionWithField(int whichField)
    {
        for (int i = 0; i < numberOfSelected_soll; i++)
        {
            for (int ik = 0; ik < Max_Anzahl_katProKarte; ik++)
            {
                int temp_a = 0;
                for (int j = 0; j < numberOfSelected_soll; j++)
                {
                    if (i != j)
                    {

                        if (fieldOfCards_GetIt(whichField, array_cards_selected_temp_for_gen_field[i], ik) == fieldOfCards_GetIt(whichField, array_cards_selected_temp_for_gen_field[j], ik))
                        {
                            temp_a++;
                        }
                    }
                }
                if (temp_a == 0 || temp_a == numberOfSelected_soll - 1)
                {
                }
                else
                {
                    //Debug.Log("Kein_Set");
                    return false;
                }
            }
        }
        //Debug.Log("Ein_Set");
        return true;
    }
    public static bool checkForSetinSelected_VersionWithField(int whichField)
    {
        scanForSelected(whichField);
        for (int i = 0; i < numberOfSelected; i++)
        {
            for (int ik = 0; ik < Max_Anzahl_katProKarte; ik++)
            {
                int temp_a = 0;
                for (int j = 0; j < numberOfSelected; j++)
                {
                    if (i != j)
                    {

                        if (fieldOfCards_GetIt(whichField, array_cards_selected_GetIt(0,i), ik) == fieldOfCards_GetIt(whichField, array_cards_selected_GetIt(0,j), ik))
                        {
                            temp_a++;
                        }
                    }
                }
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

    public static void CheckAndFillCardField(int whichField)
    {
        bool a = true;
        int lastNumOfSets = 0;
        bool c=false;
        for (int i2 = 0; i2 < Max_Anzahl_Versuche_KartePlatzieren; i2++)
        {
            c = false;
            for (int i = 0; i < array_cards_status_LengthIt(whichField, 1); i++)
            {
                if (array_cards_status_GetIt(whichField, i) == 0)
                {
                    FillCardInField(whichField, i);
                    c = true;
                }
            }
            if (c) { SetsFoundInField_gen = checkForSetInField(whichField, false); }
            if (SetsFoundInField_gen >= numberOfSelected_soll_gen) { a = false; break; }
            if (lastNumOfSets < SetsFoundInField_gen) { saveField2backup(whichField); lastNumOfSets = SetsFoundInField_gen; }
        }
        if (a) { Debug.Log("SetInField MaxVersuche erreicht"); loadField2backup(whichField); SetsFoundInField_gen = lastNumOfSets; }
        if (c) { player2_thereIsNewField(); }

        //Debug.Log("b" + SetsFoundInField_gen);
        for (int i = 0; i < array_cards_status_LengthIt(whichField, 1); i++)
        {
            if (array_cards_status_GetIt(whichField,i) == 0)
            {
                array_cards_status_SetIt(whichField,i,3);
            }
        }
    }

    public static void saveField2backup(int whichField)
    {
        fieldOfCards_backup = new int[Game_numberOfCardsOnDeck, Max_Anzahl_katProKarte];
        for (int i = 0; i < Game_numberOfCardsOnDeck; i++)
        {
            for (int ii = 0; ii < Max_Anzahl_katProKarte; ii++)
            {
                fieldOfCards_backup[i, ii] = fieldOfCards_GetIt(whichField, i, ii);


            }
        }
    }
    public static void loadField2backup(int whichField)
    {
        for (int i = 0; i < Game_numberOfCardsOnDeck; i++)
        {
            for (int ii = 0; ii < Max_Anzahl_katProKarte; ii++)
            {
                fieldOfCards_SetIt(whichField, i, ii, fieldOfCards_backup[i, ii]);


            }
        }
    }


    public static void FillCardInField(int whichField, int pos)
    {
        for (int i2 = 0; i2 < Max_Anzahl_Versuche_KartePlatzieren; i2++)
        {
            for (int i = 0; i < Max_Anzahl_katProKarte; i++)
            {
                fieldOfCards_SetIt(whichField, pos, i ,(int)UnityEngine.Random.Range(0, numberofUnitsPerKat_max));
                //fieldOfCards[pos, i] = 0;
            }
            //Debug.Log(Max_Anzahl_katProKarte +" "+ numberofUnitsPerKat_max +" "+ (Mathf.Pow(Max_Anzahl_katProKarte,numberofUnitsPerKat_max)+ " "+ Mathf.Pow(numberofUnitsPerKat_max, Max_Anzahl_katProKarte)));
            //if (!checkForDoubles_rel(pos) && checkForSetinSelected_gen_3(pos) ) { return; }
            //if (!checkForDoubles_rel(pos) && i2 > (Max_Anzahl_Versuche_KartePlatzieren*3/4 )) { Debug.Log("MaxVersuche_Sets erreicht"); return; }
            if (!checkForDoubles_rel(whichField, pos)) { return; }
            if (Game_numberOfCardsOnDeck >= Mathf.Pow(Max_Anzahl_katProKarte, numberofUnitsPerKat_max)) { return; }
            if (Game_numberOfCardsOnDeck >= Mathf.Pow(numberofUnitsPerKat_max, Max_Anzahl_katProKarte)) { return; }
        }

        Debug.Log("MaxVersuche erreicht");
    }

    public static bool checkForDoubles(int whichField)
    {
        int d;
        //int z = 0 ;
        for (int i = 0; i < array_cards_status_LengthIt(whichField, 1); i++)
        {

            for (int i2 = 0; i2 < array_cards_status_LengthIt(whichField, 1); i2++)
            {

                d = 0;
                for (int ij = 0; ij < Max_Anzahl_katProKarte; ij++)
                {

                    if (i != i2 && fieldOfCards_GetIt(whichField, i, ij) == 0 && fieldOfCards_GetIt(whichField, i, ij) == fieldOfCards_GetIt(whichField, i2, ij))
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

    public static bool checkForDoubles_rel(int whichField, int k)
    {
        int d;
        //int z = 0 ;

        for (int i2 = 0; i2 < array_cards_status_LengthIt(whichField, 1); i2++)
        {

            d = 0;
            for (int ij = 0; ij < Max_Anzahl_katProKarte; ij++)
            {

                if (k != i2 && fieldOfCards_GetIt(whichField, k, ij) == fieldOfCards_GetIt(whichField, i2, ij))
                {
                    d++;
                }


            }
            //Debug.Log("M" + d + "  "+ Max_Anzahl_katProKarte);
            if (d >= Max_Anzahl_katProKarte) { return true; }
        }


        return false;
    }






    public static void checkForPlayerSetSelection(int whichField)
    {
        whichField=0;
        scanForSelected( whichField);

        if (numberOfSelected >= numberOfSelected_soll)
        {
            bool cfs = checkForSetinSelected(whichField);


            if (cfs == true)
            {
                gefundeneSets++;
                //Debug.Log("k: " + array_cards_selected_LengthIt(0, 1) + " " + fieldOfCards_LengthIt(0, 2));
                for (int i=0; i < array_cards_selected_LengthIt(0,1); i++)
                {
                    //array_cards_selected_SetIt(1, i, array_cards_selected_GetIt(0,i));
                    array_cards_status_SetIt(3, i, 5);

                    for (int ik = 0; ik < fieldOfCards_LengthIt(0, 2); ik++)
                    {
                        //Debug.Log("aj: "+ array_cards_selected_GetIt(0, i) + " "+ik);
                        fieldOfCards_SetIt(3, i, ik, fieldOfCards_GetIt(0, array_cards_selected_GetIt(0, i), ik));
                    }

                    array_cards_status_SetIt(whichField, array_cards_selected_GetIt(0, i), 0);

                }


            }
            else { korrekturZeit += Wertung_Strafzeit_inSekunden; }
                
            
            for (int i = 0; i < array_cards_status_LengthIt(whichField, 1); i++)
            {
                    //ebug.Log(array_cards_selected + " ; " + array_cards_status + " ; " + array_cards_used_with_id);
                    if (array_cards_status_GetIt(whichField,i) == 2)
                    {
                        array_cards_status_SetIt(whichField, i, 1);
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
        fieldOfCards_RenewIt();
        array_cards_status_RenewIt();
        array_cards_used_with_id = new int[Game_numberOfCardsOnDeck]; //Kartenabbild_ID

        array_cards_selected_RenewIt();
                                                                 //kategorien_n_sorted = new string[Max_Anzahl_katProKarte_SLIDER_MAX + 10];
                                                                 //werte_n_sorted = new string[Max_Anzahl_katProKarte_SLIDER_MAX + 10, numberofUnitsPerKat_max_SLIDER_MAX + 1];

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

        if (false)
        {
            for (int i = 0; i < 5; i++)
            {

                for (int ii = 0; ii < 2; ii++)
                {
                    Debug.Log("hier " + ii + " " + i + " " + werte_n_sorted[ii, i]);
                }
            }
        }



        //Debug.Log("kat "+kategorien_n[0]+ kategorien_n[1]);
    }


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
    public static void UIKHG_Kaz_update(bool random)
    { 
        range1[0] = _0_arr_Kostuem_HG_ID.Length;
        range1[1] = _1_arr_Kostuem_HG_ID.Length;
        range1[2] = _2_arr_Kostuem_HG_ID.Length;
        range1[3] = _3_arr_Kostuem_HG_ID.Length;
        range1[4] = _4_arr_Kostuem_HG_ID.Length;

        if (random) { arr_kartenKostuem_HG_Pointer[kartenKostuem_HG_Kat_ID] = (int)UnityEngine.Random.Range(0, range1[kartenKostuem_HG_Kat_ID] - 1); }

        for (int i = 0; i < range1.Length; i++)
        {
            if (arr_kartenKostuem_HG_Pointer[i] < 0) { arr_kartenKostuem_HG_Pointer[i] = (range1[i] - 1); }
            if (arr_kartenKostuem_HG_Pointer[i] > (range1[i] - 1)) { arr_kartenKostuem_HG_Pointer[i] = 0; }
        }
        arr_kartenKostuem_HG_ID[0] = _0_arr_Kostuem_HG_ID[arr_kartenKostuem_HG_Pointer[0]];
        arr_kartenKostuem_HG_ID[1] = _1_arr_Kostuem_HG_ID[arr_kartenKostuem_HG_Pointer[1]];
        arr_kartenKostuem_HG_ID[2] = _2_arr_Kostuem_HG_ID[arr_kartenKostuem_HG_Pointer[2]];
        arr_kartenKostuem_HG_ID[3] = _3_arr_Kostuem_HG_ID[arr_kartenKostuem_HG_Pointer[3]];
        arr_kartenKostuem_HG_ID[4] = _4_arr_Kostuem_HG_ID[arr_kartenKostuem_HG_Pointer[4]];


    }

    public static int Karte_get_placeID_From_ObjektName(GameObject gobj_mit_placeID)
    {
        string name2 = gobj_mit_placeID.name;
        int laenge = Editor_NameCardslots_howmany0;
        name2 = name2.Substring(name2.Length - laenge, laenge);
       // Debug.Log("n "+name2);
        // return 0;
        int i = (int)int.Parse(name2);
       // Debug.Log("ni " + i);
        return i;

        //Debug.Log("i "+place_id);
    }
    public static int Karte_get_FieldID_From_ObjektName(GameObject gobj_mit_placeID)
    {
        int hui = Editor_NameCardslots_howmany0 + Editor_NameCardslots_fieldID_HowManyCharsBetween;
        string name2 = gobj_mit_placeID.name;
        int laenge= Editor_NameCardslots_fieldID_howmany0;
        int anfang = name2.Length - hui - laenge;
        int i;
        name2 = name2.Substring(anfang, laenge);
        //Debug.Log("n "+name2 + " a "+ anfang + " h "+ hui );
        // return 0;
        i = (int)int.Parse(name2);
        //Debug.Log("ni " + i);
        return i;
        //Debug.Log("i "+place_id);

    }
    public static string buildCardNameWithPlaceIDinName(int whichField, int placeID)
    {
        
        int hui =  Editor_NameCardslots_howmany0 + Editor_NameCardslots_fieldID_HowManyCharsBetween;
        string name0 = Editor_NameCardslots;
        string name00 = name0;
        //return name0;
        string name3 = whichField.ToString();
        string name4 = placeID.ToString();
        string name1 = name0.Substring(0, name0.Length - name3.Length - hui);
        //Debug.Log(name1);
        string name2 = name00.Substring(name00.Length - hui, hui);
        //Debug.Log(name2);
        name1 = name1 + name3;
        //Debug.Log(name1);
        name1 = name1 + name2;
        //Debug.Log(name1);
        name1 = name1.Substring(0, name1.Length - name4.Length);
        //Debug.Log(name1);
        name1 = name1 + name4;
        //Debug.Log(name1);
        
        return name1;
        
    }


    public static void CreateAndDistributeCardsOnScreen(int whichField, GameObject kartenPrefab, Canvas canvasForCards)
    {
        string name1;
        int k = 0;
        int n = array_cards_status_LengthIt(whichField,1); 

        int nb = (int)Mathf.Ceil(Mathf.Sqrt(n));
        int nh = (int)Mathf.Ceil((float)n / (float)nb);
        if (n == 3)
        {
            nb = 3; nh = 1;
        }

        float jh = 0;
        int jb = 0;
        //Debug.Log("nh, nb" + nh + " " + nb);
        //CreateGameObjectFromPrefab(kartenPrefab, canvasForCards, cornerTopRight, cornerBottemLeft);
        Vector2 cornerTopRight = new Vector2(0.5f, 0.6f);
        Vector2 cornerBottemLeft = new Vector2(0, 0);

        while (k < n)
        {

            name1= buildCardNameWithPlaceIDinName(whichField, k);
            cornerTopRight = new Vector2((float)(jb + 1) / nb, (float)1 - (jh / nh));
            cornerBottemLeft = new Vector2((float)jb / nb, (float)1 - ((1 + jh) / nh));
            //Debug.Log("k" + k+ " ctr" + cornerTopRight + " cbl " + cornerBottemLeft);
            CreateGameObjectFromPrefab(name1, kartenPrefab, canvasForCards, cornerTopRight, cornerBottemLeft);

            jb++;
            if (jb >= nb)
            {
                jh++;
                jb = 0;
            }
            k++;

        }


    }

    public static void CreateAndDistributeCardsOnScreen_v2(int whichFieldIam, GameObject vorlageGobj, Canvas canvasEntp, int anzahl, GameObject vater)
    {
        //activChilds(canvasEntp.gameObject, false);
        //Debug.Log(karteScript.GetComponent<karte>().anzahl_id);
        // if (Element1_visible == 1 || (Element1_visible == 2 && array_cards_status_GetIt(whichFieldIam, karteScript.GetComponent<karte>().place_id) == 2))
        //int anzahl = array_cards_status_LengthIt(whichFieldIam, 1);
        List<int> hui_list = new List<int>();
        //Debug.Log(kinder[0]);
        if (true)
        {
            int p_max = 2;
            for (int p = 0; p < (p_max); p++) {
                bool hates;
                float jh = 0;
                int jb = 0;

                for (int ii = 0; ii < anzahl; ii++)
                {
                    hates = false;
                    for (int i = 0; i < vater.transform.childCount; i++)
                    {
                        string name2 = vater.transform.GetChild(i).name;
                        int j = Editor_NameCardslots_mu_howmany0;
                        name2 = name2.Substring(name2.Length - j, j);
                        try
                        {
                            if (ii == (int)Int32.Parse(name2))
                            {
                                hates = true;
                                if (p == p_max - 1) { hui_list.Add(i); }
                                //==========================================

                                int nb = (int)Mathf.Ceil(Mathf.Sqrt(anzahl));

                                int nh = (int)Mathf.Ceil((float)anzahl / (float)nb);

                                if (anzahl == 3)
                                {
                                    nb = 3; nh = 1;
                                }





                                Vector2 cornerTopRight = new Vector2(0.5f, 0.6f);
                                Vector2 cornerBottemLeft = new Vector2(0, 0);

                                cornerTopRight = new Vector2((float)(jb + 1) / nb, (float)1 - (jh / nh));
                                cornerBottemLeft = new Vector2((float)jb / nb, (float)1 - ((1 + jh) / nh));

                                vater.transform.GetChild(i).gameObject.GetComponent<RectTransform>().anchorMax = cornerTopRight;
                                vater.transform.GetChild(i).gameObject.GetComponent<RectTransform>().anchorMin = cornerBottemLeft;

                                jb++;
                                if (jb >= nb)
                                {
                                    jh++;
                                    jb = 0;
                                }

                                //``````````````````````````````````````
                                /*
                                var rotationvector = transform.rotation.eulerAngles;
                                rotationvector.z = dyn_ausrichtung[ausrichtung_wg];
                                transform.rotation = Quaternion.Euler(rotationvector);
                                for(int jj=0; jj< transform.GetChild(i).gameObject.transform.GetChildCount(); jj++)
                                transform.GetChild(i).gameObject.transform.rotation = Quaternion.Euler(rotationvector);
                                transform.GetChild(i).GetChildCount
                                */

                                //=========================================
                                vater.transform.GetChild(i).gameObject.SetActive(true);





                            };
                        }
                        catch { }



                    }
                    if (hates == false)
                    {
                        erstellen(ii, vorlageGobj, canvasEntp, whichFieldIam);
                    }


                }
            }
        }
        for (int i = 0; i < vater.transform.childCount; i++)
        {
           // Debug.Log("h +" +i+  ""+hui_list.Contains(i));
            if (!hui_list.Contains(i)) { vater.transform.GetChild(i).gameObject.SetActive(false); }
        }

    }


    public static void erstellen(int ii, GameObject vorlageGobj, Canvas canvasEntp, int whichField)
    {
        Vector2 cornerTopRight = new Vector2(0.5f, 0.6f);
        Vector2 cornerBottemLeft = new Vector2(0, 0);
        String name1 = buildCardNameWithPlaceIDinName(whichField, ii);
        CreateGameObjectFromPrefab(name1, vorlageGobj, canvasEntp, cornerTopRight, cornerBottemLeft);

    }
    public static void activChilds(GameObject gobj ,bool act)
    {
        for (int i = 0; i < gobj.transform.childCount; i++)
        {
            gobj.transform.GetChild(i).gameObject.SetActive(act);

        }
    }
}
