using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class GridUpdater : MonoBehaviour
{
    [Tooltip("Seconds per grid refresh.")]
    [Range(0,1)] public float refreshRate; 

    public bool selfUpdate;

    AstarPath aStar;

    void Awake()
    {
        aStar = GetComponent<AstarPath>();

        if(selfUpdate)
            InvokeRepeating("ScanGrid", 0, refreshRate);
    }

    public void ScanGrid() { aStar.Scan(); }
}
