using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; //andr
using UnityEngine.SceneManagement;
using System.IO; //
using UnityEngine.Video;//streaming
using UnityEngine.Networking;

using static UIconfig;
using static StreamingAssetLoader;


public static class updateMouseCursor
{





    public static void setMouse()
    {


        //updateMouseCursor.setMouse();

        if (CommunicationEvents.Opsys == CommunicationEvents.OperationSystem.Android)
        {
            CommunicationEvents.CursorVisDefault = false;
            //Cursor.visible = false;
        }
        else
        {
            Cursor.visible = true;
            CommunicationEvents.CursorVisDefault = true;
        }




        //Android crashes in level scene;
        if (CommunicationEvents.Opsys!= CommunicationEvents.OperationSystem.Windows)
        {

            double curssz = 1 / (UIconfig.cursorSize);
            // print(UIconfig.cursorSize);

            double doesItFit;
            double bestCursTexturSize = double.MaxValue;
            //print(curssz);
            Texture2D bestCursTextur = null;
            for (int i = 0; i < CursorTexture_List_01.Count; i++)
            {
                (int, Texture2D) tupelelement = CursorTexture_List_01[i];

                doesItFit = (UIconfig.screWidth / tupelelement.Item1);

                if ((doesItFit < curssz) && (tupelelement.Item1 < bestCursTexturSize))
                {
                    bestCursTexturSize = tupelelement.Item1;
                    bestCursTextur = tupelelement.Item2;


                }
            }
            Cursor.SetCursor(bestCursTextur, Vector2.zero, CursorMode.ForceSoftware);
            //print("bestCursTexturSize " + bestCursTexturSize + " " + CursorTexture_List_01.Count);



        }
    }
}