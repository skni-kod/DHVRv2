using System.Collections;
using System.Collections.Generic;
using PathCreation;
using RoboRyanTron.Events;
using UnityEngine;

public class DuckController : Damageable {

    public float _fleeDstToEndPoint;
    public static event System.Action<DuckController> OnDuckDeath;
    public static event System.Action<DuckController> OnDuckFlee;

    public Vector2 _scoreMinMax;
    public Vector2 _scoreDistThreshold;

    VertexPath _path;
    float _distanceTravelled;
    float _speed;

    Vector3 _endPoint;

    public void Initialize(float speed, VertexPath path) {
        _speed = speed;
        _path = path;

        _endPoint = path.vertices[path.NumVertices - 1];
    }

    void Update() {
        _distanceTravelled += _speed * Time.deltaTime;
        transform.position = _path.GetPointAtDistance(_distanceTravelled);

        var forward = _path.GetDirectionAtDistance(_distanceTravelled);
        var rot = Quaternion.LookRotation(forward, Vector3.up);

        transform.rotation = rot;

        if (Vector3.SqrMagnitude(_endPoint - transform.position) <= _fleeDstToEndPoint * _fleeDstToEndPoint) {
            OnDuckFlee?.Invoke(this);

            Destroy(gameObject);
        }
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
    }

    public int GetScore() {
        var t = Mathf.Lerp(_scoreDistThreshold.x, _scoreDistThreshold.y, _distanceTravelled / _path.length);
        int score = Mathf.RoundToInt(Mathf.Lerp(_scoreMinMax.x, _scoreMinMax.y, t));

        return score;
    }

    public override void Death() {
        OnDuckDeath?.Invoke(this);

        base.Death();
    }
}