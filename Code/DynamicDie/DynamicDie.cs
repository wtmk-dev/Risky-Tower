using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DynamicDie 
{
    public int Count { get => _Sides.Count; }
    public DynamicDieSide CurrentFace { get => _Sides[0]; }
    public List<DynamicDieSide> Sides { get => _Sides; }

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
    public Sprite Icon { get => _Effect.Icon; }

    public string DoEffect(PlayerStats stats)
    {
        stats.Blood += _Effect.Healing;
        stats.Gold += _Effect.Gold;

        stats.Blood -= _Effect.Damage;
        stats.Gold -= _Effect.Steal;

        string text = "";

        if(_Effect.Healing > 0)
        {
            text += $"Healed {_Effect.Healing}";
        }

        if (_Effect.Gold > 0)
        {
            text += $"Gold gained! {_Effect.Gold}";
        }

        if (_Effect.Damage > 0)
        {
            text += $"Damage taken! {_Effect.Damage}";
        }

        if (_Effect.Steal > 0)
        {
            text += $"Gold stolen! {_Effect.Steal}";
        }

        return text;
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
    public Sprite Icon { get; private set; }

    public DynamicDieEffect(int damage, int gold, int healing, int steal, Sprite icon)
    {
        Damage = damage;
        Gold = gold;
        Healing = healing;
        Steal = steal;
        Icon = icon;
    }

    public DynamicDieEffect()
    {

    }
}

