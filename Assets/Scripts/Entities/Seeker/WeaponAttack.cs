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

    public Controller controller;

    void Start()
    {
        controller = GetComponentInParent<Controller>();
        attacked = false;    
    }

    protected virtual void Update()
    {
        if(IsAttacking())
        {
            timeSinceAttack += Time.unscaledDeltaTime;

            if(timeSinceAttack >= timeOfAttack)
            {
                timeSinceAttack = 0;
                attacked = false;
            }
        }   
    }

    public void Attack()
    {
        if(attacked || controller.freeze) 
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
