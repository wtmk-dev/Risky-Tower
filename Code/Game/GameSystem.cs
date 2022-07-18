using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameSystem
{
    public event Action OnExit;

    public void Start()
    {
        _DungeonDie.EnterFloor(_Model.Floor);
        _GamePhase.StateChange(GamePhase.SelectRollAgain);
    }

    public void StartOnFloor(int floor)
    {
        _Model.Floor = floor;
        _DungeonDie.EnterFloor(_Model.Floor);
        _GamePhase.StateChange(GamePhase.SelectRollAgain);
    }

    private StateActionMap<GamePhase> _GamePhase;
    private GameSystemModel _Model;
    private DungeonDie _DungeonDie;
    private WTMK _Tools = WTMK.Instance;
    private Dood _Debug = Dood.Instance;

    private Player _Player;
    private TowerView _View;
    private bool _IsRolling;
    public GameSystem(Player player, TowerView view, DungeonDieView dungeonDieView)
    {
        _Player = player;
        _View = view;

        _GamePhase = new StateActionMap<GamePhase>();

        _GamePhase.RegisterEnter(GamePhase.Enter, OnEnter_Enter);
        _GamePhase.RegisterEnter(GamePhase.Exit, OnEnter_Exit);
        _GamePhase.RegisterEnter(GamePhase.Resolve, OnEnter_Resolve);
        _GamePhase.RegisterEnter(GamePhase.RollDungeon, OnEnter_RollDungeon);
        _GamePhase.RegisterEnter(GamePhase.RollPlayer, OnEnter_RollPlayer);
        _GamePhase.RegisterEnter(GamePhase.SelectAction, OnEnter_SelectAction);
        _GamePhase.RegisterEnter(GamePhase.SelectRollAgain, OnEnter_SelectRollAgain);

        _DungeonDie = new DungeonDie(dungeonDieView);

        _Model = new GameSystemModel();
        _Model.Floor = 0;
        _Model.Round = 0;
        _Model.CurrentDungeonRoll = DungeonDieSide.Exit;

        _View.Encounter.onClick.AddListener(OnClick_Encounter);
        _View.Trap.onClick.AddListener(OnClick_Trap);
        _View.Treasure.onClick.AddListener(OnClick_Treasure);

        _View.RollAgain.onClick.AddListener(OnClick_RollAgain);
        _View.Exit.onClick.AddListener(OnClick_Exit);

        _View.NextFloor.onClick.AddListener(OnClick_NextFloor);

        _View.DieSelection.SetActive(false);
        _View.RollAgainSelection.SetActive(false);
        _View.NextFloor.gameObject.SetActive(false);
    }

    private void OnClick_NextFloor()
    {
        _View.NextFloor.gameObject.SetActive(false);
        _View.RollAgainSelection.SetActive(false);

        _GamePhase.StateChange(GamePhase.Enter);
    }

    private void OnClick_RollAgain()
    {
        _View.RollAgainSelection.SetActive(false);
        _View.NextFloor.gameObject.SetActive(false);

        _Model.Round++;
        _View.Round.SetText($"Round {_Model.Round}");
        _Player.View.EffectText.SetText("");
        _View.DieSelection.SetActive(false);
        _IsRolling = false;

        _GamePhase.StateChange(GamePhase.RollDungeon);
    }

    private void OnClick_Exit()
    {
        _View.RollAgainSelection.SetActive(false);
        _Player.View.EffectText.SetText("");
        _GamePhase.StateChange(GamePhase.Exit);
    }

    private void OnClick_Encounter()
    {
        if(_IsRolling)
        {
            return;
        }
        _IsRolling = true;
        var outcome = _Player.Roll(DynamicDieType.Encounter);
        _Player.View.DoRoll(_Player.Dice[DynamicDieType.Encounter], DynamicDieType.Encounter, outcome, OnRollComplete);
    }

    private void OnClick_Trap()
    {
        if (_IsRolling)
        {
            return;
        }
        _IsRolling = true;
        var outcome = _Player.Roll(DynamicDieType.Trap);
        _Player.View.DoRoll(_Player.Dice[DynamicDieType.Trap], DynamicDieType.Trap, outcome, OnRollComplete);
    }

    private void OnClick_Treasure()
    {
        if (_IsRolling)
        {
            return;
        }
        _IsRolling = true;
        var outcome = _Player.Roll(DynamicDieType.Treasure);
        _Player.View.DoRoll(_Player.Dice[DynamicDieType.Treasure], DynamicDieType.Treasure, outcome, OnRollComplete);
    }

    private void OnRollComplete()
    {
        _GamePhase.StateChange(GamePhase.Resolve);
    }

    private void OnEnter_Enter()
    {
        _Debug.Log("Enter");

        _Model.Floor++;

        if(_Model.Floor > _Player.Model.HighestFloor)
        {
            _Player.Model.HighestFloor = _Model.Floor;
        }

        _Model.Round = 0;

        _View.Floor.SetText($"Floor {_Model.Floor}");
        _View.Round.SetText($"Round {_Model.Round}");
        _View.CurrentDie.SetText("");
        

        _DungeonDie.EnterFloor(_Model.Floor);
        _GamePhase.StateChange(GamePhase.RollDungeon);
    }

    private void OnEnter_Exit()
    {
        _Debug.Log("OnEnter_Exit");
        OnExit?.Invoke();
    }

    private void OnEnter_RollDungeon()
    {
        _Debug.Log("OnEnter_RollDungeon");
        _Model.CurrentDungeonRoll = _DungeonDie.Roll(OnDungeonRollComplete);
    }

    private void OnDungeonRollComplete()
    {
        _Debug.Log("Dungeon rolled " + _Model.CurrentDungeonRoll);
        _View.CurrentDie.SetText(_Model.CurrentDungeonRoll.ToString());

        _GamePhase.StateChange(GamePhase.RollPlayer);
    }

    private void OnEnter_RollPlayer()
    {
        _Debug.Log("OnEnter_RollPlayer");

        switch (_Model.CurrentDungeonRoll)
        {
            case DungeonDieSide.Wild:
                _GamePhase.StateChange(GamePhase.SelectAction);
                break;
            case DungeonDieSide.Exit:
                _View.NextFloor.gameObject.SetActive(true);
                _GamePhase.StateChange(GamePhase.SelectRollAgain);
                break;
            case DungeonDieSide.Encounter:
                _View.DieSelection.SetActive(true);
                _View.Encounter.gameObject.SetActive(true);
                _View.Trap.gameObject.SetActive(false);
                _View.Treasure.gameObject.SetActive(false);
                break;
            case DungeonDieSide.Trap:
                _View.DieSelection.SetActive(true);
                _View.Encounter.gameObject.SetActive(false);
                _View.Trap.gameObject.SetActive(true);
                _View.Treasure.gameObject.SetActive(false);
                break;
            case DungeonDieSide.Treasure:
                _View.DieSelection.SetActive(true);
                _View.Encounter.gameObject.SetActive(false);
                _View.Trap.gameObject.SetActive(false);
                _View.Treasure.gameObject.SetActive(true);
                break;
            default:
                _Player.Roll(_Model.CurrentDungeonRoll);
                _GamePhase.StateChange(GamePhase.Resolve);
                break;
        }
    }

    private void OnEnter_Resolve()
    {
        _Debug.Log("OnEnter_Resolve");

        string effect = _Player.CurrentFace.DoEffect(_Player.Model);

        _Player.View.EffectText.SetText(effect);

        _View.Blood.SetText($"Blood {_Player.Model.Blood}");
        _View.Gold.SetText($"Gold {_Player.Model.Gold}");

        _Debug.Log("Player rolled " + _Player.CurrentFace.IsPermanent);

        if (_Player.Model.Blood >= 0)
        {
            _GamePhase.StateChange(GamePhase.SelectRollAgain);
        }
        else
        {
            _GamePhase.StateChange(GamePhase.Exit);
        }
    }

    private void OnEnter_SelectAction()
    {
        _Debug.Log("OnEnter_SelectAction");
        _View.DieSelection.SetActive(true);
    }

    private void OnEnter_SelectRollAgain()
    {
        _Debug.Log("OnEnter_SelectRollAgain");
        _View.RollAgainSelection.SetActive(true);
    }
}

public class GameSystemModel
{
    public int Floor;
    public int Round;
    public DungeonDieSide CurrentDungeonRoll;

}

public enum GamePhase
{
    Enter,
    Exit,
    RollDungeon,
    RollPlayer,
    Resolve,
    SelectAction,
    SelectRollAgain
}


/*
public class CarSystem 
{
    //public class CarSystem
}

public class Car
{
    int MaxGear { get; set; }
    int CurrentGear { get; set; }
    int Acceleration { get; set; }
    int TopSpeed { get; set; }
    int CurrentSpeed { get; set; }

    float BodyDamage { get; set; }
    float BreakDamage { get; set; }
    float TireDamage { get; set; }
}
*/