using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class LevelSave 
{
    public int level;
    public int stars;

    public LevelSave(int level, int stars)
    {
        this.level = level;
        this.stars = stars;
    }
}
