using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateCanvasAroundPlayer : MonoBehaviour
{
    [SerializeField] float distance = 5f;
    [SerializeField] float moveSpeed = 5f;  // Speed of the smooth movement

    private void Update()
    {
        Transform camTr = Camera.main.transform;

        // Calculate the target position (on the horizontal plane)
        Vector3 horizontalCamForward = camTr.forward;
        horizontalCamForward.y = 0;
        horizontalCamForward.Normalize();
        Vector3 targetPosition = camTr.position + horizontalCamForward * distance;

        // Smoothly move towards the target position
        transform.position = Vector3.Lerp(transform.position, targetPosition, moveSpeed * Time.deltaTime);

        // Instantly align rotation with camera's Y rotation
        transform.rotation = Quaternion.Euler(0, camTr.rotation.eulerAngles.y, 0);
    }
}

