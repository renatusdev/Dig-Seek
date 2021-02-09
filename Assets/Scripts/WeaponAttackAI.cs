using UnityEngine;

public class WeaponAttackAI : WeaponAttack
{
    private readonly static float timeOfAttackDelay = 0.4f;

    public SeekerAI seekerAI;
    public Transform hider;
    public Transform weaponSlot;
    [Range(0,3)] public int atkRange;

    private float timerDelayForNoobs = 0;

    private void Start()
    {
        seekerAI = GetComponentInParent<SeekerAI>();
        timerDelayForNoobs = 0;    
    }

    protected override void Update()
    {
        base.Update();

        if(!IsAttacking() & !controller.freeze) 
        {
            Vector2 dir = (hider.position - transform.position).normalized;
            float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
            weaponSlot.rotation = Quaternion.Euler(0,0, angle);
        }
    }

    void FixedUpdate()
    {
        if(hider.GetComponent<Controller>().isDead)
            return;

        if(Vector2.Distance(transform.position, hider.position) <= atkRange
        & !IsAttacking() & seekerAI.hiderIsInView)
        {
            if(timerDelayForNoobs >= timeOfAttackDelay)
            {
                Attack();
            }
            else
                timerDelayForNoobs += Time.fixedDeltaTime;
        }
        else
        {
            timerDelayForNoobs = 0;
        }
    }
}