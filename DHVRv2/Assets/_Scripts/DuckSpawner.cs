using System.Collections;
using System.Collections.Generic;
using PathCreation;
using UnityEngine;
public class DuckSpawner : MonoBehaviour {
    public DuckController _duckPrefab;
    public Vector3 _duckMovementBox;
    public int _pathControlPointCount;
    public float _duckSpeed;

    public void Spawn() {
        //Generating duck path
        var positionArray = new Vector3[_pathControlPointCount];
        for (int i = 0; i < _pathControlPointCount; i++) {
            positionArray[i] = new Vector3(Random.Range(-_duckMovementBox.x, _duckMovementBox.x),
                Random.Range(-_duckMovementBox.y, _duckMovementBox.x),
                Random.Range(-_duckMovementBox.z, _duckMovementBox.z)) + transform.position;
        }

        BezierPath bezierPath = new BezierPath(positionArray, false, PathSpace.xyz);
        var path = new VertexPath(bezierPath);
        //Duck creation with generated path
        var duck = Instantiate(_duckPrefab, positionArray[0], Quaternion.identity);
        duck.Initialize(_duckSpeed, path);
    }

    void Start() {
        Spawn();
    }

    private void OnDrawGizmosSelected() {
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireCube(transform.position, _duckMovementBox);
    }
}