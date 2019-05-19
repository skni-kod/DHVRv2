using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {

    public bool _debugFeatures;

    public DuckSpawner _spawner;

    public GameStateVariable _gameState;

    public List<DifficultyData> _difficulties;
    public DifficultyData CurrentDifficulty => _difficulties[_currentDifficultyIndex];

    private int _currentDifficultyIndex;

    int _numberOfDuckToSpawn;
    int _ducksKilledThisRound;

    List<DuckController> _spawnedDucks = new List<DuckController>();

    private void Awake() {
        DuckController.OnDuckDeath += OnDuckDeath;
        DuckController.OnDuckFlee += OnDuckFlee;

        if (_debugFeatures) {
            Debug.LogWarning("DEBUG FEATURES ACTIVE");
        }
    }

    void Start() {
        // Change it later to button callback
        StartCoroutine(GameRoutine());
        _gameState.gameState = GameState.Idle;
    }

    void Update() {
        if (_debugFeatures) {
            if (Input.GetKeyDown(KeyCode.Space)) {
                if (_spawnedDucks.Count > 0) {
                    _spawnedDucks[0].Damage(_spawnedDucks[0]._maxHealth);
                }
            }
        }

    }

    IEnumerator GameRoutine() {

        // Wait for game preparation to end
        //yield return new WaitUntil(() => _gameState.gameState == GameState.MainGame);

        int waveCount = CurrentDifficulty.wavesDifficulty.Count;
        _ducksKilledThisRound = 0;

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
        }

        // Determine if player win or lost
        if (_ducksKilledThisRound >= CurrentDifficulty.ducksToWinGame) {
            // Won Game
            // Spawn Some nice particles
            // and duck roasting on bonfire
            Debug.Log("Game Won");
        } else {
            Debug.Log("Game Lost");
        }

        // Back to idle state and end routine
        _gameState.gameState = GameState.Idle;
    }

    void OnDuckDeath(DuckController duck) {
        _spawnedDucks.Remove(duck);

        _ducksKilledThisRound++;
    }

    void OnDuckFlee(DuckController duck) {
        _spawnedDucks.Remove(duck);
    }
}