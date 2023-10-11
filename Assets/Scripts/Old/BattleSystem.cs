using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
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

public enum BattleResult
{
    DRAW,
    WIN,
    LOSE
}

public class BattleSystem : MonoBehaviour
{
    public static BattleSystem Instance { get; set; }
    public float encountChance = 0f;

    public Transform playerTransform;
    private PlayerMovement playerMovement;
    public GameObject playerUI;
    public LivingEntity playerEntity;

    public List<Enemy> enemyList = new List<Enemy>();
    private Enemy currentEnemy;
    public TextMeshProUGUI enemyActionText;

    public static BattleState state = BattleState.NONE;

    public QuickTimeEvent qte;

    public SkillData enemyAction;
    public SkillData playerAction;

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

        playerMovement = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerMovement>();
        playerUI.SetActive(false);
    }

    private void Start()
    {
        var skillList = DataTableManager.GetTable<SkillTable>().ToList();
        foreach(var skill in skillList)
        {
            SkillButtonManager.Instance.CreateSkillButton(skill);
            SkillButtonManager.Instance.CreateUpgradeButton(skill);
        }

        Debug.Log("Start\nPress Alpha1 is Immediately Enemy Encount");
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1) && state == BattleState.NONE)
        {
            SetupBattle();
        }
        if (Input.GetKeyDown(KeyCode.Alpha2) && state != BattleState.NONE && state != BattleState.QTE)
        {
            Win();
        }
        if (Input.GetKeyDown(KeyCode.Alpha3) && state != BattleState.NONE && state != BattleState.QTE)
        {
            Lose();
        }
        if (Input.GetKeyDown(KeyCode.Alpha4) && state == BattleState.NONE)
        {
            SkillButtonManager.Instance.ResetAllSkill();
        }

        if(encountChance >= 100f)
        {
            playerMovement.enabled = false;
            SetupBattle();
        }
    }

    public void SetupBattle()
    {
        encountChance = 0f;

        if (currentEnemy != null)
        {
            Destroy(currentEnemy.gameObject);
        }

        currentEnemy = Instantiate(enemyList[0], playerTransform.position + new Vector3(0,0,5f), Quaternion.identity);

        currentEnemy.Setup(100, 0, 0, 0);

        currentEnemy.gameObject.SetActive(true);
        playerUI.SetActive(true);

        Debug.Log("SetupBattle");
        state = BattleState.QTE;
        QTE();
    }

    public void QTE()
    {
        Debug.Log("------Start QuickTimeEvent");
        currentEnemy.SetActionText(string.Empty);
        StartCoroutine(CoQTE());
    }
    private IEnumerator CoQTE()
    {
        yield return StartCoroutine(qte.StartSlider());
        Debug.Log("------END QuickTimeEvent");
        state = BattleState.ENEMYTURN;
        EnemyTurn();
    }

    public void EnemyTurn()
    {
        Debug.Log($"QTE Result: {qte.IsSuccess}");

        enemyAction = DataTableManager.GetTable<SkillTable>().GetRandomSkill();

        if (qte.IsSuccess)
        {
            string actionText = enemyAction.Attribute.ToString();
            currentEnemy.SetActionText(actionText);
        }

        state = BattleState.PLAYERTURN;
        PlayerTurn();
    }

    public void OnSkillButton(SkillData skill)
    {
        playerAction = skill;
        Battle();
    }

    //public void SkillButton1()
    //{
    //    if (state != BattleState.PLAYERTURN)
    //    {
    //        return;
    //    }
    //    playerAction = DataTableManager.GetTable<SkillTable>().GetSkill(1);
    //    Battle();
    //}
    //public void SkillButton2()
    //{
    //    if (state != BattleState.PLAYERTURN)
    //    {
    //        return;
    //    }
    //    playerAction = DataTableManager.GetTable<SkillTable>().GetSkill(2);
    //    Battle();
    //}
    //public void SkillButton3()
    //{
    //    if (state != BattleState.PLAYERTURN)
    //    {
    //        return;
    //    }
    //    playerAction = DataTableManager.GetTable<SkillTable>().GetSkill(3);
    //    Battle();
    //}

    public void PlayerTurn()
    {
        Debug.Log("Player Turn");
    }

    public void Battle()
    {
        switch (AttributeCheck(enemyAction.Attribute))
        {
            case BattleResult.DRAW:
                Debug.Log("== RESULT: DRAW ==");
                state = BattleState.QTE;
                QTE();
                break;
            case BattleResult.WIN:
                Debug.Log("== RESULT: PLAYER ATTACK ==");
                currentEnemy.OnDamage(playerAction.Amount, Vector3.zero, Vector3.zero);
                if (currentEnemy.Dead)
                {
                    Win();
                }
                else
                {
                    state = BattleState.QTE;
                    QTE();
                }
                break;
            case BattleResult.LOSE:
                Debug.Log("== RESULT: ENEMY ATTACK ==");
                playerEntity.OnDamage(20, Vector3.zero, Vector3.zero);
                if (playerEntity.Dead)
                {
                    Lose();
                }
                else
                {
                    state = BattleState.QTE;
                    QTE();
                }
                break;
        }
    }

    public void Win()
    {
        state = BattleState.WIN;
        SkillButtonManager.Instance.SelectUpgrade();
    }
    
    public void PlayerUpgrade()
    {
        if (state != BattleState.WIN)
        {
            return;
        }
        BattleOut();
    }

    public void Lose()
    {
        state = BattleState.LOSE;
        Debug.Log("Player Lose");
        playerEntity.Revive();
        BattleOut();
    }

    private void BattleOut()
    {
        Destroy(currentEnemy.gameObject);
        playerMovement.enabled = true;
        playerUI.SetActive(false);

        Debug.Log("End Battle");
        state = BattleState.NONE;
    }

    private BattleResult AttributeCheck(SkillData.SkillAttribute targetAttribute)
    {
        switch (playerAction.Attribute)
        {
            case SkillData.SkillAttribute.Rock:
                switch (targetAttribute)
                {
                    case SkillData.SkillAttribute.Rock:
                        break;
                    case SkillData.SkillAttribute.Scissors:
                        return BattleResult.WIN;
                    case SkillData.SkillAttribute.Paper:
                        return BattleResult.LOSE;
                }
                break;
            case SkillData.SkillAttribute.Scissors:
                switch (targetAttribute)
                {
                    case SkillData.SkillAttribute.Rock:
                        return BattleResult.LOSE;
                    case SkillData.SkillAttribute.Scissors:
                        break;
                    case SkillData.SkillAttribute.Paper:
                        return BattleResult.WIN;
                }
                break;
            case SkillData.SkillAttribute.Paper:
                switch (targetAttribute)
                {
                    case SkillData.SkillAttribute.Rock:
                        return BattleResult.WIN;
                    case SkillData.SkillAttribute.Scissors:
                        return BattleResult.LOSE;
                    case SkillData.SkillAttribute.Paper:
                        break;
                }
                break;
        }

        return BattleResult.DRAW;
    }
}
