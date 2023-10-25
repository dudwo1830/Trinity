using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class VirtualJoyStick : MonoBehaviour, IDragHandler, IPointerDownHandler, IPointerUpHandler
{
    public enum Axis
    {
        Horizontal,
        Vertical
    }

    public Image stick;
    public float radius;
    private Vector3 originalPoint;
    private RectTransform rectTransform;

    private Vector2 value;

    private int pointerId;
    private bool isDragging;

    private void Start()
    {
        rectTransform = GetComponent<RectTransform>();
        originalPoint = stick.rectTransform.position;
        radius = rectTransform.sizeDelta.x / 2;
    }

    public float GetAxis(Axis axis)
    {
        switch(axis)
        {
            case Axis.Horizontal:
                return value.x;
            case Axis.Vertical:
                return value.y;
        }
        return 0f;
    }

    private void Update()
    {
    }

    public void UpdateStickPosition(Vector3 position)
    {
        RectTransformUtility.ScreenPointToWorldPointInRectangle(rectTransform, position, null, out Vector3 newPoint);
        var delta = Vector3.ClampMagnitude(newPoint - originalPoint, radius);
        stick.rectTransform.position = originalPoint + delta;
        value = delta / radius;
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (pointerId != eventData.pointerId)
        {
            return;
        }

        UpdateStickPosition(eventData.position);
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (isDragging)
        {
            return;
        }

        isDragging = true;
        pointerId = eventData.pointerId;
        UpdateStickPosition(eventData.position);
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if (pointerId != eventData.pointerId)
        {
            return;
        }

        isDragging = false;
        stick.rectTransform.position = originalPoint;
        value = Vector2.zero;
    }
}
