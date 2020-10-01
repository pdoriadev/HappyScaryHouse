using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(Collider2D))]
public class TrapMono : DamagerMono
{
    private SpriteRenderer SR;
    protected override void Awake()
    {
        base.Awake();
        SR = GetComponent<SpriteRenderer>();
        SR.sprite = DamagerSO.sprite;
    }

    DamageableMono LastHitDamageable = null;
    public override void Attack()
    {
        LastHitDamageable.damSO.AddAmountToHealth(-DamagerSO.damage);
    }
    void OnCollisionEnter2D(Collision2D collider)
    {
        DamageableMono damageable = collider.gameObject.GetComponent<DamageableMono>();
        if(damageable && damageable.damSO.faction != DamagerSO.faction)
        {
            LastHitDamageable = damageable;
            Attack();
        } 
    }
}
