using Newtonsoft.Json.Bson;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class QuickTimeEvent : MonoBehaviour
{
    private Coroutine sliderCoroutine;

    public Slider slider;
    public float minSliderSpeed = 1.5f;
    public float maxSliderSpeed = 3f;
    public float sliderSpeed = 0f;
    private bool isFlip = false;

    public Image curtain;
    //public float duration = 3f;
    //private float timer = 0f;

    private RectTransform sliderRect;
    private RectTransform successRangeRect;
    public float successRangeMin = 0.4f;
    public float successRangeMax = 0.6f;
    
    public bool IsSuccess {  get; private set; }
    private bool IsRunning
    {
        get
        {
            return slider.gameObject.activeSelf;
        }
    }
    public bool waitForResult = false;

    private void Awake()
    {
        sliderRect = slider.GetComponent<RectTransform>();
        foreach (Image image in GetComponentsInChildren<Image>())
        {
            if (image.gameObject.name == "SuccessRange")
            {
                successRangeRect = image.rectTransform;
                break;
            }
        }
    }

    private void Start()
    {
        slider.minValue = 0f;
        slider.maxValue = 1f;
        slider.gameObject.SetActive(false);

        ResetSuccessRange();
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0) && IsRunning)
        {
            IsSuccess = slider.value >= successRangeMin && slider.value <= successRangeMax;
            StopSlider();

            waitForResult = true;
        }
    }

    public IEnumerator StartSlider()
    {
        curtain.gameObject.SetActive(true);
        slider.gameObject.SetActive(true);

        ResetSlider();
        sliderCoroutine = StartCoroutine(MoveSlider());

        yield return new WaitUntil(() => waitForResult);

        yield return IsSuccess;
        waitForResult = false;
    }

    private void StopSlider()
    {
        if (sliderCoroutine != null)
        {
            StopCoroutine(sliderCoroutine);
        }
        slider.gameObject.SetActive(false);
        curtain.gameObject.SetActive(false);
    }

    private IEnumerator MoveSlider()
    {
        while (true)
        {
            if (!isFlip)
            {
                slider.value += Time.deltaTime * sliderSpeed;
            }
            else
            {
                slider.value -= Time.deltaTime * sliderSpeed;
            }
            
            if (slider.value >= slider.maxValue || slider.value <= slider.minValue)
            {
                isFlip = !isFlip;
            }

            if (!slider.gameObject.activeSelf)
            {
                yield break;
            }
            yield return null;
        }
    }

    private void ResetSlider()
    {
        IsSuccess = false;
        isFlip = false;
        slider.value = slider.minValue;
        sliderSpeed = Random.Range(minSliderSpeed, maxSliderSpeed);
    }

    private void ResetSuccessRange()
    {
        successRangeRect.offsetMin = new Vector2(sliderRect.sizeDelta.x * successRangeMin, successRangeRect.offsetMin.y);
        successRangeRect.offsetMax = new Vector2(-(sliderRect.sizeDelta.x * (1 - successRangeMax)), successRangeRect.offsetMin.y);
    }
}
