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

public enum TempSkillAttribute
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
    public TempSkillAttribute attribute;
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

        Debug.Log("Awake");
        tempSkills.Add(new TempSkill() { attribute = TempSkillAttribute.Rock, damage = 10, name = "SKILL_A" });
        tempSkills.Add(new TempSkill() { attribute = TempSkillAttribute.Scissors, damage = 20, name = "SKILL_B" });
        tempSkills.Add(new TempSkill() { attribute = TempSkillAttribute.Paper, damage = 30, name = "SKILL_C" });
    }

    private void Start()
    {
        Debug.Log("Start");
        Debug.Log("Press Alpha1");
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
        enemyGO.transform.position = new Vector3 (0f, 0.5f, 5f);
        enemyEntity = enemyGO.GetComponent<LivingEntity>();
        enemyEntity.startingHealth = 100;

        battleHUD.SetEnemyHp(enemyEntity);
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
                    Win();
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

    public void Win()
    {
        enemyEntity = null;
    }

    public void Lose()
    {

    }

    private BattleResult AttributeCheck(TempSkillAttribute targetAttribute)
    {
        switch (playerAction.attribute)
        {
            case TempSkillAttribute.Rock:
                switch (targetAttribute)
                {
                    case TempSkillAttribute.Rock:
                        break;
                    case TempSkillAttribute.Scissors:
                        return BattleResult.WIN;
                    case TempSkillAttribute.Paper:
                        return BattleResult.LOSE;
                }
                break;
            case TempSkillAttribute.Scissors:
                switch (targetAttribute)
                {
                    case TempSkillAttribute.Rock:
                        return BattleResult.LOSE;
                    case TempSkillAttribute.Scissors:
                        break;
                    case TempSkillAttribute.Paper:
                        return BattleResult.WIN;
                }
                break;
            case TempSkillAttribute.Paper:
                switch (targetAttribute)
                {
                    case TempSkillAttribute.Rock:
                        return BattleResult.WIN;
                    case TempSkillAttribute.Scissors:
                        return BattleResult.LOSE;
                    case TempSkillAttribute.Paper:
                        break;
                }
                break;
        }

        return BattleResult.DRAW;
    }
}
