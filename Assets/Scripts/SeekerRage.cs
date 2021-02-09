using UnityEngine;

public class SeekerRage : MonoBehaviour
{
    private readonly static float timeTillExplosion = 0.1f;

    public GameObject explosion;
    [Range(1, 6)] public float cooldownTime;

    Rigidbody2D rB;
    Vector2 aiLastPosition;
    float timer;
    bool onCooldown;

    private void Start()
    {
        rB = GetComponentInParent<Rigidbody2D>();
        aiLastPosition = transform.position;   
    }

    private void Update()
    {
        if(onCooldown)
        {
            timer += Time.deltaTime;
            if(timer >= cooldownTime)
            {
                timer = 0;
                onCooldown = false;
            }
            return;
        }

        if(transform.position.Equals(aiLastPosition) || rB.velocity.Equals(Vector2.zero))
        {
            if(timer >= timeTillExplosion)
            {
                
                Instantiate(explosion, transform.position, Quaternion.identity, null);
                timer = 0;
                onCooldown = true;
            }
            else
            {
                timer += Time.deltaTime;
            }
        }
        else
        {
            timer = 0;
        }

        aiLastPosition = transform.position; 
    }

}