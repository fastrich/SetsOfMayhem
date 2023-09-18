using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static methods_unity;
using UnityEngine.SceneManagement;
using static config_parameters;
using static medium;
using static algos;
using static methods;
using TMPro;
public class internalScenesManager : MonoBehaviour
{
    public GameObject ladeSzene;
   public GameObject mainMenue;
    public GameObject game_1;


    private void Awake()
    {
        changeInternalScenesOff();
        ChangeInternalScene("MainMenue");
        ladeSzene.SetActive(false);
    }


    // Start is called before the first frame update
    void Start()
    {
      


    }


    public void changeInternalScenesOff()
    {
        mainMenue.SetActive(false);
        game_1.SetActive(false);
    }
    public void ChangeInternalScene(bool LadeSchirmAn)
    {

       ladeSzene.SetActive(LadeSchirmAn);
       
    }

    public void ChangeInternalScene(string Szene)
    {

        changeInternalScenesOff();
        if (Szene.CompareTo("MainMenue")==0) { mainMenue.SetActive(true); }
        if (Szene.CompareTo("Stage")==0) { game_1.SetActive(true); }
    }
    public void ChangeScene(string Szene)
    {
        SceneManager.LoadScene(sceneName: Szene); 
    }




}
