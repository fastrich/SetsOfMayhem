using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static methods_unity;
using UnityEngine.SceneManagement;
using static config_parameters;

public class gamemode_main : MonoBehaviour
{
    


    // Start is called before the first frame update
    void Start()
    {
        //StartCoroutine(Waiting(3));
        
    }

    // Update is called once per frame
    void Update()
    {
       
    }
    public void ClickBack()
    {
        SceneManager.LoadScene(sceneName: "MainMenue");
    }

    public void Start_Gamemode_Classic()
    {
        SceneManager.LoadScene(sceneName: "GameOptions");
    }


}
