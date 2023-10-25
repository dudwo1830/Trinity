using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ButtonEvent : MonoBehaviour
{
    public Button startButton;
    public Button endButton;

    private void Start()
    {
        startButton.onClick.AddListener(() =>
        {
            SceneManager.LoadScene("Tutorial");
        });

        endButton.onClick.AddListener(() =>
        {
            Application.Quit();
        });
    }
}