using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Camera))]
public class PlayerCamera : MonoBehaviour
{
    private Camera Cam;
    private Vector3 velocity = Vector3.zero;

    [SerializeField]
    private Transform Target = default;
    [SerializeField]
    private Vector3 Offset = default;
    [SerializeField]
    private float SmoothDampTime = 0.3f;

    void Awake()
    {
        Cam = GetComponent<Camera>();
        if (Target == null)
            Debug.LogError("Target is null");
        Offset = Cam.transform.position - Target.position;
    }


    void LateUpdate()
    {
        Vector3 nextPos = Target.position + Offset;
        Cam.transform.position = Vector3.SmoothDamp(transform.position, nextPos, ref velocity, SmoothDampTime);
    }


}
