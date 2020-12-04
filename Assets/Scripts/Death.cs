using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EZCameraShake;

public class Death : MonoBehaviour
{
    public SpriteRenderer originalSprite;
    public GameObject deadBody;
    public GameObject deathEffect;

    public bool isDead;
    public GameObject drill;

    void Start()
    {
        isDead = false;    
    }

    public void Die(Vector2 direction)
    {
        if(isDead)
            return;
        
        // // Deactivate original sprite
        originalSprite.enabled = false;
        
        // Replace with physics based sprites
        Instantiate(deadBody, transform.position, Quaternion.identity);

        isDead = true;
        drill.AddComponent<Rigidbody2D>();
        drill.transform.parent = null;
        Destroy(GetComponent<Animator>());
        Destroy(GetComponent<Driller>());
        Destroy(GetComponent<PlayerController>());
        Destroy(GetComponentInChildren<Drill>());

        StartCoroutine(DeathPause());
    }

    IEnumerator DeathPause()
    {   
        CameraShaker.Instance.ShakeOnce(3,20,0.1f,0.4f);

        // Instantiate death gpx and sfx    
        GameObject deathgpx = Instantiate(deathEffect, transform.position, Quaternion.AngleAxis(Random.Range(0,360), Vector3.forward));
        yield return new WaitForSeconds(0.1f);

        
        // Slow down time
        Time.timeScale = 0.5f;
        yield return new WaitForSeconds(0.2f);

        // Return time to normal
        Time.timeScale = 1;
    }
}
