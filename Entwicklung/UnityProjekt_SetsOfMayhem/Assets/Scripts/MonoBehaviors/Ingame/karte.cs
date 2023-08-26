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
    public int place_id;
    public GameObject gobj_mit_placeID;

    // Start is called before the first frame update
    void Start()
    {
        string name2 = gobj_mit_placeID.name;
        int i = Editor_NameCardslots_howmany0;
        name2 = name2.Substring(name2.Length-i, i );
        //Debug.Log("n "+name2);
        place_id = (int)Int32.Parse(name2);
        //Debug.Log("i "+place_id);
        array_cards_status[place_id] = 0;
        
    }

    // Update is called once per frame
    void Update()
    {
        if (array_cards_status[place_id] == 0)
        {
            pickrandomcardsettings();
            changeToNotMarked();
        }
            
          if(array_cards_status[place_id] == 1)
        {
            changeToNotMarked();
        }
        if (array_cards_status[place_id] == 2)
        {
            changeToMarked();

        }
    }




    public void OnClick()
    {
        //Debug.Log("sel: " + ArrayToString(array_cards_selected));
        //Debug.Log("stat: " + ArrayToString(array_cards_status));
        //Debug.Log("used: " + ArrayToString(array_cards_used_with_id));
        //Debug.Log(place_id);
        switch (array_cards_status[place_id])
        {
            case 2:
                changeToNotMarked();
                break;
            default:
                changeToMarked();
                scanForSelected();
                if (numberOfSelected >= numberOfSelected_soll)
                {
                    for (int i = 0; i < array_cards_status.Length; i++)
                    {
                        bool cfs = checkForSetinSelected();
                        if (cfs == true)
                        {
                            //ebug.Log(array_cards_selected + " ; " + array_cards_status + " ; " + array_cards_used_with_id);
                            if (array_cards_status[i] == 2)
                            {
                                changeToMark(i, 0);
                            }
                        }
                        else
                        {
                            changeToMark(i, 1);
                        }
                    }
                    

                }
                break;






        }
        

        
    }

    private void ggg()
    {
        pickrandomcardsettings();
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
}
