using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class MultiTouchManager : MonoBehaviour
{
    public bool IsTouching { get; private set; }
    public float minZoomInch = 0.2f;
    public float maxZoomInch = 0.5f;
    public float minZoomPixel;
    public float maxZoomPixel;
    public float ZoomNormal { get; private set; }
    public float ZoomInch { get; private set; }
    private List<int> fingerIdList = new List<int>();
    private int primaryFingerId = int.MinValue;

    private void Awake()
    {
        minZoomPixel = minZoomInch * Screen.dpi;
        maxZoomPixel = maxZoomInch * Screen.dpi;
    }
    public void UpdatePinchZoom()
    {
        if (fingerIdList.Count >= 2)
        {
            //[0] 1st Touch / [1] 2nd Touch
            Vector2[] prevTouchPos = new Vector2[2];
            Vector2[] currentTouchPos = new Vector2[2];

            for (int i = 0; i < 2; i++)
            {
                var touch = Array.Find(Input.touches, x => x.fingerId == fingerIdList[i]);
                currentTouchPos[i] = touch.position;
                prevTouchPos[i] = touch.position - touch.deltaPosition;
            }

            // PrevFrame Distance
            var prevFrameDist = Vector2.Distance(prevTouchPos[0], currentTouchPos[0]);
            // CurrentFrame Distance
            var currFrameDist = Vector2.Distance(prevTouchPos[1], currentTouchPos[1]);

            var distancePixel = prevFrameDist - currFrameDist;
            ZoomInch = distancePixel / Screen.dpi;
        }
    }

    private void Update()
    {
        foreach (var touch in Input.touches)
        {
            switch (touch.phase)
            {
                case TouchPhase.Began:
                    if (fingerIdList.Count == 0 && primaryFingerId == int.MinValue)
                    {
                        primaryFingerId = touch.fingerId;
                    }

                    fingerIdList.Add(touch.fingerId);
                    break;
                case TouchPhase.Moved:
                case TouchPhase.Stationary:
                    break;
                case TouchPhase.Ended:
                case TouchPhase.Canceled:
                    if (primaryFingerId == touch.fingerId)
                    {
                        primaryFingerId = int.MinValue;
                    }
                    fingerIdList.Remove(touch.fingerId);
                    break;
                default:
                    break;
            }
        }

        //if (EventSystem.current.IsPointerOverGameObject(Input.touches[0].fingerId))
        //{
        //    Debug.Log("Event!");
        //}

#if UNITY_EDITOR || UNITY_STANDALONE
        //Mouse Wheel Zoom
#elif UNITY_ANDROID || UNITY_IOS
        UpdatePinchZoom();
#endif
    }

}
