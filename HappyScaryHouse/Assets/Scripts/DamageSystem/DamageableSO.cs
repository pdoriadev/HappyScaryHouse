using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "DamageableSO", menuName = "DamageSystem/DamageableSO", order = 2)]
public class DamageableSO : ScriptableObject
{
    #region ----- VARIABLES -----
    [NonSerialized]
    private int Health = default;
    public int health => Health;
    [SerializeField]
    private int MaxHealthOnStart = default;
    private int MaxHealth;
    public int maxHealth => MaxHealth;

    [SerializeField]
    private FactionSO Faction = default;
    public FactionSO faction => Faction;
    [SerializeField]
    private DamageableSet Set = default;
    public DamageableSet set => Set;
    // PD - 9/19/2020
    // - Whether the
    [SerializeField]
    private bool ShouldInstantiate = true;
    public bool shouldClone => ShouldInstantiate;

    // PD 9/19/20
    // subscribed to by DamageableMono callbacks
    public delegate void onDeath();
    public event onDeath onDeathEvent;
    public delegate void onDamage();
    public event onDamage onDamageEvent;

    #endregion

    private void OnEnable()
    {
        MaxHealth = MaxHealthOnStart;
        Health = MaxHealth;   
    }
    private void Die()
    {
        onDeathEvent?.Invoke();
    }

    public void AddAmountToHealth(int amount)
    {
        Debug.Log(name + " <color> TOOK " + amount + " DAMAGE </color>");
        Health += amount;
        Health = Health > MaxHealth ? MaxHealth : Health;
        onDamageEvent?.Invoke();
        if (Health <= 0)
            Die();
    }
    public void AddToMaxHealth(int amount)
    {
        MaxHealth += amount;
    }
    public void SetHealth(int amount)
    {
        Health = amount;
    }
}
