using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Collider2D))]
[RequireComponent(typeof(Collector))]
public class Player : MonoBehaviour
{
    [SerializeField]
    private DamagerMono Damager = default;
    [SerializeField]
    private GameObject DominantHand = default;
    private bool ShouldAttack = false;
    private Collector Collector;
    private Rigidbody2D RB2D;
    private Vector2 MoveDir;
    private float DirLastFacing;

    public float moveSpeed;

    void Awake()
    {
        Collector = GetComponent<Collector>();
        RB2D = GetComponent<Rigidbody2D>();
    }
    void Update()
    {
        MoveDir.x = Input.GetAxisRaw("Horizontal");
        MoveDir.y = Input.GetAxisRaw("Vertical");
        ShouldAttack = Input.GetButtonDown("Fire1");

        if(MoveDir.x > 0)
            transform.localScale = Vector3.one;
        else if (MoveDir.x < 0)
            transform.localScale = -Vector3.one;
        
        DirLastFacing = MoveDir.x;
    }
    void FixedUpdate()
    {
        RB2D.velocity = MoveDir * moveSpeed;
        if (ShouldAttack && Damager != null)
        {
            Damager.Attack();
        }
    }
    private void PickupWeapon(DamagerMono damager)
    {
        Damager = GameObject.Instantiate(damager, DominantHand.transform);
    }
    private void DropWeapon()
    {
        
    }

    void OnTriggerEnter2D(Collider2D collider)
    {
        Pickup p = collider.GetComponent<Pickup>();
        if (p != null)
        {
            DamagerMono damager = p.GetPickup().GetComponent<DamagerMono>();
            if (damager != null)
            {
                PickupWeapon(damager);
            }
        }
    }



}
