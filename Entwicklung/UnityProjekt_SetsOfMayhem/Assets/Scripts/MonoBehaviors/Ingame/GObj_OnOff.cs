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

public class GObj_OnOff : MonoBehaviour
{
    public int id = 0;

    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        bool same = false;
        if (id == kartenKostuem_ID) { same = true; }
        for (int i = 0; i < transform.childCount; i++)
        {
            transform.GetChild(i).gameObject.SetActive(same);
        }
    }
    void OnEnable()
    {


        
        
    }


}
