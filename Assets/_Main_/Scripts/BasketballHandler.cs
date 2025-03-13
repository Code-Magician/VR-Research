using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasketballHandler : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.CompareTag("Basketball Collider"))
        {
        }
    }
}
