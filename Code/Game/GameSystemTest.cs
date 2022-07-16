using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameSystemTest : MonoBehaviour
{
    [SerializeField]
    private TowerView _TowerView;

    private DynamicDieFactory _DieFactory;
    private GameSystem _GameSystem;
    private Player _Player;

    private Dood _Debug = Dood.Instance;

    public void Awake()
    {
        _DieFactory = new DynamicDieFactory();
        _Player = new Player(_DieFactory.BuildStartingDie(DynamicDieType.Encounter), 
                                _DieFactory.BuildStartingDie(DynamicDieType.Trap),
                                _DieFactory.BuildStartingDie(DynamicDieType.Treasure),
                                new PlayerStats());

        _Player.Model.Blood = 9;
        _Player.Model.Gold = 0;

        _GameSystem = new GameSystem(_Player, _TowerView);

        Dood.IsLogging = true;
    }

    public void Start()
    {
        _GameSystem.Start();
    }
}
