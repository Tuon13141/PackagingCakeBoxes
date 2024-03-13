using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelController : MonoBehaviour
{
    public List<Level> levels = new List<Level>();
    public int levelChoice;

    public GameObject mCamera;
    private Slot[,] slots;
    public GameObject cake;
    public GameObject candy;
    public GameObject box;
    public GameObject frame;

    private GameObject cakeObj;
    private GameObject boxObj;

    private bool cakeEndMove = true;
    private bool boxEndMove = true;

    [SerializeField]
    private bool canMove = true;
    [SerializeField]
    private int hadMoveVertical = 0;

    private Vector2 cakeTar;
    private Vector2 boxTar;

    public GameObject victory;
    public GameObject failed;

    private bool isVictory = false;
    private int time = 45;
    public Text timeText;

    InputSystem inputActions;
    private void Start()
    {
        timeText.text = "Time: " + time.ToString();
        InputSystemPlugin();
        levelChoice = PlayerPrefs.GetInt("Level");
        RenderMap();
        StartCoroutine(Failed());
        StartCoroutine(CountDown());
    }

    private void Update()
    {
       

        if(cakeObj != null && boxObj != null)
        {
            if (cakeEndMove && boxEndMove && canMove)
            {

                if (Input.GetKeyDown(KeyCode.W))
                {
                    Debug.Log(1);
                    cakeEndMove = false;
                    boxEndMove = false;
                    canMove = false;
                    StartCoroutine(choiceTarget(1));
                }
                else if (Input.GetKeyDown(KeyCode.S))
                {
                    cakeEndMove = false;
                    boxEndMove = false;
                    canMove = false;
                    StartCoroutine(choiceTarget(-1));
                }
                else if (Input.GetKeyDown(KeyCode.A))
                {
                    cakeEndMove = false;
                    boxEndMove = false;
                    canMove = false;
                    StartCoroutine(choiceTarget(2));
                }
                else if (Input.GetKeyDown(KeyCode.D))
                {
                    cakeEndMove = false;
                    boxEndMove = false;
                    canMove = false;
                    StartCoroutine(choiceTarget(-2));
                }
            }
            cakeObj.transform.position = Vector3.MoveTowards(cakeObj.transform.position, cakeTar, 5 * Time.deltaTime);
            boxObj.transform.position = Vector3.MoveTowards(boxObj.transform.position, boxTar, 5 * Time.deltaTime);

            if ((Vector2)cakeObj.transform.position == cakeTar && (Vector2)boxObj.transform.position == boxTar && cakeEndMove && boxEndMove)
            {
                canMove = true;
            }

            if(cakeObj.transform.position.y == boxObj.transform.position.y + 2 && cakeObj.transform.position.x == boxObj.transform.position.x && hadMoveVertical != 0)
            {
                //Debug.Log("winning");
                if(hadMoveVertical == 1)
                {
                    canMove = false ;
                    boxTar = new Vector2(boxTar.x, cakeObj.transform.position.y);
                }
                else
                {
                    canMove = false;
                    cakeTar = new Vector2(cakeTar.x, boxObj.transform.position.y);
                }
            }

            if (cakeObj.transform.position == boxObj.transform.position)
            {
                //Debug.Log("win");
                victory.SetActive(true);
                isVictory = true;
            }
        }  

        
    }

    private void RenderMap()
    {
        slots = new Slot[levels[levelChoice - 1].x + 2, levels[levelChoice - 1].y + 2];

        for (int i = 0; i < levels[levelChoice - 1].x + 2; i++)
        {
            for (int j = 0; j < levels[levelChoice - 1].y + 2; j++)
            {
                slots[i, j] = new Slot();
                slots[i, j] = levels[levelChoice - 1].StartLevel()[i, j];
            
            }
        }

        mCamera.transform.position = new Vector3(levels[levelChoice - 1].x + 1, levels[levelChoice - 1].y + 1, -10);
      
        for(int i = 0; i < levels[levelChoice - 1].x + 2; i++)
        {
            for(int j = 0; j < levels[levelChoice - 1].y + 2; j++)
            { 
                GameObject tempFrame = Instantiate(frame, new Vector3(i * 2, j * 2, 0), Quaternion.identity); //Tren unity thi 1 o ma tran = 4 o pixel
                if (slots[i, j].isBorder)
                {
                    tempFrame.GetComponent<SpriteRenderer>().sortingOrder = -11;
                }
                if (slots[i, j].isWalled)
                {
                    Instantiate (candy, new Vector3(i * 2, j * 2, 0), Quaternion.identity);
                }

                if (slots[i, j].hadCake)
                {
                    cakeObj = Instantiate(cake, new Vector3(i * 2, j * 2, 0), Quaternion.identity);
                    cakeTar = cakeObj.transform.position;
                }

                if (slots[i, j].hadBox)
                {
                    boxObj = Instantiate (box, new Vector3(i * 2, j * 2, 0), Quaternion.identity);
                    boxTar = boxObj.transform.position; 
                }
            }
        }
    }

    private IEnumerator choiceTarget(int dir)
    {
        slots[(int)cakeObj.transform.position.x / 2, (int)cakeObj.transform.position.y / 2].isBlocked = false;
        slots[(int)boxObj.transform.position.x / 2, (int)boxObj.transform.position.y / 2].isBlocked = false;
        yield return null;
      
        if (dir == 1)
        {
            hadMoveVertical = 1;
            if (slots[(int)boxTar.x / 2, (int)boxTar.y / 2 + 1].isBlocked)
            {
                //Debug.Log("t1");

                slots[(int)boxTar.x / 2, (int)boxTar.y / 2].isBlocked = true;
                boxEndMove = true;
                //check1 = true;
            }

            if (slots[(int)cakeTar.x / 2, (int)cakeTar.y / 2 + 1].isBlocked)
            {
                //Debug.Log("t2");

                slots[(int)cakeTar.x / 2, (int)cakeTar.y / 2].isBlocked = true;
                cakeEndMove = true;
                //check2 = true;
            }


            if (!slots[(int)boxTar.x / 2, (int)boxTar.y / 2].isBlocked && !boxEndMove)
            {
                boxTar = new Vector2(boxTar.x, boxTar.y + 2);
            }
            if (slots[(int)boxTar.x / 2, (int)boxTar.y / 2].isBlocked && !boxEndMove)
            {
                boxEndMove = true;
                slots[(int)boxTar.x / 2, (int)boxTar.y / 2 - 1].isBlocked = true;
                boxTar = new Vector2(boxTar.x, boxTar.y - 2);
                //Debug.Log(2);
            }


            if (!slots[(int)cakeTar.x / 2, (int)cakeTar.y / 2].isBlocked && !cakeEndMove)
            {
                cakeTar = new Vector2(cakeTar.x, cakeTar.y + 2);
            }
            if (slots[(int)cakeTar.x / 2, (int)cakeTar.y / 2].isBlocked && !cakeEndMove)
            {
                cakeEndMove = true;
                slots[(int)cakeTar.x / 2, (int)cakeTar.y / 2 - 1].isBlocked = true;
                //Debug.Log(3);
                cakeTar = new Vector2(cakeTar.x, cakeTar.y - 2);
            }


            //Debug.Log("box " + boxTar);
            //Debug.Log("cake " + cakeTar);
        }
        else if(dir == 2)
        {
            hadMoveVertical = 0;
            if (slots[(int)boxTar.x / 2 - 1, (int)boxTar.y / 2].isBlocked)
            {
                //Debug.Log("t1");

                slots[(int)boxTar.x / 2, (int)boxTar.y / 2].isBlocked = true;
                boxEndMove = true;
                //check1 = true;
            }

            if (slots[(int)cakeTar.x / 2 - 1, (int)cakeTar.y / 2].isBlocked)
            {
                //Debug.Log("t2");

                slots[(int)cakeTar.x / 2, (int)cakeTar.y / 2].isBlocked = true;
                cakeEndMove = true;
                //check2 = true;
            }


            if (!slots[(int)boxTar.x / 2, (int)boxTar.y / 2].isBlocked && !boxEndMove)
            {
                boxTar = new Vector2(boxTar.x - 2, boxTar.y);
            }
            if (slots[(int)boxTar.x / 2, (int)boxTar.y / 2].isBlocked && !boxEndMove)
            {
                boxEndMove = true;
                slots[(int)boxTar.x / 2 + 1, (int)boxTar.y / 2].isBlocked = true;
                boxTar = new Vector2(boxTar.x + 2, boxTar.y);
                //Debug.Log(2);
            }


            if (!slots[(int)cakeTar.x / 2, (int)cakeTar.y / 2].isBlocked && !cakeEndMove)
            {
                cakeTar = new Vector2(cakeTar.x - 2, cakeTar.y);
            }
            if (slots[(int)cakeTar.x / 2, (int)cakeTar.y / 2].isBlocked && !cakeEndMove)
            {
                cakeEndMove = true;
                slots[(int)cakeTar.x / 2 + 1, (int)cakeTar.y / 2].isBlocked = true;
                //Debug.Log(3);
                cakeTar = new Vector2(cakeTar.x + 2, cakeTar.y);
            }

            //Debug.Log("box " + boxTar);
            //Debug.Log("cake " + cakeTar);
        }
        else if(dir == -1)
        {
            hadMoveVertical = -1;
            if (slots[(int)boxTar.x / 2, (int)boxTar.y / 2 - 1].isBlocked)
            {
                //Debug.Log("t1");

                slots[(int)boxTar.x / 2, (int)boxTar.y / 2].isBlocked = true;
                boxEndMove = true;
               
            }

            if (slots[(int)cakeTar.x / 2, (int)cakeTar.y / 2 - 1].isBlocked)
            {
                //Debug.Log("t2");

                slots[(int)cakeTar.x / 2, (int)cakeTar.y / 2].isBlocked = true;
                cakeEndMove = true;
               
            }

            if (!slots[(int)boxTar.x / 2, (int)boxTar.y / 2].isBlocked && !boxEndMove)
            {
                boxTar = new Vector2(boxTar.x, boxTar.y - 2);
            }
            if (slots[(int)boxTar.x / 2, (int)boxTar.y / 2].isBlocked && !boxEndMove)
            {
                boxEndMove = true;
                slots[(int)boxTar.x / 2, (int)boxTar.y / 2 + 1].isBlocked = true;
                boxTar = new Vector2(boxTar.x, boxTar.y + 2);
                //Debug.Log(2);
            }

            if (!slots[(int)cakeTar.x / 2, (int)cakeTar.y / 2].isBlocked && !cakeEndMove)
            {
                cakeTar = new Vector2(cakeTar.x, cakeTar.y - 2);
            }
            if (slots[(int)cakeTar.x / 2, (int)cakeTar.y / 2].isBlocked && !cakeEndMove)
            {
                cakeEndMove = true;
                slots[(int)cakeTar.x / 2, (int)cakeTar.y / 2 + 1].isBlocked = true;
                //Debug.Log(3);
                cakeTar = new Vector2(cakeTar.x, cakeTar.y + 2);
            }

           
            //Debug.Log("box " + boxTar);
            //Debug.Log("cake " + cakeTar);
        }
        else if(dir == -2)
        {
            hadMoveVertical = 0;

            if (slots[(int)boxTar.x / 2 + 1, (int)boxTar.y / 2].isBlocked)
            {
                //Debug.Log("t1");

                slots[(int)boxTar.x / 2, (int)boxTar.y / 2].isBlocked = true;
                boxEndMove = true;
                //check1 = true;
            }

            if (slots[(int)cakeTar.x / 2 + 1, (int)cakeTar.y / 2].isBlocked)
            {
                //Debug.Log("t2");

                slots[(int)cakeTar.x / 2, (int)cakeTar.y / 2].isBlocked = true;
                cakeEndMove = true;
                //check2 = true;
            }


            if (!slots[(int)boxTar.x / 2, (int)boxTar.y / 2].isBlocked && !boxEndMove)
            {
                boxTar = new Vector2(boxTar.x + 2, boxTar.y);
            }
            if (slots[(int)boxTar.x / 2, (int)boxTar.y / 2].isBlocked && !boxEndMove)
            {
                boxEndMove = true;
                slots[(int)boxTar.x / 2 - 1, (int)boxTar.y / 2].isBlocked = true;
                boxTar = new Vector2(boxTar.x - 2, boxTar.y);
                //Debug.Log(2);
            }


            if (!slots[(int)cakeTar.x / 2, (int)cakeTar.y / 2].isBlocked && !cakeEndMove)
            {
                cakeTar = new Vector2(cakeTar.x + 2, cakeTar.y);
            }
            if (slots[(int)cakeTar.x / 2, (int)cakeTar.y / 2].isBlocked && !cakeEndMove)
            {
                cakeEndMove = true;
                slots[(int)cakeTar.x / 2 - 1, (int)cakeTar.y / 2].isBlocked = true;
                //Debug.Log(3);
                cakeTar = new Vector2(cakeTar.x - 2, cakeTar.y);
            }

        }

        if (cakeEndMove && boxEndMove)
        {
            //Debug.Log("Done");
           // canMove = true;
            StopCoroutine(choiceTarget(dir));
            yield break;
        }

        StartCoroutine(choiceTarget(dir));
    }

    IEnumerator Failed()
    {
        yield return new WaitForSeconds(45);
        if(!isVictory)
        {
            timeText.text = "Time: 0";
            canMove = false;
            StopAllCoroutines();

            failed.SetActive(true);
        }
       
    }

    IEnumerator CountDown()
    {
        yield return new WaitForSeconds(1);
        time--;
        timeText.text = "Time: " + time.ToString();
        if (time > 0)
        {
            StartCoroutine(CountDown());
        }
    }

    void InputSystemPlugin()
    {
        inputActions = new InputSystem();
        inputActions.Enable();

        inputActions.Main.MoveUp.performed += ctx =>
        {
            if (cakeEndMove && boxEndMove && canMove)
            {
                cakeEndMove = false;
                boxEndMove = false;
                canMove = false;
                StartCoroutine(choiceTarget(1));
            }


        };
        inputActions.Main.MoveDown.performed += ctx => {
            if (cakeEndMove && boxEndMove && canMove)
            {
                cakeEndMove = false;
                boxEndMove = false;
                canMove = false;
                StartCoroutine(choiceTarget(-1));
            }


        };


        inputActions.Main.MoveLeft.performed += ctx => {
            if (cakeEndMove && boxEndMove && canMove)
            {
                cakeEndMove = false;
                boxEndMove = false;
                canMove = false;
                StartCoroutine(choiceTarget(2));
            }


        };

        inputActions.Main.MoveRight.performed += ctx => {
            if (cakeEndMove && boxEndMove && canMove)
            {
                cakeEndMove = false;
                boxEndMove = false;
                canMove = false;
                StartCoroutine(choiceTarget(-2));
            }


        };
    }
}
