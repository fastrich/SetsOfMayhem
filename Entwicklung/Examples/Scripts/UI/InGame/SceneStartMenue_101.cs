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
using UnityEngine.EventSystems;
using static StageStatic;



/*
  https://gamedevbeginner.com/the-right-way-to-pause-the-game-in-unity/#:~:text=The%20most%20convenient%20method%20for%20pausing%20the%20game,will%20return%20the%20game%20to%20its%20normal%20speed.
*/

public class SceneStartMenue_101 : MonoBehaviour//, IPointerDownHandler, IPointerUpHandler
{

    //public GameObject myself_GObj_Txt;
    public Text myself_GObj_Txt;
    public int myself_Txt_ID;
    //public int myUI_ID;
    //public int setValueTo;
    //public bool ResetLevel;
    //public bool checkTimeToStop;
    //public bool ResetUI;
    //public int switchToScene_ID_;
    string missionbriefing = "";
    string missionbriefing1 = "After a long trip through the forest, you have found a resting place near a stream. After drinking some water you have heared cracking of branches. It seems you are not alone.";
    string missionbriefing330 = "other gameplay";
    string missionbriefing331 = "Moorhuhn-like 2D Jump and Run ";
    string missionbriefing332 = "Point and click gameplay with predefined camera positions";
    string missionbriefing333 = "Third person gameplay with manual following camera";
    string missionbriefing334 = "Third person gameplay with camera holding distance.";
    string missionbriefing335 = "First person gameplay";
    string missionbriefing336 = "First person game with old input";
    string missionbriefing337 = "Third person gameplay with near following camera";
    string missionbriefing338 = "Mario-like 2D Jump and Run";



    void Start()
    {
        

        //myself_GObj_Txt.text = "hello World";
        //StageStatic StgStc = new StageStatic();
        //myself_GObj_Txt.text = "ssss"+StageStatic.stage.number;
        //myself_GObj_Txt.text = "ssss" + StageStatic.stage.scene;
        //myself_GObj_Txt.text = "ssss" + StageStatic.stage.description;

        switch (myself_Txt_ID) {
            case 1 :
                myself_GObj_Txt.text = "World: " + StageStatic.stage.scene + "\n" +
                                        "Stage: " + StageStatic.stage.name + "\n"; 
                                                //"Info:" + StageStatic.stage.description;
                        break;
            case 2:
                missionbriefing = missionbriefing1;
                myself_GObj_Txt.text = StageStatic.stage.description + "\n" + "\n" +
                    //"\n"+
                    missionbriefing;
                break;
            case 330:
                missionbriefing = missionbriefing1;
                myself_GObj_Txt.text = StageStatic.stage.description + "\n" + "\n" +
                    //"\n"+
                    missionbriefing330;
             
                break;
            case 331:
                missionbriefing = missionbriefing1;
                myself_GObj_Txt.text = StageStatic.stage.description + "\n" + "\n" +
                    //"\n"+
                    missionbriefing331;
               
                break;
            case 332:
                missionbriefing = missionbriefing1;
                myself_GObj_Txt.text = StageStatic.stage.description + "\n" + "\n" +
                    //"\n"+
                    missionbriefing332;
               
                break;
            case 333:
                missionbriefing = missionbriefing1;
                myself_GObj_Txt.text = StageStatic.stage.description + "\n" + "\n" +
                    //"\n"+
                    missionbriefing333;
                  
                break;
            case 334:
                missionbriefing = missionbriefing1;
                myself_GObj_Txt.text = StageStatic.stage.description + "\n" + "\n" +
                    //"\n"+
                    missionbriefing334;
                   
                break;
            case 335:
                missionbriefing = missionbriefing1;
                myself_GObj_Txt.text = StageStatic.stage.description + "\n" + "\n" +
                    //"\n"+
                    missionbriefing335;
                  
                break;
            case 336:
                missionbriefing = missionbriefing1;
                myself_GObj_Txt.text = StageStatic.stage.description + "\n" + "\n" +
                    //"\n"+
                    missionbriefing336;

                break;
            case 337:
                missionbriefing = missionbriefing1;
                myself_GObj_Txt.text = StageStatic.stage.description + "\n" + "\n" +
                    //"\n"+
                    missionbriefing337;

                break;
            case 338:
                missionbriefing = missionbriefing1;
                myself_GObj_Txt.text = StageStatic.stage.description + "\n" + "\n" +
                    //"\n"+
                    missionbriefing338;

                break;
        }

    }

    private void Update()
    {

    }
}

