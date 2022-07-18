using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameSystemTest : MonoBehaviour
{
    [SerializeField]
    private StartView _StartView;

    [SerializeField]
    private TavernView _TavernView;

    [SerializeField]
    private TowerView _TowerView;

    [SerializeField]
    private GameOverView _GameOverView;

    [SerializeField]
    private DungeonDieView _DungeonDieView;

    [SerializeField]
    private PlayerView _PlayerView;

    [SerializeField]
    private List<AudioSource> _AudioSources;
    [SerializeField]
    private AudioClip _StartScreenLoop, _GameOverLoop, _TavernLoop;
    [SerializeField]
    private List<AudioClip> _TowerScreenLoops;

    private DynamicDieFactory _DieFactory;
    private GameSystem _GameSystem;
    private Player _Player;

    private SoundManager _SoundManager;
    private Dood _Debug = Dood.Instance;
    private WTMK _Tools = WTMK.Instance;

    public void Awake()
    {
        _SoundManager = new SoundManager(_AudioSources);
        _DieFactory = new DynamicDieFactory();
        _Player = new Player(_DieFactory.BuildStartingDie(DynamicDieType.Encounter), 
                                _DieFactory.BuildStartingDie(DynamicDieType.Trap),
                                _DieFactory.BuildStartingDie(DynamicDieType.Treasure),
                                new PlayerStats(), _PlayerView);

        _Player.Model.Blood = 9;
        _Player.Model.Gold = 0;

        _GameSystem = new GameSystem(_Player, _TowerView, _DungeonDieView);

        Dood.IsLogging = false;

        _TavernView.EnterTower.onClick.AddListener(OnClick_EnterTower);
        _TowerView.Exit.onClick.AddListener(OnClick_ExitTower);
        _StartView.StartGame.onClick.AddListener(OnClick_StartGame);
        _GameOverView.Restart.onClick.AddListener(OnClick_Restart);

        _TowerView.gameObject.SetActive(false);
        _TavernView.gameObject.SetActive(false);
        _GameOverView.gameObject.SetActive(false);
        _StartView.gameObject.SetActive(true);
    }

    private void OnClick_Restart()
    {
        _Player.Model.Blood = 9;
        _Player.Model.Gold = 0;

        _GameOverView.gameObject.SetActive(false);
        _StartView.gameObject.SetActive(true);

        _SoundManager.PlayOnMainAudio(_StartScreenLoop, 27f, true);
    }

    private void OnClick_StartGame()
    {
        _TavernView.Enter(_Player);
        _StartView.gameObject.SetActive(false);

        _SoundManager.PlayOnMainAudio(_TavernLoop, 27f, true);
    }

    private void OnClick_ExitTower()
    {
        if(_Player.Model.Gold < 0)
        {
            _Player.Model.Blood -= Mathf.Abs(_Player.Model.Gold);
            _Player.Model.Gold = 0;
        }

        if(_Player.Model.Blood < 0)
        {
            _GameOverView.gameObject.SetActive(true);
            _SoundManager.PlayOnMainAudio(_GameOverLoop, 27f, true);
        }
        else
        {
            _Player.Model.Blood = 9;
            _TowerView.gameObject.SetActive(false);
            _SoundManager.PlayOnMainAudio(_TavernLoop, 27f, true);
        }

        _TavernView.Enter(_Player);
    }

    private void OnClick_EnterTower()
    {
        _TavernView.gameObject.SetActive(false);
        _TowerView.gameObject.SetActive(true);

        _GameSystem.Start();

        int roll = _Tools.Pick(_TowerScreenLoops.Count);
        _SoundManager.PlayOnMainAudio(_TowerScreenLoops[roll], 27f, true);
    }

    public void Start()
    {
        _SoundManager.PlayOnMainAudio(_StartScreenLoop, 27f, true);
    }
}
