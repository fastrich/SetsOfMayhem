using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static CommunicationEvents;

public class LineTool : Gadget
{
    /// \copydoc Gadget.s_type
    [JsonProperty]
    protected static new string s_type = "LineTool";

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
                FactManager.AddRayFact(Workflow[0], Workflow[1], gadget: this);
                ResetGadget();
                return;
        }
    }


    protected override void _ActivateLineDrawing()
    {
        GadgetBehaviour.LineRenderer.enabled = true;
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
}
