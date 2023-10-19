using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameButtonCommon : MonoBehaviour
{
    public List<TextMeshProUGUI> buttonText;
    private float hoverMovementY = 12f;
    private Vector2 originalTextPosition;

    private void Awake()
    {
        originalTextPosition = transform.position;
    }

    public void OnButtonDown()
    {
        buttonText.ForEach(text =>
        {
            //originalTextPosition = text.transform.position;
            text.transform.position -= new Vector3(0, hoverMovementY, 0);
        });
    }

    public void OnButtonUp()
    {
        buttonText.ForEach(text =>
        {
            text.transform.position += new Vector3(0, hoverMovementY, 0);
        });
    }
}
