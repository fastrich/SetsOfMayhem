using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Medium;
using static KartenInformationen;

public class KartenID : MonoBehaviour
{
    public int kartenID;
   
   


    void OnEnable()
    {
        //array_cards_used_with_id[transform.parent.gameObject.GetComponent<karte>().place_id]=kartenID;
       

    }
    


    // Start is called before the first frame update
    void Start()
    {
        array[transform.parent.gameObject.GetComponent<karte>().place_id, kartenID] = gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        
    }


}
