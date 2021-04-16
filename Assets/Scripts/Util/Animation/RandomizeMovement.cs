using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomizeMovement : MonoBehaviour
{
    public bool onAwake;
    public Vector2 xClamp;
    public Vector2 yClamp;
    public Vector2 zClamp;

    public Vector3 rotationThreshold;

    public Vector3 velocity;
    public Vector3 angularVelocity;

    void Awake()
    {
        if(onAwake)
            Randomize(xClamp, yClamp, zClamp, rotationThreshold);
    }
    
    public void Randomize(Vector2 xClamp, Vector2 yClamp, Vector2 zClamp, Vector2 rotationThreshold)
    {
        float x = Random.Range(xClamp.x, xClamp.y);
        float y = Random.Range(yClamp.x, yClamp.y);
        float z = Random.Range(zClamp.x, zClamp.y);

        transform.position = new Vector3(x, y, z);

        if(!gameObject.isStatic)
        {
            // TODO: Apply movement
        }
    }
}
