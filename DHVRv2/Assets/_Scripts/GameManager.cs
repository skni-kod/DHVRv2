using System.Collections;
using System.Collections.Generic;
using RoboRyanTron.Events;
using UnityEngine;

public class GameManager : MonoBehaviour {

    [Header("Events")]
    public GameEvent _OnGameReset;
    public GameEvent _OnGameStart;
    public GameEvent _OnGameWin;
    public GameEvent _OnGameLost;

    [Header("Other")]
    public DuckSpawner _spawner;

    public GameStateVariable _gameState;

    public List<DifficultyData> _difficulties;
    public DifficultyData CurrentDifficulty => _difficulties[_currentDifficultyIndex];

    private int _currentDifficultyIndex;

    int _numberOfDuckToSpawn;
    int _ducksKilledThisRound;
    int _ducksMissedThisRound;

    List<DuckController> _spawnedDucks = new List<DuckController>();

    private void Awake() {
        DuckController.OnDuckDeath += OnDuckDeath;
        DuckController.OnDuckFlee += OnDuckFlee;
    }

    void Start() {
        _gameState.gameState = GameState.Idle;

    }

    public void StartGame() {
        if (_gameState.gameState == GameState.Idle) {
            StartCoroutine(GameRoutine());
        } else if (_gameState.gameState == GameState.MainGame) {
            _gameState.gameState = GameState.Idle;
            _OnGameReset.Raise();
        } else {
            _gameState.gameState = GameState.Idle;
            _OnGameReset.Raise();
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

            // check if lost game
            if (_ducksMissedThisRound >= CurrentDifficulty.maxMissedDucks) {
                lost = true;
                break;
            }
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

    void OnDuckDeath(DuckController duck) {
        _spawnedDucks.Remove(duck);

        _ducksKilledThisRound++;
    }

    void OnDuckFlee(DuckController duck) {
        _spawnedDucks.Remove(duck);

        _ducksMissedThisRound++;
    }
}