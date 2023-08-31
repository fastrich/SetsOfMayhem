using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; //andr
using UnityEngine.SceneManagement;
using System.IO; //
using UnityEngine.Video;//streaming
using UnityEngine.Networking;
using static CommunicationEvents;
using static UIconfig;
using System;


//Uploading Files from StreamingAsset folder to the Persistent Folder for Android Devices
public static class StreamingAssetLoader
{

    public static string StreamToDataPath_Folder = "StreamToDataPath";
    public static string StreamToDataPath_Folder_Cookie = "cookie_dataPath.txt";
    public static string StreamToPersistentDataPath_Folder = "StreamToPersistentDataPath";
    public static string StreamToPersistentDataPath_Folder_Cookie = "cookie_persistentDataPath.txt";
    public static string StreamToDataPath_withHandler_Folder = "StreamToDataPath_withHandler";
    public static string PersDataPathToPlayerSaveGame_Path = "stages/SaveGames";


    //Config
    public static string ConfigDir = "Config";
    public static string ConfigFile_Network = "Network.JSON";

    //For Android, Everything in StreamingAssets must be registered!
    public static string StreamToPersistentDataPath_FILE_1 = "scrolls.json";
    public static string Stage_Folder = "Stages";
    public static string Stage_1 = "TechDemo A.JSON";
    public static string Stage_2 = "TechDemo B.JSON";

    public static void ResetPlayerConfig()
    {
        string sourcePath = Path.Combine(Application.streamingAssetsPath, StreamToPersistentDataPath_Folder, ConfigDir);
        string targetFolder = Application.persistentDataPath;

        RereadFileWithUnityWebRequest(sourcePath, ConfigDir, ConfigFile_Network, targetFolder);
        NetworkJSON_Load();
    }

    public static bool ReloadManualy_StreamToPers()
    {
        ResetPlayerConfig();
        string sourcePath = Path.Combine(Application.streamingAssetsPath, StreamToPersistentDataPath_Folder);
        string targetFolder = Application.persistentDataPath;
        RereadFileWithUnityWebRequest(sourcePath, "", StreamToPersistentDataPath_Folder_Cookie, targetFolder);
        RereadFileWithUnityWebRequest(sourcePath, "", StreamToPersistentDataPath_FILE_1, targetFolder);
        NetworkJSON_Load();
        //Debug.Log("Reloaded_PP");
        return true;
    }
    public static bool ReloadManualy_StreamToDataPath()
    {
        string sourcePath = Path.Combine(Application.streamingAssetsPath, StreamToDataPath_Folder);
        string targetFolder = Application.dataPath;
        RereadFileWithUnityWebRequest(sourcePath, "", StreamToDataPath_Folder_Cookie, targetFolder);
    
        NetworkJSON_Load();
        //Debug.Log("Reloaded_DP");
        return true;
    }
    public static bool ReloadManualy_StreamToDataPathWithHandler(string TargetDir_1)
    {
        string sourcePath = Path.Combine(Application.streamingAssetsPath, StreamToDataPath_withHandler_Folder);
        string targetFolder_dpwh = TargetDir_1 ;
        //Debug.Log(targetFolder_dpwh);

        string sourcePath2 = Path.Combine(Application.streamingAssetsPath, StreamToDataPath_withHandler_Folder, Stage_Folder);
        RereadFileWithUnityWebRequest(sourcePath2, Stage_Folder, Stage_1, targetFolder_dpwh);
        RereadFileWithUnityWebRequest(sourcePath2, Stage_Folder, Stage_2, targetFolder_dpwh);

        NetworkJSON_Load();
        //Debug.Log("Reloaded_DPwH");
        return true;
    }
    //---------------------------------------------------------------------------------------




    //public static ID_toPath toPath = ID_toPath.DataPath;
    public enum ID_toPath
    {
        DataPath = 0,
        PersistentDataPath = 1,
    }


    public static bool checkPersistentDataPath()
    { 
        return checkFileExistence(Application.persistentDataPath, StreamToPersistentDataPath_Folder_Cookie);
    }
    public static bool checkDataPath()
    {
        return checkFileExistence(Application.dataPath, StreamToDataPath_Folder_Cookie);
    }
    public static bool checkFileExistence(string sourcepath, string filename)
    {

        string filePath = sourcepath;
        filePath = Path.Combine(filePath, filename);
        if (System.IO.File.Exists(filePath))
        {
            //Debug.Log("FileFound: " + filePath );
            return true;
        }

        //Debug.Log("NoFileFound: " + filePath);
        return false;

    }




    public static void NetworkJSON_Save()
    {
        NetworkJSON myObject = new NetworkJSON();

        
        //MyClass myObject = new MyClass();
        myObject.newIP = CommunicationEvents.newIP;
        myObject.lastIP = CommunicationEvents.lastIP;
        myObject.IPslot1 = CommunicationEvents.IPslot1;
        myObject.IPslot2 = CommunicationEvents.IPslot2;
        myObject.IPslot3 = CommunicationEvents.IPslot3;
        myObject.selecIP = CommunicationEvents.selecIP;
        myObject.ControlMode = UIconfig.controlMode.ToString();
        myObject.TouchMode = UIconfig.touchControlMode;
        myObject.TAvisibility = UIconfig.TAvisibility;
        myObject.autoOSrecognition = CommunicationEvents.autoOSrecognition;
        myObject.autoSettingsAdaption = UIconfig.autoSettingsAdaption;
        myObject.Opsys = CommunicationEvents.Opsys.ToString();
        myObject.FrameITUIversion = UIconfig.FrameITUIversion;
        myObject.InputManagerVersion = UIconfig.InputManagerVersion;
        myObject.colliderScale_all = UIconfig.colliderScale_all;
        myObject.cursorSize = UIconfig.cursorSize;
        myObject.camRotatingSensitivity = UIconfig.camRotatingSensitivity;
        myObject.MouseKeepingInWindow = UIconfig.MouseKeepingInWindow;




        //Data storage
        SafeCreateDirectory(Path.Combine(Application.persistentDataPath ,ConfigDir));
        //string json = JsonUtility.ToJson(date);
        string json = JsonUtility.ToJson(myObject);
        StreamWriter Writer = new StreamWriter(Path.Combine(Application.persistentDataPath, ConfigDir, ConfigFile_Network));
        Writer.Write(json);
        Writer.Flush();
        Writer.Close();
    }
    public static DirectoryInfo SafeCreateDirectory(string path)
    {
               
        //Generate if you don't check if the directory exists
        if (Directory.Exists(path))
        {
            //Debug.Log(path +  " exists");
            return null;

        }
        //Debug.Log(path + " create");
        return Directory.CreateDirectory(path);
    }
    public static void ResetPlayerSaveGame()
    {
        string path_a = Path.Combine(Application.persistentDataPath, PersDataPathToPlayerSaveGame_Path);
        deleteADirectoryAndSubDir(path_a);
        SafeCreateDirectory(path_a);
    }



    public static void ResetPersistentDataPath()
    {
        RereadFiles_PersistentDataPath();
        NetworkJSON_Load();
    }

    public static void ResetDataPath()
    {
        RereadFiles_DataPath();
    }

    public static void ReloadStreamingAsset()
    {

        RereadFiles_PersistentDataPath();
        RereadFiles_DataPath();
        NetworkJSON_Load();
        //CSform.CheckIPAdr();
    }
    public static void deleteADirectoryAndSubDir(string path)
    {
        // Delete a directory and all subdirectories with Directory static method...
        if (System.IO.Directory.Exists(@path))
        {
            try
            {
                System.IO.Directory.Delete(@path, true);
            }

            catch (System.IO.IOException e)
            {
                Console.WriteLine(e.Message);
            }
        }
    }



    public static void RereadFiles_PersistentDataPath()
    {
        
        //Debug.Log("Reread_PersistentDataPath");
        //RereadFileUWR(StreamingAssetLoader.file_1_path, StreamingAssetLoader.file_1, ID_toPath.PersistentDataPath);
        //----
        string rootPath = Path.Combine(Application.streamingAssetsPath, StreamToPersistentDataPath_Folder);
        string targetFolder = Application.persistentDataPath;
        if(!ReReadFiles(rootPath, targetFolder)){
            ReloadManualy_StreamToPers();
        }
    }
    public static void RereadFiles_DataPath()
    {
        //Debug.Log("Reread_DataPath");
        string rootPath = Path.Combine(Application.streamingAssetsPath, StreamToDataPath_Folder);
        string targetFolder = Application.dataPath;
        
        RereadFiles_DataPath_withHandler();
        if (!ReReadFiles(rootPath, targetFolder))
        {
            ReloadManualy_StreamToDataPath();
        }
        
    }

    public static void RereadFiles_DataPath_withHandler()
    {
        string rootPath = Path.Combine(Application.streamingAssetsPath, StreamToDataPath_withHandler_Folder);
        string targetFolder_wh = Application.dataPath;
        
        //Debug.Log(CommunicationEvents.Opsys);
        if (CommunicationEvents.Opsys == OperationSystem.Android)
        {
            targetFolder_wh = Application.persistentDataPath;
            //Debug.Log(OperationSystem.Android + " " + targetFolder_wh);
        }

        if (!ReReadFiles(rootPath, targetFolder_wh))
        {
            //Debug.Log( " 2 " + targetFolder_wh);
            ReloadManualy_StreamToDataPathWithHandler(targetFolder_wh);
        }
    }

    public static bool ReReadFiles(string rootPath, string targetFolder)//ID_toPath PathHandler)
    {
        if (!Directory.Exists(rootPath)) { Debug.Log("no Dir: " + rootPath); return false ; }

        //----
        //Debug.Log("Loading Dir");
        string dir = "";
        DirectoryInfo dirInfo = new DirectoryInfo(@rootPath);
        FileInfo[] Files = dirInfo.GetFiles("*");
        foreach (FileInfo file in Files)
        {
            if (file.Name.Contains(".meta")) { continue; };
            if (!RereadFileWithUnityWebRequest(rootPath, dir, file.Name, targetFolder))
            {
                return false;

            }
        }
        //----
        //Debug.Log("Saving Dir");
        string[] dirs = Directory.GetDirectories(rootPath, "*", SearchOption.AllDirectories);
        foreach (string dir_fullpath in dirs)
        {
            int pos = dir_fullpath.IndexOf(rootPath);
            int endpos = pos;
            if (dir_fullpath.Length > (rootPath.Length + 1)) { endpos = rootPath.Length + 1; } else { endpos = rootPath.Length; }
            dir = dir_fullpath.Remove(pos, endpos);
            dirInfo = new DirectoryInfo(@dir_fullpath);
            Files = dirInfo.GetFiles("*");

            foreach (FileInfo file in Files)
            {
                if (file.Name.Contains(".meta")) { continue; };
                if(!RereadFileWithUnityWebRequest(dir_fullpath, dir, file.Name, targetFolder)){
                    return false;
                }
            }
        }

        //Debug.Log("Dir Reloaded");
        return true;
    }

    public static void NetworkJSON_Load()
    {
        NetworkJSON_Load_x(Application.persistentDataPath);
    }


    public static void NetworkJSON_Load_0()
    {
        var x = Path.Combine(Application.streamingAssetsPath, StreamToPersistentDataPath_Folder);

        NetworkJSON_Load_x(x);
    }

    public static void NetworkJSON_Load_x(string path)
    {
        var reader = new StreamReader(Path.Combine(path, ConfigDir, ConfigFile_Network));
        string json = reader.ReadToEnd();
        reader.Close();

        NetworkJSONonlyString myObjsOnlyStrings = JsonUtility.FromJson<NetworkJSONonlyString>(json);
        NetworkJSON myObjs = JsonUtility.FromJson<NetworkJSON>(json);
        if (string.IsNullOrEmpty(myObjsOnlyStrings.newIP)) {
            CommunicationEvents.newIP = "";
        } else {
            CommunicationEvents.newIP = myObjs.newIP;
        }
        if (string.IsNullOrEmpty(myObjsOnlyStrings.lastIP)) {
            CommunicationEvents.lastIP = "";
        } else {
            CommunicationEvents.lastIP = myObjs.lastIP;
        }
        if (string.IsNullOrEmpty(myObjsOnlyStrings.IPslot1)) {
            CommunicationEvents.IPslot1 = "";
        } else {
            CommunicationEvents.IPslot1 = myObjs.IPslot1;//myObjs.IPslot1;
        }
        if (string.IsNullOrEmpty(myObjsOnlyStrings.IPslot2)) {
            CommunicationEvents.IPslot2 = "";//"Empty";
        } else {
            CommunicationEvents.IPslot2 = myObjs.IPslot2;
        }
        if (string.IsNullOrEmpty(myObjsOnlyStrings.IPslot3)) {
            CommunicationEvents.IPslot3 = "";
        } else {
            CommunicationEvents.IPslot3 = myObjs.IPslot3;
        }
        if (string.IsNullOrEmpty(myObjsOnlyStrings.selecIP)) {
            CommunicationEvents.selecIP = "";
        } else {
            CommunicationEvents.selecIP = myObjs.selecIP;
        }
        if (string.IsNullOrEmpty(myObjsOnlyStrings.ControlMode)) {
        } else {
            UIconfig.controlMode = (ControlMode)Enum.Parse(typeof(ControlMode), myObjs.ControlMode);
        }
        if (string.IsNullOrEmpty(myObjsOnlyStrings.TouchMode)) {
        } else {
            UIconfig.touchControlMode = myObjs.TouchMode;
        }
        if (string.IsNullOrEmpty(myObjsOnlyStrings.TAvisibility)) {
        } else {
            UIconfig.TAvisibility = myObjs.TAvisibility;
        }
        if (string.IsNullOrEmpty(myObjsOnlyStrings.autoOSrecognition)) {
        } else {
            CommunicationEvents.autoOSrecognition = myObjs.autoOSrecognition;
        }
        if (string.IsNullOrEmpty(myObjsOnlyStrings.autoOSrecognition)){        
        } else {
            UIconfig.autoSettingsAdaption = myObjs.autoSettingsAdaption;
        }
        if (string.IsNullOrEmpty(myObjsOnlyStrings.Opsys)) {
        } else {
            CommunicationEvents.Opsys = (OperationSystem)Enum.Parse(typeof(OperationSystem), myObjs.Opsys);
        }
        if (string.IsNullOrEmpty(myObjsOnlyStrings.FrameITUIversion)) {
        } else {
            UIconfig.FrameITUIversion = myObjs.FrameITUIversion;
        }
        if (string.IsNullOrEmpty(myObjsOnlyStrings.InputManagerVersion)) {
        } else {
            UIconfig.InputManagerVersion = myObjs.InputManagerVersion;
        }
        if (string.IsNullOrEmpty(myObjsOnlyStrings.colliderScale_all)) {

        } else {
            UIconfig.colliderScale_all = myObjs.colliderScale_all;
        }
        if (string.IsNullOrEmpty(myObjsOnlyStrings.cursorSize)) {
        } else {
            UIconfig.cursorSize = myObjs.cursorSize;
        }
        if (string.IsNullOrEmpty(myObjsOnlyStrings.camRotatingSensitivity)) {
        } else {
            UIconfig.camRotatingSensitivity = myObjs.camRotatingSensitivity;
        }
        if (string.IsNullOrEmpty(myObjsOnlyStrings.MouseKeepingInWindow)){ 
        }else{
            UIconfig.MouseKeepingInWindow = myObjs.MouseKeepingInWindow;
        }

    }

    public static void RereadFileUWR(string pathfolders, string fileName, ID_toPath toMainpath)
    {
        if (fileName == ""){      return;     }
        string sourcePath = Path.Combine(Application.streamingAssetsPath, pathfolders);
        string destpathf = pathfolders;
        string destname = fileName;


        sourcePath = Path.Combine(sourcePath, fileName);
        using var loadingRequest = UnityWebRequest.Get(sourcePath);
        loadingRequest.SendWebRequest();
        while (!loadingRequest.isDone)
        {
            if (loadingRequest.result == UnityWebRequest.Result.ConnectionError || loadingRequest.result == UnityWebRequest.Result.ProtocolError)
            {
                break;
            }
        }
        if (loadingRequest.result == UnityWebRequest.Result.ConnectionError || loadingRequest.result == UnityWebRequest.Result.ProtocolError)
        {

        }
        else
        {
            //copies and unpacks file from apk to persistentDataPath where it can be accessed
            string destinationPath;
            if (toMainpath == ID_toPath.DataPath && CommunicationEvents.Opsys != OperationSystem.Android) { destinationPath = Path.Combine(Application.dataPath, destpathf); }
            else
            {
                destinationPath = Path.Combine(Application.persistentDataPath, destpathf);
            }

            SafeCreateDirectory(destinationPath);
            File.WriteAllBytes(Path.Combine(destinationPath, destname), loadingRequest.downloadHandler.data);




        }
    }

    public static bool RereadFileWithUnityWebRequest(string sourcePath1, string pathfolders,  string fileName,  string targetpath)
    {
        

        if (fileName == "")     { Debug.Log("no File");    return false;     }
        string destpathf = pathfolders;
        string destname = fileName;


        string sourcePath = Path.Combine(sourcePath1, fileName);
        //Debug.Log(sourcePath);
        using var loadingRequest = UnityWebRequest.Get(sourcePath);
        loadingRequest.SendWebRequest();
        while (!loadingRequest.isDone)
        {
            if (loadingRequest.result == UnityWebRequest.Result.ConnectionError || loadingRequest.result == UnityWebRequest.Result.ProtocolError)
            {
                break;
            }
        }
        if (loadingRequest.result == UnityWebRequest.Result.ConnectionError || loadingRequest.result == UnityWebRequest.Result.ProtocolError)
        {
            Debug.Log("ConnectionError" + sourcePath);
            return false;
        }
        else
        {


            string destinationPath = Path.Combine(targetpath, destpathf);

            //Debug.Log("ss" + destinationPath + "," + Application.persistentDataPath + "," + Application.dataPath + "," +  destpathf + " , " + destname);

            SafeCreateDirectory(destinationPath);
            File.WriteAllBytes(Path.Combine(destinationPath, destname), loadingRequest.downloadHandler.data);


            //Debug.Log("ss" + destinationPath);

        }
        return true;
    }

    //Path.Combine() but without the Path.IsPathRooted()
    public static string CombineTwoPaths(string path1, string path2)
    {
        if (path1 == null || path2 == null)
        {
            throw new ArgumentNullException((path1 == null) ? "path1" : "path2");
        }
        //Path.CheckInvalidPathChars(path1, false);
        //Path.CheckInvalidPathChars(path2, false);
        if (path2.Length == 0)
        {
            return path1;
        }
        if (path1.Length == 0)
        {
            return path2;
        }
        char c = path1[path1.Length - 1];
        if (c != Path.DirectorySeparatorChar && c != Path.AltDirectorySeparatorChar && c != Path.VolumeSeparatorChar)
        {
            return path1 + Path.DirectorySeparatorChar + path2;
        }
        return path1 + path2;
    }


    //WWW has been replaced with UnityWebRequest.
    /*
     public static string RereadFileNA(string pathfolders, string fileName, string destpathf, string destname)
     {
             if (fileName == ""){return "noName";  }
             // copies and unpacks file from apk to persistentDataPath where it can be accessed
             string destinationPath = Path.Combine(Application.persistentDataPath, destpathf);

             if (Directory.Exists(destinationPath) == false)
             {
                 Directory.CreateDirectory(destinationPath);
             }
             destinationPath = Path.Combine(destinationPath, destname);

             string sourcePath = Path.Combine(Application.streamingAssetsPath, pathfolders);
             sourcePath = Path.Combine(sourcePath, fileName);

  #if UNITY_EDITOR
         //string sourcePath = Path.Combine(Application.streamingAssetsPath, pathfolders);
         //sourcePath = Path.Combine(sourcePath, fileName);
  #else
         //string sourcePath = "jar:file://" + Application.dataPath + "!/assets/" + fileName;

  #endif

         //UnityEngine.Debug.Log(string.Format("{0}-{1}-{2}-{3}", sourcePath,  File.GetLastWriteTimeUtc(sourcePath), File.GetLastWriteTimeUtc(destinationPath)));

         //copy whatsoever

         //if DB does not exist in persistent data folder (folder "Documents" on iOS) or source DB is newer then copy it
         //if (!File.Exists(destinationPath) || (File.GetLastWriteTimeUtc(sourcePath) > File.GetLastWriteTimeUtc(destinationPath)))
         if (true)
             {
                 if (sourcePath.Contains("://"))
                 {
                     // Android  
                     WWW www = new WWW(sourcePath);
                     while (!www.isDone) {; }                // Wait for download to complete - not pretty at all but easy hack for now 
                     if (string.IsNullOrEmpty(www.error))
                     {
                         File.WriteAllBytes(destinationPath, www.bytes);
                     }
                     else
                     {
                         Debug.Log("ERROR: the file DB named " + fileName + " doesn't exist in the StreamingAssets Folder, please copy it there.");
                     }
                 }
                 else
                 {
                     // Mac, Windows, Iphone                
                     //validate the existens of the DB in the original folder (folder "streamingAssets")
                     if (File.Exists(sourcePath))
                     {
                         //copy file - alle systems except Android
                         File.Copy(sourcePath, destinationPath, true);
                     }
                     else
                     {
                         Debug.Log("ERROR: the file DB named " + fileName + " doesn't exist in the StreamingAssets Folder, please copy it there.");
                     }
                 }
             }

             StreamReader reader = new StreamReader(destinationPath);
             var jsonString = reader.ReadToEnd();
            reader.Close();

            return jsonString;
     }
    */

    public static void RereadFileUW4(string pathfolders, string fileName, string destpathf, string destname)
    {
            if (fileName == "")
            {
                return;
            }


            string sourcePath = Path.Combine(Application.streamingAssetsPath, pathfolders);
            sourcePath = Path.Combine(sourcePath, fileName);
            using var loadingRequest = UnityWebRequest.Get(sourcePath);
            loadingRequest.SendWebRequest();
            while (!loadingRequest.isDone)
            {
                if (loadingRequest.result == UnityWebRequest.Result.ConnectionError || loadingRequest.result == UnityWebRequest.Result.ProtocolError)
                {
                    break;
                }
            }
            if (loadingRequest.result==UnityWebRequest.Result.ConnectionError || loadingRequest.result==UnityWebRequest.Result.ProtocolError)
            {

            }
            else
            {
                //copies and unpacks file from apk to persistentDataPath where it can be accessed
                string destinationPath = Path.Combine(Application.persistentDataPath, destpathf);

                if (Directory.Exists(destinationPath) == false)
                {
                    Directory.CreateDirectory(destinationPath);
                }
                File.WriteAllBytes(Path.Combine(destinationPath, destname), loadingRequest.downloadHandler.data);
            }




    }
    


    public class MyClass
    {
        public int level;
        public float timeElapsed;
        public string playerName;
    }


    public static void Score_Save(string Directory_path, string date)
    {
        MyClass myObject = new MyClass();
        myObject.level = 1;
        myObject.timeElapsed = 47.5f;
        myObject.playerName = "Dr Charles Francis";

        //Data storage
        SafeCreateDirectory(Application.persistentDataPath + "/" + Directory_path);
        //string json = JsonUtility.ToJson(date);
        string json = JsonUtility.ToJson(myObject);
        StreamWriter Writer = new StreamWriter(Application.persistentDataPath + "/" + Directory_path + "/date.json");
        Writer.Write(json);
        Writer.Flush();
        Writer.Close();
    }



    public static string Score_Load(string Directory_path)
    {
        //Data acquisition
        var reader = new StreamReader(Path.Combine(Application.persistentDataPath, ConfigDir, ConfigFile_Network));
        string json = reader.ReadToEnd();
        reader.Close();

        //MyClass myObjs = JsonUtility.FromJson<MyClass>(json);

        //SampleData mySampleFile = JsonUtility.FromJson<SampleData>(jsonStr);
        return json;//Convert for ease of use
                    //return myObjs.level.ToString();
    }









}
