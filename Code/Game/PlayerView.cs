using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerView : MonoBehaviour
{
    public Image EncounterDie, TrapDie, TreasureDie;
    public TextMeshProUGUI EffectText;

    public void DoRoll(DynamicDie die, DynamicDieType type, DynamicDieSide outcome, Action onComplete)
    {
        StartCoroutine(Roll(die, type, outcome, onComplete));
    }

    IEnumerator Roll(DynamicDie die, DynamicDieType type, DynamicDieSide outcome, Action onComeplete)
    {
        int count = 0;
        while(count < die.Count)
        {
            _Tools.Shuffle<DynamicDieSide>(die.Sides);
            _DieMap[type].sprite = die.Sides[count].Icon;
            count++;
            yield return new WaitForSeconds(0.3f);
        }

        _DieMap[type].sprite = outcome.Icon;
        yield return new WaitForSeconds(0.6f);
        onComeplete?.Invoke();
    }

    private WTMK _Tools = WTMK.Instance;
    private Dictionary<DynamicDieType, Image> _DieMap = new Dictionary<DynamicDieType, Image>();
    private void Start()
    {
        _DieMap.Clear();

        _DieMap.Add(DynamicDieType.Encounter, EncounterDie);
        _DieMap.Add(DynamicDieType.Trap, TrapDie);
        _DieMap.Add(DynamicDieType.Treasure, TreasureDie);
        
    }
}
