using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class RangedWeaponSO : ScriptableObject
{
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
    private bool ShootToTarget = false;
    public bool shootForwardOrToTarget => ShootToTarget;

    private BulletMono BulletMono = null;
    public BulletMono bulletMono => BulletMono;

    public delegate void OnShoot();
    public event OnShoot onShootEvent;

    void OnEnable()
    {
        BulletMono = BulletPrefab.GetComponent<BulletMono>();
        if (BulletMono == null)
            Debug.LogError("No BulletMono on bullet prefab");
    }

    private MonoBehaviour Shooter = null;
    private bool IsShooting = false;
    public bool isShooting => IsShooting;
    private IEnumerator CoShoot()
    {
        Debug.Log("co started");
        float t = 0;
        while (true)
        {
            Debug.Log("Co iterating");
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
        onShootEvent?.Invoke();
    }
    public void RequestShooting(ref MonoBehaviour shooter)
    {
        Debug.Log("Trying to shoot");
        Shooter = shooter;
        if (!IsAutomatic)
        {
            Shoot();
        }
        else if (IsShooting == false)
        {
            Shooter.StartCoroutine(CoShoot());
            IsShooting = true;
        }
        else 
            Debug.LogWarning("Weapon already shooting");
    }
    public void CancelShooting()
    {
        if (IsShooting)
        {
            if (Shooter == null)
                Debug.LogError("Shooter is null?!");
            Shooter.StopCoroutine(CoShoot());
            IsShooting = false;
            Shooter = null;
        }
        else Debug.LogWarning("Weapon isn't shooting right now");

    }

}
