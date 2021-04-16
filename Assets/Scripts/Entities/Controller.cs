using System.Collections;
using EZCameraShake;
using UnityEngine;
using UnityEngine.Events;

public class Controller : MonoBehaviour
{
    [Header("Movement")]
    [Range(0, 20)]  public int movementSpeed;
    [Range(0, 20)]  public int gravitySpeed;
    [Range(0, 20)]  public int gravityMultiplierOnFall;
    [Range(0, 20)]  public int smallJumpGravityMultiplier;
    [Range(0, 20)]  public int jumpSpeed;
    [Range(0, 1)]   public float jumpTime;

    public bool isGrounded = false;
    public bool isDead = false;

    public LayerMask groundLayer;

    public GameObject bloodSpray;
    public GameObject carcassByScythe;
    public GameObject carcassByHazard;

    public AnimationHolder thunderbolt;

    public UnityEvent eventOnFreeze;
    public UnityEvent eventOnUnfreeze;

    public bool jumping     { get; protected set;}
    public Vector2 velocity { get; protected set; }    

    [Header("Sounds")]
    public AudioSource kinematicsSS;
    public AudioClip footstepSFX;
    public AudioClip jumpSFX;

    protected Animator m_Animator;
    protected SpriteRenderer  m_SpriteRenderer;
    protected Rigidbody2D m_Rigidbody;
    protected float jumpTimer = 0;

    public bool freeze;

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
        // All entities freeze during game pause
        if(freeze) { return; }

        // Animation
        m_Animator.SetBool("IsRunning", horz != 0);

        // Sprite flipping
        if(horz != 0)
            m_SpriteRenderer.flipX = horz == -1;

        // Create velocity vector
        velocity = Vector2.right * horz;
        velocity.Normalize();
        velocity *= movementSpeed;

        // Add to rigidbody
        m_Rigidbody.velocity = velocity;
    }

    protected void Jump()
    {
        if(freeze)
            return;
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
            Die(DeathType.Scythe);
        else if(col.CompareTag("Hazard"))
            Die(DeathType.Hazard);
    }

    protected virtual void Die(DeathType deathType)
    {
        isDead = true;
        m_SpriteRenderer.enabled = false;

        if(Scoreboard.i != null)
        {
            if(this.name.StartsWith("Hider"))
                Scoreboard.i.AddRedPoint();
            else    // Seeker died
                Scoreboard.i.AddBluePoint(ScoreType.ELIMINATION);
        }
        
        Destroy(GetComponent<Animator>());

        if(deathType.Equals(DeathType.Scythe))
        {
            m_Rigidbody.constraints = RigidbodyConstraints2D.FreezePositionX;
            Instantiate(carcassByScythe, transform.position, Quaternion.identity);
            Instantiate(bloodSpray, transform.position, Quaternion.AngleAxis(UnityEngine.Random.Range(0,360), Vector3.forward));

            StartCoroutine(DeathEffects(Color.red));
        }
        else if(deathType.Equals(DeathType.Hazard))
        {
            Instantiate(carcassByHazard, transform.position, Quaternion.identity);
            Instantiate(bloodSpray, transform.position, Quaternion.AngleAxis(UnityEngine.Random.Range(0,360), Vector3.forward));

            StartCoroutine(DeathEffects(Color.green));
        }
    }

    public void Freeze()
    {
        freeze = true;
        
        if(m_Rigidbody != null)
            m_Rigidbody.constraints = RigidbodyConstraints2D.FreezeAll;
        else
        {
            m_Rigidbody = GetComponent<Rigidbody2D>();
            Freeze();
            return;
        }
        
        eventOnFreeze.Invoke();
    }
    
    public void Unfreeze()
    {
        freeze = false;
        if(m_Rigidbody != null)
            m_Rigidbody.constraints = RigidbodyConstraints2D.FreezeRotation;
        else
        {
            m_Rigidbody = GetComponent<Rigidbody2D>();
            Unfreeze();
            return;
        }
        eventOnUnfreeze.Invoke();
    }

    IEnumerator DeathEffects(Color flashColor)
    {   
        // Camera and screen flash effects
        CameraShaker.Instance.ShakeOnce(3,20,0.1f,0.8f);
        ImageEffects.i.Flash(flashColor, 12);
        thunderbolt.Play(transform.position, Random.Range(0, 360));
        
        // ImageEffects.i.Dim(Color.black, 10, 0.4f);
        yield return new WaitForSeconds(0.1f);

        // Slowmotion effect
        Time.timeScale = 0.2f;
        yield return new WaitForSeconds(0.15f);

        // Return time to normal
        Time.timeScale = 1;

        // TODO: Add point
    }

    #region Animation Triggers

    void OnFootstep()
    {
        if(!kinematicsSS.isPlaying & !freeze)
            PlaySound(kinematicsSS, footstepSFX);
    }
    
    void OnJump()
    {
        
    }

    #endregion
}