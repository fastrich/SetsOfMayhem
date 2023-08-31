using System;
using UnityEngine;

/// <summary>
/// Initiates named <see cref="Fact"/> and adds it to <see cref="StageStatic.stage.factState"/>
/// </summary>
/// <param name="samestep">set <c>true</c> if <see cref="Fact"/> creation happens as a subsequent/ consequent step of multiple <see cref="Fact"/> creations and/or deletions, 
/// and you whish that these are affected by a single <see cref="FactOrganizer.undo"/>/ <see cref="FactOrganizer.redo"/> step</param>
/// <returns><see cref="Fact.Id"/> of generated <see cref="Fact"/> if not yet existent in <see cref="StageStatic.stage.factState"/>, else <see cref="Fact.Id"/> of existent equivalent <see cref="Fact"/> </returns>
public static class FactManager
{
    //TODO! communicate success/ failure + consequences
    /// <summary>
    /// Adds <paramref name="fact"/> to <see cref="StageStatic.stage.factState"/>
    /// </summary>
    /// <param name="fact">to be added</param>
    /// <param name="exists"><c>true</c> iff <paramref name="fact"/> already has a equivalent counterpart in <paramref name="fact._Facts"/></param>
    /// \copydetails FactManager
    public static Fact AddFactIfNotFound(Fact fact, out bool exists, bool samestep, Gadget gadget, string scroll_label)
    {
            if (fact is AngleCircleLineFact)
            return AddParsedAngleCircleLineFactIfNotFound((AngleCircleLineFact)fact, out exists, samestep, null, scroll_label);

        else if (fact is AngleFact)
            return AddParsedAngleFactIfNotFound((AngleFact) fact, out exists, samestep, null, scroll_label);
      
        else
            return StageStatic.stage.factState[
            StageStatic.stage.factState.Add(
                fact, out exists, samestep
                , gadget ?? (scroll_label == null ? GadgetBehaviour.ActiveGadget : null)
                , scroll_label
            )];
    }

    /// \copybrief FactManager <summary></summary>
    /// <param name="hit"><c>RaycastHit</c> where and how (orientation) to spawn <see cref="PointFact"/></param>
    /// \copydetails FactManager
    public static PointFact AddPointFact(RaycastHit hit, bool samestep = false, Gadget gadget = null, string scroll_label = null)
    {
        return (PointFact) AddFactIfNotFound(
            new PointFact(hit.point, hit.normal, StageStatic.stage.factState)
            , out _, samestep, gadget, scroll_label);
    }

    /// \copybrief FactManager <summary></summary>
    /// <param name="point">where to spawn <see cref="PointFact"/></param>
    /// <param name="normal">how (orientation) to spawn <see cref="PointFact"/></param>
    /// \copydetails FactManager
    public static PointFact AddPointFact(Vector3 point, Vector3 normal, bool samestep = false, Gadget gadget = null, string scroll_label = null)
    {
        return (PointFact) AddFactIfNotFound(
            new PointFact(point, normal, StageStatic.stage.factState)
            , out _, samestep, gadget, scroll_label);
    }

    /// \copybrief FactManager <summary></summary>
    /// <param name="pid"><see cref="Fact.Id"/> of <see cref="PointFact"/> which lies on <paramref name="lid"/></param>
    /// <param name="lid"><see cref="Fact.Id"/> of <see cref="LineFact"/> on which <paramref name="pid"/> lies</param>
    /// \copydetails FactManager
    public static OnLineFact AddOnLineFact(string pid, string lid, bool samestep = false, Gadget gadget = null, string scroll_label = null, bool is_certain = false)
    {
        if (!is_certain)
        {
            AbstractLineFact line = (AbstractLineFact)StageStatic.stage.factState[lid];
            PointFact line_point = (PointFact)StageStatic.stage.factState[line.Pid1];
            PointFact point = (PointFact)StageStatic.stage.factState[pid];

            if (!Math3d.IsPointApproximatelyOnLine(line_point.Point, line.Dir, point.Point))
                return null;
        }

        return (OnLineFact)AddFactIfNotFound(
            new OnLineFact(pid, lid, StageStatic.stage.factState)
            , out _, samestep, gadget, scroll_label);
    }

    /// \copybrief FactManager <summary></summary>
    /// <param name="pid1"><see cref="Fact.Id"/> of first <see cref="PointFact"/> defining a <see cref="LineFact"/></param>
    /// <param name="pid2"><see cref="Fact.Id"/> of second <see cref="PointFact"/> defining a <see cref="LineFact"/></param>
    /// \copydetails FactManager
    public static LineFact AddLineFact(string pid1, string pid2, bool samestep = false, Gadget gadget = null, string scroll_label = null)
    {
        return (LineFact)AddFactIfNotFound(
            new LineFact(pid1, pid2, StageStatic.stage.factState)
            , out _, samestep, gadget, scroll_label);
    }

    /// \copybrief FactManager
    /// <summary>
    /// Creates aditionally <see cref="OnLineFact">OnLineFacts</see> when <see cref="RayFact"/> crosses <see cref="PointFact">PointFacts</see>.
    /// </summary>
    /// <param name="pid1"><see cref="Fact.Id"/> of first <see cref="PointFact"/> defining a <see cref="RayFact"/></param>
    /// <param name="pid2"><see cref="Fact.Id"/> of second <see cref="PointFact"/> defining a <see cref="RayFact"/></param>
    /// \copydetails FactManager
    public static RayFact AddRayFact(string pid1, string pid2, bool samestep = false, Gadget gadget = null, string scroll_label = null)
    {
        RayFact rayFact = (RayFact)AddFactIfNotFound(
            new RayFact(pid1, pid2, StageStatic.stage.factState)
            , out bool exists, samestep, gadget, scroll_label);

        if (exists)
            return rayFact;

        //Add all PointFacts on Ray as OnLineFacts
        PointFact rayP1 = (PointFact)StageStatic.stage.factState[rayFact.Pid1];
        PointFact rayP2 = (PointFact)StageStatic.stage.factState[rayFact.Pid2];
        int layerMask = LayerMask.GetMask("Point");
        RaycastHit[] hitsA = Physics.RaycastAll(rayP1.Point,  rayFact.Dir, Mathf.Infinity, layerMask);
        RaycastHit[] hitsB = Physics.RaycastAll(rayP2.Point, -rayFact.Dir, Mathf.Infinity, layerMask);

        void AddHitIfOnLine(RaycastHit hit)
        {
            if (Math3d.IsPointApproximatelyOnLine(rayP1.Point, rayFact.Dir, hit.transform.position))
            {
                AddOnLineFact(
                    hit.transform.gameObject.GetComponent<FactObject>().URI, rayFact.Id, true, gadget, scroll_label, true);
            }
        }

        foreach (RaycastHit hit in hitsA)
            AddHitIfOnLine(hit);

        foreach (RaycastHit hit in hitsB)
            AddHitIfOnLine(hit);

        // for good measure
        AddOnLineFact(rayFact.Pid1, rayFact.Id, true, gadget, scroll_label, true);
        AddOnLineFact(rayFact.Pid2, rayFact.Id, true, gadget, scroll_label, true);

        return rayFact;
    }

    /// \copybrief FactManager <summary></summary>
    /// <param name="pid1"><see cref="Fact.Id"/> of first <see cref="PointFact"/> defining a <see cref="AngleFact"/></param>
    /// <param name="pid2"><see cref="Fact.Id"/> of second <see cref="PointFact"/> defining a <see cref="AngleFact"/></param>
    /// <param name="pid3"><see cref="Fact.Id"/> of third <see cref="PointFact"/> defining a <see cref="AngleFact"/></param>
    /// \copydetails FactManager
    public static AngleFact AddAngleFact(string pid1, string pid2, string pid3, bool samestep = false, Gadget gadget = null, string scroll_label = null)
    {

        AngleFact angle = (AngleFact)AddFactIfNotFound(
            new AngleFact(pid1, pid2, pid3, StageStatic.stage.factState)
            , out _, samestep, gadget, scroll_label);

        if (angle.is_right_angle) { 
            AddFactIfNotFound(
                new RightAngleFact(pid1, pid2, pid3, StageStatic.stage.factState)
                , out _, samestep, gadget, scroll_label);
        }

        return angle;
    }

    public static AngleCircleLineFact AddAngleCircleLineFact(string cid, string lid, float angleValue, bool samestep = false, Gadget gadget = null, string scroll_label = null)
    {

        AngleCircleLineFact angle = (AngleCircleLineFact)AddFactIfNotFound(
            new AngleCircleLineFact(cid, lid, angleValue, StageStatic.stage.factState)
            , out _, samestep, gadget, scroll_label);

        if (Mathf.Abs(Mathf.Abs(angleValue) - 90.0f) < 0.1)
        {
            AddFactIfNotFound(
                new OrthogonalCircleLineFact(cid, lid, StageStatic.stage.factState)
                , out _, samestep, gadget, scroll_label);
        }

        return angle;
    }
    public static Fact AddParsedAngleCircleLineFactIfNotFound(AngleCircleLineFact fact, out bool exists, bool samestep, Gadget gadget, string scroll_label)
    {

        if (Mathf.Abs(Mathf.Abs(fact.angle) - 90.0f) < 0.1)
        {
            AddFactIfNotFound(
                new OrthogonalCircleLineFact(fact.Cid1, fact.Rid2, StageStatic.stage.factState)
                , out _, samestep, gadget, scroll_label);
        }

        return StageStatic.stage.factState[
            StageStatic.stage.factState.Add(
                fact, out exists, samestep
                , gadget ?? (scroll_label == null ? GadgetBehaviour.ActiveGadget : null)
                , scroll_label
            )];
    }
    public static Fact AddParsedAngleFactIfNotFound(AngleFact fact, out bool exists, bool samestep, Gadget gadget, string scroll_label)
    {
        if (fact.is_right_angle)
            AddFactIfNotFound(
                new RightAngleFact(fact.Pid1, fact.Pid2, fact.Pid3, StageStatic.stage.factState)
                , out _, samestep, gadget, scroll_label);

        return StageStatic.stage.factState[
            StageStatic.stage.factState.Add(
                fact, out exists, samestep
                , gadget ?? (scroll_label == null ? GadgetBehaviour.ActiveGadget : null)
                , scroll_label
            )];
    }

    public static EqualCirclesFact AddEqualCirclesFact(string cid1, string cid2, bool samestep = false, Gadget gadget = null, string scroll_label = null)
    {
        return (EqualCirclesFact)AddFactIfNotFound(new EqualCirclesFact(cid1, cid2, StageStatic.stage.factState), out _, samestep,gadget,scroll_label);
    }
    public static UnEqualCirclesFact AddUnEqualCirclesFact(string cid1, string cid2, bool samestep = false, Gadget gadget = null, string scroll_label = null)
    {
        return (UnEqualCirclesFact)AddFactIfNotFound(new UnEqualCirclesFact(cid1, cid2, StageStatic.stage.factState), out _, samestep,gadget,scroll_label);
    }
}
