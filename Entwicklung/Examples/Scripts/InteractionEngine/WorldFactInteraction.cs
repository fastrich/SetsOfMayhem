using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

/// <summary>
/// This class handles displaying Fact tooltips, when hovering over a fact in the Gameworld
/// </summary>
public class WorldFactInteraction : MonoBehaviour
{
    [SerializeField] private LayerMask factLayerMask;
    [SerializeField] private Transform HidingCanvas;

    private GameObject currentDisplay;
    private Transform lastHit = null;
    private bool canRun = false;
    private void Update()
    {
        // disable this script if HidingCanvas does not render
        canRun = HidingCanvas.GetComponent<Canvas>().enabled;
    }
    void LateUpdate()
    {
        if (!canRun)
            return;

        if (currentDisplay != null && currentDisplay.GetComponent<DragHandling>().dragged)
        {
            // currently dragging -> remove transparency to indicate dragging and let DragHandling.cs take over
            currentDisplay.GetComponent<CanvasGroup>().alpha = 1;
            return;
        }

        UpdateDisplay();
    }

    private void UpdateDisplay()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        // if no fact was hit or pointer was over other UI
        if (!Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity, factLayerMask) || WasOtherUIHit())
        {
            // destroy currentDisplay if it exists
            lastHit = null;
            Destroy(currentDisplay);
            return;
        }

        FactObject factObj = hit.transform.gameObject.GetComponentInChildren<FactObject>();

        if (factObj == null)
        {
            // should never happen, if the layerMask is set up correctly
            Debug.LogError("WorldFactInteraction Raycast collided with object in factLayerMask, that did not contain a FactObject script: " + hit.transform.gameObject.name);
            lastHit = null;
            Destroy(currentDisplay);
            return;
        }

        if (hit.transform != lastHit) // a fact has been hit for the first time -> delete old display and instantiate new one
        {
            InstantiateNewDisplay(factObj);
        }

        currentDisplay.transform.position = Input.mousePosition; // move currentDisplay to mousePosition
        currentDisplay.GetComponent<CanvasGroup>().alpha = 0.5f; // ensure that image alpha is correct, since it could have changed due to dragging

        lastHit = hit.transform;
    }

    private void InstantiateNewDisplay(FactObject factObj)
    {
        if (currentDisplay)
            Destroy(currentDisplay);
        Fact fact = StageStatic.stage.factState[factObj.URI];
        // TODO: this link to DisplayFacts is not ideal: maybe refactor to SciptableObject or such
        currentDisplay = fact.instantiateDisplay(DisplayFacts.prefabDictionary[fact.GetType()], HidingCanvas);
    }

    #region Helper
    /// <summary>
    /// Returns true if any UI other than currentDisplay was hit
    /// </summary>
    /// <returns></returns>
    private bool WasOtherUIHit()
    {
        PointerEventData pointerData = new(EventSystem.current)
        {
            position = Input.mousePosition
        };

        List<RaycastResult> results = new();
        EventSystem.current.RaycastAll(pointerData, results);

        foreach (var res in results)
            if (currentDisplay == null || !res.gameObject.transform.IsChildOf(currentDisplay.transform))
                return true;

        return false;
    }
    #endregion Helper
}

