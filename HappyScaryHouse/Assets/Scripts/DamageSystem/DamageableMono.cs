using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageableMono : MonoBehaviour
{
    [SerializeField]
    private DamageableSO Data = default;
    public DamageableSO data => Data;
    public delegate void OnDeathMono();
    public event OnDeathMono OnDeathMonoEvent;
    public delegate void onDamageMono();
    public event onDamageMono onDamageMonoEvent;
    
    private void Awake()
    {
        if (Data != null)
        {   
            // PD - 9/19/2020
            // To avoid directly referencing an SO, if undesired, we replace the reference with 
            // a clone of it. 
            if (Data.shouldClone)
            {
                DamageableSO instance = Object.Instantiate(Data);
                Data = instance;
            }
            Data.onDeathEvent += OnDeath;
            Data.onDamageEvent += OnDamage;
        }
        else Debug.LogError("MISSING DamageableSO on DamageableMono class: " + this + " for gameobject: " + gameObject);
    }
    private void Start()
    {
        Data.set.RegisterDamageable(this, Data.faction);
    }
    private void OnDestroy()
    {
        Data.onDeathEvent -= OnDeath;
        Data.onDamageEvent -= OnDamage;
        Data.set.UnregisterDamageable(this, Data.faction);
    }

    private void OnDamage()
    {
        onDamageMonoEvent?.Invoke();
    }
    private void OnDeath()
    {
        OnDeathMonoEvent?.Invoke();
        Destroy(gameObject);
        Debug.Log(gameObject + "<color> DIED </color>");
    }


}
