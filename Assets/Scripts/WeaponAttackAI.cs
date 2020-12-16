using UnityEngine;

public class WeaponAttackAI : WeaponAttack
{
    public Transform hider;
    public Transform weaponSlot;
    [Range(0,3)] public int atkRange;

    protected override void Update()
    {
        base.Update();

        if(!IsAttacking())
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
        & !IsAttacking() & GetComponentInParent<SeekerAI>().hiderIsInView)
            Attack();
    }
}