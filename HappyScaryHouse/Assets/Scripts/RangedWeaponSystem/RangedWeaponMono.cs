using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(Collider2D))]
public class RangedWeaponMono : MonoBehaviour
{
    [SerializeField]
    private RangedWeaponSO WeaponSO = default;
    public RangedWeaponSO weaponSO => WeaponSO;

    private Vector3 ShotDir;
    public Vector3 shotDir => ShotDir;
    private bool ReadyForShootInput = false;
    public bool readyForShootInput => ReadyForShootInput;
    private Transform TargetTrans;
    private MonoBehaviour ShooterRef;
    
    public void RequestShooting()
    {
        WeaponSO.RequestShooting(ref ShooterRef);
        Debug.Log("Requesting SO to shoot");
    }
    public void CancelShooting()
    {
        WeaponSO.CancelShooting();
    }
    
    #region UNITY_EXECUTION_ORDER_CALLBACKS
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

            WeaponSO.onShootEvent += OnShot;
        }
        else Debug.LogError("MISSING ScriptableObject class for gameobject: " + gameObject);
    }
    private void Destroy()
    {
        WeaponSO.onShootEvent -= OnShot;
    }
    private void Update()
    {
        UpdateShotInfo();
    }
    #endregion

    private void UpdateShotInfo()
    {
        ReadyForShootInput = TargetTrans && weaponSO.isShooting == false;
        
        if (WeaponSO.shootForwardOrToTarget)
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
    private void OnShot()
    {
        Debug.Log("on shot chonker");
        GameObject bullet = GameObject.Instantiate(WeaponSO.bulletPrefab, transform.position, Quaternion.identity);
        BulletMono bMono = bullet.GetComponent<BulletMono>();
        bMono.ChangeShotDir(shotDir);
    }
    
    void OnTriggerEnter2D(Collider2D collider)
    {
        DamageableMono dam = collider.GetComponent<DamageableMono>();
        if (dam)
        {
            if (dam.damSO.faction != WeaponSO.bulletMono.damagerSO.faction)
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
