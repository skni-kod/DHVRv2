using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameEndHandler : MonoBehaviour {
    public ParticleSystem[] _fireworks;
    public GameObject[] _friedDucks;

    public void GameWon() {
        for (int i = 0; i < _fireworks.Length; i++) {
            _fireworks[i].Play();
        }

        for (int i = 0; i < _friedDucks.Length; i++) {
            _friedDucks[i].SetActive(true);
        }
    }

    public void GameLost() {

        //Do something I guess...?
    }

    public void GameReset() {
        for (int i = 0; i < _fireworks.Length; i++) {
            _fireworks[i].Stop();
        }

        for (int i = 0; i < _friedDucks.Length; i++) {
            _friedDucks[i].SetActive(false);
        }
    }
}