using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance { get; private set; }
    public GameObject gameoverUI;

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
    }

    private void Start()
    {
        gameoverUI.SetActive(false);
    }

    public void SetActiveGameoverUI(bool active, BattleState state)
    {
        switch (state)
        {
            case BattleState.WIN:
                SetWinUI();
                break;
            case BattleState.LOSE:
                SetLoseUI();
                break;
            default: 
                return;
        }
        gameoverUI.SetActive(active);
    }
    private void SetWinUI()
    {
        var titleGO = GameObject.FindGameObjectWithTag("GameoverTitle").GetComponent<TextMeshProUGUI>();
        titleGO.text = "Win";
        var buttonGO = GameObject.FindGameObjectWithTag("GameoverButton");
        buttonGO.GetComponent<TextMeshProUGUI>().text = "Next Battle";
    }
    private void SetLoseUI()
    {
        var titleGO = GameObject.FindGameObjectWithTag("GameoverTitle").GetComponent<TextMeshProUGUI>();
        titleGO.text = "Lose";
        var buttonGO = GameObject.FindGameObjectWithTag("GameoverButton");
        buttonGO.GetComponent<TextMeshProUGUI>().text = "Restart";
    }

    public void GameRestart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
