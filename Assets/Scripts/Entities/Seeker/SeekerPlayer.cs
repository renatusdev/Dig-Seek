using UnityEngine;

public class SeekerPlayer : PlayerController
{
    public Transform weaponSlot;
    public WeaponAttack attack;

    float lastRotation;

    protected override void Update()
    {
        base.Update();
        
        // Weapon Rotation
        {
            if(!attack.IsAttacking())
            {
                weaponSlot.rotation = Quaternion.Euler(0,0,lastRotation);
                if(!m_Input.Move.Equals(Vector2.zero))
                {
                    int x = Mathf.FloorToInt(m_Input.Move.x);
                    int y = Mathf.FloorToInt(m_Input.Move.y);

                    // Diagonal Drill Rotation
                    if(new Vector2(Mathf.Abs(x), Mathf.Abs(y)).Equals(Vector2.one))
                    {
                        lastRotation = y*45 + Mathf.Abs((x-1))*(y*45);
                    }
                    // Up-Down-Left-Right rotation.
                    else
                    {
                        lastRotation = (Mathf.Abs((x - 1)) * 90) * Mathf.Abs(x) + y*90;
                    }   
                }
            }
        }
    }
}