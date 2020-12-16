using System;
using UnityEngine;

public class HiderPlayer : PlayerController
{
    public Transform drillSlot;
    public ParticleSystem drillParticle;

    public AudioSource drillSoundSource;
    public AudioClip startSound;
    public AudioClip runningSound;

    
    [Header("On Death")]

    [Tooltip("If death occurs by Scythe.")]
    public GameObject deadBody;
    [Tooltip("If death occurs by Scythe.")]
    public GameObject bloodSpray;
    
    int lastRotation;
    
    protected override void Update()
    {
        if(isDead)
            return;
        
        base.Update();
        
        // Drill Rotation
        {
            drillSlot.rotation = Quaternion.Euler(0,0,lastRotation);

            if(!m_Input.Move.Equals(Vector2.zero))
            {
                int x = Mathf.FloorToInt(m_Input.Move.x);
                int y = Mathf.FloorToInt(m_Input.Move.y);

                // Diagonal Drill Rotation
                if(new Vector2(Mathf.Abs(x), Mathf.Abs(y)).Equals(Vector2.one))
                {
                    lastRotation = y*45 + Mathf.Abs((x-1))*(y*45);
                }
                // Up-Down-Left-Right rotation.
                else
                {
                    lastRotation = (Mathf.Abs((x - 1)) * 90) * Mathf.Abs(x) + y*90;
                }   
            }
        }
    
        Drilling();
    }

    private void Drilling()
    {
        if(m_Input.DrillDown)
        {
            drillSlot.gameObject.SetActive(true);
            PlaySound(drillSoundSource, startSound);
        }
        else if(m_Input.DrillHold)
        {
            // Drill Particle Effect
            if(!drillParticle.isPlaying)
                drillParticle.Play();

            // Drill Sound Effect
            if(!drillSoundSource.isPlaying)
                PlaySound(drillSoundSource, runningSound);

            // Drill Animation Effect
            m_Animator.SetLayerWeight(1, 1);
        }
        else if(m_Input.DrillUp)
        {
            drillSlot.gameObject.SetActive(false);
            PlaySound(drillSoundSource, startSound);
        }
        else
        {
            m_Animator.SetLayerWeight(1, 0);
        }
    }

    protected override void Die(Color flashColor)
    {
        base.Die(flashColor);
        
        // Deactivate original sprite
        m_SpriteRenderer.enabled = false;
        
        // Replace with physics based sprites
        Instantiate(deadBody, transform.position, Quaternion.identity);
        Instantiate(bloodSpray, transform.position, Quaternion.AngleAxis(UnityEngine.Random.Range(0,360), Vector3.forward));

        GameObject drill = drillSlot.GetChild(0).gameObject;

        drill.transform.parent = null;
        drill.AddComponent<Rigidbody2D>();
        Destroy(drill.GetComponent<Drill>());
    }
}