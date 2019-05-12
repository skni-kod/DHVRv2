using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PathCreation;

public class DuckMovement : MonoBehaviour
{


    public EndOfPathInstruction endOfPathInstruction;
    public VertexPath path;
    float distanceTravelled;
    public int speed;

    void Update()
    {
        if (path != null)
        {
            distanceTravelled += speed * Time.deltaTime;
            transform.position = path.GetPointAtDistance(distanceTravelled, endOfPathInstruction);
            transform.rotation = path.GetRotationAtDistance(distanceTravelled, endOfPathInstruction);
        }
    }
}
