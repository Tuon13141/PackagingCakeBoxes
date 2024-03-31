using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MatrixController : MonoBehaviour
{
    private Slot[,] slots;
    public int X { get; set; }
    public int Y { get; set; }

    public int totalCake;

    [SerializeField]
    private List<MoveableObject> moveableObjects;

    public ObjectController objectController;
    public LevelController levelController;
    public SwipeController swipeController;

    private void Start()
    {
        StartCoroutine(CheckWinCondition());
    }
    private void Update()
    {
        CheckForMoveableObjectReachTargetPoint(); 
    }
    public void AddMoveableObjects(MoveableObject moveable)
    {
        moveableObjects.Add(moveable);
    }
    public void SetSlots(Slot[,] slots)
    {
        this.slots = slots;     
    }

    public void Swipe()
    {
        if (SwipeChoice.swipeDirection == SwipeDirection.up)
        {
            ScanMatrixTopToBottom();          
        }

        else if (SwipeChoice.swipeDirection == SwipeDirection.down)
        {
            ScanMatrixBottomToTop();
        }

        else if (SwipeChoice.swipeDirection == SwipeDirection.left)
        {
            ScanMatrixLeftToRight();
        }

        else if (SwipeChoice.swipeDirection == SwipeDirection.right)
        {      
            ScanMatrixRightToLeft();
        }

    }
    IEnumerator CheckWinCondition()
    {
        yield return new WaitUntil(() => totalCake == 0);
        levelController.Victory = true;
    }

    void CheckForMoveableObjectReachTargetPoint()
    {
        for(int i = 0; i < moveableObjects.Count; i++)
        {
            if (moveableObjects[i] == null)
            {
                moveableObjects.RemoveAt(i);
                i--;
            }
            else if (moveableObjects[i].reachTargetPoint == false)
            {
                swipeController.SetCoroutineAllowed(false);
                return;
            }
        }
        swipeController.SetCoroutineAllowed(true);
    }

    IEnumerator WaitForCakeCapture(MoveableObject moveableObject)
    {
        moveableObject.reachTargetPoint = false;
        yield return new WaitUntil(() => moveableObject.reachTargetPoint);

        totalCake--;
    }
    void CheckCakeGoInBoxCondition(int i, int j)
    {
        if (slots[i, j].hadCake && slots[i, j - 1].hadBox)
        {
            if (SwipeChoice.swipeDirection == SwipeDirection.up)
            {
                slots[i, j - 1].MoveableObject.SetTargetPoint(new Vector2(i, j));
                StartCoroutine(WaitForCakeCapture(slots[i, j - 1].MoveableObject));
                slots[i, j - 1].hadBox = false;
                slots[i, j - 1].isMoveInable = true;
                slots[i, j].hadCake = false;
                slots[i, j].hadBox = true;
                slots[i, j].isMoveInable = false;
                StartCoroutine(slots[i, j - 1].MoveableObject.DestroyObject(slots[i, j].MoveableObject.gameObject));

                slots[i, j].MoveableObject = slots[i, j - 1].MoveableObject;
                slots[i, j - 1].MoveableObject = null;
                
                objectController.PlayCakeCaptureAudio();
            }

            else if (SwipeChoice.swipeDirection == SwipeDirection.down)
            {
                slots[i, j].MoveableObject.SetTargetPoint(new Vector2(i, j - 1));
                StartCoroutine(WaitForCakeCapture(slots[i, j].MoveableObject));
                slots[i, j].hadCake = false;
                slots[i, j].isMoveInable = true;

                StartCoroutine(slots[i, j].MoveableObject.DestroyObject(slots[i, j].MoveableObject.gameObject));

                slots[i, j].MoveableObject = null;
                objectController.PlayCakeCaptureAudio();
            }
    
        }
        else
        {
            slots[i, j].isMoveInable = false;
        }
    }
    bool CheckElement(int xExtend, int yExtend, int i, int j)
    {
        if ((slots[i, j].hadCake || slots[i, j].hadBox))
        {
            slots[i, j].isMoveInable = true;
            if (slots[i + xExtend, j + yExtend].isMoveInable)
            {             
                slots[i, j].MoveableObject.SetTargetPoint(new Vector2(i + xExtend, j + yExtend));

                if(slots[i, j].hadCake)
                {
                    slots[i, j].hadCake = false;
                    slots[i + xExtend, j + yExtend].hadCake = true;
                }

                if (slots[i, j].hadBox)
                {
                    slots[i, j].hadBox = false;
                    slots[i + xExtend, j + yExtend].hadBox = true;
                }
               
                MoveableObject clone = slots[i + xExtend, j + yExtend].MoveableObject;
                slots[i + xExtend, j + yExtend].MoveableObject = slots[i, j].MoveableObject;
                slots[i, j].MoveableObject = clone;

                slots[i + xExtend, j + yExtend].isMoveInable = false;

                return false;
            }
            else
            {
                CheckCakeGoInBoxCondition(i, j);

                return true;
            }
        }
        return true;
       
    }

    void ScanMatrixBottomToTop()
    {
        bool inCompleted = false;
        for (int j = 0; j < Y; j++)
        {
            for (int i = 0; i < X; i++)
            {
                if(!CheckElement(0, -1, i, j))
                {
                    inCompleted = true;
                }                      
            }
        }

        if (inCompleted)
        {
            ScanMatrixBottomToTop();
        }
    }

    void ScanMatrixTopToBottom()
    {
        bool inCompleted = false;
        for (int j = Y - 1; j >= 0; j--)        
        {
            for (int i = 0; i < X; i++)              
            {
                if(!CheckElement(0 , 1, i, j))
                {
                    inCompleted = true;
                }
            }
        }

        if (inCompleted)
        {
            ScanMatrixTopToBottom();
        }
    }

    void ScanMatrixRightToLeft()
    {
        bool inCompleted = false;
        for (int i = X-1; i >= 0; i--)
        {
            for (int j = Y - 1; j >= 0; j--)              
            {
                if(!CheckElement(1, 0, i, j))
                {
                    inCompleted = true;
                }
            }
        }

        if (inCompleted)
        {
            ScanMatrixRightToLeft();
        }
    }

    void ScanMatrixLeftToRight()
    {
        bool inCompleted = false;
        for (int i = 0; i < X; i++)
        {
            for (int j = Y - 1; j >= 0; j--)
            {
                if(!CheckElement(-1, 0, i, j))
                {
                    inCompleted = true;
                }
            }
        }

        if (inCompleted)
        {
            ScanMatrixLeftToRight();
        }
    }
}
