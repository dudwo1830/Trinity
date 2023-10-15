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
}
