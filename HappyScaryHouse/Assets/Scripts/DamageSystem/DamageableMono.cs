using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageableMono : MonoBehaviour
{
    [SerializeField]
    private DamageableSO DamSo = default;
    public DamageableSO damSO => DamSo;
    public delegate void OnDeathMono();
    public event OnDeathMono onDeathMonoEvent;
    public delegate void onDamageMono();
    public event onDamageMono onDamageMonoEvent;
    
    private void Awake()
    {
        if (DamSo != null)
        {   
            // PD - 9/19/2020
            // To avoid directly referencing an SO, if undesired, we replace the reference with 
            // a clone of it. 
            if (DamSo.shouldClone)
            {
                DamageableSO instance = Object.Instantiate(DamSo);
                DamSo = instance;
            }
            DamSo.onDeathEvent += OnDeath;
            DamSo.onDamageEvent += OnDamage;
        }
        else Debug.LogError("MISSING DamageableSO on DamageableMono class: " + this + " for gameobject: " + gameObject);
    }
    private void Start()
    {
        DamSo.set.RegisterDamageable(this, DamSo.faction);
    }
    private void OnDestroy()
    {
        DamSo.onDeathEvent -= OnDeath;
        DamSo.onDamageEvent -= OnDamage;
        DamSo.set.UnregisterDamageable(this, DamSo.faction);
    }

    private void OnDamage()
    {
        onDamageMonoEvent?.Invoke();
    }
    private void OnDeath()
    {
        onDeathMonoEvent?.Invoke();
        Debug.Log(gameObject + "<color> DIED </color>");
    }
}
