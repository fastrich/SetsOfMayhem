using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static medium;
using static methods;
using static kartenInformationen;
using static algos;
using System;
using static config_parameters;
using static bruecke;
using static setsUndFelder;
using static karteZuBild;

public class karte_HG : MonoBehaviour
{

    
    
   
    

    public bool checkForPlaceID;
    public int place_id;
    public GameObject gobj_mit_placeID;
    public int whichFieldIam = 0;

    // Start is called before the first frame update
    void Start()
    {
        if (checkForPlaceID)
        {
            whichFieldIam = Karte_get_FieldID_From_ObjektName(gobj_mit_placeID);
            place_id = Karte_get_placeID_From_ObjektName(gobj_mit_placeID);
            //array_cards_status_SetIt(whichFieldIam, place_id, 0);
        }
    }

    // Update is called once per frame
    void Update()
    {


        if (array_cards_status_GetIt(whichFieldIam, place_id) != 2)
        {
            gameObject.transform.localScale = new Vector3((float)1, (float)1, (float)1);
        }
        if (array_cards_status_GetIt(whichFieldIam, place_id) == 2)
        {
            gameObject.transform.localScale = new Vector3((float)1.1, (float)1.1, (float)1.1);

        }

    }

 


}
