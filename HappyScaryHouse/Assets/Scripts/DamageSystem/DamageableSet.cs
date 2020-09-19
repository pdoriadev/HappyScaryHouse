using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "DamageableSet", menuName = "DamageableSystem/DamageableSet", order = 1)]
public class DamageableSet : ScriptableObject
{
    private List<DamageableMono> Damageables = new List<DamageableMono>();
    public List<DamageableMono> damageables => Damageables;
    private List<FactionSO> Factions = new List<FactionSO>();
    public List<FactionSO> factions => Factions;

    private void Awake()
    {
        Damageables.Clear();
        Factions.Clear();
    }

    public void RegisterDamageable(DamageableMono damageable, FactionSO faction)
    {
        if (!Damageables.Contains(damageable))
        {
            Damageables.Add(damageable);
            Factions.Add(faction);
        }
        else Debug.LogError("damageable " + damageable.name + " already exists in set.");
    }
    public void UnregisterDamageable(DamageableMono damageable, FactionSO faction)
    {
        if (Damageables.Contains(damageable))
        {
            int i = Damageables.IndexOf(damageable);
            Damageables.Remove(damageable);
            Factions.RemoveAt(i);
        }
        else Debug.LogError("damageable " + damageable.name + " does not exist in set.");
        
    }

}
