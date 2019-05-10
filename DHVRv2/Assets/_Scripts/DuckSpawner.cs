using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PathCreation;
public class DuckSpawner : MonoBehaviour
{
    public GameObject ducky;
    public VertexPath path;
    

    // Start is called before the first frame update
    void Start()
    {
        var positionArray = new Vector3[4];
        positionArray[0] = new Vector3(Random.Range(-5f, 5f), Random.Range(-1f, 5f), Random.Range(-5f, 5f));
        positionArray[1] = new Vector3(Random.Range(-5f, 5f), Random.Range(0, 5f), Random.Range(-5f, 5f));
        positionArray[2] = new Vector3(Random.Range(-5f, 5f), Random.Range(-2f, 5f), Random.Range(-5f, 5f));
        positionArray[3] = new Vector3(Random.Range(-5f, 5f), Random.Range(-1f, 5f), Random.Range(-5f, 5f));
        BezierPath bezierPath = new BezierPath(positionArray, false, PathSpace.xyz);
        path = new VertexPath(bezierPath);

        ducky.AddComponent<DuckMovement>();
        DuckMovement pathy = ducky.GetComponent<DuckMovement>();
        pathy.path = path;
        Instantiate(ducky, transform.position, transform.rotation);
        
        


    }
}
