using UnityEngine;
using UnityEngine.SceneManagement;
using static UIconfig;
//using static Restart_script;

public class Restart : MonoBehaviour
{
    public static void LevelReset()
    {
        StageStatic.stage.ResetPlay();
        //UIconfig.CanvasOnOff_Array[2] = 0;
        //UIconfig.GamePaused = false;
        //Time.timeScale = 1; // UIconfig.Game_TimeScale;
        Loader.LoadStage(StageStatic.stage.name, !StageStatic.stage.use_install_folder, false);
        //StageStatic.stage.factState.softreset();
    }

    public static void LoadMainMenue()
    {
        //not over SceneManager.LoadingScreen as MainMenue is too light to need to load over a LoadingScreen
        SceneManager.LoadScene("MainMenue");
    }


    public static void StageFactState_modundo()
    {
        StageStatic.stage.factState.undo();
    }
    public static void StageFactState_modredo()
    {
        StageStatic.stage.factState.redo();
    }

    public static void StageFactState_modreset()
    {
        StageStatic.stage.factState.softreset();
    }

    public static void Stage_modsave()
    {
        StageStatic.stage.push_record();
    }

    public static void StageFactState_modload()
    {
        StageStatic.stage.factState.hardreset();
        StageStatic.LoadInitStage(StageStatic.stage.name, !StageStatic.stage.use_install_folder);
    }




    public static void LoadStartScreen()
    {
        StartServer.process.Kill(); // null reference exception if Server started manually
        SceneManager.LoadScene(0);
    }

    public static void OnApplicationQuit()
    {
        StartServer.process.Kill(); // null reference exception if Server started manually
    }
}
