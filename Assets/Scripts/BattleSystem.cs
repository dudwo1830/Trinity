using GooglePlayGames.BasicApi;
using GooglePlayGames.BasicApi.Events;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

public enum BattleState
{
    NONE,
    START,
    CARD_DRAW,
    ENEMY_READY,
    PLAYER_TURN,
    ENEMY_TURN,
    WIN,
    LOSE,
}

public class BattleSystem : MonoBehaviour
{
    public static BattleSystem Instance { get; set; }
    public Player player;
    public Button turnEndButton;

    public Transform enemySpawnTarget;
    public List<Enemy> enemyPrefabs = new List<Enemy>();
    public List<Enemy> battleEnemyList = new List<Enemy>();

    public int minEnemyCount = 1;
    public int maxEnemyCount = 3;
    public float enemyDistance = 2f;
    public float playerPosition = -3f;
    public float enemyPosition = 1.5f;

    public int battleCount = 0;
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

    private void Update()
    {
        if (UIManager.Instance.isPause)
        {
            return;
        }

        if (state == BattleState.PLAYER_TURN)
        {
            if (Input.GetMouseButtonDown(0) && HandCard.Instance.selectedCard != null)
            {
                FindEnemy(Camera.main.ScreenToWorldPoint(Input.mousePosition));
            }
        }
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
        ++battleCount;
        var enemyCount = Random.Range(minEnemyCount, maxEnemyCount + 1);
        for (int i = 0; i < enemyCount; i++)
        {
            var prefab = enemyPrefabs[Random.Range(0, enemyPrefabs.Count)];
            var position = new Vector3(enemyPosition + i * enemyDistance, prefab.transform.position.y, 0);
            var enemy = Instantiate(prefab, position, Quaternion.identity, enemySpawnTarget);
            battleEnemyList.Add(enemy);
        }

        CardDraw();
    }

    public void CardDraw()
    {
        state = BattleState.CARD_DRAW;
        HandCard.Instance.Ready();
        player.UpdateConditions();
        EnemyReady();
    }

    public void EnemyReady()
    {
        state = BattleState.ENEMY_READY;
        foreach (var enemy in battleEnemyList)
        {
            enemy.SetAction();
            enemy.UpdateConditions();
        }
        PlayerTurn();
    }

    public void PlayerTurn()
    {
        state = BattleState.PLAYER_TURN;
        player.ResetCoast();
        player.ResetShield();
        turnEndButton.gameObject.SetActive(true);
    }
    public void TurnEnd()
    {
        if (state != BattleState.PLAYER_TURN)
        {
            return;
        }
        HandCard.Instance.TurnEnd();
        state = BattleState.ENEMY_TURN;
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
            Lose();
        }
        else
        {
            CardDraw();
        }
    }


    public void Win()
    {
        state = BattleState.WIN;
        UIManager.Instance.SetActiveGameoverUI(true, state);
        BattleExit();
    }

    public void Lose()
    {
        state = BattleState.LOSE;
        UIManager.Instance.SetActiveGameoverUI(true, state);
    }

    private void BattleExit()
    {
        foreach(var enemy in battleEnemyList)
        {
            Destroy(enemy.gameObject);
        }

        HandCard.Instance.TurnEnd();
        HandCard.Instance.ResetCard();

        state = BattleState.NONE;
    }
}
