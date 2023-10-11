using Newtonsoft.Json.Bson;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Card : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public CardData cardData;
    private Button button;
    public bool IsSelected { get; private set; }
    public float hoverMovementY = 20f;
    private Vector3 defaultPosition;

    private void Awake()
    {
        Debug.Log("Awake");
        button = GetComponent<Button>();
    }

    private void Start()
    {
        button.onClick.AddListener(() =>
        {
            if (IsSelected)
            {
                IsSelected = false;
            }
            else
            {
                button.Select();
            }
        });

        foreach (var item in button.GetComponentsInChildren<TextMeshProUGUI>())
        {
            switch (item.gameObject.name)
            {
                case "Name":
                    item.text = $"{cardData.Name}";
                    break;
                case "Description":
                    item.text = cardData.GetDescription();
                    break;
                case "Summary":
                    item.text = $"{cardData.Type} / {cardData.Amount}";
                    break;
            }
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (IsSelected)
        {
            return;
        }
        defaultPosition = transform.position;
        gameObject.transform.position += new Vector3(0, hoverMovementY, 0);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (IsSelected)
        {
            return;
        }
        gameObject.transform.position = defaultPosition;
    }

    public void OnSelect()
    {
        HandCard.Instance.selectCard = null;
        IsSelected = true;
    }

    public void OnDeselect()
    {
        HandCard.Instance.selectCard = null;
        gameObject.transform.position = defaultPosition;
    }
}
