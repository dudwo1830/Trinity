using System.Runtime.InteropServices;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance { get; private set; }
    public GameObject gameoverUI;

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
    //private void SetWinUI()
    //{
    //    var titleGO = GameObject.FindGameObjectWithTag("GameoverTitle").GetComponent<TextMeshProUGUI>();
    //    titleGO.text = "Win";
    //    var buttonGO = GameObject.FindGameObjectWithTag("GameoverButton");
    //    buttonGO.GetComponentInChildren<TextMeshProUGUI>().text = "Next Battle";
    //}
    //private void SetLoseUI()
    //{
    //    var titleGO = GameObject.FindGameObjectWithTag("GameoverTitle").GetComponentInChildren<TextMeshProUGUI>();
    //    titleGO.text = "Lose";
    //    var buttonGO = GameObject.FindGameObjectWithTag("GameoverButton");
    //    buttonGO.GetComponentInChildren<TextMeshProUGUI>().text = "Restart";
    //    buttonGO.GetComponent<Button>().onClick.RemoveAllListeners();
    //    buttonGO.GetComponent<Button>().onClick.AddListener(() =>
    //    {
    //        GameRestart();
    //        gameoverUI.SetActive(false);
    //    });
    //}

    public void GameRestart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
