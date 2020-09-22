using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(DamageableMono))]
public class Chonker : MonoBehaviour, IInteractable
{
    #region VARIABLES
    [SerializeField]
    private SpriteRenderer SR = default;
    [SerializeField]
    private float TimeToSubdue = default;
    [SerializeField]
    private RangedWeaponMono RangedWeaponMono;
    private DamageableMono DamageableMono;

    private bool IsSubued = false;
    public bool isSubdued => IsSubued;
    private bool IsSubdueing = false;
    #endregion

    #region IINTERACTABLE_METHODS
    private bool IsInteracting = false;
    public void Interact()
    {
        if (!IsSubued)
        {
            if (IsInteracting)
            {
                StopCoroutine(CoSubdueProcess());            
            }
            else
            {
                StartCoroutine(CoSubdueProcess());
                IsInteracting = true;
            }
        }
    }
    public MonoBehaviour GetMonoBehaviour()
    {
        return this;
    }
    #endregion

    #region AWAKE() AND DESTROY() CALLBACKS
    private void Awake()
    {
        if (SR == null)
            Debug.Log("Yo where's my sprite renderer");
        DamageableMono = GetComponent<DamageableMono>();
        DamageableMono.damSO.onDeathEvent += OnSubdued;
    }  
    private void OnDestroy()
    {
        DamageableMono.damSO.onDeathEvent -= OnSubdued;
    }
#endregion

    private void Update()
    {
        if (!IsSubued && !IsSubdueing)
        {
            if ( RangedWeaponMono.readyForShootInput)
            {
                RangedWeaponMono.RequestShooting();
                Debug.Log("Shoot plz");
            }
        }
        else if (IsSubued || IsSubdueing)
        {
            RangedWeaponMono.CancelShooting();
        }
    }
    private void Subdue()
    {
        DamageableMono.damSO.AddAmountToHealth(-1);
        // PD - 9/21/2020
            // #TODO - prrr animation / FX
    }
    private void OnSubdued()
    {
        IsSubued = true;
        // PD - 9/21/2020
            // #TODO  - levitation functionality
    }
    private IEnumerator CoSubdueProcess()
    {
        float t = 0;
        IsSubdueing = true;
        while (IsSubdueing)
        {
            t += Time.deltaTime;
            if (t >= TimeToSubdue)
            {
                Subdue();
                IsSubdueing = false;
            }

            yield return null;
        }
    }
}
