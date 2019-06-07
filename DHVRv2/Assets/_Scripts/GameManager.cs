using System.Collections;
using System.Collections.Generic;
using RoboRyanTron.Events;
using TMPro;
using UnityEngine;

public class GameManager : MonoBehaviour {

    [Header("Events")]
    public GameEvent _OnGameReset;
    public GameEvent _OnGameStart;
    public GameEvent _OnGameWin;
    public GameEvent _OnGameLost;
    public GameEvent _OnGameBackToIdle;

    [Header("Other")]
    public DuckSpawner _spawner;
    public float _timeBetweenSpawns;

    public GameStateVariable _gameState;

    public List<DifficultyData> _difficulties;
    public DifficultyData CurrentDifficulty => _difficulties[_currentDifficultyIndex];
    public TMP_Text _infoText;

    private int _currentDifficultyIndex;

    int _numberOfDuckToSpawn;
    int _ducksKilledThisRound;
    int _ducksMissedThisRound;

    ScoreManager _scoreManager;

    List<DuckController> _spawnedDucks = new List<DuckController>();

    private void Awake() {
        DuckController.OnDuckDeath += OnDuckDeath;
        DuckController.OnDuckFlee += OnDuckFlee;

        _scoreManager = GetComponent<ScoreManager>();
    }

    void Start() {
        _gameState.gameState = GameState.Idle;
        UpdateInfoText();
    }

    public void StartGame() {
        if (_gameState.gameState == GameState.Idle) {
            StartCoroutine(GameRoutine());

        } else if (_gameState.gameState == GameState.MainGame) {
            _gameState.gameState = GameState.Idle;
            StopAllCoroutines();

            for (int i = 0; i < _spawnedDucks.Count; i++) {
                Destroy(_spawnedDucks[i].gameObject);
            }
            _spawnedDucks.Clear();

            _OnGameReset.Raise();
        }

        if (_gameState.gameState == GameState.Win || _gameState.gameState == GameState.Loose) {
            _OnGameBackToIdle.Raise();
            _gameState.gameState = GameState.Idle;

            _ducksKilledThisRound = 0;
            _ducksMissedThisRound = 0;
        }
    }

    void Update() {

#if UNITY_EDITOR
        if (Input.GetKeyDown(KeyCode.Space)) {
            if (_spawnedDucks.Count > 0) {
                _spawnedDucks[0].Damage(_spawnedDucks[0]._maxHealth);
            }
        }
#endif

    }

    IEnumerator GameRoutine() {

        // Wait for game preparation to end
        //yield return new WaitUntil(() => _gameState.gameState == GameState.MainGame);

        _gameState.gameState = GameState.MainGame;
        _OnGameStart.Raise();

        int waveCount = CurrentDifficulty.wavesDifficulty.Count;
        _ducksKilledThisRound = 0;

        bool lost = false;

        for (int j = 0; j < waveCount; j++) {
            var currentWaveData = CurrentDifficulty.wavesDifficulty[j];

            var(duckSpeed, controlPoints, ducksCount) = CurrentDifficulty.GetDifficultyDataTuple(j);

            // Spawn duck group
            for (int i = 0; i < ducksCount; i++) {
                var duck = _spawner.Spawn(duckSpeed, controlPoints);
                _spawnedDucks.Add(duck);

                _numberOfDuckToSpawn--;

                if (_numberOfDuckToSpawn == 0)
                    break;
            }

            // wait until all ducks fleed or destroyed
            yield return new WaitUntil(() => _spawnedDucks.Count == 0);

            if (_numberOfDuckToSpawn == 0) {
                // game won
                break;
            }

            // check if lost game
            if (_ducksMissedThisRound >= CurrentDifficulty.maxMissedDucks) {
                lost = true;
                break;
            }

            yield return new WaitForSeconds(_timeBetweenSpawns);
        }

        // Determine if player win or lost
        if (lost) {
            Debug.Log("Game Lost");
            _OnGameLost.Raise();

            _gameState.gameState = GameState.Loose;
        } else {
            // Won Game
            // Spawn Some nice particles
            // and duck roasting on bonfire
            Debug.Log("Game Won");
            _OnGameWin.Raise();

            _gameState.gameState = GameState.Win;
        }

        // Back to idle state and end routine
        //_gameState.gameState = GameState.Idle;
    }

    void UpdateInfoText() {
        _infoText.text = string.Format("Points: {0}\nDucks Hunted: {1}\nDucks Missed: {2}", _scoreManager.GetScore(), _ducksKilledThisRound, _ducksMissedThisRound);
    }

    void OnDuckDeath(DuckController duck) {
        _spawnedDucks.Remove(duck);

        _ducksKilledThisRound++;
        UpdateInfoText();
    }

    void OnDuckFlee(DuckController duck) {
        _spawnedDucks.Remove(duck);

        _ducksMissedThisRound++;
        UpdateInfoText();

    }
}