using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractablesTracker : MonoBehaviour
{
    private List<MonoBehaviour> InteractableMonos = new List<MonoBehaviour>();
    private List<IInteractable> InteractablesNearby = new List<IInteractable>();
    public List<IInteractable> interactablesNearby => InteractablesNearby;

    public IInteractable GetNearestInteractable()
    {
        IInteractable closest = null;
        float closestDist = float.MaxValue;
        for (int i = 0; i < interactablesNearby.Count; i++)
        {
            float dist = (InteractableMonos[i].transform.position - transform.position).sqrMagnitude; 
            if (dist < closestDist)
            {
                closest = InteractablesNearby[i];
                closestDist = dist;
            }
        }
        return closest;
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        IInteractable inter = collider.GetComponent<IInteractable>();
        if (inter != null)
        {
            if (!InteractablesNearby.Contains(inter))
            {
                InteractablesNearby.Add(inter);
                InteractableMonos.Add(inter.GetMonoBehaviour());
            }
        }
    }
    private void OnTriggerExit2D(Collider2D collider)
    {
        IInteractable inter = collider.GetComponent<IInteractable>();
        if (inter != null)
        {
            if (InteractablesNearby.Contains(inter))
            {
                InteractableMonos.Remove(inter.GetMonoBehaviour());
                InteractablesNearby.Remove(inter);
            }
        }
    }
}
