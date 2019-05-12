using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PathCreation;
public class DuckSpawner : MonoBehaviour
{
    public GameObject duckPrefab;
    private GameObject duck;
    public VertexPath path;
    public float min_height;
    public float max_height;
    public float min_length;
    public float max_length;
    public float min_width;
    public float max_width;

    public void Spawn()
    {
        var positionArray = new Vector3[6];
        positionArray[0] = new Vector3(Random.Range(-min_length, max_length), Random.Range(-min_height, max_height), Random.Range(-min_width, max_width));
        positionArray[1] = new Vector3(Random.Range(-min_length, max_length), Random.Range(-min_height, max_height), Random.Range(-min_width, max_width));
        positionArray[2] = new Vector3(Random.Range(-min_length, max_length), Random.Range(-min_height, max_height), Random.Range(-min_width, max_width));
        positionArray[3] = new Vector3(Random.Range(-min_length, max_length), Random.Range(-min_height, max_height), Random.Range(-min_width, max_width));
        positionArray[4] = new Vector3(Random.Range(-min_length, max_length), Random.Range(-min_height, max_height), Random.Range(-min_width, max_width));
        positionArray[5] = new Vector3(Random.Range(-min_length, max_length), Random.Range(-min_height, max_height), Random.Range(-min_width, max_width));
        BezierPath bezierPath = new BezierPath(positionArray, false, PathSpace.xyz);
        path = new VertexPath(bezierPath);
        duck = Instantiate(duckPrefab, positionArray[0], Quaternion.identity);
        duck.GetComponent<DuckMovement>().path = path;   
    }

    // Start is called before the first frame update
    void Start()
    {
        Spawn();
    }
    void Update()
    {
        if (Mathf.Abs(path.vertices[path.NumVertices - 1].x - duck.transform.position.x)  <= 0.1 && Mathf.Abs(path.vertices[path.NumVertices - 1].y - duck.transform.position.y) <= 0.1 && Mathf.Abs(path.vertices[path.NumVertices - 1].z - duck.transform.position.z) <= 0.1)
        {
            Destroy(duck);
            Destroy(this);
        }

    }
}
