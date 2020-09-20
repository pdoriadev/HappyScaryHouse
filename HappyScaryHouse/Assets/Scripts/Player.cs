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
    private bool ShouldAttack = false;
    private Collector Collector;
    private Rigidbody2D RB2D;
    private Vector2 MoveDir;

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
    }
    void FixedUpdate()
    {
        RB2D.velocity = MoveDir * moveSpeed;
        if (ShouldAttack && Damager != null)
        {
            Damager.Attack();
        }
    }



}
