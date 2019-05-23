using System.Collections;
using System.Collections.Generic;
using PathCreation;
using UnityEngine;
public class DuckSpawner : MonoBehaviour {
    public DuckController _duckPrefab;
    public Vector3 _duckMovementBox;
    public float _pathDistance;

    void Update() {
        if (Input.GetKeyDown(KeyCode.P)) {
            Spawn(4, 10);
        }
    }
    public DuckController Spawn(float duckSpeed, int controlPointsCount) {
        //Generating duck path
        var positionArray = CreatePath(controlPointsCount);

        BezierPath bezierPath = new BezierPath(positionArray, false, PathSpace.xyz);
        var path = new VertexPath(bezierPath);
        //Duck creation with generated path
        var duck = Instantiate(_duckPrefab, positionArray[0], Quaternion.identity);
        duck.Initialize(duckSpeed, path);

        //Debug.Log(path.cumulativeLengthAtEachVertex[path.NumVertices - 1]);

        return duck;
    }

    Vector3[] CreatePath(int pathLength) {
        var path = new Vector3[pathLength];
        Bounds box = new Bounds(transform.position, _duckMovementBox);
        var segmentLength = _pathDistance / pathLength;

        // random starting point
        path[0] = new Vector3(Random.Range(-_duckMovementBox.x, _duckMovementBox.x),
            Random.Range(-_duckMovementBox.y, _duckMovementBox.y),
            Random.Range(-_duckMovementBox.z, _duckMovementBox.z)) / 2f + transform.position;

        for (int i = 1; i < pathLength; i++) {
            // find next direction
            var dir = Random.onUnitSphere;

            // check it until it lands inside box
            var point = path[i - 1] + dir * segmentLength;
            while (!box.Contains(point)) {
                dir = Random.onUnitSphere;
                point = path[i - 1] + dir * segmentLength;
            }

            path[i] = point;
        }

        return path;
    }

    private void OnDrawGizmosSelected() {
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireCube(transform.position, _duckMovementBox);
    }
}