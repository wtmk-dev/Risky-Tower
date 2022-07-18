using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TavernView : MonoBehaviour
{
    public Button EnterTower;
    public TextMeshProUGUI Blood, Gold, Floor;
    public GameObject UpgradeSelect;
    public DynamicDieView Encounter, Trap, Treasure;

    public void Enter(Player player)
    {
        gameObject.SetActive(true);

        Blood.SetText($"Blood: {player.Model.Blood}");
        Gold.SetText($"Gold: {player.Model.Gold}");
        Floor.SetText($"Highest Floor: {player.Model.HighestFloor}");

        Encounter.UpdateDie(player.Dice[DynamicDieType.Encounter]);
        Trap.UpdateDie(player.Dice[DynamicDieType.Trap]);
        Treasure.UpdateDie(player.Dice[DynamicDieType.Treasure]);
    }
}
