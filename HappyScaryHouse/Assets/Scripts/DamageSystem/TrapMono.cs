using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(Collider2D))]
public class TrapMono : DamagerMono
{
    private SpriteRenderer SR;
    private void Awake()
    {
        SR = GetComponent<SpriteRenderer>();
        SR.sprite = Data.sprite;
    }

    DamageableMono LastHitDamageable = null;
    public override void Attack()
    {
        LastHitDamageable.data.AddAmountToHealth(-Data.damage);
    }
    void OnCollisionEnter2D(Collision2D collider)
    {
        DamageableMono damageable = collider.gameObject.GetComponent<DamageableMono>();
        if(damageable && damageable.data.faction != Data.faction)
        {
            LastHitDamageable = damageable;
            Attack();
        } 
    }
}
