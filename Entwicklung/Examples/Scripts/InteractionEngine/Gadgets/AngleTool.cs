using Newtonsoft.Json;
using UnityEngine;

public class AngleTool : Gadget
{
    /// \copydoc Gadget.s_type
    [JsonProperty]
    protected static new string s_type = "AngleTool";

    //Cache for drawing Angle
    private readonly Vector3[] AnglePoints = new Vector3[3];

    //Vertices for the Curve
    private const int curveDrawingVertexCount = 36;
    private float curveRadius;

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
                AnglePoints[0] = hit[0].transform.position;
                break;

            case 2:
                AnglePoints[1] = hit[0].transform.position;
                AnglePoints[2] = AnglePoints[0] - AnglePoints[1]; // cache for _UpdateLineDrawing()
                ActivateLineDrawing();
                break;

            case 3:
                FactManager.AddAngleFact(Workflow[0], Workflow[1], Workflow[2], gadget: this);

                ResetGadget();
                return;
        }
    }

    //Expect a LineFact here, where Line.Pid2 will be the Basis-Point of the angle
    protected override void _ActivateLineDrawing()
    {
        //In AngleMode with 3 Points we want to draw nearly a rectangle so we add a startPoint and an Endpoint to this preview
        GadgetBehaviour.LineRenderer.positionCount = curveDrawingVertexCount + 2;

        GadgetBehaviour.LineRenderer.startWidth = 0.05f;
        GadgetBehaviour.LineRenderer.endWidth = 0.05f;

        curveRadius = 0.3f * (AnglePoints[0] - AnglePoints[1]).magnitude;
    }

    protected override void _UpdateLineDrawing()
    {
        //Start: AngleMiddlePoint -> FirstPoint of Curve
        SetPosition(0, AnglePoints[1]);
        //End: LastPoint of Curve -> AngleMiddlePoint
        SetPosition(GadgetBehaviour.LineRenderer.positionCount - 1, AnglePoints[1]);

        //Circle: p + r(cost)v1 + r(sint)v2: t real, v1&v2 unit orthogonal, r radius
        Vector3 v1 = (GadgetBehaviour.Cursor.transform.position - AnglePoints[1]).normalized;
        Vector3 v2 = (AnglePoints[2] - Math3d.ProjectPointOnLine(Vector3.zero, v1, AnglePoints[2])).normalized;
        float increment = 2 * Mathf.PI / (curveDrawingVertexCount-1) * Vector3.Angle(v1, AnglePoints[2]) / 360;
        float percentage = 0;
        for (int i = 1; i < curveDrawingVertexCount + 1; i++, percentage += increment)
        {
            Vector3 frag = AnglePoints[1]
                + curveRadius * (Mathf.Cos(percentage) * v1 + Mathf.Sin(percentage) * v2);
            SetPosition(i, frag);
        }
    }
}
