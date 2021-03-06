﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScrollbarAutoScroller : MonoBehaviour
{
    public Scrollbar scrollbar;
    public float speed = 10f, percentageDone = 0.99f, paddingInSeconds = 0.2f, waitForSecondsAtStart = 0.2f;
    public ScrollMode mode = ScrollMode.startOver;
    public GameObject listsize;


    public enum ScrollMode
    {
        startOver,
        backtrack,
        none
    }

    float scrollTimer = 0, timer = 0;

    void Start()
    {

        if ((scrollbar = GetComponent<Scrollbar>()) == null)
        {
            if ((scrollbar = GetComponentInParent<Scrollbar>()) == null)
                scrollbar = GetComponentInChildren<Scrollbar>();
        }
        scrollbar.value = 1;
    }

    //Controls how fast the autoscroll is when scrolling to the end of the list
    void Update() 
    {

        if (mode == ScrollMode.none)
            return;

        if (scrollbar.value == 1 && (timer += Time.deltaTime) < waitForSecondsAtStart)
            return;

        else if (scrollbar.value <= (1 - percentageDone) && speed > 0 || scrollbar.value >= (1 - percentageDone) && speed < 0)
        {

            if ((timer += Time.deltaTime) < paddingInSeconds)
            {
                return;
            }
            else
                timer = 0;
            
            ReachedEnd();
           
        }

        scrollbar.value = Mathf.LerpUnclamped(1, 0, scrollTimer);
        scrollTimer += Time.deltaTime * (100 / listsize.GetComponent<RectTransform>().rect.height);

    }
    
    //Two options for when the deadline has reached the end, currently using ScrollMode.startOver
    //Backtrack makes the list scroll back up in reverse
    //Start over makes the list start over again from the top
    public void ReachedEnd()
    {
        
        switch (mode)
        {
            case ScrollMode.backtrack:
                speed *= -1;
                percentageDone = 1 - percentageDone;
                break;

            case ScrollMode.startOver:
                
                scrollbar.value = 1;
                scrollTimer = 0;
                timer = 0;
                break;

            default:
                break;
        }
    }
}
