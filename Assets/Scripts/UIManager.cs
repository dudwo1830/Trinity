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

    public GameObject cardListContainer;
    public List<GameObject> cardListUIs;

    public Image curtain;

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
                        gameoverUI.SetActive(false);
                        curtain.gameObject.SetActive(false);
                        BattleSystem.Instance.SetupBattle();
                    });
                    break;
                case "AddCardButton":
                    button.onClick.AddListener(() =>
                    {
                        HandCard.Instance.AddRandomCard();
                        gameoverUI.SetActive(false);
                        curtain.gameObject.SetActive(false);
                        BattleSystem.Instance.SetupBattle();
                    });
                    break;
                case "OnHealButton":
                    button.onClick.AddListener(() =>
                    {
                        BattleSystem.Instance.player.OnHealByRate(0.3f);
                        gameoverUI.SetActive(false);
                        curtain.gameObject.SetActive(false);
                        BattleSystem.Instance.SetupBattle();
                    });
                    break;
                case "EnforceCardButton":
                    button.onClick.AddListener(() =>
                    {
                        HandCard.Instance.EnforceRandomCard();
                        gameoverUI.SetActive(false);
                        curtain.gameObject.SetActive(false);
                        BattleSystem.Instance.SetupBattle();
                    });
                    break;
                case "DeleteCardButton":
                    button.onClick.AddListener(() =>
                    {
                        HandCard.Instance.DeleteRandomCard();
                        gameoverUI.SetActive(false);
                        curtain.gameObject.SetActive(false);
                        BattleSystem.Instance.SetupBattle();
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
                        GoToTitle();
                    });
                    break;
                default:
                    break;
            }
        }

        winUI.SetActive(false);
        loseUI.SetActive(false);
        gameoverUI.SetActive(false);

        cardListContainer.SetActive(false);
    }

    public void SetActiveGameoverUI(bool active, BattleState state)
    {
        curtain.gameObject.SetActive(active);
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

    public void SetActiveWaitCardListUI()
    {
        bool active = false;
        cardListUIs.ForEach((cardList) => 
        {
            if (cardList.name == "WaitCardList" && !cardList.activeSelf)
            {
                active = true;
                cardList.SetActive(active);
            }
            else
            {
                cardList.SetActive(false);
            }
        });
        cardListContainer.SetActive(active);
        curtain.gameObject.SetActive(active);
    }

    public void SetActiveAllCardListUI()
    {
        bool active = false;
        cardListUIs.ForEach((cardList) =>
        {
            if (cardList.name == "AllCardList" && !cardList.activeSelf)
            {
                active = true;
                cardList.SetActive(active);
            }
            else
            {
                cardList.SetActive(false);
            }
        });
        cardListContainer.SetActive(active);
        curtain.gameObject.SetActive(active);
    }

    public void SetActiveUsedCardListUI()
    {
        bool active = false;
        cardListUIs.ForEach((cardList) =>
        {
            if (cardList.name == "UsedCardList" && !cardList.activeSelf)
            {
                active = true;
                cardList.SetActive(active);
            }
            else
            {
                cardList.SetActive(false);
            }
        });
        cardListContainer.SetActive(active);
        curtain.gameObject.SetActive(active);
    }

    public void GameRestart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void GoToTitle()
    {
        SceneManager.LoadScene("Title");
    }
}
