using System;
using System.Globalization;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using static GadgetBehaviour;
using UnityEngine.InputSystem;
//TODO check whether this can be deleted 
//using System.Linq;
//using static GadgetManager;

public class WorldCursor : MonoBehaviour
{
    public RaycastHit Hit;
    // TODO experimentell for multiple hits
    public RaycastHit[] MultipleHits;

    public string deactivateSnapKey;
    private Camera Cam;
    private int layerMask;

    public float MaxRange = 10f;
    private float MaxRange_ = 10f;
    public bool useCamCurser = false;
    private int whichCheckMouseButton = 1;

    private void Awake()
    {
        //Ignore player and TalkingZone
        this.layerMask = ~LayerMask.GetMask("Player", "TalkingZone");
    }

    void Start()
    {
        Cam = Camera.main;
        //Set MarkPointMode as the default ActiveToolMode
        // ActiveToolMode = ToolMode.ExtraMode;//ToolMode.MarkPointMode;
        // CommunicationEvents.ToolModeChangedEvent.Invoke(activeGadget.id);
        CultureInfo.CurrentCulture = new CultureInfo("en-US");
    }

    public void setLayerMask(int layerMask)
    {
        this.layerMask = layerMask;
    }



    // working currently to include multiple hits 
    // TODO 

    void Update()
    {
        updateMaxRange();

        Cam = Camera.main; //WARN: Should not called every Update; TODO: Cache in Start/Awake?
        Vector3 mousePos = UIconfig.InputManagerVersion switch
        {
            1 or 2 or 3 => Input.mousePosition,
            _ => Vector3.zero
        };

        //Ray ray = useCamCurser ? new Ray(Cam.transform.position, Cam.transform.forward) : Cam.ScreenPointToRay(Input.mousePosition);
        Ray ray = useCamCurser ? new Ray(Cam.transform.position, Cam.transform.forward) : Cam.ScreenPointToRay(mousePos);

        transform.up = Cam.transform.forward;
        transform.position = ray.GetPoint(GlobalBehaviour.GadgetPhysicalDistance);

        //************************************************
        bool deactSnapKey =
               Input.GetButton(this.deactivateSnapKey)
            && UIconfig.InputManagerVersion >= 1
            && UIconfig.InputManagerVersion <= 3;
        //************************************************


        // in case we dont hit anything, just return
        if (!(Physics.Raycast(ray, out Hit, MaxRange, layerMask)
            || (MaxRange <= GlobalBehaviour.GadgetPhysicalDistance
            && Physics.Raycast(transform.position, Vector3.down, out Hit, GlobalBehaviour.GadgetPhysicalDistance, layerMask))))
            return;

        if (UIconfig.InputManagerVersion == 1)
            Input.GetButton(this.deactivateSnapKey);

        RaycastHit[] multipleHits = Physics.RaycastAll(ray, MaxRange, layerMask);
        if (multipleHits.Length == 0)
            multipleHits = Physics.RaycastAll(transform.position, Vector3.down, GlobalBehaviour.GadgetPhysicalDistance, layerMask);



        // sort multipleHits, so the first hit is still the closest 
        for (int i = 0; i < multipleHits.Length; i++)
        {
            int minIdx = i;
            float minValue = multipleHits[i].distance;

            for (int j = i; j < multipleHits.Length; j++)
            {
                if (multipleHits[j].distance < minValue)
                {
                    minIdx = j;
                    minValue = multipleHits[j].distance;
                }

            }

            RaycastHit buffer = multipleHits[minIdx];
            multipleHits[minIdx] = multipleHits[i];
            multipleHits[i] = buffer;

        }


        for (int i = 0; i < multipleHits.Length; i++)
        {

            // check whether we actually hit something 
            if (!((multipleHits[i].collider.transform.CompareTag("SnapZone") || multipleHits[i].collider.transform.CompareTag("Selectable"))
                && (!deactSnapKey)))
                continue;
    
            //TODO see whether the conditions needs to be adjusted
            //if (Hit.transform.TryGetComponent<FactObject>(out var obj)
            //        && StageStatic.stage.factState[obj.URI] is AbstractLineFact lineFact)

            if (multipleHits[i].collider.gameObject.layer == LayerMask.NameToLayer("Ray")
                || multipleHits[i].collider.gameObject.layer == LayerMask.NameToLayer("Line"))
            {
                var id = multipleHits[i].collider.gameObject.GetComponent<FactObject>().URI;
                AbstractLineFact lineFact = StageStatic.stage.factState[id] as AbstractLineFact;
                PointFact p1 = StageStatic.stage.factState[lineFact.Pid1] as PointFact;

                multipleHits[i].point = Math3d.ProjectPointOnLine(p1.Point, lineFact.Dir, multipleHits[i].point);
            }
            else if (multipleHits[i].collider.gameObject.layer == LayerMask.NameToLayer("Ring"))
            {
                #region Ring
                var id = multipleHits[i].transform.GetComponent<FactObject>().URI;
                CircleFact circleFact = StageStatic.stage.factState[id] as CircleFact;
                Vector3 middlePoint = ((PointFact)StageStatic.stage.factState[circleFact.Pid1]).Point;
                Vector3 edgePoint = ((PointFact)StageStatic.stage.factState[circleFact.Pid2]).Point;
                var normal = circleFact.normal;
                var radius = circleFact.radius;

                // project p on circlePlane
                var q = multipleHits[i].point - middlePoint;
                var dist = Vector3.Dot(q, normal);
                var pPlane = multipleHits[i].point - (normal * dist); // p on circlePlane

                // check if projectedPoint and circleCenter are identical
                // should never happen in practice due to floating point precision
                if (pPlane == middlePoint)
                {
                    // can be set to any point on the ring -> set to edgePoint
                    multipleHits[i].point = edgePoint;
                    return;

                }
                else
                {
                    var direction = (pPlane - middlePoint).normalized;
                    multipleHits[i].point = middlePoint + direction * radius;
                }


                // cursor orientation should match circle orientation; dont face downwards
                if (normal.y < 0) // if normal faces downwards use inverted normal instead
                    multipleHits[i].normal = -normal;
                else
                    multipleHits[i].normal = normal;
                #endregion Ring
            }
            else if (multipleHits[i].collider.gameObject.layer == LayerMask.NameToLayer("Circle"))
            {
                #region Circle
                var id = multipleHits[i].transform.GetComponent<FactObject>().URI;
                CircleFact circleFact = StageStatic.stage.factState[id] as CircleFact;
                Vector3 middlePoint = ((PointFact)StageStatic.stage.factState[circleFact.Pid1]).Point;
                Vector3 edgePoint = ((PointFact)StageStatic.stage.factState[circleFact.Pid2]).Point;
                var normal = circleFact.normal;
                var radius = circleFact.radius;

                // project p on circlePlane
                var q = multipleHits[i].point - middlePoint;
                var dist = Vector3.Dot(q, normal);
                var pPlane = multipleHits[i].point - (normal * dist); // p on circlePlane
                multipleHits[i].point = pPlane;

                // cursor orientation should match circle orientation; dont face downwards
                if (normal.y < 0) // if normal faces downwards use inverted normal instead
                    multipleHits[i].normal = -normal;
                else
                    multipleHits[i].normal = normal;
                #endregion Circle
            }
            else
            {
                multipleHits[i].point = multipleHits[i].collider.transform.position;
                multipleHits[i].normal = Vector3.up;
            }

            // checking for 2 lines intersection point
            if (!((Mathf.Abs(multipleHits[i].distance - multipleHits[0].distance) < 0.03)
                && (multipleHits.Length > 1)
                && (Mathf.Abs(multipleHits[1].distance - multipleHits[0].distance) < 0.03)))
                continue;
            // we probably have two objects intersecting 

            
            // check for line x line intersection and if they actually intersect adjust the points coordinates :)
            if (multipleHits[i].collider.gameObject.layer == LayerMask.NameToLayer("Ray")
                && multipleHits[0].collider.gameObject.layer == LayerMask.NameToLayer("Ray"))
            {

                // case for two intersecting rays 
                var idLine0 = multipleHits[0].collider.gameObject.GetComponent<FactObject>().URI;
                var id = multipleHits[i].collider.gameObject.GetComponent<FactObject>().URI;

                // get the two corresponding line facts
                AbstractLineFact lineFactLine0 = StageStatic.stage.factState[idLine0] as AbstractLineFact;
                AbstractLineFact lineFact = StageStatic.stage.factState[id] as AbstractLineFact;

                // get a point on the line 
                PointFact p1Line0 = StageStatic.stage.factState[lineFactLine0.Pid1] as PointFact;
                PointFact p1 = StageStatic.stage.factState[lineFact.Pid1] as PointFact;

                // get the intersection point and if it actually intersects set it
                Vector3 intersectionPoint = Vector3.zero;
 
                if (Math3d.LineLineIntersection(out intersectionPoint, p1Line0.Point, lineFactLine0.Dir, p1.Point, lineFact.Dir))
                    multipleHits[i].point = intersectionPoint;


            }
            //check for other types of intersection. Future Work

            

        }

        transform.up = multipleHits[0].normal ;
        //TODO check whether this is needed
        //if (!((multipleHits[0].collider.transform.CompareTag("SnapZone") || multipleHits[0].collider.transform.CompareTag("Selectable"))
        //      && !Input.GetButton(this.deactivateSnapKey)))
        //    transform.position += .01f * multipleHits[0].normal;

        transform.position = multipleHits[0].point + .01f * multipleHits[0].normal;
        this.MultipleHits = multipleHits;


        //Link to CheckMouseButtonHandler
        if (whichCheckMouseButton == 0) { CheckMouseButtons(); }
        if (whichCheckMouseButton == 1) { CheckMouseButtons1(); }

    }


    void updateMaxRange()
    {
        switch (UIconfig.GameplayMode)
        {
            case 2:
                UIconfig.interactingRangeMode = UIconfig.InteractingRangeMode.fromObserverView;
                break;
            case 5:
            case 6:
                UIconfig.interactingRangeMode = UIconfig.InteractingRangeMode.fromCharacterView;
                break;

            default:
                break;
        }


        MaxRange_ = UIconfig.interactingRangeMode switch
        {
            UIconfig.InteractingRangeMode.fromObserverView =>
                UIconfig.cursorMaxRange_fromObeserverView,
            UIconfig.InteractingRangeMode.fromCharacterView or _ =>
                MaxRange,
        };

        Debug.Log("WorldCursorMaxRange :" + MaxRange_);

    }





    //Check if left Mouse-Button was pressed and handle it
    void CheckMouseButtons()
    {
        if (Input.GetMouseButtonDown(0)
         && !EventSystem.current.IsPointerOverGameObject() //this prevents rays from shooting through ui
         && Hit.transform.gameObject.layer != LayerMask.NameToLayer("Water")) // not allowed to meassure on water
            CommunicationEvents.TriggerEvent.Invoke(MultipleHits);
    }

    //Check if left Mouse-Button was pressed and handle it
    //Alternative Version
    void CheckMouseButtons1(bool OnSnap = false, bool onLine = false)
    {
        //TODO edit for the multiple hits. Right now it only checks the first hit

        if (Input.GetMouseButtonDown(0) && checkClickPermission())
        { 

            //other Things todo first?
            if (Hit.collider.transform.CompareTag("NPC1_text") && UIconfig.nextDialogPlease < 2)
            {
                UIconfig.nextDialogPlease++;
            }


            //if (EventSystem.current.IsPointerOverGameObject()) return; //this prevents rays from shooting through ui

            if (IsPointerOverUIObject()) return; //Needed for Android
            if (!checkLayerAndTags()) return; // not allowed to meassure on water
            //if (Hit.transform.gameObject.layer == LayerMask.NameToLayer("TransparentFX")) return; // not allowed to meassure on TransparentFX
            if (!OnSnap)
            {
                CommunicationEvents.TriggerEvent.Invoke(MultipleHits);
            }
            else if (GadgetBehaviour.ActiveGadget is Pointer)
            {
                if (!onLine) Hit.collider.enabled = false;
                CommunicationEvents.TriggerEvent.Invoke(MultipleHits);
                //    CommunicationEvents.SnapEvent.Invoke(Hit);
            }

        }
    }

    private bool checkLayerAndTags()
        => Hit.transform.gameObject.layer != LayerMask.NameToLayer("Water") // not allowed to meassure on water
        && !Hit.collider.transform.CompareTag("NPC1_text");                 // not allowed to meassure on textfields

    private bool IsPointerOverUIObject()
    {
        PointerEventData eventDataCurrentPosition = new(EventSystem.current)
        {
            position = new Vector2(Input.mousePosition.x, Input.mousePosition.y)
        };
        List<RaycastResult> results = new();
        EventSystem.current.RaycastAll(eventDataCurrentPosition, results);
        return results.Count > 0;
    }

    public bool checkClickPermission() => true;
    //{
    //    if (UIconfig.CanvasOnOff_Array[14] > 0)
    //        return true;

    //    //return false; //todo
    //    return true;
    //}
}
