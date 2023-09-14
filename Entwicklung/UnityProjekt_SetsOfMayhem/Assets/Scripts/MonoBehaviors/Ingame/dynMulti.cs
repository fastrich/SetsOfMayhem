using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; //andr
using static medium;
using static methods;
using static kartenInformationen;
using static algos;
using System;
using static config_parameters;
using static methods_unity;
using static bruecke;

public class dynMulti : MonoBehaviour
{
 
    public GameObject vorlageGobj;
    public Canvas canvasEntp;
    public GameObject karteScript;
    public int anzahl =0;
    public int ausrichtung_wg = 0;
    public int farbe_wg = 0;
    public int whichFieldIam = 0;

    // Start is called before the first frame update
    void Start()
    {
        
        
    }

    // Update is called once per frame
    void Update()
    {
        anzahl = dyn_anzahl[karteScript.GetComponent<karte>().anzahl_id];
        ausrichtung_wg = karteScript.GetComponent<karte>().rotation_id;
        farbe_wg = karteScript.GetComponent<karte>().farbe_id;
        CreateAndDistributeCardsOnScreen_v2(whichFieldIam, vorlageGobj, canvasEntp, anzahl, gameObject);

        if (false) { 
            activChilds(false);
            //Debug.Log(karteScript.GetComponent<karte>().anzahl_id);
            if (Element1_visible==1 || (Element1_visible==2 && array_cards_status_GetIt(whichFieldIam, karteScript.GetComponent<karte>().place_id)==2 )) {
                bool hates;
                float jh = 0;
                int jb = 0;

                for (int ii = 0; ii < anzahl; ii++)
                {
                    hates = false;
                    for (int i = 0; i < transform.childCount; i++)
                    {
                        string name2 = transform.GetChild(i).name;
                        int j = Editor_NameCardslots_mu_howmany0;
                        name2 = name2.Substring(name2.Length - j, j);
                        try {
                            if (ii == (int)Int32.Parse(name2))
                            {
                                hates = true;



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

                                transform.GetChild(i).gameObject.GetComponent<RectTransform>().anchorMax = cornerTopRight;
                                transform.GetChild(i).gameObject.GetComponent<RectTransform>().anchorMin = cornerBottemLeft;

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

                                //=========================================/

                                transform.GetChild(i).gameObject.SetActive(true);



                            }

                        }
                        catch { }
                    }
                    if (hates == false)
                    {
                        erstellen(ii);
                    }
                }
             






            } 
        }


    }


    void erstellen(int ii)
    {
        Vector2 cornerTopRight = new Vector2(0.5f, 0.6f);
        Vector2 cornerBottemLeft = new Vector2(0, 0);
        string name1 = "";
        string name2 = ii.ToString();
        name1 = Editor_NameCardslots_mu;
        name1 = name1.Substring(0, name1.Length - name2.Length);
        name1 = name1 + name2;
        CreateGameObjectFromPrefab(name1, vorlageGobj, canvasEntp, cornerTopRight, cornerBottemLeft);

    }
    void auslagern() { 

        string name1;
        int k = 0;
        int n = Game_numberOfCardsOnDeck;
        int nh = (int)Mathf.Ceil(Mathf.Sqrt(n));
        int nb = (int)Mathf.Ceil(Mathf.Sqrt(n));
        float jh = 0;
        int jb = 0;
        //Debug.Log("nh, nb" + nh + " " + nb);
        //CreateGameObjectFromPrefab(kartenPrefab, canvasForCards, cornerTopRight, cornerBottemLeft);
        Vector2 cornerTopRight = new Vector2(0.5f, 0.6f);
        Vector2 cornerBottemLeft = new Vector2(0, 0);

        while (k < n)
        {

            string name2 = k.ToString();
            name1 = Editor_NameCardslots_mu;
            name1 = name1.Substring(0, name1.Length - name2.Length);
            name1 = name1 + name2;
            cornerTopRight = new Vector2((float)(jb + 1) / nb, (float)1 - (jh / nh));
            cornerBottemLeft = new Vector2((float)jb / nb, (float)1 - ((1 + jh) / nh));
            //Debug.Log("k" + k+ " ctr" + cornerTopRight + " cbl " + cornerBottemLeft);
            CreateGameObjectFromPrefab(name1, vorlageGobj, canvasEntp, cornerTopRight, cornerBottemLeft);

            jb++;
            if (jb >= nb)
            {
                jh++;
                jb = 0;
            }
            k++;

        }

    }




    public void activChilds(bool act)
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            transform.GetChild(i).gameObject.SetActive(act);

        }
    }
}

