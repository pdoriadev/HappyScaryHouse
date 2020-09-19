using System.Collections;
using System.Collections.Generic;
using UnityEngine;
// referenced by UI. potentially other systems. 
[CreateAssetMenu]
public class CollectableSet : ScriptableObject
{
    private List<CollectableMono> Uncollected = new List<CollectableMono>();
    private List<CollectableMono> Collected = new List<CollectableMono>();

    // PD - 9/19/2020
    // Every collectable calls this on creation. 
    public void AddUncollected(CollectableMono collectable)
    {
        for (int i = 0; i < Uncollected.Count; i++)
        {
            Debug.Log("num of collectables is " + i + ". Collectable is: " + collectable.gameObject.name);
        }
        if (!Uncollected.Contains(collectable))
            Uncollected.Add(collectable);
        else Debug.LogError("attempted to add collectable " + collectable.name + " that's already been added.");
    }
    public void AddCollected(CollectableMono collectable)
    {
        Uncollected.Remove(collectable);
        if (!Collected.Contains(collectable))
            Collected.Add(collectable);
        else Debug.LogError("attempted to add collectable " + collectable.name + " that's already been added.");
    }

    private void OnEnable()
    {
        Uncollected.Clear();
        Collected.Clear();
    }
}
