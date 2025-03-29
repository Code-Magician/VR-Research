using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallLookCamera : MonoBehaviour
{
    private Transform target;
    private Quaternion initialRotation;
    private Camera cam;
    private float initialFOV;
    public float zoomFOV = 10f; // Adjust the zoom level
    public float zoomDuration = 2f; // Smooth transition duration

    private void OnEnable()
    {
        GameEvents.OnPlayerHitBall += OnPlayerHitBall;
    }

    private void OnDisable()
    {
        GameEvents.OnPlayerHitBall -= OnPlayerHitBall;
    }

    private void Awake()
    {
        initialRotation = transform.rotation;
        cam = GetComponent<Camera>();

        if (cam != null)
        {
            initialFOV = cam.fieldOfView;
        }
    }

    private void OnPlayerHitBall(Transform ballTr)
    {
        target = ballTr;

        if (cam != null)
        {
            cam.DOFieldOfView(zoomFOV, zoomDuration); // Smooth zoom in
        }
    }

    private void LateUpdate()
    {
        if (target != null)
        {
            transform.DOLookAt(target.position, zoomDuration);
        }
        else
        {
            transform.DORotate(initialRotation.eulerAngles, zoomDuration);

            if (cam != null)
            {
                cam.DOFieldOfView(initialFOV, zoomDuration); // Smooth return to original FOV
            }
        }
    }
}
