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
    public GameObject UIKHGskin_thema;
    public GameObject UIKHGskin_name_z;
    public GameObject UIKHGskin_thema_z;








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
        int nun2 = 0;
        UIskin_name.GetComponent<TMP_Text>().text = arr_Kostuem_Name[kartenKostuem_Pointer];
        if (arr_Kostuem_klassisch[kartenKostuem_Pointer]) { nun = nun + kostuemTyp_klassisch; };
        UIskin_typ.GetComponent<TMP_Text>().text = nun;
        
        nun = "Hintergrundmotiv: ";
        if (string.IsNullOrEmpty(karteHG_Beschriftung)) { nun = nun + kartenKostuem_HG_ID; }else { nun = nun+karteHG_Beschriftung; };
        nun = nun + "\n";
        UIKHGskin_name.GetComponent<TMP_Text>().text = "" + nun;
        UIKHGskin_thema.GetComponent<TMP_Text>().text = arr_Kostuem_HG_Kat_Name[kartenKostuem_HG_Kat_Pointer];
        nun2 = arr_kartenKostuem_HG_Pointer[kartenKostuem_HG_Kat_ID] + 1;
        UIKHGskin_name_z.GetComponent<TMP_Text>().text = ""+ nun2 + " / " + range1[kartenKostuem_HG_Kat_ID];
        nun2 = kartenKostuem_HG_Kat_Pointer + 1;
        UIKHGskin_thema_z.GetComponent<TMP_Text>().text= "" + nun2 + " / " + arr_Kostuem_HG_Kat_ID.Length;

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
    public void OnKlickBttn_UI_random() { kartenKostuem_Pointer = (int)Random.Range(0, arr_Kostuem_ID.Length - 1); resettonew(); }
    public void OnKlickBttn_UI_0() { kartenKostuem_Pointer = kartenKostuem_Pointer_ClassicDefault; resettonew(); }
    public void OnKlickBttn_UIKHG_Kat_links() { kartenKostuem_HG_Kat_Pointer--; if (kartenKostuem_HG_Kat_Pointer < 0) { kartenKostuem_HG_Kat_Pointer = (arr_Kostuem_HG_Kat_ID.Length - 1); } resettonew(); }
    public void OnKlickBttn_UIKHG_Kat_rechts() { kartenKostuem_HG_Kat_Pointer++; if (kartenKostuem_HG_Kat_Pointer > (arr_Kostuem_HG_Kat_ID.Length - 1)) { kartenKostuem_HG_Kat_Pointer = 0; } resettonew(); }
    public void OnKlickBttn_UIKHG_Kat_random() { kartenKostuem_HG_Kat_Pointer = (int)Random.Range(0, arr_Kostuem_HG_Kat_ID.Length - 1); resettonew(); }
    public void OnKlickBttn_UIKHG_Kat_0() { kartenKostuem_HG_Kat_Pointer = kartenKostuem_HG_Kat_Pointer_Default; resettonew(); }

    public void OnKlickBttn_UIKHG_0() { arr_kartenKostuem_HG_Pointer[kartenKostuem_HG_Kat_ID] = arr_kartenKostuem_HG_Pointer_Default[kartenKostuem_HG_Kat_ID]; resettonew(); }
    public void OnKlickBttn_UIKHG_links() { arr_kartenKostuem_HG_Pointer[kartenKostuem_HG_Kat_ID]--; resettonew(); }
    public void OnKlickBttn_UIKHG_rechts() { arr_kartenKostuem_HG_Pointer[kartenKostuem_HG_Kat_ID]++; resettonew(); }
    public void OnKlickBttn_UIKHG_random() { UIKHG_Kaz_update(true); resettonew(); }

  

  
 
    IEnumerator Waiting2()
    {
        float wait = 0.4f;
        float wait2 = 0.4f;
        KartenReboot.SetActive(false);
        yield return new WaitForSeconds(wait2);
        KartenReboot.SetActive(true);
        /*
         * yield return new WaitForSeconds(wait);
        KartenReboot.SetActive(false);
        yield return new WaitForSeconds(wait2);
        KartenReboot.SetActive(true);
        //yield return new WaitForSeconds(wait);
        //KartenReboot.SetActive(false);
        */
    }


}
