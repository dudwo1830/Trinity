using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

public class Curtain : MonoBehaviour, IPointerClickHandler
{
    private RectTransform rectTransform;
    private int originalIndex;

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

    public void OnPointerClick(PointerEventData eventData)
    {
        Debug.Log("Curtain Clicked");
        var nextSiblingIndex = transform.GetSiblingIndex() + 1;
        var target = transform.parent.GetChild(nextSiblingIndex);
        if (target != null)
        {
            return;
        }
        target.gameObject.SetActive(false);
        gameObject.SetActive(false);
    }
}
