using System.Collections.Generic;
using System.Runtime.CompilerServices;
using TMPro;
using UnityEngine;

public class HandCard : MonoBehaviour
{
    public static HandCard Instance {  get; set; }

    public Card CardPrefab;
    public Transform cardTransform;

    private List<Card> cardList = new List<Card>();
    private int maxCardCount = 0;
    public TextMeshProUGUI cardCountText;

    private List<Card> usedCardList = new List<Card>();
    public TextMeshProUGUI usedCardCountText;

    private List<Card> handCardList = new List<Card>();

    public int startDrawCount = 5;
    public int drawCount = 2;
    private CardTable cardTable;

    public Card selectedCard;
    private int selectedCardIndex;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Debug.LogError("Instance is already");
            Destroy(gameObject);
        }

        cardTable = DataTableManager.GetTable<CardTable>();
    }

    private void Start()
    {
        for (int i = 0; i < 5; i++)
        {
            AddCard(cardTable.GetCardByName("타격"));
            AddCard(cardTable.GetCardByName("수비"));
        }

        for (int i = 0; i < startDrawCount; i++)
        {
            DrawCard();
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            DrawCard();
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            AddCard(cardTable.GetCardByName("타격"));
        }
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            AddCard(cardTable.GetCardByName("수비"));
        }

        //Delete
        if (Input.GetKeyDown(KeyCode.Alpha5))
        {
            DeleteCard(cardList.Count - 1);
        }
    }

    private void AddCard(CardData data)
    {
        var card = Instantiate(CardPrefab, cardTransform);
        card.cardData = data;
        card.gameObject.SetActive(false);
        cardList.Add(card);
        maxCardCount = cardList.Count;
        UpdateCardCount();
    }

    public void DrawCard()
    {
        if(handCardList.Count >= maxCardCount)
        {
            return;
        }

        if (cardList.Count == 0)
        {
            ResetCard();
        }
        var randomIndex = Random.Range(0, cardList.Count);
        var card = cardList[randomIndex];
        card.gameObject.SetActive(true);
        handCardList.Add(card);
        cardList.Remove(card);
        UpdateCardCount();
    }

    private void ResetCard()
    {
        foreach(var card in usedCardList)
        {
            cardList.Add(card);
        }
        usedCardList.Clear();
        UpdateCardCount();
    }

    public void UseCard(LivingEntity target)
    {
        if (selectedCard == null)
        {
            return;
        }
        //var card = handCardList[selectedCardIndex];
        selectedCard.CardAction(target);

        selectedCard.gameObject.SetActive(false);
        usedCardList.Add(selectedCard);
        handCardList.Remove(selectedCard);
        selectedCard = null;
        UpdateCardCount();
    }

    private void DeleteCard(int index)
    {
        Destroy(cardList[index].gameObject);
        cardList.RemoveAt(index);
        UpdateCardCount();
    }

    private void UpdateCardCount()
    {
        cardCountText.text = cardList.Count.ToString();
        usedCardCountText.text = usedCardList.Count.ToString();
    }
}
