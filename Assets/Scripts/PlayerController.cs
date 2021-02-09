using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerController : Controller
{
    #region Variables
    public ParticleSystem jumpSmokeParticle;
    public InputController m_Input;


    BoxCollider2D   m_Collider;

    #endregion

    protected override void Start()
    {
        base.Start();
        m_Input = InputController.i;
        m_Collider  = GetComponent<BoxCollider2D>();    
    }
    protected virtual void Update()
    {
        if(m_Input.JumpDown)
        {
            if(isGrounded)
            {
                jumpSmokeParticle.Play();
                PlaySound(kinematicsSS, jumpSFX);
            }
            jumping = true;
        }
        else if(m_Input.JumpHold)
            jumping = true;
        else if(m_Input.JumpUp)
            jumpTimer = Mathf.Infinity;
    }

    protected void FixedUpdate()
    {
        if(isDead)
            return;

        Movement(m_Input.Move.x);

        if(jumping)
            Jump();

        if(!IsGrounded())
            Gravity(m_Input.JumpHold);
    }   
}
