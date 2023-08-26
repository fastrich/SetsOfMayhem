using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class methods { 
 
    public static string ArrayToString(int[] iar)
    {
        string sar = "";
        for (int i =0; i< iar.Length; i++)
        {
            sar = sar + " ; i=" + i + " : " + iar[i];
        }


        return sar;
    }

}
