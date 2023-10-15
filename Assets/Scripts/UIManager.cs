using System.Collections.Generic;
using System.Net;
using System.Runtime.InteropServices;
using TMPro;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance { get; private set; }
    public GameObject gameoverUI;
    public GameObject usedCardListUI;
    public GameObject waitCardListUI;
    public GameObject allCardListUI;

    private GameObject winUI;
    private GameObject loseUI;
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(Instance);
        }

        winUI = GameObject.FindGameObjectWithTag("GameoverWin");
        loseUI = GameObject.FindGameObjectWithTag("GameoverLose");
    }

    private void Start()
    {
        winUI.GetComponentInChildren<Button>().onClick.AddListener(() =>
        {
            BattleSystem.Instance.SetupBattle();
            gameoverUI.SetActive(false);
        });
        loseUI.GetComponentInChildren<Button>().onClick.AddListener(() =>
        {
            GameRestart();
            gameoverUI.SetActive(false);
        });

        winUI.SetActive(false);
        loseUI.SetActive(false);
        gameoverUI.SetActive(false);
        allCardListUI.SetActive(false);
        usedCardListUI.SetActive(false);
        waitCardListUI.SetActive(false);
    }

    public void SetActiveGameoverUI(bool active, BattleState state)
    {
        gameoverUI.SetActive(active);
        winUI.SetActive(false);
        loseUI.SetActive(false);
        switch (state)
        {
            case BattleState.WIN:
                winUI.SetActive(true);
                break;
            case BattleState.LOSE:
                loseUI.SetActive(true);
                break;
            default: 
                return;
        }
    }

    public void SetActiveWaitCardListUI(bool active)
    {
        waitCardListUI.gameObject.SetActive(active);
    }
    
    public void SetActiveAllCardListUI(bool active)
    {
        allCardListUI.gameObject.SetActive(active);
    }

    public void SetActiveUsedCardListUI(bool active)
    {
        usedCardListUI.SetActive(active);
    }    

    public void GameRestart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
