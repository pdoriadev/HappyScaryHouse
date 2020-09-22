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
    private RangedWeaponMono RangedWeaponMono = default;
    private DamageableMono DamageableMono;

    private bool IsSubdued = false;
    public bool isSubdued => IsSubdued;
    private bool IsSubdueing = false;
    private Coroutine SubdueCo;
    #endregion

    #region IINTERACTABLE_METHODS
    public void Interact()
    {
        if (!IsSubdued)
        {
            if (!IsSubdueing)
            {
                SubdueCo = StartCoroutine(CoSubdueProcess());            
            }
        }
    }
    public void CancelInteract()
    {
        if (IsSubdueing)
        {
            IsSubdueing = false;
            StopCoroutine(SubdueCo);
            SubdueCo = null;
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
        DamageableMono.OnDeathMonoEvent += OnSubdued;
    }  
    private void OnDestroy()
    {
        DamageableMono.OnDeathMonoEvent -= OnSubdued;
    }
#endregion

    private void Update()
    {
        ShootChecks();
    }
    private void ShootChecks()
    {
        if (IsSubdued == false)
        {
            if (IsSubdueing == false)
            {
                if (RangedWeaponMono.readyForShootInput)
                {
                    RangedWeaponMono.RequestShooting();
                }
            }
            else 
            {
                if (RangedWeaponMono.isShooting)
                {
                    RangedWeaponMono.CancelShooting();
                }
            }
        }
    }
    private void Subdue()
    {
        DamageableMono.damSO.AddAmountToHealth(-1);
        IsSubdued = true;
        // PD - 9/21/2020
            // #TODO - prrr animation / FX
    }
    private void OnSubdued()
    {
        Debug.Log("Is subdued");
        if (RangedWeaponMono.isShooting)
        {
            RangedWeaponMono.CancelShooting();
        }
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
