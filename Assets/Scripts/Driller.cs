using System;
using UnityEngine;

public class Driller : PlayerController
{
    public Transform drillSlot;
    public ParticleSystem drillParticle;

    public AudioSource drillSoundSource;
    public AudioClip startSound;
    public AudioClip runningSound;

    float lastRotation;

    protected override void Update()
    {
        base.Update();
        
        // Drill Rotation
        {
            drillSlot.rotation = Quaternion.Euler(0,0,lastRotation);
            if(!m_Input.Move.Equals(Vector2.zero))
            {
                // Horizontal Drill Rotation
                lastRotation = (m_Input.Move.x - 1) * 90;

                // Vertical Drill Rotation
                if(m_Input.Move.y != 0)
                {
                    lastRotation = m_Input.Move.y * 90;

                    // If the player points the drill downward (which can overlap the ground)
                    if(m_Input.Move.y == -1 & isGrounded)
                    {
                        if(!movementSoundSource.isPlaying)
                            PlaySound(movementSoundSource, soundJump);
                        jumping = true;
                    }
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
            PlaySound(drillSoundSource, startSound);
        }
        else
        {
            m_Animator.SetLayerWeight(1, 0);
        }
    }
}