using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class QuickTimeEvent : MonoBehaviour
{
    public float sliderSpeed = 1f;
    public Slider slider;
    private bool isFlip = false;

    public KeyCode actionKey = KeyCode.Space;
    private bool isSuccess = false;
    bool isStart = false;

    private void Awake()
    {
        slider.minValue = 0f;
        slider.maxValue = 1f;
        slider.gameObject.SetActive(false);
    }

    private void Start()
    {
    }

    public bool GetQuickTimeEvent()
    {
        if (!isFlip)
        {
            slider.value += Time.deltaTime * sliderSpeed;
            if (slider.value >= slider.maxValue)
            {
                isFlip = true;
            }
        }
        else
        {
            slider.value += Time.deltaTime * -sliderSpeed;
            if (slider.value <= slider.minValue)
            {
                isFlip = false;
            }
        }
        if (Input.GetKeyDown(actionKey))
        {
            isSuccess = slider.value > 0.4f && slider.value < 0.6f;
            isStart = false;
        }

        return isSuccess;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            slider.gameObject.SetActive(true);
            slider.value = 0f;
            isStart = true;
        }

        if (!isStart)
        {
            return;
        }


    }
}
