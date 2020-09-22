using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(Collider2D))]
public class RangedWeaponMono : MonoBehaviour
{

    private Vector3 ShotDir;
    public Vector3 shotDir => ShotDir;
    private bool ReadyForShootInput = false;
    public bool readyForShootInput => ReadyForShootInput;
    private Transform TargetTrans;
    private MonoBehaviour ShooterRef;

    [SerializeField]
    private GameObject BulletPrefab = default;
    public GameObject bulletPrefab => BulletPrefab;
    [SerializeField]
    private float FireRate = default;   
    [SerializeField]
    private bool IsAutomatic = true;
    public bool isAutomatic => IsAutomatic;
    
    // PD - 9/21/2020
        // Shoots directly toward target position at time of shooting. If not, then will shoot direction
        // weapon is pointed. 
    [SerializeField]
    private bool ShootAtTarget = false;
    public bool shootAtTarget => ShootAtTarget;

    private BulletMono BulletMono = null;
    public BulletMono bulletMono => BulletMono;

    public delegate void OnShoot();
    public event OnShoot onShootEvent;

    private Coroutine ShootingCo;
    public void RequestShooting()
    {
        if (!IsAutomatic)
        {
            Shoot();
        }
        else if (IsShooting == false)
        {
            ShootingCo = StartCoroutine(CoShoot());
        }
        else 
            Debug.LogWarning("Weapon already shooting");
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
    
    void OnEnable()
    {
        BulletMono = BulletPrefab.GetComponent<BulletMono>();
        if (BulletMono == null)
            Debug.LogError("No BulletMono on bullet prefab");
    }
    private void Awake()
    {
        ShooterRef = this;
        if (WeaponSO != null)
        {  
             // PD - 9/21/2020   
                // To avoid directly referencing an SO we replace the reference with 
                // a clone of it. 
            RangedWeaponSO instance = Object.Instantiate(WeaponSO);
            WeaponSO = instance;

            // WeaponSO.onShootEvent += OnShot;
        }
        else Debug.LogError("MISSING ScriptableObject class for gameobject: " + gameObject);
    }
    private void Destroy()
    {
        // WeaponSO.onShootEvent -= OnShot;
    }
    private void Update()
    {
        UpdateShotInfo();
    }
    #endregion

    
    private bool IsShooting = false;
    public bool isShooting => IsShooting;
    private IEnumerator CoShoot()
    {
        IsShooting = true;
        Debug.Log("shoooooooot");
        float t = 0;
        while (IsShooting)
        {
            t -= Time.deltaTime;
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
        GameObject bullet = GameObject.Instantiate(bulletPrefab, transform.position, Quaternion.identity);
        BulletMono bMono = bullet.GetComponent<BulletMono>();
        bMono.ChangeShotDir(shotDir);
        
        onShootEvent?.Invoke();
    }
    private void UpdateShotInfo()
    {
        // ReadyForShootInput = TargetTrans && weaponSO.isShooting == false;

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
