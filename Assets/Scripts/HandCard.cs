using System.Collections.Generic;
using UnityEngine;

public class HandCard : MonoBehaviour
{
    public static HandCard Instance { get; set; }

    public Card CardPrefab;
    private List<Card> cardList = new List<Card>();
    public Card selectCard;
    private CardTable cardTable;

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
    }

    private void Start()
    {
        cardTable = DataTableManager.GetTable<CardTable>();
        AddCard(cardTable.GetCardByName("타격"));
        AddCard(cardTable.GetCardByName("수비"));
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            AddCard(cardTable.GetCardByName("타격"));
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            AddCard(cardTable.GetCardByName("수비"));
        }

        //Delete
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            DeleteCard(cardList.Count - 1);
        }
    }

    private void AddCard(CardData data)
    {
        var card = Instantiate(CardPrefab, transform);
        card.cardData = data;
    }

    private void DeleteCard(int index)
    {
        Destroy(cardList[index]);
    }
}
