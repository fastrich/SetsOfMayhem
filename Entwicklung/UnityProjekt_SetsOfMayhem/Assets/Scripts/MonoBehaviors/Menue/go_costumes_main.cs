using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; //andr
using UnityEngine.SceneManagement;
using System.IO; //
using UnityEngine.Video;//streaming
using UnityEngine.Networking;
//using static CommunicationEvents;
//using static UIconfig;
//using static StreamingAssetLoader;
using static config_parameters;
using static medium;
using static methods;
using static methods_unity;
using TMPro;
using static algos;

public class go_costumes_main : MonoBehaviour
{
    //public GameObject UIskin_links;
    //public GameObject UIskin_rechts;
    public GameObject UIskin_name;
    public GameObject UIskin_typ;
    public GameObject KartenReboot;
    public GameObject UIKHGskin_name;







    private Color colChangeable = new Color(1f, 1f, 1f, 0.5f);
    private Color colChangeable2 = new Color(1f, 1f, 1f, 0.5f);
    private float transCol;
    private ColorBlock[] tempColB = new ColorBlock[2];



    void Start()
    {
        
    }
    void OnEnable()
    {
        resettonew();
    }

    private void Update()
    {
        string nun = "";
        UIskin_name.GetComponent<TMP_Text>().text = arr_Kostuem_Name[kartenKostuem_Pointer];
        if (arr_Kostuem_klassisch[kartenKostuem_Pointer]) { nun = nun + kostuemTyp_klassisch; };
        UIskin_typ.GetComponent<TMP_Text>().text = nun;
        UIKHGskin_name.GetComponent<TMP_Text>().text = arr_Kostuem_HG_Name[kartenKostuem_HG_Pointer];

    }

    private void resettonew()
    {
      kategorien_n = new string[Max_Anzahl_katProKarte_SLIDER_MAX + 1];
     werte_n = new string[Max_Anzahl_katProKarte_SLIDER_MAX + 1, numberofUnitsPerKat_max_SLIDER_MAX + 1];
    werte_n_length = new int[Max_Anzahl_katProKarte_SLIDER_MAX + 1];
        //Debug.Log(kategorien_n[0]);
    ChoosenKats = new int[100];
        kategorien_n_sorted = new string[Max_Anzahl_katProKarte_SLIDER_MAX + 10];
    werte_n_sorted = new string[Max_Anzahl_katProKarte_SLIDER_MAX + 10, numberofUnitsPerKat_max_SLIDER_MAX + 1];
        StartCoroutine(Waiting2());
    }


    public void OnKlickBttn_UI_links() { kartenKostuem_Pointer--; if (kartenKostuem_Pointer < 0) { kartenKostuem_Pointer = (arr_Kostuem_ID.Length - 1); } resettonew(); }
    public void OnKlickBttn_UI_rechts() { kartenKostuem_Pointer++; if (kartenKostuem_Pointer > (arr_Kostuem_ID.Length - 1)) { kartenKostuem_Pointer = 0; } resettonew(); }
    public void OnKlickBttn_UIKHG_links() { kartenKostuem_HG_Pointer--; if (kartenKostuem_HG_Pointer < 0) { kartenKostuem_HG_Pointer = (arr_Kostuem_HG_ID.Length - 1); }  }
    public void OnKlickBttn_UIKHG_rechts() { kartenKostuem_HG_Pointer++; if (kartenKostuem_HG_Pointer > (arr_Kostuem_HG_ID.Length - 1)) { kartenKostuem_HG_Pointer = 0; }  }
    public void OnKlickBttn_UI_random() { kartenKostuem_Pointer = (int)Random.Range(0, arr_Kostuem_ID.Length-1); resettonew(); }
    public void OnKlickBttn_UIKHG_random() { kartenKostuem_HG_Pointer = (int)Random.Range(0, arr_Kostuem_HG_ID.Length - 1); }
    public void OnKlickBttn_UI_0() { kartenKostuem_Pointer = kartenKostuem_Pointer_ClassicDefault; resettonew(); }
    public void OnKlickBttn_UIKHG_0() { kartenKostuem_HG_Pointer = kartenKostuem_HG_Pointer_Default; }

    IEnumerator Waiting2()
    {
        float wait = 0.3f;
        float wait2 = 0.1f;
        KartenReboot.SetActive(false);
        yield return new WaitForSeconds(wait2);
        KartenReboot.SetActive(true);
        yield return new WaitForSeconds(wait);
        KartenReboot.SetActive(false);
        yield return new WaitForSeconds(wait2);
        KartenReboot.SetActive(true);
        //yield return new WaitForSeconds(wait);
        //KartenReboot.SetActive(false);
    }


}
