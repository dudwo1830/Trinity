using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum BattleState
{
    NONE,
    START,
    CARD_DRAW,
    ENEMYREADY,
    PLAYERTURN,
    ENEMYTURN,
    END,
    WIN,
    LOSE,
}

public class BattleSystem : MonoBehaviour
{
    public static BattleSystem Instance { get; set; }
    public Player player;
    public Button turnEndButton;

    public RectTransform enemySpawnTarget;
    public List<Enemy> enemyPrefabs = new List<Enemy>();
    public List<Enemy> battleEnemyList = new List<Enemy>();
    public int minEnemyCount = 1;
    public int maxEnemyCount = 3;

    public static BattleState state = BattleState.NONE;

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

        state = BattleState.NONE;
        battleEnemyList.Clear();

        turnEndButton.onClick.RemoveAllListeners();
        turnEndButton.onClick.AddListener(() => TurnEnd());
        turnEndButton.gameObject.SetActive(false);
    }

    private void Start()
    {
    }

    private void Update()
    {
        if (state == BattleState.PLAYERTURN)
        {
            if (Input.GetMouseButtonDown(0) && HandCard.Instance.selectedCard != null)
            {
                FindEnemy(Camera.main.ScreenToWorldPoint(Input.mousePosition));
            }
        }

        //if (Input.GetKeyDown(KeyCode.F1) && state == BattleState.NONE)
        //{
        //    SetupBattle();
        //}
    }

    private void FindEnemy(Vector2 clickPosition)
    {
        RaycastHit2D hit = Physics2D.Raycast(clickPosition, Vector2.zero);
        if (hit.collider != null)
        {
            var gameObject = hit.collider.gameObject;
            if (gameObject.GetComponent<LivingEntity>() != null)
            {
                if (HandCard.Instance.selectedCard.cardData.IsAllAble)
                {
                    foreach (var enemy in battleEnemyList)
                    {
                        HandCard.Instance.UseCard(enemy);
                    }
                }
                else
                {
                    HandCard.Instance.UseCard(gameObject.GetComponent<LivingEntity>());
                }
            }
        }
    }

    public void SetupBattle()
    {
        Debug.Log("SetupBattle");
        HandCard.Instance.ResetAllCard();
        var enemyCount = Random.Range(minEnemyCount, maxEnemyCount + 1);
        for (int i = 0; i < enemyCount; i++)
        {
            var enemy = Instantiate(enemyPrefabs[Random.Range(0, enemyPrefabs.Count)], enemySpawnTarget);
            battleEnemyList.Add(enemy);
        }

        state = BattleState.CARD_DRAW;
        Draw();
    }

    public void Draw()
    {
        HandCard.Instance.Ready();
        player.UpdateConditions();
        state = BattleState.ENEMYREADY;
        EnemyReady();
    }

    public void EnemyReady()
    {
        var cardTable = DataTableManager.GetTable<CardTable>();
        foreach (var enemy in battleEnemyList)
        {
            //enemy.SetAction(cardTable.GetRandomData());
            enemy.SetAction();
            enemy.UpdateConditions();
        }
        state = BattleState.PLAYERTURN;
        PlayerTurn();
    }

    public void PlayerTurn()
    {
        player.ResetCoast();
        player.ResetShield();
        turnEndButton.gameObject.SetActive(true);
    }
    public void TurnEnd()
    {
        if (state != BattleState.PLAYERTURN)
        {
            return;
        }
        HandCard.Instance.TurnEnd();
        state = BattleState.ENEMYTURN;
        EnemyTurn();
    }

    public void EnemyTurn()
    {
        battleEnemyList.ForEach(enemy => enemy.ResetShield());
        foreach (var enemy in battleEnemyList)
        {
            enemy.EnemyAction(player);
        }

        if (player.Dead)
        {
            state = BattleState.LOSE;
            Lose();
        }
        else
        {
            state = BattleState.END;
            End();
        }
    }

    public void End()
    {
        state = BattleState.CARD_DRAW;
        Draw();
    }

    public void Win()
    {
        state = BattleState.WIN;
        HandCard.Instance.ResetAllCard();
        UIManager.Instance.SetActiveGameoverUI(true, state);
    }

    public void PlayerUpgrade()
    {
        if (state != BattleState.WIN)
        {
            return;
        }
        BattleExit();
    }

    public void Lose()
    {
        state = BattleState.LOSE;
        Debug.Log("Player Lose");
        UIManager.Instance.SetActiveGameoverUI(true, state);
    }

    private void BattleExit()
    {
        foreach(var enemy in battleEnemyList)
        {
            Destroy(enemy.gameObject);
        }

        Debug.Log("End Battle");
        state = BattleState.NONE;
    }
}
