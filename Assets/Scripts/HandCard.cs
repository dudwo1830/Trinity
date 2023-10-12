using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class HandCard : MonoBehaviour
{
    public static HandCard Instance {  get; set; }

    public Card CardPrefab;
    public Transform cardTransform;

    private List<Card> cardList = new List<Card>();
    private int maxCardCount = 0;
    private List<Card> usedCardList = new List<Card>();
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
            Debug.Log("Delete CardList At LastIndex");
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
        Debug.Log(randomIndex);
        var card = cardList[randomIndex];
        card.gameObject.SetActive(true);
        handCardList.Add(card);
        cardList.Remove(card);
    }

    private void ResetCard()
    {
        foreach(var card in usedCardList)
        {
            cardList.Add(card);
        }
        usedCardList.Clear();
    }

    public void UseCard(LivingEntity target)
    {
        //var card = handCardList[selectedCardIndex];
        selectedCard.CardAction(target);

        selectedCard.gameObject.SetActive(false);
        usedCardList.Add(selectedCard);
        handCardList.Remove(selectedCard);
    }

    private void DeleteCard(int index)
    {
        Debug.Log($"DeleteIndex: {index}");
        Destroy(cardList[index].gameObject);
        cardList.RemoveAt(index);
    }
}
