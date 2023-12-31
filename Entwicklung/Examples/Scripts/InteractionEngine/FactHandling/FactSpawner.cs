﻿using System;
using System.Collections;
using TMPro;
using UnityEngine;
using static CommunicationEvents;
using static GlobalBehaviour;

public class FactSpawner : MonoBehaviour
{
    public GameObject
        Sphere,
        Line,
        Ray,
        Angle,
        Ring,
        Circle;

    void Start()
    {
        AddFactEvent.AddListener(SpawnFactRepresentation_Wrapped);
        RemoveFactEvent.AddListener(DeleteObject);

        AnimateNonExistingFactEvent.AddListener(animateNonExistingFactTrigger);
    }

    private void SpawnFactRepresentation_Wrapped(Fact fact) => SpawnFactRepresentation(fact);

    public Fact SpawnFactRepresentation(Fact fact)
    {
        Func<Fact, Fact> func = fact switch
        {
            PointFact   => SpawnPoint,
            LineFact    => SpawnLine,
            AngleFact   => SpawnAngle,
            RayFact     => SpawnRay,
            CircleFact  => SpawnRingAndCircle,
            _ => null,
        };

        return func?.Invoke(fact);

        //TODO check if the above breaks anything
        //return fact switch
        //{
        //    PointFact pointFact => SpawnPoint,
        //    LineFact lineFact => SpawnLine,
        //    AngleFact angleFact => SpawnAngle,
        //    RayFact rayFact => SpawnRay,
        //    CircleFact circleFact => SpawnRingAndCircle,
        //    _ => null,
        //};

    }
  

    public Fact SpawnPoint(Fact pointFact)
    {
        PointFact fact = ((PointFact)pointFact);
     
        GameObject point = GameObject.Instantiate(Sphere);
        point.transform.position = fact.Point;
        point.transform.up = fact.Normal;
        point.GetComponentInChildren<TextMeshPro>().text = fact.Label;
        point.GetComponent<FactObject>().URI = fact.Id;
        fact.Representation = point;
        return fact;
    }

    public Fact SpawnLine(Fact fact)
    {
        LineFact lineFact = ((LineFact)fact);

        PointFact pointFact1 = (StageStatic.stage.factState[lineFact.Pid1] as PointFact);
        PointFact pointFact2 = (StageStatic.stage.factState[lineFact.Pid2] as PointFact);
        Vector3 point1 = pointFact1.Point;
        Vector3 point2 = pointFact2.Point;
        //Change FactRepresentation to Line
        GameObject line = GameObject.Instantiate(Line);
        //Place the Line in the centre of the two points
        line.transform.position = Vector3.Lerp(point1, point2, 0.5f);
        //Change scale and rotation, so that the two points are connected by the line
        //Get the Line-GameObject as the first Child of the Line-Prefab -> That's the Collider
        var v3T = line.transform.GetChild(0).localScale;

        //For every Coordinate x,y,z we have to devide it by the LocalScale of the Child,
        //because actually the Child should be of this length and not the parent, which is only the Collider
        v3T.x = (point2 - point1).magnitude / line.transform.GetChild(0).GetChild(0).localScale.x;

        //Change Scale/Rotation of the Line-GameObject without affecting Scale of the Text
        line.transform.GetChild(0).localScale = v3T;
        line.transform.GetChild(0).rotation = Quaternion.FromToRotation(Vector3.right, point2 - point1);

        //string letter = ((Char)(64 + lineFact.Id + 1)).ToString();
        //line.GetComponentInChildren<TextMeshPro>().text = letter;
        line.GetComponentInChildren<TextMeshPro>().text = pointFact1.Label + pointFact2.Label;
        line.GetComponentInChildren<TextMeshPro>().text += " = " + Math.Round((point1-point2).magnitude, 2) + " m";
        line.GetComponentInChildren<FactObject>().URI = lineFact.Id;
        lineFact.Representation = line;
        return lineFact;

    }

    public Fact SpawnRay(Fact fact)
    {
        RayFact rayFact = ((RayFact)fact);

        PointFact pointFact1 = (StageStatic.stage.factState[rayFact.Pid1] as PointFact);
        PointFact pointFact2 = (StageStatic.stage.factState[rayFact.Pid2] as PointFact);

 
        Vector3 point1 = pointFact1.Point;
        Vector3 point2 = pointFact2.Point;

        Vector3 dir = (point2 - point1).normalized;
        point1 -= dir * 100;
        point2 += dir * 100;

        //Change FactRepresentation to Line
        GameObject line = GameObject.Instantiate(Ray);
        //Place the Line in the centre of the two points
        line.transform.position = Vector3.Lerp(point1, point2, 0.5f);
        //Change scale and rotation, so that the two points are connected by the line
        //Get the Line-GameObject as the first Child of the Line-Prefab -> That's the Collider
        var v3T = line.transform.GetChild(0).localScale;

        //For every Coordinate x,y,z we have to devide it by the LocalScale of the Child,
        //because actually the Child should be of this length and not the parent, which is only the Collider
        v3T.x = (point2 - point1).magnitude / line.transform.GetChild(0).GetChild(0).localScale.x;

        //Change Scale/Rotation of the Line-GameObject without affecting Scale of the Text
        line.transform.GetChild(0).localScale = v3T;
        line.transform.GetChild(0).rotation = Quaternion.FromToRotation(Vector3.right, point2 - point1);

        line.GetComponentInChildren<TextMeshPro>().text = rayFact.Label;
        line.GetComponentInChildren<FactObject>().URI = rayFact.Id;

        rayFact.Representation = line;
        return rayFact;
    }
    
    //Spawn an angle: point with id = angleFact.Pid2 is the point where the angle gets applied
    public Fact SpawnAngle(Fact fact)
    {
        AngleFact angleFact = (AngleFact)fact;

        Vector3 point1 = (StageStatic.stage.factState[angleFact.Pid1] as PointFact).Point;
        Vector3 point2 = (StageStatic.stage.factState[angleFact.Pid2] as PointFact).Point;
        Vector3 point3 = (StageStatic.stage.factState[angleFact.Pid3] as PointFact).Point;

        //Length of the Angle relative to the Length of the shortest of the two lines (point2->point1) and (point2->point3)
        float lengthFactor = 0.3f;
        
        float length;
        if ((point1 - point2).magnitude >= (point3 - point2).magnitude)
            length = lengthFactor * (point3 - point2).magnitude;
        else
            length = lengthFactor * (point1 - point2).magnitude;

        //Change FactRepresentation to Angle
        GameObject angle = GameObject.Instantiate(Angle);

        //Calculate Angle:
        Vector3 from = (point3 - point2).normalized;
        Vector3 to = (point1 - point2).normalized;
        float angleValue = Vector3.Angle(from, to); //We always get an angle between 0 and 180° here

        //Change scale and rotation, so that the angle is in between the two lines
        var v3T = angle.transform.localScale;
        v3T = new Vector3(length, v3T.y, length);

        Vector3 up = Vector3.Cross(to, from);
        //Place the Angle at position of point2
        angle.transform.SetPositionAndRotation(point2, Quaternion.LookRotation(Vector3.Cross((from+to).normalized,up), up));

        //Set text of angle
        TextMeshPro[] texts = angle.GetComponentsInChildren<TextMeshPro>();
        foreach (TextMeshPro t in texts) {
            //Change Text not to the id, but to the angle-value (from both sides) AND change font-size relative to length of the angle (from both sides)
            t.text = Math.Round((double) angleValue, 2) + "°";
            t.fontSize = angle.GetComponentInChildren<TextMeshPro>().fontSize * angle.transform.GetChild(0).transform.GetChild(0).localScale.x;
        }

        //Generate angle mesh
        CircleSegmentGenerator[] segments = angle.GetComponentsInChildren<CircleSegmentGenerator>();
        foreach (CircleSegmentGenerator c in segments)
            c.setAngle(angleValue);

        angle.GetComponentInChildren<FactObject>().URI = angleFact.Id;
        angleFact.Representation = angle;
        return angleFact;
    }

    public Fact SpawnRingAndCircle(Fact fact)
    {
        var ringAndCircleGO = new GameObject("RingAndCircle");
        _ = SpawnRing(fact, ringAndCircleGO.transform);
        var circleFact = SpawnCircle(fact, ringAndCircleGO.transform);

        //TODO check whether this is necessary?
       // this.FactRepresentation = ringAndCircleGO;
        circleFact.Representation = ringAndCircleGO;

        return circleFact;
    }

    public Fact SpawnRing(Fact fact, Transform parent = null)
    {
        CircleFact circleFact = (CircleFact)fact;

        PointFact middlePointFact = StageStatic.stage.factState[circleFact.Pid1] as PointFact;
        PointFact basePointFact = StageStatic.stage.factState[circleFact.Pid2] as PointFact;

        Vector3 middlePoint = middlePointFact.Point;
        Vector3 normal = circleFact.normal;
        float radius = circleFact.radius;

        //Change FactRepresentation to Ring
        //TODO check whether this is necessary?

        //this.FactRepresentation = Ring;
        //GameObject ring = Instantiate(FactRepresentation, parent);
        GameObject ring = GameObject.Instantiate(Ring,parent);

        var tori = ring.GetComponentsInChildren<TorusGenerator>();
        var tmpText = ring.GetComponentInChildren<TextMeshPro>();
        var FactObj = ring.GetComponentInChildren<FactObject>();

        //Move Ring to middlePoint
        ring.transform.position = middlePoint;

        //Rotate Ring according to normal
        if (normal.y < 0) // if normal faces downwards use inverted normal instead
            ring.transform.up = -normal;
        else
            ring.transform.up = normal;

        //Set radii
        foreach (var torus in tori)
            torus.torusRadius = radius;

        string text = $"○{middlePointFact.Label}";
        tmpText.text = text;
        ////move TMP Text so it is on the edge of the circle
        //tmpText.rectTransform.position = tmpText.rectTransform.position - new Vector3(0, 0, -radius);

        FactObj.URI = circleFact.Id;
        circleFact.Representation = ring;

        return circleFact;
    }

    public Fact SpawnCircle(Fact fact, Transform parent = null)
    {
        CircleFact circleFact = (CircleFact)fact;

        PointFact middlePointFact = StageStatic.stage.factState[circleFact.Pid1] as PointFact;
        PointFact basePointFact = StageStatic.stage.factState[circleFact.Pid2] as PointFact;

        Vector3 middlePoint = middlePointFact.Point;
        Vector3 normal = circleFact.normal;
        float radius = circleFact.radius;

        //TODO check whether this is necessary
        //Change FactRepresentation to Ring
       // this.FactRepresentation = Circle;
        GameObject circle = Instantiate(Circle, parent);

        var FactObj = circle.GetComponentInChildren<FactObject>();

        //Move Circle to middlePoint
        circle.transform.position = middlePoint;

        //Rotate Circle according to normal
        if (normal.y < 0) // if normal faces downwards use inverted normal instead
            circle.transform.up = -normal;
        else
            circle.transform.up = normal;

        //Set radius
        circle.transform.localScale = new Vector3(radius, circle.transform.localScale.y, radius);

        FactObj.URI = circleFact.Id;
        circleFact.Representation = circle;

        return circleFact;
    }


    public void DeleteObject(Fact fact)
    {
        GameObject factRepresentation = fact.Representation;
        print("Deleting: " + fact.Representation?.name);
        GameObject.Destroy(factRepresentation);
    }

    public void animateNonExistingFactTrigger(Fact fact) {
        StartCoroutine(animateNonExistingFact(fact));
    }

    public IEnumerator animateNonExistingFact(Fact fact) {
        Fact returnedFact = SpawnFactRepresentation(fact);

        ShinyThings.HighlightFact(returnedFact, FactObject.FactMaterials.Hint);

        yield return new WaitForSeconds(GlobalBehaviour.hintAnimationDuration);

        GameObject.Destroy(returnedFact.Representation);
    }
}
