using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DungeonDie
{
    public List<DungeonDieSide> Die { get; private set; }

    public void EnterFloor(int floor)
    {
        Die.Clear();
        switch (floor)
        {
            default:
                Die.Add(DungeonDieSide.Encounter);
                Die.Add(DungeonDieSide.Encounter);
                Die.Add(DungeonDieSide.Treasure);
                Die.Add(DungeonDieSide.Trap);
                Die.Add(DungeonDieSide.Exit);
                Die.Add(DungeonDieSide.Wild);
                break;
        }
    }

    public DungeonDie()
    {
        Die = new List<DungeonDieSide>();
    }
}
