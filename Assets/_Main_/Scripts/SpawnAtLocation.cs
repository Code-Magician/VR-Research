using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;

public class SpawnAtLocation : MonoBehaviour
{
    [SerializeField] Transform spawnTr;

    private void Start()
    {
        transform.position = spawnTr.position;
    }

    private void Update()
    {
        InputDevice rightHand = InputDevices.GetDeviceAtXRNode(XRNode.RightHand);

        if (rightHand.TryGetFeatureValue(CommonUsages.secondaryButton, out bool secondaryButton) && secondaryButton)
        {
            Debug.Log("Spawn at location");
            transform.position = spawnTr.position;
        }
    }
}
