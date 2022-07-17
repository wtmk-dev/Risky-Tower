using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player
{
    public Dictionary<DynamicDieType, DynamicDie> Dice { get; private set; }
    public List<DynamicDieSide> Inventory { get; private set; }
    public PlayerStats Model { get; private set; }
    public PlayerView View { get; private set; }
    public DynamicDieSide CurrentFace { get; private set; }

    public DynamicDieSide Roll(DungeonDieSide side)
    {
        switch(side)
        {
            case DungeonDieSide.Encounter:
                var encounterDie = Dice[DynamicDieType.Encounter];
                CurrentFace = encounterDie.Roll();
                break;
            case DungeonDieSide.Trap:
                var trapDie = Dice[DynamicDieType.Trap];
                CurrentFace = trapDie.Roll();
                break;
            case DungeonDieSide.Treasure:
                var lootDie = Dice[DynamicDieType.Treasure];
                CurrentFace = lootDie.Roll();
                break;
        }

        return CurrentFace;
    }

    public DynamicDieSide Roll(DynamicDieType side)
    {
        CurrentFace = Dice[side].Roll();
        return CurrentFace;
    }

    public Player(DynamicDie encounter, DynamicDie trap, DynamicDie loot, PlayerStats model, PlayerView view)
    {
        Dice = new Dictionary<DynamicDieType, DynamicDie>();
        Dice.Add(DynamicDieType.Encounter, encounter);
        Dice.Add(DynamicDieType.Trap, trap);
        Dice.Add(DynamicDieType.Treasure, loot);

        Model = model;
        View = view;
    }
}

public class PlayerStats
{
    public int Blood { get; set; }
    public int Gold { get; set; }
    public int HighestFloor { get; set; }
}
