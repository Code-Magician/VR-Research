using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class TriggerZone : MonoBehaviour
{
    public string targetTag;
    public UnityEvent<GameObject> OnEventTrigger;


    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.CompareTag(targetTag))
        {
            OnEventTrigger.Invoke(other.gameObject);
        }
    }
}
