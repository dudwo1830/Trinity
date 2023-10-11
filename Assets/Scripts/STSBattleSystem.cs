using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public enum BattleState
{
    NONE,
    START,
    ENEMYTURN,
    PLAYERTURN,
    WIN,
    LOSE,
}

public class STSBattleSystem : MonoBehaviour
{
    public static STSBattleSystem Instance { get; set; }
    public LivingEntity playerEntity;

    public List<Enemy> enemyPrefabs = new List<Enemy>();
    private List<Enemy> currentEnemyList = new List<Enemy>();
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
    }

    private void Start()
    {

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
    }

    public void SetupBattle()
    {

        Debug.Log("SetupBattle");
    }

    public void EnemyTurn()
    {

        state = BattleState.PLAYERTURN;
        PlayerTurn();
    }

    public void OnSkillButton(SkillData skill)
    {
        Battle();
    }

    public void PlayerTurn()
    {
        Debug.Log("Player Turn");
    }

    public void Battle()
    {
        switch (1)
        {
            //case CardData.CardType.None:
            //    Debug.Log("== RESULT: DRAW ==");
            //    break;
            //case 2:
            //    Debug.Log("== RESULT: PLAYER ATTACK ==");
            //    currentEnemy.OnDamage(playerAction.Amount, Vector3.zero, Vector3.zero);
            //    if (currentEnemy.Dead)
            //    {
            //        Win();
            //    }
            //    else
            //    {

            //    }
            //    break;
            //case 3:
            //    Debug.Log("== RESULT: ENEMY ATTACK ==");
            //    playerEntity.OnDamage(20, Vector3.zero, Vector3.zero);
            //    if (playerEntity.Dead)
            //    {
            //        Lose();
            //    }
            //    else
            //    {

            //    }
            //    break;
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
        BattleExit();
    }

    public void Lose()
    {
        state = BattleState.LOSE;
        Debug.Log("Player Lose");
        playerEntity.Revive();
        BattleExit();
    }

    private void BattleExit()
    {
        foreach(var enemy in currentEnemyList)
        {
            Destroy(enemy.gameObject);
        }

        Debug.Log("End Battle");
        state = BattleState.NONE;
    }
}
