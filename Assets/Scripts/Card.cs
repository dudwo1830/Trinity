using System;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Card : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, ISelectHandler, IDeselectHandler
{
    public enum Status
    {
        None, Wait, Draw, Used, All
    }
    public CardData cardData;
    private Button button;
    public bool IsSelected { get; private set; }
    public float hoverMovementY = 20f;
    private Vector3 prevPosition;
    
    public Status cardStatus = Status.Wait;

    private void Awake()
    {
        button = GetComponent<Button>();
    }

    private void Start()
    {
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (IsSelected)
        {
            return;
        }
        prevPosition = transform.position;
        gameObject.transform.position += new Vector3(0, hoverMovementY, 0);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (IsSelected)
        {
            return;
        }
        gameObject.transform.position = prevPosition;
    }

    public void OnSelect(BaseEventData eventData)
    {
        HandCard.Instance.selectedCard = this;
        IsSelected = true;
    }

    public void OnDeselect(BaseEventData eventData)
    {
        gameObject.transform.position = prevPosition;
        IsSelected = false;
    }

    public void SetCardData(CardData data)
    {
        cardData = data;
        UpdateData();
    }

    public void UpdateData()
    {
        foreach (var item in GetComponentsInChildren<TextMeshProUGUI>())
        {
            switch (item.gameObject.name)
            {
                case "Name":
                    item.text = $"{cardData.Name}";
                    break;
                case "Description":
                    item.text = cardData.GetDescription();
                    break;
                case "Coast":
                    item.text = cardData.Coast.ToString();
                    break;
                case "Summary":
                    item.text = $"{cardData.Type} / {cardData.Amount}";
                    break;
            }
        }
    }

    public bool LevelUp()
    {
        var result = cardData.LevelUp();
        if (result)
        {
            UpdateData();
        }
        return result;
    }
}
