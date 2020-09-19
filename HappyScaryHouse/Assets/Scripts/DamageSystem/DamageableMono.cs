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
    

    private void Awake()
    {
        if (Data != null)
        {
            Data.OnDeathEvent += OnDeath;
        }
        else Debug.LogError("MISSING DamageableSO on DamageableMono class: " + this + " for gameobject: " + gameObject);
    }
    private void OnDestroy()
    {
        Data.OnDeathEvent -= OnDeath;
        Data.set.UnregisterDamageable(this, Data.faction);
    }
    private void Start()
    {
        Data.set.RegisterDamageable(this, Data.faction);
    }
    private void OnDeath()
    {
        OnDeathMonoEvent?.Invoke();
        Destroy(gameObject);
        Debug.Log(gameObject + "<color> DIED </color>");
    }

}
