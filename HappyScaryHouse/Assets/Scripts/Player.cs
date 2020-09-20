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

        if (MoveDir.x != 0 && (DirLastFacing < MoveDir.x || DirLastFacing > MoveDir.x)) 
            transform.localScale *= -1;
        
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



}
