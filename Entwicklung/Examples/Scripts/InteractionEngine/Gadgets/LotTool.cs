using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static CommunicationEvents;

public class LotTool : Gadget
//constructs a Perpendicular between a Line and a Point
{
    /// \copydoc Gadget.s_type
    [JsonProperty]
    protected static new string s_type = "LotTool";

    //Cache for drawing Lines
    private AbstractLineFact BaseLine;
    private Vector3 BaseLineRoot;
    private Vector3 IntersectionPoint;
    private Vector3 BaseLineHit;



    public override void _Hit(RaycastHit[] hit)
    {
        void CreateRayAndAngles(string IntersectionId, string LotPointId, bool samestep)
        {
            FactManager.AddRayFact(IntersectionId, LotPointId, samestep, gadget: this);

            //TODO? create at all? / for all points on basline?
            FactManager.AddAngleFact(
                BaseLine.Pid1 == IntersectionId ? BaseLine.Pid2 : BaseLine.Pid1
                , IntersectionId, LotPointId, samestep: true, gadget: this);
        }


        string tempFactId = null;
        if (!((LayerMask) hit[0].transform.gameObject.layer).IsAnyByName(new string[] { "Default", "Tree" })
         && (!hit[0].transform.TryGetComponent(out FactObject obj)
           || Workflow.Contains(tempFactId = obj.URI)))
            return;

        Fact tempFact = tempFactId == null ? null : StageStatic.stage.factState[tempFactId];
        switch (Workflow.Count)
        {
            case 0: // select basline
                if (tempFact is not AbstractLineFact)
                    return;
                Workflow.Add(tempFactId);

                BaseLine = (AbstractLineFact) tempFact;
                BaseLineRoot = ((PointFact) StageStatic.stage.factState[BaseLine.Pid1]).Point;

                BaseLineHit = hit[0].point;
                ActivateLineDrawing();
                return;

            case 1: // select point perpendiculum/lot goes through
                if (tempFact is not PointFact)
                    return;

                IntersectionPoint = Math3d.ProjectPointOnLine(BaseLineRoot, BaseLine.Dir, hit[0].transform.position);
                if (Math3d.IsApproximatelyEqual(IntersectionPoint, hit[0].transform.position))
                {   // TempFact is on baseLine
                    Workflow.Add(tempFactId);
                    return;
                }
                else
                {   // create perpendicular through existing Point off Line
                    Vector3 normal = Vector3.Cross(BaseLine.Dir, hit[0].transform.position - IntersectionPoint).normalized;
                    normal *= Mathf.Sign(Vector3.Dot(normal, Vector3.up)); // point up
                    var intersectionId = FactManager.AddPointFact(IntersectionPoint, normal, gadget: this).Id;

                    if (BaseLine is RayFact) // Add OnLineFact only on Ray not Line
                        FactManager.AddOnLineFact(intersectionId, Workflow[0], true, gadget: this, is_certain: true);

                    CreateRayAndAngles(intersectionId, tempFactId, true);
                    ResetGadget();
                }
                break;

            case 2: // create perpendicular through new Point off Line
                if (!((LayerMask) hit[0].transform.gameObject.layer).IsAnyByName(new string[] { "Default", "Tree" }))
                    return;

                Vector3 LotPoint = Math3d.ProjectPointOnLine(hit[0].point, BaseLine.Dir, IntersectionPoint);
                CreateRayAndAngles(Workflow[1], FactManager.AddPointFact(LotPoint, hit[0].normal, gadget: this).Id, true);
                ResetGadget();
                return;

        }
    }

    protected override void _ActivateLineDrawing()
    {
        GadgetBehaviour.LineRenderer.positionCount = 3;

        GadgetBehaviour.LineRenderer.startWidth = 0.095f;
        GadgetBehaviour.LineRenderer.endWidth = 0.095f;

        //SetPositions(new Vector3[] { GadgetBehaviour.Cursor.transform.position, LotPoints[0], LotPoints[0] });
        //start at curser
        SetPosition(0, GadgetBehaviour.Cursor.transform.position);
        //Project curser perpendicular on Line for intersection-point
        SetPosition(1, Math3d.ProjectPointOnLine(BaseLineRoot, BaseLine.Dir, GetPosition(0)));
        //end at Point on the line (i.c.o. projection beeing outside a finite line)
        SetPosition(2, Math3d.ProjectPointOnLine(BaseLineRoot, BaseLine.Dir, BaseLineHit));
    }

    //Updates the points of the Lines when baseLine was selected in LineMode
    protected override void _UpdateLineDrawing()
    {
        if (Workflow.Count < 2) {
            SetPosition(0, GadgetBehaviour.Cursor.transform.position);
            SetPosition(1, Math3d.ProjectPointOnLine(BaseLineRoot, BaseLine.Dir, GetPosition(0)));
        }
        else {
            SetPosition(1, IntersectionPoint);
            SetPosition(0, Math3d.ProjectPointOnLine(GadgetBehaviour.Cursor.transform.position, BaseLine.Dir, GetPosition(1)));
        }
    }
}
