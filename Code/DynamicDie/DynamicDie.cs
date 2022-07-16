using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DynamicDie 
{
    public int Count { get => _Sides.Count; }
    public DynamicDieSide CurrentFace { get => _Sides[0]; }

    public DynamicDieSide Roll()
    {
        _Tools.Shuffle<DynamicDieSide>(_Sides);
        return CurrentFace;
    }

    private DynamicDieType _Type;
    private List<DynamicDieSide> _Sides;
    private WTMK _Tools = WTMK.Instance;

    public DynamicDie(DynamicDieType type, List<DynamicDieSide> sides)
    {
        _Type = type;
        _Sides = sides;
    }
}

public class DynamicDieSide
{
    public event Action<DynamicDieEffect> OnSideEffect;
    public bool IsPermanent;

    public void DoEffect(PlayerStats stats)
    {
        stats.Blood += _Effect.Healing;
        stats.Gold += _Effect.Gold;

        stats.Blood -= _Effect.Damage;
        stats.Gold -= _Effect.Steal;
    }

    private DynamicDieEffect _Effect;
    public DynamicDieSide(DynamicDieEffect effect, bool isPermanent = false)
    {
        _Effect = effect;
        IsPermanent = isPermanent;
    }
}

public class DynamicDieEffect
{
    public int Damage { get; private set; }
    public int Gold { get; private set; }
    public int Healing { get; private set; }
    public int Steal { get; private set; }

    public DynamicDieEffect(int damage, int gold, int healing, int steal)
    {
        Damage = damage;
        Gold = gold;
        Healing = healing;
        Steal = steal;
    }

    public DynamicDieEffect()
    {

    }
}

