using System.Collections;
using System.Collections.Generic;
using PathCreation;
using UnityEngine;

public class DuckMovement : MonoBehaviour {

    public float _deathDstToEndPoint;
    VertexPath _path;
    float _distanceTravelled;
    float _speed;

    Vector3 _endPoint;

    DuckHealth _health;

    void Awake() {
        _health = GetComponent<DuckHealth>();
    }

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

        if (Vector3.SqrMagnitude(_endPoint - transform.position) <= _deathDstToEndPoint * _deathDstToEndPoint) {
            // Just for now, change to Flee later
            _health.Death();
        }
    }

    void OnDrawGizmos() {
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
}