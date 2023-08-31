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



public class uploadMouseCursor : MonoBehaviour
{

    public Texture2D cursorArrow_35;
    public Texture2D cursorArrow_50;
    public Texture2D cursorArrow_60;
    public Texture2D cursorArrow_70;
    public Texture2D cursorArrow_100;
    public Texture2D cursorArrow_140;
    public Texture2D cursorArrow_200;
    public Texture2D cursorArrow_300;
    /*
    public GameObject TAV_Slider;
    public GameObject TAvisibilityT;
    */

    private Color colChangeable = new Color(1f, 1f, 1f, 0.5f);
    private Color colChangeable2 = new Color(1f, 1f, 1f, 0.5f);

    private bool uploadedCursorSkins = false;

    //public GameObject TouchModeButton;


    //public GameObject back_GObj;
    private void Awake()
    {
        uploadCursor();

    }
    void Start()
    {
        //setMouse();
    }

    private void Update()
    {

    }
    public void uploadCursor()
    {


        //List<Tuple<int, Texture2D>> CursorTexture_List_011 = new List<Tuple<int, Texture2D>>();
        //CursorTexture_List_011.Add(new Tuple<int, Texture2D>(12, cursorArrow_35);


        CursorTexture_List_01.Add(new(35, cursorArrow_35));
        CursorTexture_List_01.Add(new(50, cursorArrow_50));
        CursorTexture_List_01.Add(new(60, cursorArrow_60));
        CursorTexture_List_01.Add(new(70, cursorArrow_70));
        CursorTexture_List_01.Add(new(100, cursorArrow_100));
        CursorTexture_List_01.Add(new(140, cursorArrow_140));
        CursorTexture_List_01.Add(new(200, cursorArrow_200));
        CursorTexture_List_01.Add(new(300, cursorArrow_300));
        uploadedCursorSkins = true;
        setMouse();

    }






    public void setMouse()
    {

        if (uploadedCursorSkins == false)
        {
            uploadCursor();
        }
        updateMouseCursor.setMouse();
    }

}