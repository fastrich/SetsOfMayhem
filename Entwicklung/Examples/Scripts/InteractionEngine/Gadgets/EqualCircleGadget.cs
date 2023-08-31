using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static CommunicationEvents;

/// <summary>
/// a Gadget that checks whether two given circles have equal size and if yes it returns an EqualCirclesFact
/// </summary>
public class EqualCircleGadget : Gadget
{

    [Newtonsoft.Json.JsonProperty]
    protected static new string s_type = "EqualCircles";

    //Variables to safe if one circle has already been selected
    private bool FirstCircleSelected = false;
    private CircleFact FirstCircle = null;




    new void Awake()
    {
        base.Awake();
        UiName = "EqualCircles Mode";
        if (MaxRange == 0)
            MaxRange = GlobalBehaviour.GadgetLaserDistance;
    }


    public override void _Hit(RaycastHit[] hit)
    {
        if (hit[0].transform.gameObject.layer == LayerMask.NameToLayer("Circle"))
        {
            CircleFact tempFact = (CircleFact)StageStatic.stage.factState[hit[0].transform.GetComponent<FactObject>().URI];

            //If the first circle got already selected
            if (this.FirstCircleSelected)
            {
                // Debug.Log("hit it");
               //  Debug.Log("data: radius dif" + Mathf.Abs(this.FirstCircle.radius - tempFact.radius) +" ids: 1. "+ this.FirstCircle.Id+", 2."+ tempFact.Id);
                //Create EqualCirclesFact
                //Check if new Point is equal to one of the previous points -> if true -> cancel
                if ((Mathf.Abs(this.FirstCircle.radius - tempFact.radius) < 0.01) && !(this.FirstCircle.Id == tempFact.Id))
                {
                    FactManager.AddEqualCirclesFact(((CircleFact)this.FirstCircle).Id, ((CircleFact)tempFact).Id);
                }
                else {
                    if(!(this.FirstCircle.Id == tempFact.Id)) 
                        FactManager.AddUnEqualCirclesFact(((CircleFact)this.FirstCircle).Id, ((CircleFact)tempFact).Id);

                }

                ResetGadget();
            }
            //If no circle was selected before
            else
            {
                //Save the first point selected
                this.FirstCircleSelected = true;
                this.FirstCircle= tempFact;
            }
        }
        //No Circles were hit
        else
        {
            ResetGadget();

        }
    }


    private void ResetGadget()
    {
        this.FirstCircleSelected= false;
        this.FirstCircle = null;

    }


}
