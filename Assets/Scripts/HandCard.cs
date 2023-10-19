using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Xsl;
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
            UIManager.Instance.SetActiveAllCardListUI();
        });

        waitCardCountText.GetComponentInParent<Button>().onClick.AddListener(() => 
        {
             UIManager.Instance.SetActiveWaitCardListUI();
        });

        usedCardCountText.GetComponentInParent<Button>().onClick.AddListener(() =>
        {
             UIManager.Instance.SetActiveUsedCardListUI();
        });
    }

    private void Start()
    {
        for (int i = 0; i < 5; i++)
        {
            //AddCard(cardTable.GetDataById(7));
            AddCard(cardTable.GetDataById(1));
        }
        for (int i = 0; i < 4; i++)
        {
            AddCard(cardTable.GetDataById(2));
        }
        for (int i = 0; i < 1; i++)
        {
            AddCard(cardTable.GetDataById(3));
        }
        Shuffle();
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
        //if (Input.GetKeyDown(KeyCode.Alpha1))
        //{
        //    AddCard(cardTable.GetDataById(1));
        //}
        //if (Input.GetKeyDown(KeyCode.Alpha2))
        //{
        //    AddCard(cardTable.GetDataById(2));
        //}
        //if (Input.GetKeyDown(KeyCode.Alpha3))
        //{
        //    AddCard(cardTable.GetDataById(7));
        //}
        //if (Input.GetKeyDown(KeyCode.Alpha4))
        //{
        //    GameObject.FindWithTag("Player").GetComponent<LivingEntity>().AddCondition(1,1);
        //}
        //if (Input.GetKeyDown(KeyCode.Alpha5))
        //{
        //    GameObject.FindWithTag("Player").GetComponent<LivingEntity>().AddCondition(2,1);
        //}
        //if (Input.GetKeyDown(KeyCode.Alpha6))
        //{
        //    BattleSystem.Instance.player.OnHealByRate(1);
        //}
        //if (Input.GetKeyDown(KeyCode.Alpha7))
        //{
        //    TurnEnd();
        //    ResetCard();
        //}
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

    public void ResetCard()
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

    public void UseCard(LivingEntity target)
    {
        var player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
        if (!player.CanUseCard())
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
        if (BattleSystem.Instance.battleEnemyList.Count <= 0)
        {
            BattleSystem.Instance.Win();
        }
    }
    private void DeleteCard(int index)
    {
        Destroy(waitCardList[index].gameObject);
        waitCardList.RemoveAt(index);
        UpdateCardCount();
    }

    public void DeleteCardUI(CardData data)
    {
        List<CardUI>[] uiArray = { allCardListUI, waitCardListUI, usedCardListUI };

        for (int i = 0; i < uiArray.Length; i++)
        {
            var list = uiArray[i];
            for (int j = list.Count - 1; 0 <= j; j--)
            {
                if (ReferenceEquals(list[j].cardData, data))
                {
                    Destroy(list[j].gameObject);
                    list.RemoveAt(j);
                }
            }
        }
    }

    private void UpdateCardCount()
    {
        allCardCountText.text = $"{allCardListUI.Count}";
        waitCardCountText.text = $"{waitCardList.Count}";
        usedCardCountText.text = $"{usedCardList.Count}";
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

    public void DeleteRandomCard()
    {
        var random = Random.Range(0, waitCardList.Count);
        var card = waitCardList[random];

        DeleteCardUI(card.cardData);
        Destroy(card.gameObject);
        waitCardList.Remove(card);

        UpdateCardCount();
    }

    public void AddRandomCard()
    {
        int minDrawCount = 1, maxDrawCount = 2;
        int random;
        var table = DataTableManager.GetTable<CardTable>();
        var maxCardCount = table.ToList().Count;
        var addCount = Random.Range(minDrawCount, maxDrawCount + 1);

        for (int i = 0; i < addCount; i++)
        {
            random = Random.Range(1, maxCardCount+1);
            AddCard(table.GetDataById(random));
        }
    }

    public void EnforceRandomCard(int count = 0)
    {
        if (waitCardList.Count == 0)
        {
            return;
        }
        int limitCount = waitCardList.Count;
        int random = Random.Range(0, limitCount);
        if (!waitCardList[random].LevelUp() && count < limitCount)
        {
            EnforceRandomCard(++count);
        }

        List<CardUI>[] uiArr = { allCardListUI, waitCardListUI, usedCardListUI };
        foreach (var list in uiArr)
        {
            foreach (var cardUI in list)
            {
                cardUI.UpdateCardUI();
            }
        }
    }
}
