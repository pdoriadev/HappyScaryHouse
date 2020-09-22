using System.Collections;
using System.Collections.Generic;
using UnityEngine;
// PD 9/20/20
[CreateAssetMenu]
public class PickupPoolSO : ScriptableObject
{
    #if UNITY_EDITOR
    [Multiline]
    public string Description = "";
    #endif
    private List<Pickup> PooledPickups = new List<Pickup>();

    void OnEnable()
    {
        PooledPickups.Clear();
    }
    void OnDisable()
    {
        PooledPickups.Clear();
    }

    // PD - 9/20/2020
    // Returns true if pickup of matching type exists in pool
    private Pickup GetPooledPickup(Pickup pickup)
    {
        for (int i = 0; i < PooledPickups.Count; i++)
        {    
            Pickup p = PooledPickups[i];
            if (pickup.GetType() == p.GetType())
            {
                return PooledPickups[i];
            }   
        }
        return null;
    }

    public void PoolAPickup(Pickup pickup)
    {
        if (pickup.isInScene)
        {
            if (!PooledPickups.Contains(pickup))
            {
                PooledPickups.Add(pickup);
                Debug.Log("pooled");
            }
            else 
                Debug.LogError("Attempting to pool object that already exists in pool: " + pickup);
        }
        else 
            Debug.LogError("Attempting to pool object that does not exist in scene: " + pickup);
    }

    public Pickup TakePooledPickup(Pickup pickup)
    {
        Pickup p = GetPooledPickup(pickup);
        if (p)
        {
            PooledPickups.Remove(p);
            p.gameObject.SetActive(true);
            return p;
        }
        return null;
    }
    
}
