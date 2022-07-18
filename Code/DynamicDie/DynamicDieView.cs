using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DynamicDieView : MonoBehaviour
{
    public List<Image> _Sides;

    public void UpdateSides(List<DynamicDieSide> sides)
    {
        for(int i = 0; i < _Sides.Count; i++)
        {
            _Sides[i].gameObject.SetActive(false);
        }

        for (int i = 0; i < sides.Count; i++)
        {
            _Sides[i].gameObject.SetActive(true);
            _Sides[i].sprite = sides[i].Icon;
        }
    }

    public void UpdateDie(DynamicDie die)
    {
        UpdateSides(die.Sides);
    }
}
