using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static medium;
using static algos;
using static kartenInformationen;
using static methods_unity;
using static config_parameters;
using TMPro;
using static bruecke;

public class SET_controller : MonoBehaviour
{
    public GameObject kartenPrefab;
    public GameObject text_sets;
    public GameObject text_watt;
    public GameObject text_sets_feld;
    public Canvas canvasForCards;
    private Vector2 cornerTopRight = new Vector2(0.5f, 0.6f);
    private Vector2 cornerBottemLeft = new Vector2(0, 0);

    public System.DateTime startTime;
    private string s;
    private float watt;
    private bool a = false;
    private bool b=false;

    // Start is called before the first frame update
    void Start()
    {
        arr_listeUberSetZeitenInSpiel = new System.DateTime[SETsBisZurWertung + 1];
        arr_listeUberSetZeitenInSpiel_korrektur = new float[SETsBisZurWertung + 1];
        korrekturZeit = 0;
        neuerRekord = false;
        neuerRekord_lokal_temp = false;
        bestzeiten_lokal_temp = 0;
        //LadeKartenMaterial1(); veraltet
        CreateAndDistributeCardsOnScreen();
        startTime = System.DateTime.UtcNow;
        gefundeneSets = 0;
        arr_listeUberSetZeitenInSpiel_pointer_neu = 0;
        arr_listeUberSetZeitenInSpiel_pointer_alt = 0;
        arr_listeUberSetZeitenInSpiel[arr_listeUberSetZeitenInSpiel_pointer_neu]=startTime;
    
        //Debug.Log("Zeit0");

    }


    // Update is called once per frame
    void Update()
    {
        checkForPlayerSetSelection(0);
        CheckAndFillCardField(0);

        //=====================================
        if (gefundeneSets > SETsBisZurWertung) { text_sets.GetComponent<TMP_Text>().text = "SETs gefunden: " + gefundeneSets.ToString(); }
        else { text_sets.GetComponent<TMP_Text>().text = "SETs gefunden: " + gefundeneSets.ToString() + "/" + SETsBisZurWertung; }

        s = "Zeit Pro SET: -";


        //Debug.Log(ts.Seconds.ToString());

        if (zuLetztemSet_AnzahlSET != gefundeneSets)
        {

            //
            arr_listeUberSetZeitenInSpiel_pointer_neu++;
            if (arr_listeUberSetZeitenInSpiel_pointer_neu >= arr_listeUberSetZeitenInSpiel.Length) { arr_listeUberSetZeitenInSpiel_pointer_neu = 0; }
            arr_listeUberSetZeitenInSpiel[arr_listeUberSetZeitenInSpiel_pointer_neu] = System.DateTime.UtcNow;
            arr_listeUberSetZeitenInSpiel_korrektur[arr_listeUberSetZeitenInSpiel_pointer_neu]=korrekturZeit;

            if (gefundeneSets > SETsBisZurWertung)
            {
                arr_listeUberSetZeitenInSpiel_pointer_alt++;
                if (arr_listeUberSetZeitenInSpiel_pointer_alt >= arr_listeUberSetZeitenInSpiel.Length) { arr_listeUberSetZeitenInSpiel_pointer_alt = 0; }
            }

            System.TimeSpan ts = arr_listeUberSetZeitenInSpiel[arr_listeUberSetZeitenInSpiel_pointer_neu] - arr_listeUberSetZeitenInSpiel[arr_listeUberSetZeitenInSpiel_pointer_alt];
            double zeitz = ts.TotalSeconds;
            //for (int i=0; i< arr_listeUberSetZeitenInSpiel_korrektur.Length;i++)
            //
                zeitz = zeitz + arr_listeUberSetZeitenInSpiel_korrektur[arr_listeUberSetZeitenInSpiel_pointer_neu] - arr_listeUberSetZeitenInSpiel_korrektur[arr_listeUberSetZeitenInSpiel_pointer_alt];
            
            

            double z = 0;
            if (gefundeneSets >= SETsBisZurWertung) { 
                z = SETsBisZurWertung; }
                 else
                {
                z = gefundeneSets;
            }
            z = (zeitz / z) * 10;
            if (z > float.MaxValue) { z = float.MaxValue; }

            watt = (Mathf.Floor((float)z)) / 10;

            //
            geschwin_zuLetztemSet = watt;
            zuLetztemSet_AnzahlSET = gefundeneSets;
            a = (geschwin_zuLetztemSet < bestzeiten[Game_numberOfCardsOnDeck, Max_Anzahl_katProKarte, numberofUnitsPerKat_max, numberOfSelected_soll, numberOfSelected_soll_gen]);
            b = (0 == bestzeiten[Game_numberOfCardsOnDeck, Max_Anzahl_katProKarte, numberofUnitsPerKat_max, numberOfSelected_soll, numberOfSelected_soll_gen]);

            if (gefundeneSets >= SETsBisZurWertung)
            {
                if (a || b)
                {
                    bestzeiten[Game_numberOfCardsOnDeck, Max_Anzahl_katProKarte, numberofUnitsPerKat_max, numberOfSelected_soll, numberOfSelected_soll_gen] = geschwin_zuLetztemSet;
                    neuerRekord = true;
                }
                a = (geschwin_zuLetztemSet < bestzeiten_lokal_temp);
                b = (0 == bestzeiten_lokal_temp);
                if (a || b)
                {
                    bestzeiten_lokal_temp = geschwin_zuLetztemSet;
                    neuerRekord_lokal_temp = true;
                }
            }


        }

        if (gefundeneSets > 0) { s = "Zeit pro SET in Sekunden: \n Neu: " + geschwin_zuLetztemSet.ToString();
            if (bestzeiten_lokal_temp!=0) { s += "\n Lokal: " + bestzeiten_lokal_temp.ToString(); }
            if (bestzeiten[Game_numberOfCardsOnDeck, Max_Anzahl_katProKarte, numberofUnitsPerKat_max, numberOfSelected_soll, numberOfSelected_soll_gen]!=0) { s+="\n Bestes: " + bestzeiten[Game_numberOfCardsOnDeck, Max_Anzahl_katProKarte, numberofUnitsPerKat_max, numberOfSelected_soll, numberOfSelected_soll_gen].ToString(); }
        } 




            text_watt.GetComponent<TMP_Text>().text = s;
    
      //________________________________________
        s = "";
        if (Set_InfosAnzeigen_Anzahl != 0 || KartenInfosAnzeigen != 0) { s = "SETs im Feld: " + SetsFoundInField_gen; }
        text_sets_feld.GetComponent<TMP_Text>().text = s;
    }


    public void bttn_click_kartenInfos() { KartenInfosAnzeigen++; if (KartenInfosAnzeigen > 2) { KartenInfosAnzeigen = 0; } }
    public void bttn_click_frischeKarten() { frischeKarten(0); if (SetsFoundInField_gen>0) { korrekturZeit += Wertung_Strafzeit_NeuesFeld_inSekunden; } }

    public void frischeKarten(int whichField)
    {
        for (int i = 0; i < array_cards_status_LengthIt(whichField,1); i++)
        {
            array_cards_status_SetIt(whichField, i,0);
        }

    }
    void CreateAndDistributeCardsOnScreen()
    {
        string name1;
        int k = 0;
        int n = Game_numberOfCardsOnDeck;
       
        int nb = (int)Mathf.Ceil(Mathf.Sqrt(n));
        int nh = (int)Mathf.Ceil((float)n / (float)nb);
        float jh = 0;
        int jb = 0;
        //Debug.Log("nh, nb" + nh + " " + nb);
        //CreateGameObjectFromPrefab(kartenPrefab, canvasForCards, cornerTopRight, cornerBottemLeft);
        Vector2 cornerTopRight = new Vector2(0.5f, 0.6f);
        Vector2 cornerBottemLeft = new Vector2(0, 0);

        while (k < n)
        {

            string name2 = k.ToString();
            name1 = Editor_NameCardslots;
            name1 = name1.Substring(0, name1.Length - name2.Length);
            name1 = name1 + name2;
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
}
