using Newtonsoft.Json.Bson;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum BattleState
{
    START,
    PLAYERTURN,
    ENEMYTURN,
    WIN,
    LOSE
}

public class BattleSystem : MonoBehaviour
{
    public GameObject player;
    public GameObject enemyPrefab;

    private LivingEntity playerEntity;
    private LivingEntity enemyEntity;

    public GameObject battleHUD;

    public BattleState state;

    private void Start()
    {
        state = BattleState.START;
        SetupBattle();
        Debug.Log("Start");
    }

    private void SetupBattle()
    {
        playerEntity = player.GetComponent<LivingEntity>();

        var enemyGO = Instantiate(enemyPrefab);
        enemyEntity = enemyGO.GetComponent<LivingEntity>();

        Debug.Log("SetupBattle");

        state = BattleState.PLAYERTURN;
    }
}
