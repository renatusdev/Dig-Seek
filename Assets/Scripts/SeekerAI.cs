using UnityEngine;
using Pathfinding;

public class SeekerAI : Controller  
{
    #region Variables
    public Transform hider;

    [Range(0.1f,4)] public float distanceToWaypointChange;

    [Tooltip("Seconds per pathfinding refresh.")]
    [Range(0.1f,1)] public float pathfindingRefreshRate;

    [Range(3, 20)] public int rangeOfSight;

    [Tooltip("The higher the value, the more random the range.")]
    [Range(1, 20)] public int guessRange;

    public LayerMask obstacles;
    public AudioSource interactionAS;
    public AudioClip spottedSound;
    public bool hiderIsInView = false;

    Seeker pathSeeking;
    Path path;
    CapsuleCollider2D col;
    int currWaypointIndex;
    
    Vector2 lastSpotted;
    Vector2 guessedPosition;
    Vector2 direction;
    Vector2 aiLastPosition;

    #endregion

    protected override void Start()
    {
        base.Start();
        pathSeeking = GetComponent<Seeker>();
        col = GetComponent<CapsuleCollider2D>();

        guessedPosition = Guess();
        guessedPosition.y = hider.position.y;

        aiLastPosition = transform.position;  

        InvokeRepeating("UpdatePath", 0f, pathfindingRefreshRate);
    }

    protected void FixedUpdate()
    {
        if(isDead || freeze)
        {
            return;
        }


        if(HiderIsSpotted())
        {
            guessedPosition = Vector2.zero;
            lastSpotted = hider.position;
        }

        Move();
        aiLastPosition = transform.position; 
    }

    void Move()
    {
        if(path == null || currWaypointIndex == path.vectorPath.Count-1)
            return;

        direction = ((Vector2)path.vectorPath[currWaypointIndex] - (Vector2)path.vectorPath[currWaypointIndex-1]).normalized;

        Debug.DrawLine(path.vectorPath[currWaypointIndex-1], ((Vector2)path.vectorPath[currWaypointIndex]));

        if(Vector2.Distance(m_Rigidbody.position, path.vectorPath[currWaypointIndex]) <= distanceToWaypointChange)
        {
            currWaypointIndex++;

            // If the path is completed, then the seeker has killed the player,
            // or we reached a guessedPosition, in which case we update it.
            if(currWaypointIndex == path.vectorPath.Count-1)
                guessedPosition = Guess();
        }

        // Override direction if player is stuck
        if(transform.position.Equals(aiLastPosition) && m_Rigidbody.velocity.Equals(Vector2.zero))
        {
            RaycastHit2D hit = Physics2D.CapsuleCast(transform.position, col.size * 0.2f, CapsuleDirection2D.Vertical, 0, Vector2.down);

            if(hit.collider != null)
            {
                Debug.DrawLine(hit.point, hit.point + (-hit.normal), Color.red, 1);
                direction = -hit.normal;
            }
        }

        int horz = 0;
        int vert = 0;

        if(direction.x != 0)
            horz = 1 * (int)Mathf.Sign(direction.x);
        if(direction.y != 0)
            vert = 1 * (int)Mathf.Sign(direction.y);

        Movement(horz);

        if(vert == 1 & isGrounded)   
            jumping = true; 

        if(vert == -1)
            jumping = false;

        if(jumping)
            Jump();

        if(!IsGrounded())
            Gravity(true);
    }

    /// <summary> 
    /// Checks and updates if the hider is in view. 
    /// </summary>
    bool HiderIsSpotted()
    {
        RaycastHit2D hit = Physics2D.Raycast(m_Rigidbody.position, ((Vector2)hider.position - m_Rigidbody.position).normalized, rangeOfSight, ~LayerMask.GetMask("Enemy"));

        // Something is spotted!
        if(hit.collider != null)
            // Player is spotted!
            if(hit.collider.CompareTag("Player"))
                return hiderIsInView = true;

        return hiderIsInView = false;
    }

    /// <summary> 
    /// Starts a path between the entities position, and the last spotted
    /// occurence of the hider. After the path is generated, the next position
    /// of the seeker is updated.
    /// </summary>
    void UpdatePath()
    {
        if(pathSeeking.IsDone())
        {
            if(guessedPosition.Equals(Vector2.zero))
                pathSeeking.StartPath(transform.position, lastSpotted, OnPathComplete);
            else
                pathSeeking.StartPath(transform.position, guessedPosition, OnPathComplete);
        }

        void OnPathComplete(Path p)
        {
            if(!p.error)
            {
                this.path = p;
                currWaypointIndex = 1;
            }            
        }
    }

    Vector2 Guess()
    {
        for(int i = 0; i < 4; i++)
        {
            Vector2 rnd = hider.position + (Random.onUnitSphere * guessRange);
            Collider2D col = Physics2D.OverlapCircle(rnd, 1);

            if(col == null)
                return rnd;
        }

        return hider.position;
    }

    // private void OnDrawGizmos()
    // {
    //     if(Application.isPlaying)
    //     {
    //         for (int i = currWaypointIndex; i < path.vectorPath.Count-1; i++)
    //             Gizmos.DrawSphere(path.vectorPath[i], 0.05f);

    //         Gizmos.color = Color.green;
    //         Gizmos.DrawSphere(path.vectorPath[0], 0.2f); 
    //         Gizmos.DrawSphere(path.vectorPath[1], 0.2f);

    //         Gizmos.color = Color.blue;
    //         if(!guessedPosition.Equals(Vector2.zero))
    //             Gizmos.DrawSphere(guessedPosition, 0.2f);
    //     }
    // }

    // private void OnGUI()
    // {
    //     if(direction.y == 1)
    //         GUI.TextField(new Rect(15,15,50,25), "UP");
    //     else if (direction.y == -1)
    //         GUI.TextField(new Rect(15,15,50,25), "DOWN");
    //     else 
    //         GUI.TextField(new Rect(15,15,50,25), "NONE");

    //     if(direction.x == 1)
    //         GUI.TextField(new Rect(15,55,50,25), "RIGHT");
    //     else if (direction.x == -1)
    //         GUI.TextField(new Rect(15,55,50,25), "LEFT");
    //     else 
    //         GUI.TextField(new Rect(15,55,50,25), "NONE");
    // }
}
