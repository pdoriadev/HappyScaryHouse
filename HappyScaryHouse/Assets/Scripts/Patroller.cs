using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Patroller : MonoBehaviour
{
    public float minDist = 2;
    public float smoothTime = 1;
    [SerializeField]
    private Transform[] PatrolPoints = default;
    private Transform TargetPoint;
    private int NextPointIndex;


    void Awake()
    {
        if (PatrolPoints.Length == 0)
        {
            Debug.LogError("No patrol points to patrol to for " + gameObject.name);
        }
    }
    void Start()
    {
        float closestDist = float.MaxValue;
        for (int i = 0; i < PatrolPoints.Length; i++)
        {
            float dist = (transform.position - PatrolPoints[i].position).sqrMagnitude;
            if (dist < closestDist)
            {
                closestDist = dist;
                TargetPoint = PatrolPoints[i];
                NextPointIndex = i + 1 < PatrolPoints.Length ? ++i  : 0;
            }
        }

    }

    Vector3 Velocity = Vector3.zero;
    Vector3 CurvePos = Vector3.forward;
    void FixedUpdate()
    {
  
        float distToTargetPoint = (transform.position - TargetPoint.position).sqrMagnitude;
        Vector3 newPos = Vector3.zero;
        
        if (distToTargetPoint < minDist * minDist)
        {
            // #TODO  - add curve functionality
            TargetPoint = PatrolPoints[NextPointIndex];
            NextPointIndex = NextPointIndex + 1 < PatrolPoints.Length ? ++NextPointIndex : 0;
        }
        newPos = Vector3.SmoothDamp(transform.position, TargetPoint.position, ref Velocity, smoothTime) ;

        transform.position = newPos;

 
    }   
}
