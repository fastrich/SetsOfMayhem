using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static methods_unity;
using UnityEngine.SceneManagement;

public class mainmenue_main : MonoBehaviour
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
    IEnumerator Waiting(int t)
    {
        yield return new WaitForSeconds(t);
        SceneManager.LoadScene(sceneName: "Stage");
    }

    public void StartGame_Fast()
    {
        SceneManager.LoadScene(sceneName: "Stage");
    }

    public void StartGame_withOptionsM()
    {
        SceneManager.LoadScene(sceneName: "Stage");
    }



}
