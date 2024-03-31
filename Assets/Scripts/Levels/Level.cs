using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class Level : ScriptableObject
{
    public int matrixX;
    public int matrixY;

    public List<Vector2> cakePos;
    public Vector2 boxPos;

    public List<Vector2> wallsPos;

    private MatrixController matrixController;
    private ObjectController objectController;
    public void RenderMap(GameObject cakePref, GameObject boxPref, GameObject wallPref, GameObject framePref)
    {
        int xLength = matrixX + 2;
        int yLength = matrixY + 2;
        matrixController.X = xLength;
        matrixController.Y = yLength;

        objectController.CenterizeCamera(new Vector2((xLength - 1) / 2, (yLength - 1) / 2));
        matrixController.totalCake = cakePos.Count;

        Slot[, ] matrix = new Slot[xLength, yLength];

        for (int i = 0; i < xLength; i++)
        {
            for (int j = 0; j < yLength; j++)
            {
                matrix[i, j] = new Slot();

                bool isMoveInable = true;
                foreach (Vector2 item in cakePos)
                {
                    if (i == item.x && j == item.y)
                    {
                        GameObject o = Instantiate(cakePref, new Vector2(i, j), Quaternion.identity);
                        isMoveInable = false;
                        matrix[i, j].hadCake = true;
                        matrix[i, j].MoveableObject = o.GetComponent<MoveableObject>();
                        matrix[i, j].MoveableObject.SetTargetPoint(new Vector2(i, j));  

                        matrixController.AddMoveableObjects(o.GetComponent<MoveableObject>());
                    }
                }
                   

                if (i == boxPos.x && j == boxPos.y)
                {
                    GameObject o = Instantiate(boxPref, new Vector2(i, j), Quaternion.identity);
                    isMoveInable = false;
                    matrix[i, j].hadBox = true;
                    matrix[i, j].MoveableObject = o.GetComponent<MoveableObject>();
                    matrix[i, j].MoveableObject.SetTargetPoint(new Vector2(i, j));

                    matrixController.AddMoveableObjects(o.GetComponent<MoveableObject>());
                }

                foreach(Vector2 item in wallsPos)
                {
                    if(item.x == i && item.y == j)
                    {
                        Instantiate(wallPref, new Vector2(i, j), Quaternion.identity);
                        isMoveInable = false;
                    }
                }

                GameObject frame = Instantiate(framePref, new Vector2(i, j), Quaternion.identity);

                if(i == 0 || i == xLength - 1 || j == 0 ||  j == yLength - 1)
                {
                    frame.GetComponent<Renderer>().sortingOrder = -10;
                    isMoveInable = false;
                }

                if (!isMoveInable)
                {
                    matrix[i, j].isMoveInable = false;
                }

            }
        }

        matrixController.SetSlots(matrix);
    }

    public void SetMatrixController(MatrixController matrixController)
    {
        this.matrixController = matrixController;
    }

    public void SetObjectController(ObjectController objectController)
    {
        this.objectController = objectController;
    }
}
