using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomizeRotation : MonoBehaviour
{
    public bool onAwake;

    public Vector2 range;

    void Start()
    {
        if(onAwake)
            Set();        
    }

    public void Set()
    {
        transform.Rotate(Vector3.forward *  Random.Range(range.x, range.y), Space.Self);
    }
}
