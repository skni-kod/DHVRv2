using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RoboRyanTron.Variables;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ScoreManager : MonoBehaviour {
    private const string keyPrefix = "Score";

    public int _keepedScoreCount = 10;
    public TMP_Text _scoreListText;
    
    private int _currentScore;

    private List<ScoreData> _scoresData = new List<ScoreData>();
    
    
    private void Awake() {
        DuckController.OnDuckDeath += OnDuckKill;
    }

    private void Start() {
        LoadData();
    }

    void OnDuckKill(DuckController duck) {
        _currentScore += duck.GetScore();
    }

    void LoadData() {
        for (int i = 0; i < _keepedScoreCount; i++) {
            var key = keyPrefix + i;

            if (PlayerPrefs.HasKey(key)) {
                var str = PlayerPrefs.GetString(key);
                var data = JsonUtility.FromJson<ScoreData>(str);
                
                _scoresData.Add(data);
            }
            else {
                _scoresData.Add(new ScoreData());
            }
        }
        
        UpdateText();
    }

    public void ResetScore() {
        _currentScore = 0;
    }

    void UpdateText() {
        _scoresData = _scoresData.OrderByDescending(d => d.score).ToList();
        
        var builder = new StringBuilder();
        for (int i = 0; i < _keepedScoreCount; i++) {
            builder.Append($"{i + 1}. Score: {_scoresData[i].score}\n");
        }

        _scoreListText.text = builder.ToString();
    }

    public void SaveScore() {
        var scoreData = new ScoreData() {
            name = "test1",
            score = _currentScore,
        };
        
        _scoresData.Add(scoreData);
        _scoresData = _scoresData.OrderByDescending(d => d.score).ToList();

        for (int i = 0; i < _keepedScoreCount; i++) {
            var key = keyPrefix + i;

            var json = JsonUtility.ToJson(_scoresData[i]);
        
            //Debug.Log($"Saving: {json}");
        
            PlayerPrefs.SetString(key, json);
        }

        PlayerPrefs.Save();
        UpdateText();
    }
    
    [Serializable]
    struct ScoreData {
        public string name;
        public int score;
    }
}
