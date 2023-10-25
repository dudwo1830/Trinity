using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class VirtualJoyStick2 : MonoBehaviour, IDragHandler, IPointerDownHandler, IPointerUpHandler
{
    public Vector2 Value { get; private set; }

    private int pointerId;
    private bool isDragging;


    public void OnDrag(PointerEventData eventData)
    {
        Value = eventData.delta / Screen.dpi;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        throw new System.NotImplementedException();
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        Value = Vector2.zero;
    }
}
