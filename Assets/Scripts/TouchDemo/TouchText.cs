using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.AI;

public class TouchText : MonoBehaviour
{
    public TextMeshProUGUI text;

    private void Update()
    {
        var message = string.Empty;

        foreach (Touch touch in Input.touches)
        {
            
            
        }

        message += "\n";
        text.text = message;
    }
}
