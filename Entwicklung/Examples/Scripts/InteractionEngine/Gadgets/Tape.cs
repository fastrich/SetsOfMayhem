using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static CommunicationEvents;

public class Tape : Gadget
{
    /// \copydoc Gadget.s_type
    [Newtonsoft.Json.JsonProperty]
    protected static new string s_type = "Tape";

    //Cache for drawing Line
    private readonly Vector3[] LineOrigin = new Vector3[1];


    public override void _Hit(RaycastHit[] hit)
    {
        if (hit[0].transform.gameObject.layer != LayerMask.NameToLayer("Point"))
            return;

        string tempFactId = hit[0].transform.GetComponent<FactObject>().URI;
        if (!Workflow.Contains(tempFactId))
            Workflow.Add(tempFactId);

        switch (Workflow.Count)
        {
            case 1:
                LineOrigin[0] = hit[0].transform.position;
                ActivateLineDrawing();
                break;

            case 2:
                FactManager.AddLineFact(Workflow[0], Workflow[1], gadget: this);
                ResetGadget();
                return;

        }
    }

    protected override void _ActivateLineDrawing()
    {
        GadgetBehaviour.LineRenderer.positionCount = 2;

        GadgetBehaviour.LineRenderer.startWidth = 0.095f;
        GadgetBehaviour.LineRenderer.endWidth = 0.095f;
        //Add the position of the Fact for the start of the Line
        SetPosition(0, LineOrigin[0]);
        //The second point is the same point at the moment
        SetPosition(1, LineOrigin[0]);
    }

    //Updates the second-point of the Line when First Point was selected in LineMode
    protected override void _UpdateLineDrawing()
    {
        SetPosition(1, GadgetBehaviour.Cursor.transform.position);
    }



    protected override void _Update_Range()
    {
        float NewMaxRange = UIconfig.interactingRangeMode switch
        {
            UIconfig.InteractingRangeMode.fromObserverView =>
                UIconfig.cursorMaxRange_fromObeserverView,
            UIconfig.InteractingRangeMode.fromCharacterView or _ =>
                MaxRange,
        };
        GadgetBehaviour.Cursor.MaxRange = NewMaxRange;
        Debug.Log("MaxRange :" + NewMaxRange);
    }
}