using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class Collector : MonoBehaviour
{
    private List<CollectableMono> Collected = new List<CollectableMono>();
    public List<CollectableMono> collected => Collected;

    void OnTriggerEnter2D(Collider2D collider)
    {
        CollectableMono collectable = collider.gameObject.GetComponent<CollectableMono>();
        if (collectable != null)
        {
            Collect(collectable);
            collectable.OnCollect();
        }
    }

    private void Collect(CollectableMono collectable)
    {
        Collected.Add(collectable);
    }
}
