using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZoomEffect : MonoBehaviour
{
    public float magnitude;

    public bool onAwake;
    public ZoomTool zoom;
    public Transform target;

    void Start()
    {
        if(onAwake)
            zoom.ZoomPunch(target, magnitude, 0.5f, 1, 0.25f);       
    }
}
