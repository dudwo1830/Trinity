using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Card : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, ISelectHandler, IDeselectHandler
{
    public CardData cardData;
    private Button button;
    public bool IsSelected { get; private set; }
    public float hoverMovementY = 20f;
    private Vector3 prevPosition;

    private void Awake()
    {
        button = GetComponent<Button>();
    }

    private void Start()
    {
        //button.onClick.AddListener(() =>
        //{
        //    if (IsSelected)
        //    {
        //        IsSelected = false;
        //    }
        //    else
        //    {
        //        button.Select();
        //    }
        //});
    

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

    //public void CardAction(LivingEntity target)
    //{
    //    switch (cardData.Type)
    //    {
    //        case CardData.CardType.None:
    //            break;
    //        case CardData.CardType.Attack:
    //            if (target.GetComponentInParent<Player>() != null)
    //            {
    //                return;
    //            }
    //            target.OnDamage(cardData.Amount, Vector3.zero, Vector3.zero);
    //            break;
    //        case CardData.CardType.Defense:
    //            if (target.GetComponentInParent<Player>() == null)
    //            {
    //                return;
    //            }
    //            target.AddShield(cardData.Amount);
    //            break;
    //        case CardData.CardType.Heal:
    //            if (target.GetComponentInParent<Player>() == null)
    //            {
    //                return;
    //            }
    //            target.OnHeal(cardData.Amount);
    //            break;
    //        default:
    //            break;
    //    }
    //}

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
}
