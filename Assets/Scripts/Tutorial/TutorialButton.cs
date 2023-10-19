using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class TutorialButton : MonoBehaviour
{
    public List<Sprite> images;
    private int index = 0;
    public Button nextButton;
    public Button prevButton;
    public Button skipButton;
    public Transform target;

    private void Start()
    {
        nextButton.onClick.AddListener(() =>
        {
            if (index >= images.Count - 1)
            {
                return;
            }
            target.gameObject.GetComponent<Image>().sprite = images[++index];
        });
        prevButton.onClick.AddListener(() =>
        {
            if (index <= 0)
            {
                return;
            }
            target.gameObject.GetComponent<Image>().sprite = images[--index];
        });
        skipButton.onClick.AddListener(() =>
        {
            SceneManager.LoadScene("Battle");
        });
    }
}
