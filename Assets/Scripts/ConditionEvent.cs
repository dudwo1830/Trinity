using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

public class ConditionEvent : MonoBehaviour, IPointerClickHandler
{
    public GameObject conditionInfo;
    public int conditionId;

    public GameObject conditionGuide;
    public TextMeshProUGUI text;
    private int currentConditionId;

    private void Awake()
    {
        conditionGuide = GameObject.FindGameObjectWithTag("ConditionGuide");
        conditionGuide.SetActive(false);
    }

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

    private void TextUpdate(int conditionId)
    {
        if (currentConditionId == conditionId)
        {
            return;
        }

        currentConditionId = conditionId;
        var conditionData = DataTableManager.GetTable<ConditionTable>().GetDataById(currentConditionId);
        conditionGuide.transform.Find("Title").GetComponent<TextMeshProUGUI>().text = conditionData.Name;
        conditionGuide.transform.Find("Description").GetComponent<TextMeshProUGUI>().text = conditionData.Description;
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

    public void OnPointerClick(PointerEventData eventData)
    {
        Debug.Log("CLICKED");
        TextUpdate(conditionId);
        conditionGuide.SetActive(!conditionGuide.activeSelf);
    }
}
