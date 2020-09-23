using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Camera))]
public class PlayerCamera : MonoBehaviour
{
    [SerializeField]
    private Transform Target = default;
    [SerializeField]
    private Vector3 Offset = default;
    [SerializeField]
    private float SmoothDampTime = 0.3f;
    private Vector3 velocity = Vector3.zero;

    void Awake()
    {
        if (Target == null)
            Target = GameObject.FindGameObjectWithTag("Player").transform;
    }

    void LateUpdate()
    {
        Vector3 nextPos = Target.position + Offset;
        transform.position = Vector3.SmoothDamp(transform.position, nextPos, ref velocity, SmoothDampTime);
    }
}
