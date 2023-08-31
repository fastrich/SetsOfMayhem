using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using static medium;
using static kartenInformationen;
using static config_parameters;
using static algos;

public class kartenID_installer : MonoBehaviour
{
    private int numberOfKats = 0;

    public bool isDefaultCard;
    public GameObject TxtInfos_gross;
    public GameObject TxtInfos;
    public string Name;
    public string skin_ID;
    public string E1_n;
    public string E1_w;
    public string E2_n;
    public string E2_w;
    public string E3_n;
    public string E3_w;
    public string E4_n;
    public string E4_w;
    public string E5_n;
    public string E5_w;
    public string E6_n;
    public string E6_w;
    public string E7_n;
    public string E7_w;
    public string E8_n;
    public string E8_w;
    public string E9_n;
    public string E9_w;


    private int[] io = new int[Max_Anzahl_katProKarte];
   


    void OnEnable()
    {
         io = new int[Max_Anzahl_katProKarte];

        //array_cards_used_with_id[transform.parent.gameObject.GetComponent<karte>().place_id]=kartenID;
        numberOfKats = 0;
        numberOfKats+=ubersetzeEnum(E1_n, E1_w);
        numberOfKats+=ubersetzeEnum(E2_n, E2_w);
        numberOfKats+=ubersetzeEnum(E3_n, E3_w);
        numberOfKats += ubersetzeEnum(E4_n, E4_w);
        numberOfKats += ubersetzeEnum(E5_n, E5_w);
        numberOfKats += ubersetzeEnum(E6_n, E6_w);
        numberOfKats += ubersetzeEnum(E7_n, E7_w);
        numberOfKats += ubersetzeEnum(E8_n, E8_w);
        numberOfKats += ubersetzeEnum(E9_n, E9_w);
        //numberOfKats += ubersetzeEnum(E10_n, E10_w);

        //transform.parent.gameObject.GetComponent<karte>().
        //kartenGobj_nachEigenschaften_roh[transform.parent.gameObject.GetComponent<karte>().place_id, io[0], io[1], io[2], io[3], io[4], io[5], io[6], io[7], io[8], io[9]] = gameObject;

    }



    // Start is called before the first frame update
    void Start()
    {
        //array[transform.parent.gameObject.GetComponent<karte>().place_id, kartenID] = gameObject;
        

    }

    // Update is called once per frame
    void Update()
    {
        
        TxtInfos.GetComponent<TMP_Text>().text = WhatCard_String();
        TxtInfos_gross.GetComponent<TMP_Text>().text = WhatCard_String(); //WhatCard_String_g();

        if (isDefaultCard == true)
        {
            
            bool hasAChoosenOne = transform.parent.parent.gameObject.GetComponent<karte>().hasACard;
            for (int i = 0; i < transform.childCount; i++)
            {
                transform.GetChild(i).gameObject.SetActive(!hasAChoosenOne);
            }
        }
        else
        {

            bool isTheChoosenOne = false;
            isTheChoosenOne = IsTheChoosenOne();
            if (isTheChoosenOne)
            {
                transform.parent.parent.gameObject.GetComponent<karte>().hasACard = true;
            }
            for (int i = 0; i < transform.childCount; i++)
            {
                transform.GetChild(i).gameObject.SetActive(isTheChoosenOne);

            }
        }
    }

    int ubersetzeEnum(string kat, string wert)
    {
        int i = welcheNummerHatKat(kat);
        int j = i;
        if (i < 0) { i = 0; }
        welcheNummerHatWert(i, wert);
        /*
        int i = welcheNummerHatKat(kat);
        int j = i;
        if (i < 0) { i = 0; }
        io[i] = welcheNummerHatWert(i, wert);
        if (io[i] < 0) { io[i] = 0; }
        
        */
        if (j < 0) { return 0; } else { return 1; }
    }

    bool IsTheChoosenOne()
    {
        /*
            if (numberOfKatsOnCardsNeeded> numberOfKats) { return false; }
        
        
        for (int i = 0; i < Max_Anzahl_katProKarte; i++)
        {
            if (io[i] != fieldOfCards[transform.parent.parent.gameObject.GetComponent<karte>().place_id, i])
            {
                return false;
            }
        }
        */
        int zahler = 0;
        for (int i = 0; i < transform.parent.parent.gameObject.GetComponent<karte>().KartenWerteAlsString.Length; i++)
        {
            Debug.Log("katn" + i + "  " + kategorien_n_sorted[i] + " , " + transform.parent.parent.gameObject.GetComponent<karte>().KartenWerteAlsString[i]);
            if (kategorien_n_sorted[i] == E1_n && transform.parent.parent.gameObject.GetComponent<karte>().KartenWerteAlsString[i] == E1_w) { zahler++; }
            if (kategorien_n_sorted[i] == E2_n && transform.parent.parent.gameObject.GetComponent<karte>().KartenWerteAlsString[i] == E2_w) { zahler++; }
            if (kategorien_n_sorted[i] == E3_n && transform.parent.parent.gameObject.GetComponent<karte>().KartenWerteAlsString[i] == E3_w) { zahler++; }
            if (kategorien_n_sorted[i] == E4_n && transform.parent.parent.gameObject.GetComponent<karte>().KartenWerteAlsString[i] == E4_w) { zahler++; }
            if (kategorien_n_sorted[i] == E5_n && transform.parent.parent.gameObject.GetComponent<karte>().KartenWerteAlsString[i] == E5_w) { zahler++; }
            if (kategorien_n_sorted[i] == E6_n && transform.parent.parent.gameObject.GetComponent<karte>().KartenWerteAlsString[i] == E6_w) { zahler++; }
            if (kategorien_n_sorted[i] == E7_n && transform.parent.parent.gameObject.GetComponent<karte>().KartenWerteAlsString[i] == E7_w) { zahler++; }
            if (kategorien_n_sorted[i] == E8_n && transform.parent.parent.gameObject.GetComponent<karte>().KartenWerteAlsString[i] == E8_w) { zahler++; }
            if (kategorien_n_sorted[i] == E9_n && transform.parent.parent.gameObject.GetComponent<karte>().KartenWerteAlsString[i] == E9_w) { zahler++; }
        }
        //Debug.Log("katn"+kategorien_n_sorted+ " "+ transform.parent.parent.gameObject.GetComponent<karte>().KartenWerteAlsString);
       // Debug.Log("zähler" + zahler + " " + AnazhlEintraege_nurManuell);
        if(zahler< AnazhlEintraege_nurManuell) { return false; }
        
        if(transform.parent.parent.gameObject.GetComponent<karte>().anzahl_id> dyn_anzahl.Length) { return false; }
        if (transform.parent.parent.gameObject.GetComponent<karte>().farbe_id > dyn_farbe.Length) { return false; }
        if (transform.parent.parent.gameObject.GetComponent<karte>().rotation_id > dyn_ausrichtung.Length) { return false; }


        return true;
        
    }

    string WhatCard_String()
    {
        string s = "";
        for (int i = 0; i < Max_Anzahl_katProKarte; i++)
        {
            //s = s + " " + IDkat_toString(i) + ": " + IDwert_toString(i, fieldOfCards[transform.parent.parent.gameObject.GetComponent<karte>().place_id, i])  +" | ";
            s = s + " " + kategorien_n_sorted[i] + ": " + transform.parent.parent.gameObject.GetComponent<karte>().KartenWerteAlsString[i] + " | ";
        }

        return s;

    }
    string WhatCard_String_g()
    {
        string s = "";
        for (int i = 0; i < Max_Anzahl_katProKarte; i++)
        {
            //s = s + " " + IDkat_toString( i) + ": " + IDwert_toString(i,fieldOfCards[transform.parent.parent.gameObject.GetComponent<karte>().place_id, i]) + "\n";
            s = s + " " + kategorien_n_sorted[i] + ": " + transform.parent.parent.gameObject.GetComponent<karte>().KartenWerteAlsString[i] + "\n";
        }

        return s;

    }

    string IDkat_toString(int i)
    {
        string s="";
        //werte_n
        s = kategorien_n[i];
        if (string.IsNullOrEmpty(s))
        {
            s = s+ i;
        }
        return s;
    }
    string IDwert_toString(int ikat, int jwert)
    {
        string s = "";
        //werte_n
        s = werte_n[ikat,jwert];
        if (string.IsNullOrEmpty(s))
        {
            s = s+jwert;
        }
        return s;
    }
}
