using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class MeteorGun : MonoBehaviour
{
    [SerializeField] ParticleSystem particles;
    [SerializeField] LayerMask layerMask;
    [SerializeField] Transform sourceTransform;
    [SerializeField] float distance;

    private bool isShooting = false;

    private void Start()
    {
        XRGrabInteractable grabInteractable = GetComponent<XRGrabInteractable>();
        grabInteractable.activated.AddListener(x => StartShooting());
        grabInteractable.deactivated.AddListener(x => StopShooting());
    }


    private void StartShooting()
    {
        particles.Play();
        isShooting = true;
    }

    private void StopShooting()
    {
        particles.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
        isShooting = false;
    }

    private void Update()
    {
        if(isShooting)
        {
            RaycastCheck();
        }
    }

    private void RaycastCheck()
    {
        RaycastHit hit;
        bool hasHit = Physics.Raycast(sourceTransform.position, sourceTransform.forward, out hit, distance, layerMask);

        if(hasHit)
        {
            hit.transform.SendMessage("Break", SendMessageOptions.DontRequireReceiver);
        }
    }
}
