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
            GameEvents.OnPlayerOut();
            UIHandler.Instance.SetScoreBoard(0);
            GameEvents.OnPlayerOut();
        }
        else if(collision.gameObject.CompareTag("Ground"))
        {
            Debug.Log(collision.gameObject.tag);
            audioSource.Stop();
            audioSource.Play();
        }
    }
}
