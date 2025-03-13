using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateCanvasAroundPlayer : MonoBehaviour
{
    [SerializeField] float distance;

    private void Update()
    {
        Transform camTr = Camera.main.transform;
        Vector3 horizontalCamForward = camTr.forward;
        horizontalCamForward.y = 0;
        horizontalCamForward.Normalize();

        transform.position = camTr.position + horizontalCamForward * distance;
        transform.rotation = Quaternion.Euler(0, camTr.rotation.eulerAngles.y, 0);
    }
}
