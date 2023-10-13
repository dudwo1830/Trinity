using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HandCard : MonoBehaviour
{
    public static HandCard Instance { get; set; }

    public Transform allCardTransform;
    public Transform usedCardTransform;

    public Card cardPrefab;

    public List<Card> allCardList = new List<Card>();

    private List<Card> cardList = new List<Card>();
    public TextMeshProUGUI cardCountText;

    private List<Card> usedCardList = new List<Card>();
    public TextMeshProUGUI usedCardCountText;

    private List<Card> handCardList = new List<Card>();
    public Transform handCardTransform;

    public int drawCount = 5;
    private CardTable cardTable;

    public Card selectedCard;

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

        cardCountText.GetComponentInParent<Button>().onClick.AddListener(() => 
        {
            if (!UIManager.Instance.allCardListUI.activeSelf)
            {
                UIManager.Instance.SetActiveAllCardListUI(true);
            }
            else
            {
                UIManager.Instance.SetActiveAllCardListUI(false);
            }
        });

        usedCardCountText.GetComponentInParent<Button>().onClick.AddListener(() =>
        {
            if (!UIManager.Instance.usedCardListUI.activeSelf)
            {
                UIManager.Instance.SetActiveUsedCardListUI(true);
            }
            else
            {
                UIManager.Instance.SetActiveUsedCardListUI(false);
            }
        });
    }

    private void Start()
    {
        for (int i = 0; i < 6; i++)
        {
            AddCard(cardTable.GetDataByName("타격"));
        }
        for (int i = 0; i < 4; i++)
        {
            AddCard(cardTable.GetDataByName("수비"));
        }

    }

    public void Ready()
    {
        //for (int i = handCardList.Count - 1; i >= 0; i--)
        //{
        //    handCardList[i].gameObject.SetActive(false);
        //    cardList.Add(handCardList[i]);
        //    handCardList.RemoveAt(i);
        //}

        Debug.Log("Player Draw");
        foreach (var card in handCardList)
        {
            card.gameObject.SetActive(false);
            usedCardList.Add(card);
        }
        handCardList.Clear();
        for (int i = 0; i < drawCount; i++)
        {
            DrawCard();
        }
    }

    public void Shuffle()
    {
        int random1, random2;
        for (int i = 0; i < cardList.Count; ++i)
        {
            random1 = Random.Range(0, cardList.Count);
            random2 = Random.Range(0, cardList.Count);

            var temp = cardList[random1];
            cardList[random1] = cardList[random2];
            cardList[random2] = temp;
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
            AddCard(cardTable.GetDataByName("타격"));
        }
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            AddCard(cardTable.GetDataByName("수비"));
        }

        //Delete
        if (Input.GetKeyDown(KeyCode.Alpha5))
        {
            DeleteCard(cardList.Count - 1);
        }
        //Shuffle
        if (Input.GetKeyDown(KeyCode.F2))
        {
            Shuffle();
        }
    }

    private void AddCard(CardData data)
    {
        var card = Instantiate(cardPrefab, handCardTransform);
        card.SetCardData(data);
        card.gameObject.SetActive(false);
        cardList.Add(card);
        allCardList.Add(card);

        UpdateCardCount();
    }

    public void DrawCard()
    {
        if (cardList.Count == 0)
        {
            ResetCard();
        }

        //var randomIndex = UnityEngine.Random.Range(0, cardList.Count);
        var card = cardList[cardList.Count - 1];
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
        Shuffle();
        UpdateCardCount();
    }

    public void ResetAllCard()
    {
        foreach (var card in usedCardList)
        {
            cardList.Add(card);
        }
        usedCardList.Clear();

        foreach (var card in handCardList)
        {
            card.gameObject.SetActive(false);
            usedCardList.Add(card);
        }
        handCardList.Clear();

        Shuffle();
    }

    public void UseCard(LivingEntity target)
    {
        var player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
        if (!player.CanUseCard(target))
        {
            return;
        }
        //var card = handCardList[selectedCardIndex];
        //selectedCard.CardAction(target);
        if (!player.ActiveCard(target))
        {
            return;
        }
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
