using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

[RequireComponent(typeof(FactWrapper), typeof(RectTransform), typeof(DragHandling))]
public class OpenFactExplorer : MonoBehaviour, IPointerClickHandler
{
    #region Variables
    public GameObject factExplorerPrefab;

    private static Transform factExplorer;
    private float pressTime = 0f;
    private const float LONG_PRESS_DURATION = 0.5f;
    #endregion Variables

    #region UnityMethods
    public void OnPointerClick(PointerEventData eventData)
    {
        // open FactExplorer on right click on PC
        if (eventData.button == PointerEventData.InputButton.Right)
        {
            DoOpenFactExplorer();
        }
    }

    private void Update()
    {
        // open FactExplorer on press on fact longer than LONG_PRESS_DURATION
        HandleTouches();
    }
    #endregion UnityMethods

    #region Implementation
    private void HandleTouches()
    {
        if (Input.touchCount != 1 || transform.GetComponent<DragHandling>().dragged)
        {
            pressTime = 0;
            return;
        }

        var touch = Input.GetTouch(0);
        if (!RectTransformUtility.RectangleContainsScreenPoint(transform.GetComponent<RectTransform>(), touch.position))
        {
            pressTime = 0;
            return;
        }

        switch (touch.phase)
        {
            case TouchPhase.Moved:
            case TouchPhase.Began:
            case TouchPhase.Ended:
            case TouchPhase.Canceled:
                pressTime = 0;
                break;

            case TouchPhase.Stationary:
                pressTime += Time.deltaTime;
                if (pressTime >= LONG_PRESS_DURATION)
                    DoOpenFactExplorer();
                break;
        }
    }

    private void DoOpenFactExplorer()
    {
        Destroy(factExplorer != null ? factExplorer.gameObject : null);

        var parent = transform.GetComponentInParent<Canvas>().transform;
        var fact = transform.GetComponent<FactWrapper>().fact;

        factExplorer = Instantiate(factExplorerPrefab.transform, Input.mousePosition, Quaternion.identity, parent);
        factExplorer.GetComponent<FactExplorer>().Initialize(fact, transform.position);
    }
    #endregion Implementation
}

