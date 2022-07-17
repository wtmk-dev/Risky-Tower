using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DungeonDie
{
    public List<DungeonDieSide> Die { get; private set; }
    public DungeonDieSide CurrentFace { get => Die[0]; }

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

    public DungeonDieSide Roll(Action onComplete)
    {
        _Tools.Shuffle<DungeonDieSide>(Die);
        _View.DoRoll(CurrentFace, onComplete);
        return CurrentFace;
    }

    private DungeonDieView _View;
    private WTMK _Tools = WTMK.Instance;
    public DungeonDie(DungeonDieView view)
    {
        _View = view;
        Die = new List<DungeonDieSide>();
    }
}
