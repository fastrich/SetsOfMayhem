using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static CommunicationEvents;

public class Pendulum : Gadget
//Acts as a Pendulum starting at a Point
{
    /// \copydoc Gadget.s_type
    [Newtonsoft.Json.JsonProperty]
    protected static new string s_type = "Pendulum";

    public override void _Enable()
    {
        ActivateLineDrawing();
    }



    public override void _Hit(RaycastHit[] hit)
    {
        if (hit[0].transform.gameObject.layer != LayerMask.NameToLayer("Point"))
            return;

        //Raycast downwoard
        if (Physics.Raycast(hit[0].transform.position, Vector3.down, out RaycastHit ground, Mathf.Infinity, this.SecondaryLayerMask.value))
        {
            string tempFactId = hit[0].transform.GetComponent<FactObject>().URI;
            Workflow.Add(tempFactId);
            FactManager.AddPointFact(ground, gadget: this);
            ResetGadget();

        }
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
        SetPosition(0, GadgetBehaviour.Cursor.transform.position);

        //Raycast downwoard
        if (Physics.Raycast(GetPosition(0), Vector3.down, out RaycastHit ground, Mathf.Infinity, this.SecondaryLayerMask.value))
            SetPosition(1, ground.point);
        else
            SetPosition(1, GetPosition(0));
    }
}
