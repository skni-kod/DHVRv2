using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class DifficultyData : ScriptableObject {
    public int ducksPerGroup;
    public float duckBaseSpeed;
    public int baseNumberOfControlPoints;
    public int maxMissedDucks;
    public List<WaveData> wavesDifficulty;

    [System.Serializable]
    public class WaveData {
        public float duckSpeedDelta;
        public int controlPointsDelta;
        public int additionalDucks;

    }

    public(float, int, int) GetDifficultyDataTuple(int waveIndex) {
        var diff = wavesDifficulty[waveIndex];

        var duckSpeed = diff.duckSpeedDelta + duckBaseSpeed;
        var controlPoints = diff.controlPointsDelta + baseNumberOfControlPoints;
        var ducks = diff.additionalDucks + ducksPerGroup;

        return (duckSpeed, controlPoints, ducks);
    }
}