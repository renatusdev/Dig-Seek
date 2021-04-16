using System;
using UnityEngine;

public class HiderPlayer : PlayerController
{
    public Transform drillSlot;
    public ParticleSystem drillParticle;
    
    int lastRotation;

    [Header("Sounds")]
    public AudioSource drillSS;
    public AudioClip throttleSFX;

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
            PlaySound(drillSS, throttleSFX);
        }
        else if(m_Input.DrillHold)
        {
            // Drill Particle Effect
            if(!drillParticle.isPlaying)
                drillParticle.Play();

            // Drill Animation Effect
            m_Animator.SetLayerWeight(1, 1);
        }
        else if(m_Input.DrillUp)
        {
            drillSlot.gameObject.SetActive(false);
            PlaySound(drillSS, throttleSFX);
        }
        else
        {
            m_Animator.SetLayerWeight(1, 0);
        }
    }

    protected override void Die(DeathType deathType)
    {
        base.Die(deathType);
            
        Destroy(drillSlot.GetComponentInChildren<Drill>());
        drillSlot.parent = null;
        drillSlot.gameObject.AddComponent<Rigidbody2D>();
    }
}