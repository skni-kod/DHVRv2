using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {

    public DuckSpawner _spawner;

    public GameStateVariable _gameState;

    public int _ducksCount;
    public int _ducksPerGroup;

    public int _numberOfDuckToSpawn;

    List<DuckController> _spawnedDucks;

    private void Awake() {
        DuckController.OnDuckDeath += OnDuckDeath;
    }

    void Start() {
        // Change it later to button callback
        StartCoroutine(GameRoutine());
        _gameState.gameState = GameState.Idle;
    }

    IEnumerator GameRoutine() {

        // Wait for game preparation to end
        yield return new WaitUntil(() => _gameState.gameState == GameState.MainGame);

        _numberOfDuckToSpawn = _ducksCount;

        while (_numberOfDuckToSpawn > 0) {

            // Spawn duck group
            for (int i = 0; i < _ducksPerGroup; i++) {
                var duck = _spawner.Spawn();
                _spawnedDucks.Add(duck);

                _numberOfDuckToSpawn--;

                if (_numberOfDuckToSpawn == 0)
                    break;
            }

            // wait until all ducks fleed or destroyed
            yield return new WaitUntil(() => _spawnedDucks.Count > 0);
        }

        // Determine if player win or lost

        // Back to idle state and end routine
        _gameState.gameState = GameState.Idle;
    }

    void OnDuckDeath(DuckController duck) {
        _spawnedDucks.Remove(duck);
    }

    void OnDuckFlee(DuckController duck) {
        _spawnedDucks.Remove(duck);
    }
}