using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cell
{
    private bool occupied;

    public Cell()
    {
        occupied = false;
    }

    public void SetOccupied()
    {
        occupied = true;
    }

    public void SetFree()
    {
        occupied = false;
    }
}
