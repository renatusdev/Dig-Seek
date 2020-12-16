using System.Collections;
using EZCameraShake;
using UnityEngine;

public class Controller : MonoBehaviour
{
    [Header("Movement")]
    [Range(0, 20)]  public int movementSpeed;
    [Range(0, 20)]  public int gravitySpeed;
    [Range(0, 20)]  public int gravityMultiplierOnFall;
    [Range(0, 20)]  public int smallJumpGravityMultiplier;
    [Range(0, 20)]  public int jumpSpeed;
    [Range(0, 1)]   public float jumpTime;

    public AudioSource soundSourceMovement;
    public AudioClip soundClipFootstep;
    public bool isGrounded = false;
    public bool isDead = false;

    public LayerMask groundLayer;

    public bool jumping     { get; protected set;}
    public Vector2 velocity { get; protected set; }    

    protected Animator m_Animator;
    protected SpriteRenderer  m_SpriteRenderer;
    protected Rigidbody2D m_Rigidbody;
    protected float jumpTimer = 0;

    protected virtual void Start()
    {
        m_Rigidbody         = GetComponent<Rigidbody2D>();    
        m_Animator          = GetComponent<Animator>();    
        m_SpriteRenderer    = GetComponent<SpriteRenderer>();    
    }

    protected void Gravity(bool holdJump)
    {
        m_Rigidbody.velocity += -(Vector2)transform.up * gravitySpeed;

        if(m_Rigidbody.velocity.y < 0)
            m_Rigidbody.velocity += -(Vector2)transform.up * gravityMultiplierOnFall;
        if(m_Rigidbody.velocity.y > 0 & !holdJump)
            m_Rigidbody.velocity += -(Vector2)transform.up * smallJumpGravityMultiplier;
    }

    protected void Movement(float horz)
    {   
        m_Animator.SetBool("IsRunning", horz != 0);

        if(horz != 0)
            m_SpriteRenderer.flipX = horz == -1;

        velocity = Vector2.right * horz;
        velocity.Normalize();
        velocity *= movementSpeed;

        m_Rigidbody.velocity = velocity;
    }

    protected void Jump()
    {
        // If the jump timer reached the set jump time.
        if(jumpTimer >= jumpTime)
        {
            // If the player is grounded
            if(isGrounded)
                // Reset the timer
                jumpTimer = 0;

            // The entity is falling not jumping
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

    protected bool IsGrounded()
    {
        if(Physics2D.OverlapCircle((Vector2)transform.position + (Vector2.down * 0.25f), 0.3f, groundLayer))
        {
            m_Animator.SetBool("IsGrounded", true);
            return isGrounded = true;
        }

        m_Animator.SetBool("IsGrounded", false);
        return isGrounded = false;
    }

    protected void PlaySound(AudioSource source, AudioClip clip)
    {
        source.clip = clip;
        source.Play();
    }

    protected virtual void OnTriggerEnter2D(Collider2D col)
    {
        if(isDead)
            return;

        if(col.CompareTag("Scythe"))
            Die(Color.red);
        else if(col.CompareTag("Hazard"))
            Die(Color.green);
    }

    protected virtual void Die(Color flashColor)
    {
        isDead = true;
        
        Destroy(GetComponent<Animator>());
        StartCoroutine(Death(flashColor));
    }

    IEnumerator Death(Color flashColor)
    {   
        // Camera effects
        CameraShaker.Instance.ShakeOnce(3,20,0.1f,0.4f);
        ScreenFlash.i.Flash(flashColor, 10);       
        yield return new WaitForSeconds(0.1f);

        // Slowmotion effect
        Time.timeScale = 0.2f;
        yield return new WaitForSeconds(0.15f);

        // Return time to normal
        Time.timeScale = 1;
    }

    #region Animation Triggers

    void OnFootstep()
    {
        if(!soundSourceMovement.isPlaying)
            PlaySound(soundSourceMovement, soundClipFootstep);
    }
    
    void OnJump()
    {
        
    }

    #endregion
}