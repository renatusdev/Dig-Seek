using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponAttack : MonoBehaviour
{
    public Animator animator;
    public AudioSource weaponSoundSource;
    public float timeOfAttack;

    float timeSinceAttack;
    bool attacked;

    void Start()
    {
        attacked = false;    
    }

    protected virtual void Update()
    {
        if(IsAttacking())
        {
            timeSinceAttack += Time.deltaTime;

            if(timeSinceAttack >= timeOfAttack)
            {
                timeSinceAttack = 0;
                attacked = false;
            }
        }   
    }

    public void Attack()
    {
        if(attacked)
            return;
            
        attacked = true;
        animator.SetTrigger("Attack");
        weaponSoundSource.Play();
    }

    public bool IsAttacking()
    {
        return attacked;
    }
}
