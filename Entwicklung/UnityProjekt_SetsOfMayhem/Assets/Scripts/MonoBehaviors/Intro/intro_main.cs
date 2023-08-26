using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static methods_unity;
using UnityEngine.SceneManagement;
using static config_parameters;

public class intro_main : MonoBehaviour
{

    public GameObject Karte1;
    public GameObject Karte2;
    public GameObject Karte3;

    // Start is called before the first frame update
    void Start()
    {
    Karte1.SetActive(false);
    Karte2.SetActive(false); 
    Karte3.SetActive(false);
        StartCoroutine(Waiting(Intro_waitTime));
        
    }

    // Update is called once per frame
    void Update()
    {
       
    }
    IEnumerator Waiting(int t)
    {

        float t2 = t;
        t2=   t / 4;

        yield return new WaitForSeconds(t2);
        Karte1.SetActive(true);
        yield return new WaitForSeconds(t2);
        Karte2.SetActive(true);
        yield return new WaitForSeconds(t2);
        Karte3.SetActive(true); 
        yield return new WaitForSeconds(t2);
        SceneManager.LoadScene(sceneName: "MainMenue");
    }




}
