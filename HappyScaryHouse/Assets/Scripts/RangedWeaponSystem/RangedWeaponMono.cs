using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(Collider2D))]
public class RangedWeaponMono : MonoBehaviour
{
    #region VARS
    public delegate void OnShoot();
    public event OnShoot onShootEvent;
    [SerializeField]
    public Transform ShootTransform = default;
    [SerializeField]
    private GameObject BulletPrefab = default;
    public GameObject bulletPrefab => BulletPrefab;
    [SerializeField]
    private float FireRate = default;   
    public float fireRate => FireRate;
    [SerializeField]
    private bool IsAutomatic = true;
    public bool isAutomatic => IsAutomatic;
    // PD - 9/21/2020
    // Shoots directly toward target position at time of shooting. If not, then will shoot direction
    // weapon is pointed. 
    [SerializeField]
    private bool ShootAtTarget = false;
    public bool shootAtTarget => ShootAtTarget;
    [SerializeField]
    private float sightRange = default;
    public float ShootingRange => sightRange;

    private LayerMask SightLayers;
    private Vector3 ShotDir;
    public Vector3 shotDir => ShotDir;
    private bool ReadyForShootInput = false;
    public bool readyForShootInput => ReadyForShootInput;
    private bool IsFirstShot = false;
    public bool isFirstShot => IsFirstShot;
    private BulletMono BulletMono = null;
    public BulletMono bulletMono => BulletMono;
    private Transform TargetTrans;
    private Coroutine ShootingCo;
    #endregion

    public bool RequestShooting()
    {
        if (CanSeeTarget())
        {    
            if (!IsAutomatic)
            {
                Shoot();
                IsFirstShot = true;
                return true;
            }
            else if (IsShooting == false)
            {
                ShootingCo = StartCoroutine(CoShoot());
                IsFirstShot = true;
                return true;
            }
            else 
            {
                Debug.LogWarning("Weapon already shooting");
                return false;
            }
        }
        return false;
    }

    public void CancelShooting()
    {
        if (IsShooting)
        {
            IsShooting = false;
            StopCoroutine(ShootingCo);
            ShootingCo = null;
        }
        else Debug.LogWarning("Weapon isn't shooting right now");

    }

    #region UNITY_EXECUTION_ORDER_CALLBACKS

    private void Awake()
    {
        BulletMono = BulletPrefab.GetComponent<BulletMono>();
        if (BulletMono == null)
            Debug.LogError("No BulletMono on bullet prefab");

    }
    private void Update()
    {
        UpdateShotInfo();
    }
    #endregion
    
    private bool IsShooting = false;
    public bool isShooting => IsShooting;
    private float TimeTilShoot = 0;
    private IEnumerator CoShoot()
    {
        IsShooting = true;
        float t = 0;
        while (IsShooting)
        {
            t -= Time.deltaTime;
            TimeTilShoot = t;
            if (t <= 0)
            {
                Shoot();
                t = FireRate;
            }
            yield return null;
        }
    }
    protected virtual void Shoot()
    {
        GameObject bullet;
        if (ShootTransform)
        {
            bullet = GameObject.Instantiate(bulletPrefab, ShootTransform.position, Quaternion.identity);
        }
        else 
            bullet = GameObject.Instantiate(bulletPrefab, transform.position, Quaternion.identity);
            
        BulletMono bMono = bullet.GetComponent<BulletMono>();
        bMono.ChangeShotDir(shotDir);
        
        onShootEvent?.Invoke();

        if (IsFirstShot)
            IsFirstShot = false;
    }
    private void UpdateShotInfo()
    {
        ReadyForShootInput = TargetTrans && IsShooting == false;

        if (!shootAtTarget)
        {
            ShotDir = transform.right;
        }
        else if (TargetTrans != null)
        {
            ShotDir = new Vector2 (TargetTrans.position.x - transform.position.x,
                                        TargetTrans.position.y - transform.position.y)
                                        .normalized;
        }
    }
    private bool CanSeeTarget()
    {
        bool seesTarget = false;
        if (!shootAtTarget)
        {
            seesTarget = true;
        }
        else if (TargetTrans != null)
        {
            int wallLayer = 8;
            int playerLayer = 9;
            LayerMask wallMask = 1 << wallLayer;
            LayerMask playerMask = 1 << playerLayer;
            SightLayers = wallMask | playerMask;

            RaycastHit2D hit2D;
            hit2D = Physics2D.Raycast(transform.position, shotDir, sightRange, SightLayers);
            if (hit2D.collider != null)
            {
                if (hit2D.collider.gameObject.tag == "Player")
                {
                    seesTarget = true;
                }
            }
        }

        return seesTarget;
    }
    public float GetShootTime()
    {
        return TimeTilShoot;
    }
    
    void OnTriggerEnter2D(Collider2D collider)
    {
        DamageableMono dam = collider.GetComponent<DamageableMono>();
        if (dam)
        {
            if (dam.damSO.faction != bulletMono.damagerSO.faction)
            {
                TargetTrans = dam.transform;
            }
        }
    }
    void OnTriggerExit2D(Collider2D collider)
    {
        DamageableMono dam = collider.GetComponent<DamageableMono>();
        if (dam)
        {
            if (TargetTrans == dam.transform)
            {
                TargetTrans = null;
            }
        }
    }
    
}
