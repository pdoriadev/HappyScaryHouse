using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Player : MonoBehaviour
{
    private Rigidbody2D RB2D;
    private Vector2 MoveDir;

    public float moveSpeed;

    void Awake()
    {
        RB2D = GetComponent<Rigidbody2D>();
    }
    void Update()
    {
        MoveDir.x = Input.GetAxisRaw("Horizontal");
        MoveDir.y = Input.GetAxisRaw("Vertical");
    }

    void FixedUpdate()
    {
        RB2D.velocity = MoveDir * moveSpeed;
    }
}
