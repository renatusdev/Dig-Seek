using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EZCameraShake;

public class DeathByScythe : MonoBehaviour
{
    public LevelLoader levelLoad;
    public SpriteRenderer originalSprite;
    public GameObject deadBody;
    public GameObject bloodSpray;

    public bool isDead;
    public GameObject drill;

    void Start()
    {
        isDead = false;    
    }

    public void Die(Color color)
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
        Destroy(GetComponent<HiderPlayer>());
        Destroy(GetComponent<PlayerController>());
        Destroy(GetComponentInChildren<Drill>());

        StartCoroutine(DeathPause());
    }

    IEnumerator DeathPause()
    {   
        CameraShaker.Instance.ShakeOnce(3,20,0.1f,0.4f);

        ScreenFlash.i.Flash(Color.red);

        Instantiate(bloodSpray, transform.position, Quaternion.AngleAxis(Random.Range(0,360), Vector3.forward));
        yield return new WaitForSeconds(0.1f);

        Time.timeScale = 0.2f;
        yield return new WaitForSeconds(0.1f);

        Time.timeScale = 1;

        yield return new WaitForSeconds(0.75f);
        levelLoad.LoadLevel();
    }
}
