using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "DamageableSO", menuName = "DamageableSystem/DamageableSO", order = 2)]
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

    // PD 9/19/20
    // subscribed to by DamageableMono's OnDeath() function
    public delegate void OnDeath();
    public event OnDeath OnDeathEvent;
    #endregion

    private void OnEnable()
    {
        MaxHealth = MaxHealthOnStart;
        Health = MaxHealth;   
    }
    private void Die()
    {
        OnDeathEvent?.Invoke();
    }

    public void AddAmountToHealth(int amount)
    {
        Health += amount;
        Health = Health > MaxHealth ? MaxHealth : Health;
        Debug.Log(Health);
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
