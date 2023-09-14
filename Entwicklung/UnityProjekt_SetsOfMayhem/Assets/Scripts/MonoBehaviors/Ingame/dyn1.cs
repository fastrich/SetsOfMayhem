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

public class dyn1 : MonoBehaviour
{
    public GameObject karteWG;
    public int ausrichtung_wg2 = 0;
    public int farbe_wg2 = 0;
   // private Vector3 vicki = new Vector3(0f, 0f, 0f);

    // Start is called before the first frame update
    void Start()
    {
        
        
    }

    // Update is called once per frame
    void Update()
    {
        ausrichtung_wg2 = dyn_ausrichtung[karteWG.GetComponent<dynMulti>().ausrichtung_wg];
        farbe_wg2 = karteWG.GetComponent<dynMulti>().farbe_wg;

        //ausrichtung_wg2 = -50;
        //vicki = new Vector3(0,0,ausrichtung_wg2);
        //gameObject.GetComponent<RectTransform>().eulerAngles = vicki;
        //transform.localEulerAngles = vicki;
        //transform.Rotate(0, 15 * Time.deltaTime, 0);
        var rotationvector = transform.rotation.eulerAngles;
        rotationvector.z = ausrichtung_wg2;
        // transform.rotation = Quaternion.Euler(rotationvector);
        for (int i = 0; i < transform.childCount; i++)
        {

            transform.GetChild(i).gameObject.transform.rotation = Quaternion.Euler(rotationvector);
        }
    }
    void OnEnable()
    {
        
    

    ausrichtung_wg2 = dyn_ausrichtung[karteWG.GetComponent<dynMulti>().ausrichtung_wg];
        farbe_wg2 = karteWG.GetComponent<dynMulti>().farbe_wg;

        //ausrichtung_wg2 = -50;
        //vicki = new Vector3(0,0,ausrichtung_wg2);
        //gameObject.GetComponent<RectTransform>().eulerAngles = vicki;
        //transform.localEulerAngles = vicki;
        //transform.Rotate(0, 15 * Time.deltaTime, 0);
        var rotationvector = transform.rotation.eulerAngles;
        rotationvector.z = ausrichtung_wg2;
       // transform.rotation = Quaternion.Euler(rotationvector);
        for (int i = 0; i < transform.childCount; i++)
        {
            
            transform.GetChild(i).gameObject.transform.rotation = Quaternion.Euler(rotationvector);
        }
        
        
    }


}
