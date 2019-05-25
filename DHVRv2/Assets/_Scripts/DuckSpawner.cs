using System.Collections;
using System.Collections.Generic;
using PathCreation;
using UnityEngine;
public class DuckSpawner : MonoBehaviour {
    public DuckController _duckPrefab;
    public Vector3 _duckMovementBox;
    public Transform _spawnPosition;
    public Vector3 _spawnBoxSize;
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
        duck.Initialize(duckSpeed, path, bezierPath);

        //Debug.Log(path.cumulativeLengthAtEachVertex[path.NumVertices - 1]);

        return duck;
    }

    Vector3[] CreatePath(int pathLength) {
        var path = new Vector3[pathLength];
        Bounds box = new Bounds(transform.position, _duckMovementBox);
        var segmentLength = _pathDistance / pathLength;

        // random starting point
        // path[0] = new Vector3(Random.Range(-_duckMovementBox.x, _duckMovementBox.x),
        //     Random.Range(-_duckMovementBox.y, _duckMovementBox.y),
        //     Random.Range(-_duckMovementBox.z, _duckMovementBox.z)) / 2f + transform.position;

        // First Path point inside Spawn Box
        var startX = Random.Range(-_spawnBoxSize.x, _spawnBoxSize.x);
        var startY = Random.Range(-_spawnBoxSize.y, _spawnBoxSize.y);
        var startZ = Random.Range(-_spawnBoxSize.z, _spawnBoxSize.z);

        path[0] = new Vector3(startX, startY, startZ) / 2f + _spawnPosition.position;

        // Second point on edge of movement box, so lengths wont't be so different        
        startX = Mathf.Clamp(startX, -_duckMovementBox.x, _duckMovementBox.x);
        startZ = Mathf.Clamp(startX, -_duckMovementBox.z, _duckMovementBox.z);

        path[1] = new Vector3(startX, 0, startZ) / 2f + transform.position;

        for (int i = 2; i < pathLength; i++) {
            // find next direction
            var dir = Random.onUnitSphere;

            int sanityCounter = 0;
            // check it until it lands inside box
            var point = path[i - 1] + dir * segmentLength;
            while (!box.Contains(point)) {
                dir = Random.onUnitSphere;
                point = path[i - 1] + dir * segmentLength;

                sanityCounter++;
                if(sanityCounter > 25)
                    throw new System.Exception("PATRZ CHOLERA CO PISZESZ!!!");
            }

            path[i] = point;
        }

        return path;
    }

    private void OnDrawGizmosSelected() {
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireCube(transform.position, _duckMovementBox);

        if (_spawnPosition != null) {
            Gizmos.color = Color.green;
            Gizmos.DrawWireCube(_spawnPosition.position, _spawnBoxSize);
        }
    }
}