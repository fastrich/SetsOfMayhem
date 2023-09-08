using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static medium;
using static algos;
using static kartenInformationen;
using static methods_unity;
using static config_parameters;
using TMPro;

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


    // Start is called before the first frame update
    void Start()
    {
        //LadeKartenMaterial1(); veraltet
        CreateAndDistributeCardsOnScreen();
        startTime = System.DateTime.UtcNow;
        gefundeneSets = 0;

        //Debug.Log("Zeit0");

    }


    // Update is called once per frame
    void Update()
    {
        checkForPlayerSetSelection();
        CheckAndFillCardField();
        text_sets.GetComponent<TMP_Text>().text = "SETs gefunden: "+ gefundeneSets.ToString();
        string s = "Zeit Pro SET: -";
        if (gefundeneSets > 0) {
            System.TimeSpan ts = System.DateTime.UtcNow - startTime;
            //Debug.Log(ts.Seconds.ToString());
            int zeitz=ts.Seconds;
            float watt = (float)zeitz / (float)gefundeneSets;
            if (zuLetztemSet_AnzahlSET!= gefundeneSets){ geschwin_zuLetztemSet = watt; zuLetztemSet_AnzahlSET = gefundeneSets; }


            s = "Zeit pro SET in Sekunden: "+ geschwin_zuLetztemSet.ToString();


        } ;
        text_watt.GetComponent<TMP_Text>().text = s;
        s = "";
        if (Set_InfosAnzeigen_Anzahl!=0 || KartenInfosAnzeigen!=0) { s= "SETs im Feld: " + SetsFoundInField_gen; }
        text_sets_feld.GetComponent<TMP_Text>().text = s;
    }


    public void bttn_click_kartenInfos() { KartenInfosAnzeigen = 1 - KartenInfosAnzeigen ; }
    public void bttn_click_frischeKarten() { frischeKarten(); }

    public void frischeKarten()
    {
        for (int i = 0; i < array_cards_status.Length; i++)
        {
            array_cards_status[i] = 0;
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
