using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * Does the player attack.
 * Registers collider if hit occurs.
 */
public class WeaponAttack : MonoBehaviour
{
    public InputController input;
    public Animator animator;
    public AudioSource weaponSoundSource;
    public float timeOfAttack;

    float timeSinceAttack;
    bool attacked;

    void Start()
    {
        attacked = false;    
    }

    void Update()
    {
        if(attacked)
        {
            timeSinceAttack += Time.deltaTime;
            if(timeSinceAttack >= timeOfAttack)
            {
                timeSinceAttack = 0;
                attacked = false;
            }
        }
        
        if(input.DrillDown & !attacked)
        {
            attacked = true;
            animator.SetTrigger("Attack");
            weaponSoundSource.Play();
        }   
    }

    private void OnTriggerEnter2D(Collider2D o)
    {
        if(o.CompareTag("Enemy") || o.CompareTag("Player"))
        {
            
            o.transform.GetComponent<Death>().Die(transform.position - o.transform.position);
        }
    }

    public bool IsAttacking()
    {
        return attacked;
    }
}
