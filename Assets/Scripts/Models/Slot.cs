using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slot
{
    public bool hadCake { get; set; } = false;
    public bool hadBox { get; set; } = false;
    public bool isMoveInable { get; set; } = true;

    public MoveableObject MoveableObject { get; set; } = null;
    public Slot()
    {

    }
}

