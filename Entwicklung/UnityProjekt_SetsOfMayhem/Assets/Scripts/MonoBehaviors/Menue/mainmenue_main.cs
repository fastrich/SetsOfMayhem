using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static methods_unity;
using UnityEngine.SceneManagement;
using static config_parameters;
using static medium;
using static algos;
public class mainmenue_main : MonoBehaviour
{



    // Start is called before the first frame update
    void Start()
    {
        //StartCoroutine(Waiting(3));
        kategorien_n2[0] = farbe_gen;
        kategorien_n2[1] = ausrichtung_gen;
        kategorien_n2[2] = anzahl_gen;
        for (int i =0; i < ueberschuessig.Length; i++) {
            ueberschuessig[i] =0;
                }





             


    }

    // Update is called once per frame
    void Update()
    {
       
    }
    IEnumerator Waiting(int t)
    {
        yield return new WaitForSeconds(t);
        SceneManager.LoadScene(sceneName: "Stage");
    }

    public void StartGame_Fast_Random()
    {
        update_arrays();
        SceneManager.LoadScene(sceneName: "Stage");
    }
    public void StartGame_Fast_Classic()
    {
        Game_numberOfCardsOnDeck = Game_numberOfCardsOnDeck_Classic;
        numberOfSelected_soll = numberOfSelected_soll_Classic;
        update_arrays();
    SceneManager.LoadScene(sceneName: "Stage");
    }

    public void StartGame_withOptionsM()
    {
        update_arrays();
        if (Max_Anzahl_katProKarte < 1) { return; }
        if (numberofUnitsPerKat_max < 1) { return; }
        if (numberOfSelected_soll < 1) { return; }

        SceneManager.LoadScene(sceneName: "Stage");
    }

    public void StartGame_withOptionsM2()
    {
        update_arrays();
        SceneManager.LoadScene(sceneName: "Stage");
    }



}
