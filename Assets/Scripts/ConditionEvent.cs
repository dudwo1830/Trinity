using TMPro;
using UnityEngine;

public class ConditionEvent : MonoBehaviour
{
    public GameObject conditionInfo;
    public int conditionId;

    private void Start()
    {
        if (conditionId <= 0)
        {
            return;
        }
        var data = DataTableManager.GetTable<ConditionTable>().GetDataById(conditionId);
        conditionInfo.GetComponentInChildren<TextMeshProUGUI>().text = data.Description;
    }

    private void Update()
    {
    }
    private void OnMouseEnter()
    {
        conditionInfo.SetActive(true);
        Debug.Log("OnMouseEnter");
    }
    private void OnMouseExit()
    {
        conditionInfo.SetActive(false);
        Debug.Log("OnMouseExit");
    }
}
