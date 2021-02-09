using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationHolder : MonoBehaviour
{
    public GameObject anim;

    void Start()
    {
        anim.SetActive(false);
    }

    public void Play()
    {
        anim.SetActive(true);
    }

    // Not abstract enough. Only applies in 2D rotations.
    public void Play(Vector2 position, float rotation)
    {
        transform.Rotate(Vector3.forward * rotation, Space.Self);
        transform.position = position;        
        anim.SetActive(true);
    }
}
