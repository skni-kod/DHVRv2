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
    public int speed;
    private Vector3 x;

    public void Spawn()
    {
        //Generating duck path
        var positionArray = new Vector3[6];
        positionArray[0] = new Vector3(Random.Range(-min_length, max_length), Random.Range(-min_height, max_height), Random.Range(-min_width, max_width));
        positionArray[1] = new Vector3(Random.Range(-min_length, max_length), Random.Range(-min_height, max_height), Random.Range(-min_width, max_width));
        positionArray[2] = new Vector3(Random.Range(-min_length, max_length), Random.Range(-min_height, max_height), Random.Range(-min_width, max_width));
        positionArray[3] = new Vector3(Random.Range(-min_length, max_length), Random.Range(-min_height, max_height), Random.Range(-min_width, max_width));
        positionArray[4] = new Vector3(Random.Range(-min_length, max_length), Random.Range(-min_height, max_height), Random.Range(-min_width, max_width));
        positionArray[5] = new Vector3(Random.Range(-min_length, max_length), Random.Range(-min_height, max_height), Random.Range(-min_width, max_width));
        BezierPath bezierPath = new BezierPath(positionArray, false, PathSpace.xyz);
        path = new VertexPath(bezierPath);
        //Duck creation with generated path
        duck = Instantiate(duckPrefab, positionArray[0], Quaternion.identity);
        duck.GetComponent<DuckMovement>().path = path;
        duck.GetComponent<DuckMovement>().speed = speed;
        //"optimalization" for update
        x = path.vertices[path.NumVertices - 1];
    }
    void Start()
    {
        Spawn();
    }
    void Update()
    {
        if (Vector3.SqrMagnitude(x - duck.transform.position) <= 0.05)
        {
            Destroy(duck);
            Destroy(this);
        }
   }
}
