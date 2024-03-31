using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwipeController : MonoBehaviour
{
    private Vector2 startTouchPosition, endTouchPosition;

    private Touch touch;

    private bool coroutineAllowed;

    public MatrixController matrixController;
    void Start()
    {
        coroutineAllowed = true;
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.touchCount > 0)
        {
            touch = Input.GetTouch(0);
        }

        if(touch.phase == TouchPhase.Began)
        {
           
            startTouchPosition = touch.position;
        }

        if(Input.touchCount > 0 && touch.phase == TouchPhase.Ended && coroutineAllowed)
        {
            //Debug.Log("detected");
            endTouchPosition = touch.position;

            if(endTouchPosition.y > startTouchPosition.y && Mathf.Abs(touch.deltaPosition.y) >= Mathf.Abs(touch.deltaPosition.x))
            {
                SwipeChoice.swipeDirection = SwipeDirection.up;              
            }

            else if(endTouchPosition.y < startTouchPosition.y && Mathf.Abs(touch.deltaPosition.y) >= Mathf.Abs(touch.deltaPosition.x))
            {
                SwipeChoice.swipeDirection = SwipeDirection.down;             
            }

            else if (endTouchPosition.x > startTouchPosition.x && Mathf.Abs(touch.deltaPosition.y) < Mathf.Abs(touch.deltaPosition.x))
            {
                SwipeChoice.swipeDirection = SwipeDirection.right;             
            }

            else if (endTouchPosition.x < startTouchPosition.x && Mathf.Abs(touch.deltaPosition.y) < Mathf.Abs(touch.deltaPosition.x))
            {
                SwipeChoice.swipeDirection = SwipeDirection.left;            
            }
            StartCoroutine(Swipe());
        }
    }

    private IEnumerator Swipe()
    {
        coroutineAllowed = false;

        matrixController.Swipe();
       
        yield return new WaitForSeconds(0.5f);

        //coroutineAllowed = true;
    }

    public void SetCoroutineAllowed(bool coroutineAllowed)
    {
        this.coroutineAllowed = coroutineAllowed;
    }

    public void DisableSwipe()
    {
        StopCoroutine(Swipe());
        coroutineAllowed = false;
    }
}

public enum SwipeDirection
{
    up, down, left, right, start
}

public static class SwipeChoice
{
    public static SwipeDirection swipeDirection = SwipeDirection.start;
}