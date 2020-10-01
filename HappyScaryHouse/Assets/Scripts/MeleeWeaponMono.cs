using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(Collider2D))]
public class MeleeWeaponMono : DamagerMono
{
    private List<DamageableMono> DamageablesInRange = new List<DamageableMono>();
    public List<DamageableMono> damageablesInRange => DamageablesInRange;

    public override void Attack()
    {
        for (int i = 0; i < DamageablesInRange.Count; i++)
        {
            DamageablesInRange[i].damSO.AddAmountToHealth(-DamagerSO.damage);
        }

        // #TODO - Play animation for attack
    }

    void OnTriggerEnter2D(Collider2D collider)
    {
        DamageableMono damageable = collider.GetComponent<DamageableMono>();
        if (damageable != null)
        {
            if (damageable.damSO.faction != this.DamagerSO.faction)
            {
                if (!DamageablesInRange.Contains(damageable))
                {
                    DamageablesInRange.Add(damageable);
                }
                else Debug.LogError("Attempted to add damageable " + damageable.gameObject.name 
                    + " to damageable list, but it already exists.");
            }
        }
    }
    void OnTriggerExit2D(Collider2D collider)
    {
        DamageableMono damageable = collider.GetComponent<DamageableMono>();
        if (damageable)
        {
            if (DamageablesInRange.Contains(damageable))
            {
                DamageablesInRange.Remove(damageable);
            }
        }
    }
}
