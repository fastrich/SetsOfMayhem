using UnityEngine;
using UnityEngine.EventSystems;

public class DragHandling : MonoBehaviour, IDragHandler, IEndDragHandler
{
    private Vector3 StartingPosition;
    private Transform StartingParent;
    [HideInInspector] public bool dragged = false;
    public void OnDrag(PointerEventData eventData)
    {
        if (!dragged)
        {
            StartingPosition = transform.localPosition;
            StartingParent = transform.parent;
            GetComponent<CanvasGroup>().blocksRaycasts = false;
            dragged = true;
        }
        // display dragged object in front of all other ui
        transform.SetParent(GetComponentInParent<Canvas>().transform, false);
        transform.SetAsLastSibling();

        transform.position = Input.mousePosition;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        transform.SetParent(StartingParent, false);
        transform.localPosition = StartingPosition;
        GetComponent<CanvasGroup>().blocksRaycasts = true;
        dragged = false;

    }
}
