using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamagerPlaceholder : MonoBehaviour
{
    public FactionSO myFaction;
    void OnTriggerEnter2D (Collider2D coll)
    {
        DamageableMono damageable = coll.gameObject.GetComponent<DamageableMono>();
        if (damageable && damageable.data.faction != myFaction)
        {
            damageable.data.AddAmountToHealth(-1);
        }
    }
}
