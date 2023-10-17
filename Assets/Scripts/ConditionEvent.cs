using System.Collections;
using System.Collections.Generic;
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

    private void OnMouseEnter()
    {
        conditionInfo.SetActive(true);
    }
    private void OnMouseExit()
    {
        conditionInfo.SetActive(false);
    }
}
