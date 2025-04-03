using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CricketBat : MonoBehaviour
{
    [SerializeField] AudioSource audioSource;

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.CompareTag("Wickets"))
        {
            GameEvents.OnPlayerOut.Invoke();
            UIHandler.Instance.SetFeedback("Hit Wicket");
        }
        else if(collision.gameObject.CompareTag("Ground"))
        {
            audioSource.Stop();
            audioSource.Play();
        }
    }
}
