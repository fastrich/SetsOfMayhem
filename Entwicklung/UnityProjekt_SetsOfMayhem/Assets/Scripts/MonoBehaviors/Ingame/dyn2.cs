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

public class dyn2 : MonoBehaviour
{
    public GameObject karteWG;
    public int farbe_wg3 = 0;
   // private Vector3 vicki = new Vector3(0f, 0f, 0f);

    // Start is called before the first frame update
    void Start()
    {
        
        
    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log("sss");
        farbe_wg3 = karteWG.GetComponent<dynMulti>().farbe_wg;
        gameObject.GetComponent<Image>().color = dyn_farbe[farbe_wg3];

    }
    void OnEnable()
    {
        farbe_wg3 = karteWG.GetComponent<dynMulti>().farbe_wg;
        gameObject.GetComponent<Image>().color = dyn_farbe[farbe_wg3 ];


    }


}
