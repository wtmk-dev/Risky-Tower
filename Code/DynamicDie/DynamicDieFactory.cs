using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DynamicDieFactory
{
    private DynamicDieSideFactory _SideFactory = new DynamicDieSideFactory();
    public DynamicDie BuildStartingDie(DynamicDieType ddt)
    {
        var sides = new List<DynamicDieSide>();
        switch (ddt)
        {
            case DynamicDieType.Encounter:
                sides.Add(_SideFactory.BuildSide("Start_Enc_0"));
                sides.Add(_SideFactory.BuildSide("Start_Enc_1"));
                sides.Add(_SideFactory.BuildSide("Start_Enc_2"));
                return new DynamicDie(ddt,sides);
            case DynamicDieType.Treasure:
                sides.Add(_SideFactory.BuildSide("Start_Loot_0"));
                sides.Add(_SideFactory.BuildSide("Start_Loot_1"));
                return new DynamicDie(ddt, sides);
            case DynamicDieType.Trap:
                sides.Add(_SideFactory.BuildSide("Start_Trap_0"));
                sides.Add(_SideFactory.BuildSide("Start_Trap_1"));
                sides.Add(_SideFactory.BuildSide("Start_Trap_1"));
                return new DynamicDie(ddt, sides);
        }

        return new DynamicDie(ddt, sides);
    }
}

public class DynamicDieSideFactory
{
    public DynamicDieSide BuildSide(string name)
    {
        switch(name)
        {
            case "Start_Enc_0":
                var effect = new DynamicDieEffect(1, 0, 0, 0);
                return new DynamicDieSide(effect, true);

            case "Start_Enc_1":
                var effect2 = new DynamicDieEffect(0, 1, 0, 0);
                return new DynamicDieSide(effect2);

            case "Start_Enc_3":
                var effect3 = new DynamicDieEffect(0, 0, 1, 1);
                return new DynamicDieSide(effect3);

            case "Start_Trap_0":
                var t0 = new DynamicDieEffect(0, 0, 0, 1);
                return new DynamicDieSide(t0, true);

            case "Start_Trap_1":
                var t1 = new DynamicDieEffect(1, 0, 0, 1);
                return new DynamicDieSide(t1);

            case "Start_Loot_0":
                var l0 = new DynamicDieEffect(0, 1, 0, 0);
                return new DynamicDieSide(l0, true);

            case "Start_Loot_1":
                var l1 = new DynamicDieEffect(0, 0, 1, 0);
                return new DynamicDieSide(l1);

        }

        return new DynamicDieSide(new DynamicDieEffect());
    }
}
