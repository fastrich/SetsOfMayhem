using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Medium;
using static KartenInformationen;

public class karte : MonoBehaviour
{
    public int place_id;


    // Start is called before the first frame update
    void Start()
    {
        Medium.array_cards_status[place_id] = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if (Medium.array_cards_status[place_id] == 0)
        {
            pickrandomcardsettings();

            Medium.array_cards_status[place_id] = 1;

            changeToNotMarked();

        }
    }




    public void OnClick()
    {
        switch (Medium.array_cards_status[place_id])
        {
            case 2:
                changeToNotMarked();
                break;
            default:
                changeToMarked();
                scanForSelected();
                if (numberOfSelected >= numberOfSelected_soll)
                {
                    bool cfs = checkForSetinSelected();
                    if (cfs == true)
                    {
                        for (int i = 0; i < Medium.array_cards_status.Length; i++)
                        {
                            if (Medium.array_cards_status[i] == 2)
                            {
                                Medium.array_cards_status[i] = 0;
                            }
                        }
                    }
                    

                }
                break;






        }
        

        
    }

    private void ggg()
    {
        pickrandomcardsettings();
    }

    private void pickrandomcardsettings()
    {      
        for(int i=0; i<transform.childCount; i++)
        {
            transform.GetChild(i).gameObject.SetActive(false);

        }
        int randomID = Random.Range(0, transform.childCount);
        //transform.GetChild(randomID).gameObject.SetActive(true);
        array[1,1].SetActive(true);

    }
    private void changeToMarked()
    {
        Medium.array_cards_status[place_id] = 2;
        gameObject.transform.localScale = new Vector3((float)1.2, (float)1.2, (float)1.2);
    }
    private void changeToNotMarked()
    {
        Medium.array_cards_status[place_id] = 1; 
        gameObject.transform.localScale = new Vector3((float)1, (float)1, (float)1);
    }
}
