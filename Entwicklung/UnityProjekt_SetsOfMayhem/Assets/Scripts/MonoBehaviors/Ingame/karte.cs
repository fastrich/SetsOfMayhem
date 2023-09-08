using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static medium;
using static methods;
using static kartenInformationen;
using static algos;
using System;
using static config_parameters;

public class karte : MonoBehaviour
{

    
    
    public int rotation_id=0;
    public int anzahl_id=0;
    public int farbe_id=0;
    

    public bool checkForPlaceID;
    public int place_id;
    public GameObject gobj_mit_placeID;

    public Boolean hasACard= false;

    public int hoch = 0;

    public string[] KartenWerteAlsString = new string[AnazhlEintraege];

    // Start is called before the first frame update
    void Start()
    {
        if (checkForPlaceID)
        {
            string name2 = gobj_mit_placeID.name;
            int i = Editor_NameCardslots_howmany0;
            name2 = name2.Substring(name2.Length - i, i);
            //Debug.Log("n "+name2);
            place_id = (int)Int32.Parse(name2);
            //Debug.Log("i "+place_id);
            array_cards_status[place_id] = 0;
        }
        
    }

    // Update is called once per frame
    void Update()
    {
        
        if (inSettings) { hoch++; }
        if (inSettings && hoch>1) { hoch = 0; activChilds(false); activChilds(true); }



        if (array_cards_status[place_id] == 0)
        {
            if (!inSettings) { activChilds(false); }
            hasACard = false;
        }
            
          if(array_cards_status[place_id] == 1)
        {
            changeToNotMarked();
        }
        if (array_cards_status[place_id] == 2)
        {
            changeToMarked();

        }
        if (array_cards_status[place_id] == 3)
        {

            mapit();

            hasACard = false;
            
            StartCoroutine(Waiting(0.5f));
            array_cards_status[place_id] = 4;
            gameObject.transform.localScale = new Vector3((float)1, (float)1, (float)1);
            activChilds(true);
            StartCoroutine(Waiting(0.1f));
            

        }
        if (array_cards_status[place_id] == 4)
        {
            if (hasACard) { changeToNotMarked(); }
            mapit();

            
        }

        mapit();
    }
    void OnEnable()
    {
        kategorien_n = new string[Max_Anzahl_katProKarte_SLIDER_MAX + 1];
        werte_n = new string[Max_Anzahl_katProKarte_SLIDER_MAX + 1, numberofUnitsPerKat_max_SLIDER_MAX + 1];
    }





        IEnumerator Waiting(float t)
    {
        yield return new WaitForSeconds(t);
        changeToNotMarked();
    }
    

    public void OnClick()
    {
        //Debug.Log("sel: " + ArrayToString(array_cards_selected));
        //Debug.Log("stat: " + ArrayToString(array_cards_status));
        //Debug.Log("used: " + ArrayToString(array_cards_used_with_id));
        //Debug.Log(place_id);
        //Debug.Log(array_cards_status[place_id]);
        switch (array_cards_status[place_id])
        {
            case 2:
                changeToNotMarked();
                break;
            case 1:
                changeToMarked();
                break;
            default:
                //changeToMarked();                
                break;
        }
        

        
    }

    private void ggg()
    {
        //pickrandomcardsettings();
    }

    private void pickrandomcardsettings()
    {      
        for(int i=0; i<transform.childCount; i++)
        {
            transform.GetChild(i).gameObject.SetActive(false);

        }
        int randomID = UnityEngine.Random.Range(0, transform.childCount);
        transform.GetChild(randomID).gameObject.SetActive(true);
        //array[1,1].SetActive(true);

    }
    private void changeToMarked()
    {
        array_cards_status[place_id] = 2;
        gameObject.transform.localScale = new Vector3((float)1.2, (float)1.2, (float)1.2);
        //Debug.Log("scaleUP");
    }
    private void changeToNotMarked()
    {
        array_cards_status[place_id] = 1;
       gameObject.transform.localScale = new Vector3((float)1, (float)1, (float)1);
    }
    private void changeToMark(int place_id_, int mark)
    {
        array_cards_status[place_id_] = mark;
        gameObject.transform.localScale = new Vector3((float)1, (float)1, (float)1);
        if (mark == 2){
            gameObject.transform.localScale = new Vector3((float)1.2, (float)1.2, (float)1.2);
        }
    }

 

    public void activChilds(bool act)
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            transform.GetChild(i).gameObject.SetActive(act);

        }
    }

    void mapit()
    {
        KartenWerteAlsString = new string[AnazhlEintraege];
        for (int i = 0; i < Max_Anzahl_katProKarte; i++)
        {
            try
            {
                //Todo
                //Debug.Log("hierBugArray");
                if (string.IsNullOrEmpty(werte_n_sorted[i, fieldOfCards[place_id, i]])) 
                {
                    KartenWerteAlsString[i] = i.ToString();
                }
                else
                { KartenWerteAlsString[i] = werte_n_sorted[i, fieldOfCards[place_id, i]]; }
                if (kategorien_n_sorted[i] == farbe_gen) { KartenWerteAlsString[i] = fieldOfCards[place_id, i].ToString(); farbe_id = fieldOfCards[place_id, i]; }
                if (kategorien_n_sorted[i] == ausrichtung_gen) { KartenWerteAlsString[i] = fieldOfCards[place_id, i].ToString(); rotation_id = fieldOfCards[place_id, i]; }
                if (kategorien_n_sorted[i] == anzahl_gen) { KartenWerteAlsString[i] = fieldOfCards[place_id, i].ToString(); anzahl_id = fieldOfCards[place_id, i]; }
            }
            catch
            {  Debug.Log("hierBugArray");}

                // Debug.Log("foc"+i +fieldOfCards[place_id, i]);
                //Debug.Log("foc" + place_id+ " , "+ i + KartenWerteAlsString[i]);
                //Debug.Log("hey" + werte_n_sorted[0, 0] + werte_n_sorted[0, 1] + werte_n_sorted[0, 2] + werte_n_sorted[0, 3]);
        }
        
        //Debug.Log("+Anz "+AnazhlEintraege);
        for (int i = Max_Anzahl_katProKarte; i < (AnazhlEintraege); i++)
        {

            KartenWerteAlsString[i] = werte_n_sorted[i, ueberschuessig[i]];
        }

        //Debug.Log("KartenwerteAlsString " + ArrayToString(KartenWerteAlsString));

    }
}
