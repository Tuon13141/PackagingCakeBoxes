using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class Level : ScriptableObject
{
    public int x;
    public int y;

    public List<int> xObs;
    public List<int> yObs;

    public int xCake;
    public int yCake;

    public int xBox;
    public int yBox;

    public List<int> xStar;
    public List<int> yStar;

    public Slot[,] StartLevel()
    {
        Slot[,] slots = new Slot[x+2,y+2];

        for (int i = 0; i < x + 2; i++)
        {
            for (int j = 0; j < y + 2; j++)
            {
                slots[i, j] = new Slot();        
            }
        }

        for (int i = 0; i < x + 2; i++)
        {
            slots[i, 0].isBlocked = true;
            slots[i, y+1].isBlocked = true;
            slots[i, 0].isBorder = true;
            slots[i, y + 1].isBorder = true;
        }

        for(int i = 0; i < y + 2; i++)
        {
            slots[0, i].isBlocked = true;
            slots[x+1, i].isBlocked = true;
            slots[0, i].isBorder = true;
            slots[x + 1, i].isBorder = true;
        }

        for(int i = 0; i < xObs.Count; i++)
        {
            slots[xObs[i], yObs[i]].isBlocked = true;
            slots[xObs[i], yObs[i]].isWalled = true;
        }

        for(int i = 0; i < xStar.Count; i++)
        {
            slots[xStar[i], yStar[i]].isBlocked = true;
            slots[xStar[i], yStar[i]].hadStar = true;
        }

        slots[xCake, yCake].hadCake = true;
        slots[xCake, yCake].isBlocked = true;
        slots[xBox, yBox].hadBox = true;
        slots[xBox, yBox].isBlocked = true;

        return slots;
    }
}
