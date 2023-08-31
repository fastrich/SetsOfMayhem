using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static CommunicationEvents;

public class PoleTool : Gadget
//Acts as a Pendulum starting at a Point
{
    /// \copydoc Gadget.s_type
    [Newtonsoft.Json.JsonProperty]
    protected static new string s_type = "PoleTool";

    public float poleHeight = 1f;

    public override void _Enable()
    {
        ActivateLineDrawing();
    }


    public override void _Hit(RaycastHit[] hit)
    {
        if (!Physics.Raycast(
                GadgetBehaviour.Cursor.transform.position + Vector3.up * (float)Math3d.vectorPrecission
                , Vector3.down, MaxHeight + (float)Math3d.vectorPrecission
                , LayerMask.GetMask(new string[]{"Default", "Tree"})))
            return;

        UpdateLineDrawing();



        if (hit[0].transform.gameObject.layer == LayerMask.NameToLayer("Point"))
        {

            Workflow.Add(hit[0].transform.gameObject.GetComponent<FactObject>().URI);
            //TODO check if below works else fix with the commented stuff
            // var pid2 = FactManager.AddPointFact(linePositions[1], Vector3.up).Id;
           // FactManager.AddLineFact(hit[0].transform.gameObject.GetComponent<FactObject>().URI, pid2, true);
            
            var pid2 = FactManager.AddPointFact(GetPosition(1), Vector3.up, gadget: this).Id;
            FactManager.AddLineFact(Workflow[0], pid2, true, gadget: this);
        }
        else
        {
            FactManager.AddPointFact(hit[0], gadget: this);

        }

        ResetGadget();
        ActivateLineDrawing();
    }

    protected override void _ActivateLineDrawing()
    {
        GadgetBehaviour.LineRenderer.positionCount = 2;

        GadgetBehaviour.LineRenderer.startWidth = 0.095f;
        GadgetBehaviour.LineRenderer.endWidth = 0.095f;
    }

    //Updates the points of the Lines when baseLine was selected in LineMode
    protected override void _UpdateLineDrawing()
    {

        //TODO check whether this works else
        // this.linePositions[0] = this.Cursor.transform.position;
        SetPosition(0, GadgetBehaviour.Cursor.transform.position);

        //Raycast upwoard
        if (Physics.Raycast(GetPosition(0), Vector3.up, out RaycastHit ceiling, poleHeight, this.SecondaryLayerMask.value))
            SetPosition(1, ceiling.point);
        else
            SetPosition(1, GetPosition(0) + Vector3.up * poleHeight);
    }
}
