using System;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class HandCard : MonoBehaviour
{
    public static HandCard Instance { get; set; }

    public Card cardPrefab;
    public CardUI CardUIPrefab;

    public RectTransform allCardUITransform;
    public RectTransform waitCardUITransform;
    public RectTransform usedCardUITransform;

    private List<CardUI> allCardListUI = new List<CardUI>();
    public TextMeshProUGUI allCardCountText;

    private List<Card> waitCardList = new List<Card>();
    private List<CardUI> waitCardListUI = new List<CardUI>();
    public TextMeshProUGUI waitCardCountText;

    private List<Card> usedCardList = new List<Card>();
    private List<CardUI> usedCardListUI = new List<CardUI>();
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

        allCardCountText.GetComponentInParent<Button>().onClick.AddListener(() => 
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

        waitCardCountText.GetComponentInParent<Button>().onClick.AddListener(() => 
        {
            if (!UIManager.Instance.waitCardListUI.activeSelf)
            {
                UIManager.Instance.SetActiveWaitCardListUI(true);
            }
            else
            {
                UIManager.Instance.SetActiveWaitCardListUI(false);
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
        for (int i = 0; i < 5; i++)
        {
            AddCard(cardTable.GetDataByName("타격"));
        }
        for (int i = 0; i < 4; i++)
        {
            AddCard(cardTable.GetDataByName("수비"));
        }
        for (int i = 0; i < 1; i++)
        {
            AddCard(cardTable.GetDataByName("강타"));
        }
        BattleSystem.Instance.SetupBattle();
    }

    public void Ready()
    {
        for (int i = 0; i < drawCount; i++)
        {
            DrawCard();
        }
    }

    public void Shuffle()
    {
        int random1, random2;
        for (int i = 0; i < waitCardList.Count; ++i)
        {
            random1 = Random.Range(0, waitCardList.Count);
            random2 = Random.Range(0, waitCardList.Count);

            var temp = waitCardList[random1];
            waitCardList[random1] = waitCardList[random2];
            waitCardList[random2] = temp;
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            AddCard(cardTable.GetDataByName("타격"));
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            AddCard(cardTable.GetDataByName("수비"));
        }
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            AddCard(cardTable.GetDataByName("강타"));
        }

        if (Input.GetKeyDown(KeyCode.Alpha5))
        {
            selectedCard.LevelUp();
        }
    }

    private void AddCard(CardData data)
    {
        var newData = new CardData(data);
        var card = Instantiate(cardPrefab, handCardTransform);
        card.SetCardData(newData);
        card.gameObject.SetActive(false);
        waitCardList.Add(card);
        AddCardUI(newData);

        UpdateCardCount();
    }

    public void DrawCard()
    {
        if (waitCardList.Count == 0)
        {
            ResetCard();
        }

        //var randomIndex = UnityEngine.Random.Range(0, cardList.Count);
        var card = waitCardList[0];
        card.gameObject.SetActive(true);
        handCardList.Add(card);
        waitCardList.Remove(card);
        DrawCardUI(card);

        UpdateCardCount();
    }

    private void ResetCard()
    {
        foreach(var card in usedCardList)
        {
            waitCardList.Add(card);
        }
        usedCardList.Clear();
        ResetUI();
        Shuffle();
        UpdateCardCount();
    }

    public void ResetAllCard()
    {
        foreach (var card in handCardList)
        {
            card.gameObject.SetActive(false);
            usedCardList.Add(card);
        }
        handCardList.Clear();

        foreach (var card in usedCardList)
        {
            waitCardList.Add(card);
        }
        usedCardList.Clear();

        ResetUI();
        Shuffle();

        UpdateCardCount();
    }

    public void UseCard(LivingEntity target)
    {
        var player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
        if (!player.CanUseCard(target))
        {
            return;
        }
        if (!player.ActiveCard(target))
        {
            return;
        }
        selectedCard.gameObject.SetActive(false);
        usedCardList.Add(selectedCard);
        handCardList.Remove(selectedCard);
        UseCardUI();
        selectedCard = null;
        UpdateCardCount();
    }
    private void DeleteCard(int index)
    {
        Destroy(waitCardList[index].gameObject);
        waitCardList.RemoveAt(index);
        UpdateCardCount();
    }

    private void UpdateCardCount()
    {
        allCardCountText.text = $"모든 카드 / {allCardListUI.Count}";
        waitCardCountText.text = $"남은 카드 / {waitCardList.Count}";
        usedCardCountText.text = $"사용한 카드 / {usedCardList.Count}";
    }

    public void SortByID(ref List<CardUI> list)
    {
        list = (from card in list
                orderby card.cardData.Id
                orderby card.cardData.level
                select card
                ).ToList();
    }

    public void AddCardUI(CardData data)
    {
        //var cardUI = Instantiate(CardUIPrefab, allCardUITransform);
        //cardUI.cardData = data;
        //cardUI.SetCardData(cardUI.cardData);
        //cardUI.gameObject.SetActive(true);
        //allCardListUI.Add(cardUI);
        //cardUI = Instantiate(CardUIPrefab, waitCardUITransform);
        //cardUI.cardData = data;
        //cardUI.SetCardData(cardUI.cardData);
        //cardUI.gameObject.SetActive(true);
        //waitCardListUI.Add(cardUI);
        //cardUI = Instantiate(CardUIPrefab, usedCardUITransform);
        //cardUI.cardData = data;
        //cardUI.SetCardData(cardUI.cardData);
        //cardUI.gameObject.SetActive(true);
        //usedCardListUI.Add(cardUI);
        Transform[] targetTransforms = { allCardUITransform, waitCardUITransform, usedCardUITransform };

        foreach (Transform targetTransform in targetTransforms)
        {
            var cardUI = Instantiate(CardUIPrefab, targetTransform);
            cardUI.cardData = data;
            cardUI.SetCardData(cardUI.cardData);

            if (targetTransform == allCardUITransform)
            {
                cardUI.gameObject.SetActive(true);
                allCardListUI.Add(cardUI);
            }
            else if (targetTransform == waitCardUITransform)
            {
                cardUI.gameObject.SetActive(true);
                waitCardListUI.Add(cardUI);
            }
            else if (targetTransform == usedCardUITransform)
            {
                cardUI.gameObject.SetActive(false);
                usedCardListUI.Add(cardUI);
            }
        }
    }

    public void DrawCardUI(Card card)
    {
        foreach (var item in waitCardListUI)
        {
            if (ReferenceEquals(item.cardData, card.cardData))
            {
                item.gameObject.SetActive(false);
                break;
            }
        }
    }

    public void UseCardUI()
    {
        foreach (var item in usedCardListUI)
        {
            if (ReferenceEquals(item.cardData, selectedCard.cardData))
            {
                item.gameObject.SetActive(true);
                break;
            }
        }
    }

    public void ResetUI()
    {
        foreach(var item in waitCardListUI)
        {
            item.gameObject.SetActive(true);
        }
        foreach (var item in usedCardListUI)
        {
            item.gameObject.SetActive(false);
        }
    }

    public void TurnEnd()
    {
        foreach (var card in handCardList)
        {
            card.gameObject.SetActive(false);
            usedCardList.Add(card);
            TurnEndUI(card);
        }
        handCardList.Clear();
    }
    public void TurnEndUI(Card card)
    {
        foreach (var item in usedCardListUI)
        {
            if (ReferenceEquals(item.cardData, card.cardData))
            {
                item.gameObject.SetActive(true);
            }
        }
    }
}
