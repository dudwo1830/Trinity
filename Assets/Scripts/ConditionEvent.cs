using UnityEngine;
using UnityEngine.EventSystems;

public class ConditionEvent : MonoBehaviour, IPointerClickHandler
{
    public GameObject conditionInfo;
    public int conditionId;

    public void OnPointerClick(PointerEventData eventData)
    {
        bool active = !UIManager.Instance.conditionGuide.activeSelf;
        UIManager.Instance.SetConditionGuide(conditionId);
        UIManager.Instance.SetActiveConditionGuide(active);
    }
}
