using System;
using UnityEngine;

public class Driller : PlayerController
{
    public Transform drillSlot;
    public ParticleSystem drillParticle;

    public AudioSource drillSoundSource;
    public AudioClip startSound;
    public AudioClip runningSound;

    int lastRotation;
    
    protected override void Update()
    {
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

    protected override void FixedUpdate()
    {
        base.FixedUpdate();
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
}