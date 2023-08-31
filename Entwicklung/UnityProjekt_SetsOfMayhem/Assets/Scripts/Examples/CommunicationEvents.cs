using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine.Networking;
using UnityEngine;
using UnityEngine.Events;
using System;

public static class CommunicationEvents
{
    
    public static UnityEvent<RaycastHit[]> TriggerEvent = new();
    //TODO check if this needs further adjustments
  //  public class HitEvent : UnityEvent<RaycastHit[]> { }
  /*
    public static UnityEvent<int> ToolModeChangedEvent = new();
    public static UnityEvent<Fact> AddFactEvent = new();
    public static UnityEvent<Fact> RemoveFactEvent = new();

    public static UnityEvent<Fact, FactObject.FactMaterials> PushoutFactEvent = new();
    public static UnityEvent<Fact, Scroll.ScrollApplicationInfo> PushoutFactFailEvent = new();

    public static UnityEvent gameSucceededEvent = new();
    public static UnityEvent gameNotSucceededEvent = new();
    public static UnityEvent NewAssignmentEvent = new();

    public static UnityEvent<GameObject, string> ScrollFactHintEvent = new();
    public static UnityEvent<Fact, FactObject.FactMaterials> AnimateExistingFactEvent = new();
    public static UnityEvent<Fact, FactObject.FactMaterials> AnimateExistingAsSolutionEvent = new();
    public static UnityEvent<Fact> AnimateNonExistingFactEvent = new();
    public static UnityEvent<List<string>> HintAvailableEvent = new();
    
    */

    //------------------------------------------------------------------------------------
    //-------------------------------Global Variables-------------------------------------
    // TODO! move to GlobalStatic/Behaviour

    
    public static bool ServerAutoStart = true;
    public static bool ServerRunning = true;
    
    //CHANGE HERE PORT OF SERVER
    public static string ServerPortDefault = "8085"; //used for Local

    public static string ServerAddressLocalhost = "http://localhost"; //Without Port               //Kann das weg?
    public static string ServerAddressLocal = "http://localhost:8085"; // "http://localhost:8085"  //Kann das weg?
    public static string ServerAdress = "http://localhost:8085"; //need "http://" //used by dispalyScrolls.cs //http://10.231.4.95:8085"; //IMPORTANT for MAINMENUE

    public static Process process_mmt_frameIT_server;

    public static bool takeNewToolID = false; //0=no, 1=instead, 2=both
    public static int ToolID_new;
    public static int ToolID_selected;//Script
    
    /*
     * will be loaded from other config file
     */
        public static string lastIP = "";
        public static string newIP = "";
        public static string IPslot1 = "- if you can read this";
        public static string IPslot2 = "- NetworkConfig";
        public static string IPslot3 = "- not loaded";
        public static string selecIP = "GO TO -> 'Options'\n-> 'Reset Options'\nPRESS: \n'Reset Configurations'";

    //------

    public static int[] ServerRunningA = new int[7] { 0, 0, 0, 0, 0, 0, 0 }; //other, lastIP, newIP, IP1, IP2, IP3, selecIP} //0: offline, 1: Checking, 2: online, 3: NoNetworkAddress;
    public static bool[] ServerRunningA_test = new bool[7] { false, false, false, false, false, false, false }; //other, lastIP, newIP, IP1, IP2, IP3, selecIP}
    public static double IPcheckGeneration = 0;
    public static int CheckNetLoop = 1;
    public static int[] CheckServerA = new int[7] { 0, 0, 0, 0, 0, 0, 0 };

    public static bool autoOSrecognition = true;

    public static OperationSystem Opsys = OperationSystem.Windows; //Scripts

    public enum OperationSystem
    {
        Windows=0,
        Android=1,
    }
    public static bool CursorVisDefault = true; //Script.

    public static bool GadgetCanBeUsed = false;


    // Configs
    public static bool VerboseURI = false;


    public enum Directories
    {
        misc,
        Stages,
        SaveGames,
        ValidationSets,
        FactStateMachines,
    }

    public static string debug_path = "hey";

    public static string CreateHierarchiePath(List<Directories> hierarchie, string prefix = "", string postfix = "")
    {
        foreach (var dir in hierarchie)
            prefix = System.IO.Path.Combine(prefix, dir.ToString());

        return System.IO.Path.Combine(prefix, postfix);
    }

    // TODO! avoid tree traversel with name
    public static string CreatePathToFile(out bool file_exists, string name, string format = null, List<Directories> hierarchie = null, bool use_install_folder = false)
    {
        string ending = "";
        if (!string.IsNullOrEmpty(format))
            switch (format)
            {
                case "JSON":
                    ending = ".JSON";
                    break;
                default:
                    break;
            }

        string path = Opsys switch
        {
            OperationSystem.Android => Application.persistentDataPath,
            OperationSystem.Windows or _ => use_install_folder ? Application.dataPath : Application.persistentDataPath,
        };

        if (hierarchie != null)
        {
            path = CreateHierarchiePath(hierarchie, path);
            System.IO.Directory.CreateDirectory(path);
        }

        path = System.IO.Path.Combine(path, name + ending);
        file_exists = System.IO.File.Exists(path);

        debug_path = path;

        return path;
    }
}
