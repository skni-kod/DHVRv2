using System.Collections;
using System.Collections.Generic;
using PathCreation;
using RoboRyanTron.Events;
using UnityEngine;
using UnityEngine.Events;

public class DuckController : Damageable {

    public static event System.Action<DuckController> OnDuckDeath;
    public static event System.Action<DuckController> OnDuckFlee;
    public UnityEvent _OnDuckFlee;

    public ParticleSystem _startToFleeParticle;
    [Range(0, 1)]
    public float _startToFleePercent;

    public Vector2 _scoreMinMax;
    public Vector2 _scoreDistThreshold;

    public TMPro.TMP_Text _scoreText;

    VertexPath _path;
    float _distanceTravelled;
    float _speed;

    Vector3 _endPoint;

    // Just for debug
    BezierPath _bezier;

    public void Initialize(float speed, VertexPath path, BezierPath bezier = null) {
        _speed = speed;
        _path = path;

        _endPoint = path.vertices[path.NumVertices - 1];
        _bezier = bezier;
    }

    void Update() {
        _distanceTravelled += _speed * Time.deltaTime;

        var travelPercent = _distanceTravelled / _path.length;
        if (travelPercent > _startToFleePercent) {
            _startToFleeParticle.gameObject.SetActive(true);
        }

        if (travelPercent >= 1f) {
            OnDuckFlee?.Invoke(this);
            _OnDuckFlee.Invoke();

            Destroy(gameObject);
            return;
        }

        transform.position = _path.GetPointAtDistance(_distanceTravelled);
        var forward = _path.GetDirectionAtDistance(_distanceTravelled);
        var rot = Quaternion.LookRotation(forward, Vector3.up);

        transform.rotation = rot;
    }

    void OnDrawGizmos() {
        if (_path == null)
            return;

        Gizmos.color = Color.green;
        const float resolution = 0.01f;
        int stepCount = Mathf.RoundToInt(1 / resolution);
        float percent = 0.0f;
        var previousPoint = _path.GetPoint(percent);

        for (int i = 1; i < stepCount; i++) {
            percent = i * resolution;
            var nextPoint = _path.GetPoint(percent);

            Gizmos.DrawLine(previousPoint, nextPoint);
            previousPoint = nextPoint;
        }

        if (_bezier != null) {
            for (int i = 0; i < _bezier.NumPoints; i++) {
                if (i % 4 == 0) {
                    Gizmos.color = Color.red;
                    Gizmos.DrawWireSphere(_bezier[i], 0.1f);
                }
            }
        }
    }

    public int GetScore() {
        var dstPercent = _distanceTravelled / _path.length;
        float t = 0;
        if (dstPercent < _scoreDistThreshold.x) {
            t = 1;
        } else if (dstPercent > _scoreDistThreshold.y) {
            t = 0;
        } else {
            t = Mathf.Lerp(_scoreDistThreshold.y, _scoreDistThreshold.x, _distanceTravelled / _path.length);
        }

        int score = Mathf.RoundToInt(Mathf.Lerp(_scoreMinMax.x, _scoreMinMax.y, t));

        return score;
    }

    public override void Death() {
        OnDuckDeath?.Invoke(this);

        _scoreText.transform.SetParent(null, true);
        _scoreText.transform.rotation = Quaternion.identity;
        _scoreText.transform.position = transform.position;

        _scoreText.gameObject.SetActive(true);
        _scoreText.text = GetScore().ToString();

        Destroy(_scoreText.gameObject, 3f);

        base.Death();
    }
}