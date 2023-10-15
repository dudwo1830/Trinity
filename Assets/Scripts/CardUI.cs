using System;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CardUI : MonoBehaviour
{
    public CardData cardData;

    public TextMeshProUGUI nameText;
    public TextMeshProUGUI descriptionText;
    public TextMeshProUGUI coastText;
    private Button button;
    public event Action onClickEvent;

    private void Awake()
    {
        button = GetComponent<Button>();
        button.onClick.AddListener(() => {
            OnClick();
        });
    }

    public void SetCardData(CardData data)
    {
        cardData = data;
        UpdateCardUI();
    }

    public void UpdateCardUI()
    {
        nameText.text = cardData.Name;
        descriptionText.text = cardData.GetDescription();
        coastText.text = cardData.Coast.ToString();
    }

    public void OnClick()
    {
        if (onClickEvent != null)
        {
            onClickEvent();
        }
    }
}
