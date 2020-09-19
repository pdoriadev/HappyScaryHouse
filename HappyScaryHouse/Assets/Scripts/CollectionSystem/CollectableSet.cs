using System.Collections;
using System.Collections.Generic;
using UnityEngine;
// referenced by UI. potentially other systems. 
[CreateAssetMenu(fileName = "CollectableSet", menuName = "CollectableSystem/CollectableSet", order = 1)]
public class CollectableSet : ScriptableObject
{
    private List<CollectableMono> Uncollected = new List<CollectableMono>();
    public List<CollectableMono> uncollected => Uncollected;
    private List<CollectableMono> Collected = new List<CollectableMono>();
    public List<CollectableMono> collected => Collected;

    private void Awake()
    {
        Uncollected.Clear();
        Collected.Clear();
    }

    // PD - 9/19/2020
    // Every collectable calls this on creation. 
    public void AddUncollected(CollectableMono collectable)
    {
        Collected.Remove(collectable);
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

    void OnDisable()
    {

        Uncollected.Clear();
        Collected.Clear();
    }

}
