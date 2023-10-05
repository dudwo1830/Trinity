using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEditor.Build.Content;
using UnityEngine;

public enum BattleState
{
    NONE,
    START,
    PLAYERTURN,
    ENEMYTURN,
    WIN,
    LOSE
}

public enum BattleAttribute
{
    Rock,
    Scissors,
    Paper
}

public enum BattleResult
{
    DRAW,
    WIN,
    LOSE
}

public class TempSkill
{
    public BattleAttribute attribute;
    public string name;
    public int damage;
}

public class BattleSystem : MonoBehaviour
{
    public static BattleSystem Instance { get; set; }

    public GameObject enemyPrefab;

    public LivingEntity playerEntity;
    private LivingEntity enemyEntity;

    public BattleHUD battleHUD;
    public BattleState state = BattleState.NONE;

    //Test
    List<TempSkill> tempSkills = new List<TempSkill>();
    TempSkill enemyAction;
    TempSkill playerAction;

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

        //Test
        Debug.Log("Awake");
        tempSkills.Add(new TempSkill() { attribute = BattleAttribute.Rock, damage = 10, name = "SKILL_A" });
        tempSkills.Add(new TempSkill() { attribute = BattleAttribute.Scissors, damage = 20, name = "SKILL_B" });
        tempSkills.Add(new TempSkill() { attribute = BattleAttribute.Paper, damage = 30, name = "SKILL_C" });
    }

    private void Start()
    {
        Debug.Log("Start");
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            state = BattleState.START;
            SetupBattle();
        }
    }

    private void SetupBattle()
    {
        var enemyGO = Instantiate(enemyPrefab);
        enemyEntity = enemyGO.GetComponent<LivingEntity>();
        enemyEntity.startingHealth = 100;

        battleHUD.SetEnemy(enemyEntity);
        battleHUD.SetSkill(tempSkills);
        battleHUD.gameObject.SetActive(true);

        Debug.Log("SetupBattle");
        state = BattleState.ENEMYTURN;
        EnemyTurn();
    }

    public void OnSkillButton()
    {
        if (state != BattleState.PLAYERTURN)
        {
            return;
        }

        playerAction = tempSkills[0];
        Debug.Log($"PlayerTurn: Player is {playerAction.name}");    
        Battle();
    }

    public void EnemyTurn()
    {
        enemyAction = tempSkills[Random.Range(0, tempSkills.Count)];
        Debug.Log($"EnemyTurn: Enemy is {enemyAction.name}");
        state = BattleState.PLAYERTURN;
        PlayerTurn();
    }

    public void PlayerTurn()
    {
        Debug.Log("Player Turn");
    }

    public void Battle()
    {
        //Todo: Battle Terms
        switch (AttributeCheck(enemyAction.attribute))
        {
            case BattleResult.DRAW:
                break;
            case BattleResult.WIN:
                enemyEntity.OnDamage(playerAction.damage, Vector3.zero, Vector3.zero);
                if (enemyEntity.Dead)
                {
                    state = BattleState.WIN;
                }
                else
                {
                    state = BattleState.ENEMYTURN;
                }
                break;
            case BattleResult.LOSE:
                playerEntity.OnDamage(enemyAction.damage, Vector3.zero, Vector3.zero);
                if (playerEntity.Dead)
                {
                    state = BattleState.LOSE;
                    Lose();
                }
                break;
        }
    }

    public void Lose()
    {

    }

    private BattleResult AttributeCheck(BattleAttribute targetAttribute)
    {
        switch (playerAction.attribute)
        {
            case BattleAttribute.Rock:
                switch (targetAttribute)
                {
                    case BattleAttribute.Rock:
                        break;
                    case BattleAttribute.Scissors:
                        return BattleResult.WIN;
                    case BattleAttribute.Paper:
                        return BattleResult.LOSE;
                }
                break;
            case BattleAttribute.Scissors:
                switch (targetAttribute)
                {
                    case BattleAttribute.Rock:
                        return BattleResult.LOSE;
                    case BattleAttribute.Scissors:
                        break;
                    case BattleAttribute.Paper:
                        return BattleResult.WIN;
                }
                break;
            case BattleAttribute.Paper:
                switch (targetAttribute)
                {
                    case BattleAttribute.Rock:
                        return BattleResult.WIN;
                    case BattleAttribute.Scissors:
                        return BattleResult.LOSE;
                    case BattleAttribute.Paper:
                        break;
                }
                break;
        }

        return BattleResult.DRAW;
    }
}
