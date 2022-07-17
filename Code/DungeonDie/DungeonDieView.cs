using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DungeonDieView : MonoBehaviour
{
    public Image Die;
    public List<Sprite> DungeonDieFaces;

    public void DoRoll(DungeonDieSide side, Action onClomplete)
    {
        StartCoroutine(Roll(side, onClomplete));
    }

    IEnumerator Roll(DungeonDieSide side, Action onClomplete)
    {
        int count = 0;
        while(count < DungeonDieFaces.Count)
        {
            Die.sprite = DungeonDieFaces[count];
            count++;
            yield return new WaitForSeconds(0.3f);

            _Tools.Shuffle<Sprite>(DungeonDieFaces);
        }

        //set die to roll
        onClomplete();
    }

    private WTMK _Tools = WTMK.Instance;
}
