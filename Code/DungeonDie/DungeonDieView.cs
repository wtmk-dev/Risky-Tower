using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DungeonDieView : MonoBehaviour
{
    public Image Die;
    public List<Sprite> DungeonDieFaces;
    public Sprite Encounter, Trap, Treasure, Exit, Wild;

    public void DoRoll(DungeonDieSide side, Action onClomplete)
    {
        StartCoroutine(Roll(side, onClomplete));
    }

    IEnumerator Roll(DungeonDieSide side, Action onClomplete)
    {
        int count = 0;
        while(count < DungeonDieFaces.Count)
        {
            _Tools.Shuffle<Sprite>(DungeonDieFaces);
            Die.sprite = DungeonDieFaces[count];
            count++;
            yield return new WaitForSeconds(0.3f);
        }

        Die.sprite = _DungeonDieMap[side];
        yield return new WaitForSeconds(0.3f);
        onClomplete();
    }

    private Dictionary<DungeonDieSide, Sprite> _DungeonDieMap = new Dictionary<DungeonDieSide, Sprite>();
    private WTMK _Tools = WTMK.Instance;

    private void Start()
    {
        _DungeonDieMap.Clear();

        _DungeonDieMap.Add(DungeonDieSide.Encounter, Encounter);
        _DungeonDieMap.Add(DungeonDieSide.Trap, Trap);
        _DungeonDieMap.Add(DungeonDieSide.Treasure, Treasure);
        _DungeonDieMap.Add(DungeonDieSide.Exit, Exit);
        _DungeonDieMap.Add(DungeonDieSide.Wild, Wild);
    }
}
