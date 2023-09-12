using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using static methods_unity;
using UnityEngine.SceneManagement;
using static config_parameters;
using static algos;
public class intro_main : MonoBehaviour
{

    public GameObject Karte1;
    public GameObject Karte2;
    public GameObject Karte3;
    private int backup_numberField=0;
    private int backup_genSet = 0;


   

    // Start is called before the first frame update
    void Start()
    {
        backup_numberField= Game_numberOfCardsOnDeck;
        Game_numberOfCardsOnDeck = Game_numberOfCardsOnDeck_Intro;
        backup_genSet = numberOfSelected_soll_gen;
        numberOfSelected_soll_gen = numberOfSelected_soll_gen_Íntro;
        Karte1.SetActive(false);
        Karte2.SetActive(false); 
        Karte3.SetActive(false);
        kartenKostuem_HG_Kat_Pointer = (int)Random.Range(0, arr_Kostuem_HG_Kat_ID.Length - 1);
        kartenKostuem_HG_Kat_ID = arr_Kostuem_HG_Kat_ID[kartenKostuem_HG_Kat_Pointer];
        //Debug.Log(Random.Range(0, arr_Kostuem_HG_Kat_ID.Length - 1) + " "+ Random.Range(0, 3));
        UIKHG_Kaz_update(true);
        kartenKostuem_HG_ID = arr_kartenKostuem_HG_ID[kartenKostuem_HG_Kat_ID];
        //lade123(classicSET);
        update_arrays();
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
        Game_numberOfCardsOnDeck = backup_numberField;
        numberOfSelected_soll_gen = backup_genSet;
       SceneManager.LoadScene(sceneName: "MainMenue");
    }




}
