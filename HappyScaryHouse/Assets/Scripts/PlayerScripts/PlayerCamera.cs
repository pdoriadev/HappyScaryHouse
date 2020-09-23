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
    private float SmoothDampTime = 0.1f;
    [SerializeField]
    private float LookAheadDist = default;
    [SerializeField]
    private float MinDist = 0.0005f;
    private Vector3 Velocity = Vector3.zero;
    private Vector3 LookAheadDir;
    private Vector3 TargetLastPos;


    void Awake()
    {
        if (Target == null)
            Target = GameObject.FindGameObjectWithTag("Player").transform;
        TargetLastPos = Target.position;
    }

    float LookDistPercentage = 0;
    void FixedUpdate()
    {
        LookAheadDir = (Target.position - TargetLastPos);
        float distFromTarget = new Vector2(Target.position.x - transform.position.x,
                                             Target.position.y - transform.position.y)
                                             .sqrMagnitude;
        if (Mathf.Abs(distFromTarget) > MinDist)
        {
            if (LookDistPercentage < 1)
                LookDistPercentage += Time.fixedDeltaTime;

            LookAheadDir = LookAheadDir.normalized;
            Vector3 nextPos = Target.position + (LookAheadDir * LookAheadDist * LookDistPercentage) + Offset;
            transform.position = Vector3.SmoothDamp(transform.position, nextPos, ref Velocity, SmoothDampTime);
            
            TargetLastPos = Target.position;
        }
        else 
        {
            if (LookDistPercentage > 0)
                LookDistPercentage -= Time.fixedDeltaTime;
        }
    }
}
