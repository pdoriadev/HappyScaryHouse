using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Granny : MonoBehaviour, IInteractable
{
    [SerializeField]
    private Animator Animator;

    public void Interact()
    {

    }
    public void CancelInteract()
    {

    }
    public MonoBehaviour GetMonoBehaviour()
    {
        return this;
    }

    private void Awake()
    {
        Animator = GetComponentInChildren<Animator>();
    }
}
