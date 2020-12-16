using UnityEngine;
using Pathfinding;

public class SeekerAI : Controller
{
    public Transform hider;

    [Range(0.1f,4)] public float distanceToWaypointChange;

    [Tooltip("Seconds per pathfinding refresh.")]
    [Range(0.1f,1)] public float pathfindingRefreshRate;

    [Range(3, 20)] public int rangeOfSight;

    [Tooltip("The higher the value, the more random the range.")]
    [Range(3, 20)] public int rangeOfRandomness;

    public LayerMask obstacles;
    public AudioSource interactionAS;
    public AudioClip spottedSound;
    public bool hiderIsInView = false;

    bool guessedPosition = false;
    int currWaypointIndex;
    Vector2 lastSpotted;
    Vector2 nextPos;
    Seeker pathSeeking;
    Path path;

    protected override void Start()
    {
        base.Start();
        pathSeeking = GetComponent<Seeker>();

        lastSpotted = GuessPosition();
        lastSpotted.y = hider.position.y;

        InvokeRepeating("UpdatePath", 0f, pathfindingRefreshRate);
    }

    void UpdatePath()
    {
        if(pathSeeking.IsDone())
            pathSeeking.StartPath(transform.position, lastSpotted, OnPathComplete);
    }

    void OnPathComplete(Path p)
    {
        if(!p.error)
        {
            this.path = p;
            // The center index (0), is almost the same position as the player
            // thus disregarded.
            currWaypointIndex = 1;
            nextPos = p.vectorPath[currWaypointIndex];
        }
    }

    Vector2 GuessPosition()
    {
        guessedPosition = true;

        for(int i = 0; i < 4; i++)
        {
            Vector2 rnd = Random.onUnitSphere * rangeOfRandomness + hider.position;   
            Collider2D col = Physics2D.OverlapCircle(rnd, 1);

            if(col == null)
                return rnd;
        }

        return hider.position;
    }

    protected void FixedUpdate()
    {   
        // Symptom Fix
        // Make a thing that if AI has not changed position 
        // and waypointIndex is the same after a given time threshold 
        // then we guess a new position


        RaycastHit2D hit = Physics2D.Raycast(m_Rigidbody.position, ((Vector2)hider.position - m_Rigidbody.position).normalized, rangeOfSight, ~LayerMask.GetMask("Enemy"));

        // Something is spotted!
        if(hit.collider != null)
        {
            // Player is spotted!
            if(hit.collider.CompareTag("Player"))
            {
                hiderIsInView = true;
                // Update player position!
                lastSpotted = hider.position;
            }
            else
            {
                hiderIsInView = false;
            }
        }
        // Player has succesfully hid...
        else
        {
            hiderIsInView = false;
            if(!guessedPosition)
                lastSpotted = GuessPosition();
        }

        // Movement
        {
            if(path == null || currWaypointIndex == path.vectorPath.Count-1)
                return;

            Vector2 direction = (nextPos - m_Rigidbody.position).normalized;

            Debug.DrawLine(m_Rigidbody.position, (m_Rigidbody.position + direction));

            if(Vector2.Distance(m_Rigidbody.position, nextPos) <= distanceToWaypointChange)
            {
                currWaypointIndex++;
                nextPos = path.vectorPath[currWaypointIndex];

                // Reached the last waypoint of path
                if(currWaypointIndex == path.vectorPath.Count-1)
                    lastSpotted = GuessPosition();
            }

            int horz = Mathf.RoundToInt(direction.x);
            int vert = Mathf.RoundToInt(direction.y);

            if(direction.y >= 0.1f)
                vert = 1;
            else if(direction.y <= -0.1f)
                vert = -1;
            else
                vert = 0;

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
    }

    private void OnDrawGizmos()
    {
        if(Application.isPlaying)
        {
            // For Line Of Sight Visualization
            // Gizmos.DrawRay(m_Rigidbody.position, ((Vector2)hider.position - m_Rigidbody.position).normalized * rangeOfSight);

            for (int i = currWaypointIndex; i < path.vectorPath.Count-1; i++)
                Gizmos.DrawSphere(path.vectorPath[i], 0.05f);

            Gizmos.color = Color.green;
            Gizmos.DrawSphere(nextPos, 0.08f);
            Gizmos.DrawSphere(path.vectorPath[0], 0.2f);
        }
    }
}
