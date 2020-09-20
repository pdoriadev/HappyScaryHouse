using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "DamagerSet", menuName = "DamageSystem/DamagerSet", order = 2)]
public class DamagerSet : ScriptableObject
{
    private List<DamagerMono> PooledDamagers = new List<DamagerMono>();
    public List<DamagerMono> pooledDamagers => PooledDamagers;

    public void AddPooledDamager(DamagerMono damager)
    {
        damager.gameObject.SetActive(false);
        PooledDamagers.Add(damager);
    }
    public DamagerMono TakePooledDamager(DamagerMono damager)
    {
        for (int i = 0; i < PooledDamagers.Count; i++)
        {
            DamagerMono dam = PooledDamagers[i];
            if (dam.GetType() == damager.GetType())
            {
                PooledDamagers.RemoveAt(i);
                return dam;
            }
        }

        return null;
    }
}
