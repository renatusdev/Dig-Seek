using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Drill : MonoBehaviour
{
    public TileDestruction tiles;
    public InputController input;

    private void OnCollisionStay2D(Collision2D c)
    {
        // If the drill has collided with the player and the player is drilling
        if(c.collider.CompareTag("Terrain") & input.DrillHold)
        {
            foreach(ContactPoint2D point in c.contacts)
                tiles.DestroyTileAt(point);
        }
    }
}
