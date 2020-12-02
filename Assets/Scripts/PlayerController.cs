using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerController : MonoBehaviour
{
    #region Variables
    private static readonly float GROUNDELEVATION = 0.01f;

    [Header("Movement")]
    [Range(0, 20)]  public int movementSpeed;
    [Range(0, 20)]  public int gravitySpeed;
    [Range(0, 20)]  public int gravityMultiplierOnFall;
    [Range(0, 20)]  public int smallJumpGravityMultiplier;
    [Range(0, 20)]  public int jumpSpeed;
    [Range(0, 1)]   public float jumpTime;

    public ParticleSystem jumpSmokeParticle;
    public LayerMask groundLayer;
    public Vector2 velocity { get; private set; }    
    public bool isGrounded = false;
    
    [Header("Sounds")]
    public AudioSource movementSoundSource;
    public AudioClip soundFootStep;
    public AudioClip soundJump;

    protected Animator m_Animator;
    protected InputController m_Input;
    protected bool jumping = false;

    Rigidbody2D     m_Rigidbody;
    BoxCollider2D   m_Collider;
    SpriteRenderer  m_SpriteRenderer;

    float jumpTimer = 0;

    #endregion

    void Start()
    {
        m_Rigidbody         = GetComponent<Rigidbody2D>();    
        m_Collider          = GetComponent<BoxCollider2D>();    
        m_SpriteRenderer    = GetComponent<SpriteRenderer>();    
        m_Animator          = GetComponent<Animator>();
        m_Input             = GetComponent<InputController>();
    }

    protected virtual void Update()
    {    
        if(m_Input.JumpDown)
        {
            if(isGrounded)
            {
                jumpSmokeParticle.Play();
                PlaySound(movementSoundSource, soundJump);
            }
            jumping = true;
        }
        else if(m_Input.JumpHold)
            jumping = true;
        else if(m_Input.JumpUp)
            jumpTimer = Mathf.Infinity;
    }

    protected virtual void FixedUpdate()
    {
        Movement();
        if(jumping)
            Jump();

        if(!IsGrounded())
            Gravity();
    }   

    private bool IsGrounded()
    {
        if(Physics2D.BoxCast(transform.position, m_Collider.size, 0, Vector2.down, GROUNDELEVATION, groundLayer))
        {
            m_Animator.SetBool("IsGrounded", true);
            return isGrounded = true;
        }
        
        m_Animator.SetBool("IsGrounded", false);
        return isGrounded = false;
    }

    private void Gravity()
    {
        m_Rigidbody.velocity += -(Vector2)transform.up * gravitySpeed;

        if(m_Rigidbody.velocity.y < 0)
            m_Rigidbody.velocity += -(Vector2)transform.up * gravityMultiplierOnFall;
        if(m_Rigidbody.velocity.y > 0 & !m_Input.JumpHold)
            m_Rigidbody.velocity += -(Vector2)transform.up * smallJumpGravityMultiplier;
    }

    void Jump()
    {
        // If the jump timer reached the set jump time.
        if(jumpTimer >= jumpTime)
        {
            // If the player is grounded
            if(isGrounded)
                // Reset the timer
                jumpTimer = 0;

            // The player is falling not jumping
            jumping = false;
            
            // Stop moving up.
            return;
        }
        else
        {
            // Add this current frames' time to the jump timer.
            jumpTimer += Time.fixedDeltaTime;
            // Add upwards velocity.
            m_Rigidbody.velocity += Vector2.up * jumpSpeed;            
        }
    }

    void Movement()
    {   
        float horz = m_Input.Move.x;

        m_Animator.SetBool("IsRunning", horz != 0);

        if(horz != 0)
            m_SpriteRenderer.flipX = horz == -1;

        velocity = Vector2.right * horz;
        velocity.Normalize();
        velocity *= movementSpeed;

        m_Rigidbody.velocity = velocity;
    }

    protected void PlaySound(AudioSource source, AudioClip clip)
    {
        source.clip = clip;
        source.Play();
    }

////////////////////// Animation Event Triggers //////////////////////
//////////////////////////////////////////////////////////////////////

    void OnFootstep()
    {
        if(!movementSoundSource.isPlaying)
            PlaySound(movementSoundSource, soundFootStep);
    }

    void OnJump()
    {
    }

//////////////////////////////////////////////////////////////////////
}
