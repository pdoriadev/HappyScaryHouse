using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[SelectionBase]
[RequireComponent(typeof(DamageableMono))]
public class Chonker : MonoBehaviour, IInteractable
{
    #region VARIABLES
    [SerializeField]
    private float TimeToSubdue = default;
    [SerializeField]
    private SpriteRenderer SR = default;
    [SerializeField]
    private Transform LevitationTarget = default;
    [SerializeField]
    private float LevitationSpeed = default;
    [SerializeField]
    private RangedWeaponMono RangedWeaponMono = default;
    private DamageableMono DamageableMono;

    private bool IsSubdued = false;
    public bool isSubdued => IsSubdued;
    private bool IsSubdueing = false;
    private Coroutine SubdueCo;
    private Animator FatCatAnimator;
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
        FatCatAnimator = GetComponentInChildren<Animator>();
        RangedWeaponMono.onShootEvent += OnShoot;
        DamageableMono.onDeathMonoEvent += OnSubdued;
    }  
    private void OnDestroy()
    {
        RangedWeaponMono.onShootEvent -= OnShoot;
        DamageableMono.onDeathMonoEvent -= OnSubdued;
    }
#endregion

    private void FixedUpdate()
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

#region OnShootFunctionality
    private Coroutine OnShootCo;
    private void OnShoot()
    {
        FatCatAnimator.SetBool("IsShooting", true);
        OnShootCo = StartCoroutine(CoOnShoot());
    }
    private IEnumerator CoOnShoot()
    {
        float t = 0;
        while (true)
        {
            if (t > 0.4f)
            {
                FatCatAnimator.SetBool("IsShooting", false);
                StopCoroutine(OnShootCo);
            }
            t += Time.deltaTime;
            yield return null;
        }
    }
#endregion

#region SUBDUE METHODS AND COROUTINE
    private void Subdue()
    {
        DamageableMono.damSO.AddAmountToHealth(-1);
        IsSubdued = true;
        // PD - 9/21/2020
            // #TODO - prrr animation / FX
    }
    private void OnSubdued()
    {
        if (RangedWeaponMono.isShooting)
        {
            RangedWeaponMono.CancelShooting();
        }
        Levitate();

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
#endregion

#region LEVITATION FUNCTIONALITY
    private bool IsLevitating = false;
    private bool HasLevitated = false;
    private Coroutine LevitateCo;
    private void Levitate()
    {
        if (!IsLevitating && !HasLevitated)
        {
            LevitateCo = StartCoroutine(CoLevitate());
        }
        else Debug.LogWarning("Chonker already levitating or already levitated.");

        // PD - 9/22/2020
            // #TODO  - levitation FX
    }
    private void OnFinishedLevitate()
    {
        Debug.Log("Finished levitating");
        // PD - 9/22/2020
            // #TODO  - finished levitation FX
    }
    private IEnumerator CoLevitate()
    {   
        Vector3 velocity = Vector3.zero;
        float dist = Vector2.Distance(transform.position, LevitationTarget.position);
        float s = dist / ( LevitationSpeed);
        while (true)
        {
            yield return new WaitForFixedUpdate();

            transform.position = Vector3.SmoothDamp(transform.position, LevitationTarget.position, ref velocity, s);
            
            if (Mathf.Abs(LevitationTarget.position.x - transform.position.x) < 0.075f)
            {
                HasLevitated = true;
                IsLevitating = false;
                OnFinishedLevitate();
                StopCoroutine(LevitateCo);
            }
        }
    }
    #endregion
}
