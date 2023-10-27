using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

public class Curtain : MonoBehaviour
{
    private RectTransform rectTransform;
    public int originalIndex { get; private set; }

    private void Awake()
    {
        originalIndex = transform.GetSiblingIndex();
    }

    private void Start()
    {
        rectTransform = GetComponent<RectTransform>();

        float x = Screen.width;
        float y = Screen.height;
        rectTransform.sizeDelta = new Vector2(x, y);
    }

    public void ResetSibling()
    {
        transform.SetSiblingIndex(originalIndex);
    }
}
