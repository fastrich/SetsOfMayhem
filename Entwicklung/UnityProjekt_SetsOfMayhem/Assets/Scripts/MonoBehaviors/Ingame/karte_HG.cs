using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static medium;
using static methods;
using static kartenInformationen;
using static algos;
using System;
using static config_parameters;

public class karte_HG : MonoBehaviour
{

    
    
   
    

    public bool checkForPlaceID;
    public int place_id;
    public GameObject gobj_mit_placeID;


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


        if (array_cards_status[place_id] != 2)
        {
            gameObject.transform.localScale = new Vector3((float)1, (float)1, (float)1);
        }
        if (array_cards_status[place_id] == 2)
        {
            gameObject.transform.localScale = new Vector3((float)1.1, (float)1.1, (float)1.1);

        }

    }

 


}
