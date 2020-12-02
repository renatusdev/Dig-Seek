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
    public AudioSource weaponSoundSource;

    void Update()
    {
        // If player goes for an attack. 
        if(input.DrillDown)
        {
            // Play attack animation
            weaponSoundSource.Play();
        }   
    }

    private void OnTriggerEnter2D(Collider2D o)
    {
        
    }
}
