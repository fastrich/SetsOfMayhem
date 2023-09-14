using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static medium;
using static algos;
using static kartenInformationen;
using static methods_unity;
using static config_parameters;
using UnityEngine.SceneManagement;

public class BackToMainM : MonoBehaviour
{

    public GameObject IntSceneMng;

    // Start is called before the first frame update
    void Start()
    {
        
        
    }


    // Update is called once per frame
    void Update()
    {
       
    }


   public void onClickClick()
    {

        if (IntSceneMng == null) { SceneManager.LoadScene(sceneName: "MainMenue"); }
        IntSceneMng.GetComponent<internalScenesManager>().ChangeInternalScene("MainMenue");
        
        
    }
}
