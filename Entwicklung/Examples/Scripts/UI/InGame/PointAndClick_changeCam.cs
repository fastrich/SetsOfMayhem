using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; //andr
using UnityEngine.SceneManagement;
using System.IO; //
using UnityEngine.Video;//streaming
using UnityEngine.Networking;
//using static StreamingAssetLoader;
//using static CheckServer;
//using static CommunicationEvents;
using static UIconfig;


public class PointAndClick_changeCam : MonoBehaviour
{

    public GameObject myCamera_GObj;



    public GameObject Position1_GObj;
    public GameObject Position2_GObj;
    public GameObject Position3_GObj;


    private int Position=1;



    void Start()
    {
        myCamera_GObj.transform.position = Position1_GObj.transform.position;
        myCamera_GObj.transform.rotation = Position1_GObj.transform.rotation;

    }

    private void Update()
    {

    }

    public void changePosition()
    {
        Position++;
        switch (Position)
        {
            case 1:
                myCamera_GObj.transform.position = Position1_GObj.transform.position;
                myCamera_GObj.transform.rotation = Position1_GObj.transform.rotation;
                break;
            case 2:
                myCamera_GObj.transform.position = Position2_GObj.transform.position;
                myCamera_GObj.transform.rotation = Position2_GObj.transform.rotation;
                break;
            case 3:
                myCamera_GObj.transform.position = Position3_GObj.transform.position;
                myCamera_GObj.transform.rotation = Position3_GObj.transform.rotation;
                break;
            default:
                myCamera_GObj.transform.position = Position1_GObj.transform.position;
                myCamera_GObj.transform.rotation = Position1_GObj.transform.rotation;
                Position = 1;
                break;
        }
    }
    





    
}