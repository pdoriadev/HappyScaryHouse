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

    private Collector Collector;
    private Rigidbody2D RB2D;
    private bool ShouldAttack = false;
    private bool ShouldInteract = false;
    private Vector2 MoveDir;
    private Vector3 DropPos;
    private float DirLastFacing;

    private Pickup PickupInRange = null;

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
        ShouldInteract = Input.GetButtonUp("Interact");

        if (ShouldInteract && PickupInRange)
        {
            DamagerMono damager = PickupInRange.GetPickup().GetComponent<DamagerMono>();
            if (damager != null)
            {
                PickupWeapon(damager);
            }
        }
        else if (ShouldInteract && PickupInRange == false && Damager)
        {
            DropWeapon();
        }

        if(MoveDir.x > 0)
            transform.localScale = Vector3.one;
        else if (MoveDir.x < 0)
            transform.localScale = -Vector3.one;
        
        if (MoveDir.x != 0)
            DirLastFacing = MoveDir.x;
        if (MoveDir.y!= 0)
            DirLastFacing = MoveDir.y;

        DropPos = transform.position;
        DropPos = new Vector3(MoveDir.x + transform.position.y, MoveDir.y + transform.position.y, transform.position.z);
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
        DropWeapon();
        Damager = GameObject.Instantiate(damager, DominantHand.transform);
    }
    private void DropWeapon()
    {
        if (Damager != null)
        {
            Pickup pToMatch = Damager.data.pickup.GetComponent<Pickup>();
            Pickup p = pToMatch.pool.TakePooledPickup(pToMatch); 
            if (p)
            {
                p.transform.position = DropPos;
            }
            else
                GameObject.Instantiate(Damager.data.pickup, DropPos, Quaternion.identity);

            Damager.gameObject.SetActive(false);   
        }
        Damager = null;
    }

    void OnTriggerEnter2D(Collider2D collider)
    {
        Pickup p = collider.GetComponent<Pickup>();
        if (p != null)
        {
            PickupInRange = p;
        }
    }
    void OnTriggerExit2D(Collider2D collider)
    {
        Pickup p = collider.GetComponent<Pickup>();
        if (p != null)
        {
            if (p == PickupInRange)
            {
                PickupInRange = null;
            }
        }
    }



}
