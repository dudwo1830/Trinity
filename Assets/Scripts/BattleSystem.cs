using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum BattleState
{
    NONE,
    START,
    PLAYERTURN,
    ENEMYTURN,
    WIN,
    LOSE,
    QTE
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
    public float encountChance = 0f;

    public Transform playerTransform;
    public GameObject playerUI;
    public LivingEntity playerEntity;

    public List<Enemy> enemyList = new List<Enemy>();
    private Enemy currentEnemy;

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
        //Test
        tempSkills.Add(new TempSkill() { attribute = TempSkillAttribute.Rock, damage = 10, name = "SKILL_A" });
        tempSkills.Add(new TempSkill() { attribute = TempSkillAttribute.Scissors, damage = 20, name = "SKILL_B" });
        tempSkills.Add(new TempSkill() { attribute = TempSkillAttribute.Paper, damage = 30, name = "SKILL_C" });
    }

    private void Start()
    {
        Debug.Log("Start");
        playerUI.SetActive(false);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            state = BattleState.START;
            SetupBattle();
        }
    }

    public void SetupBattle()
    {
        encountChance = 0f;

        currentEnemy = Instantiate(enemyList[0]);
        currentEnemy.transform.position = playerTransform.position + new Vector3 (0f, 0f, 5f);
        currentEnemy.Setup(100, 0, 0, 0);

        currentEnemy.gameObject.SetActive(true);
        playerUI.SetActive(true);

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
        Debug.Log("--------------Select Phase--------------");
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
                Debug.Log("== RESULT: DRAW ==");
                EnemyTurn();
                break;
            case BattleResult.WIN:
                Debug.Log("== RESULT: PLAYER ATTACK ==");
                currentEnemy.OnDamage(playerAction.damage, Vector3.zero, Vector3.zero);
                if (currentEnemy.Dead)
                {
                    state = BattleState.WIN;
                    Win();
                }
                else
                {
                    state = BattleState.ENEMYTURN;
                    EnemyTurn();
                }
                break;
            case BattleResult.LOSE:
                Debug.Log("== RESULT: ENEMY ATTACK ==");
                playerEntity.OnDamage(enemyAction.damage, Vector3.zero, Vector3.zero);
                if (playerEntity.Dead)
                {
                    state = BattleState.LOSE;
                    Lose();
                }
                else
                {
                    state = BattleState.ENEMYTURN;
                    EnemyTurn();
                }
                break;
        }
    }

    public void Win()
    {
        Destroy(currentEnemy.gameObject);
        Debug.Log("Player Win");
    }

    public void Lose()
    {
        Debug.Log("Player Lose");
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
