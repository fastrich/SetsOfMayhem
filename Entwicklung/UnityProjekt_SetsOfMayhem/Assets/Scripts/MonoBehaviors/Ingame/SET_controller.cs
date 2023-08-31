using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static medium;
using static algos;
using static kartenInformationen;
using static methods_unity;
using static config_parameters;

public class SET_controller : MonoBehaviour
{
    public GameObject kartenPrefab;
    public Canvas canvasForCards;
    private Vector2 cornerTopRight = new Vector2(0.5f, 0.6f);
    private Vector2 cornerBottemLeft = new Vector2(0, 0);



    // Start is called before the first frame update
    void Start()
    {
        //LadeKartenMaterial1(); veraltet
        CreateAndDistributeCardsOnScreen();
        
    }


    // Update is called once per frame
    void Update()
    {
        checkForPlayerSetSelection();
        CheckAndFillCardField();
        
    }


    void CreateAndDistributeCardsOnScreen()
    {
        string name1;
        int k = 0;
        int n = Game_numberOfCardsOnDeck;
        int nh = (int)Mathf.Ceil(Mathf.Sqrt(n));
        int nb = (int)Mathf.Ceil(Mathf.Sqrt(n));
        float jh = 0;
        int jb = 0;
        //Debug.Log("nh, nb" + nh + " " + nb);
        //CreateGameObjectFromPrefab(kartenPrefab, canvasForCards, cornerTopRight, cornerBottemLeft);
        Vector2 cornerTopRight = new Vector2(0.5f, 0.6f);
        Vector2 cornerBottemLeft = new Vector2(0, 0);

        while (k < n)
        {

            string name2 = k.ToString();
            name1 = Editor_NameCardslots;
            name1 = name1.Substring(0, name1.Length - name2.Length);
            name1 = name1 + name2;
            cornerTopRight = new Vector2((float)(jb + 1) / nb, (float)1 - (jh / nh));
            cornerBottemLeft = new Vector2((float)jb / nb, (float)1 - ((1 + jh) / nh));
            //Debug.Log("k" + k+ " ctr" + cornerTopRight + " cbl " + cornerBottemLeft);
            CreateGameObjectFromPrefab(name1, kartenPrefab, canvasForCards, cornerTopRight, cornerBottemLeft);

            jb++;
            if (jb >= nb)
            {
                jh++;
                jb = 0;
            }
            k++;

        }


    }
}
