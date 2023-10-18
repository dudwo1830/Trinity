using System.Collections.Generic;
using System.Net;
using System.Net.Http.Headers;
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
        var winButtons = winUI.GetComponentsInChildren<Button>();
        foreach (var button in winButtons) 
        {
            switch (button.gameObject.name)
            {
                case "NextBattleButton":
                    button.onClick.AddListener(() =>
                    {
                        BattleSystem.Instance.SetupBattle();
                        gameoverUI.SetActive(false);
                    });
                    break;
                case "AddCardButton":
                    button.onClick.AddListener(() =>
                    {
                        HandCard.Instance.AddRandomCard();
                        BattleSystem.Instance.SetupBattle();
                        gameoverUI.SetActive(false);
                    });
                    break;
                case "OnHealButton":
                    button.onClick.AddListener(() =>
                    {
                        BattleSystem.Instance.player.OnHealByRate(0.3f);
                        BattleSystem.Instance.SetupBattle();
                        gameoverUI.SetActive(false);
                    });
                    break;
                case "EnforceCardButton":
                    button.onClick.AddListener(() =>
                    {
                        HandCard.Instance.EnforceRandomCard();
                        BattleSystem.Instance.SetupBattle();
                        gameoverUI.SetActive(false);
                    });
                    break;
                case "DeleteCardButton":
                    button.onClick.AddListener(() =>
                    {
                        HandCard.Instance.DeleteRandomCard();
                        BattleSystem.Instance.SetupBattle();
                        gameoverUI.SetActive(false);
                    });
                    break;
                default:
                    break;
            }
        }
        var loseButtons = loseUI.GetComponentsInChildren<Button>();
        foreach (var button in loseButtons)
        {
            switch (button.gameObject.name)
            {
                case "RestartButton":
                    button.onClick.AddListener(() =>
                    {
                        GameRestart();
                        gameoverUI.SetActive(false);
                    });
                    break;
                case "QuitButton":
                    button.onClick.AddListener(() =>
                    {
                        Application.Quit();
                    });
                    break;
                default:
                    break;
            }
        }

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
