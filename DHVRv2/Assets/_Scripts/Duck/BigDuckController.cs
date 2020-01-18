using System.Collections;
using System.Collections.Generic;
using PathCreation;
using RoboRyanTron.Events;
using UnityEngine;
using UnityEngine.Events;

public class BigDuckController : Damageable
{

    public static event System.Action<BigDuckController> OnBigDuckDeath;

    [Range(0, 1)]
    public float _startToFleePercent;

    public int _score;

    public TMPro.TMP_Text _scoreText;

    VertexPath _path;
    float _distanceTravelled;
    float _speed;

    bool _direction = true;


    Vector3 _endPoint;

    // Just for debug
    BezierPath _bezier;

    public void Initialize(float speed, VertexPath path, BezierPath bezier = null)
    {
        _speed = speed;
        _path = path;

        _endPoint = path.vertices[path.NumVertices - 1];
        _bezier = bezier;
    }

    void Update()
    {
        float actualSpeed = _direction ? _speed : -_speed;
        _distanceTravelled += actualSpeed * Time.deltaTime;

        var travelPercent = _distanceTravelled / _path.length;

        if (travelPercent >= 1f || travelPercent <= 0.0f)
        {
            _direction = !_direction;
        }

        transform.position = _path.GetPointAtDistance(_distanceTravelled);
        var forward = _path.GetDirectionAtDistance(_distanceTravelled);
        var rot = Quaternion.LookRotation(_direction ? forward : -forward, Vector3.up);

        transform.rotation = rot;
    }

    void OnDrawGizmos()
    {
        if (_path == null)
            return;

        Gizmos.color = Color.green;
        const float resolution = 0.01f;
        int stepCount = Mathf.RoundToInt(1 / resolution);
        float percent = 0.0f;
        var previousPoint = _path.GetPoint(percent);

        for (int i = 1; i < stepCount; i++)
        {
            percent = i * resolution;
            var nextPoint = _path.GetPoint(percent);

            Gizmos.DrawLine(previousPoint, nextPoint);
            previousPoint = nextPoint;
        }

        if (_bezier != null)
        {
            for (int i = 0; i < _bezier.NumPoints; i++)
            {
                if (i % 4 == 0)
                {
                    Gizmos.color = Color.red;
                    Gizmos.DrawWireSphere(_bezier[i], 0.1f);
                }
            }
        }
    }

    public int GetScore()
    {
        return _score;
    }

    public override void Death()
    {
        OnBigDuckDeath?.Invoke(this);

        _scoreText.transform.SetParent(null, true);
        _scoreText.transform.rotation = Quaternion.identity;
        _scoreText.transform.position = transform.position;

        _scoreText.gameObject.SetActive(true);
        _scoreText.text = GetScore().ToString();

        OnBigDuckDeath?.Invoke(this);

        Destroy(_scoreText.gameObject, 3f);

        base.Death();
    }
}