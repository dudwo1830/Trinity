using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CurtainScript : MonoBehaviour
{
    private RectTransform rectTransform;

    private void Start()
    {
        rectTransform = GetComponent<RectTransform>();

        float x = Screen.width;
        float y = Screen.height;
        rectTransform.sizeDelta = new Vector2(x, y);
    }
}
