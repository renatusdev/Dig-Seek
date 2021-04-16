using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explosion : MonoBehaviour
{
    public int radius;

    void Start()
    {
        TileDestruction.i.ExplodeTilesAt(transform.position, Random.Range(1, radius));
    }
}
    