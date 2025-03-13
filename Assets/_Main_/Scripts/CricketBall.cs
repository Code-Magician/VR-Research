using System.Collections;
using UnityEngine;

public class CricketBall : MonoBehaviour
{
    [SerializeField] Rigidbody rb;
    [SerializeField] AudioSource audioSource;
    [SerializeField] float extraForce = 2f;
    [SerializeField] float waitTime;
    [SerializeField] int distOneRun, distTwoRun, distThreeRun;

    private string previousCollidedTag = null;
    private bool hasHitBat = false;

    private void Awake()
    {
        rb.isKinematic = true;
        rb.velocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
    }
    public void SetVelocity(float speed)
    {
        rb.isKinematic = false;
        rb.velocity = transform.forward * speed;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Wickets"))
        {
            GameEvents.OnPlayerOut.Invoke();
            GameEvents.HandleHighScore();

            Destroy(gameObject);
        }
        else if (previousCollidedTag == null)
        {
            if (collision.gameObject.CompareTag("Bat")) previousCollidedTag = "Bat";
            else if (collision.gameObject.CompareTag("Ground")) previousCollidedTag = "Ground";
            else if(collision.gameObject.CompareTag("Outside Ground"))
            {
                UIHandler.Instance.SetScoreBoard(0);
                GameEvents.OnBallMiss.Invoke();
                Destroy(gameObject);
            }
        }
        else if(collision.gameObject.CompareTag("Ground"))
        {
            previousCollidedTag = "Ground";
        }
        else if(collision.gameObject.CompareTag("Bat") || 
                collision.gameObject.CompareTag("Left Hand") ||
                collision.gameObject.CompareTag("Right Hand") ||
                collision.gameObject.CompareTag("Player"))
        {
            hasHitBat = true;
            previousCollidedTag = "Bat";

            audioSource.Stop();
            audioSource.Play();

            // Applying extra force
            if (rb != null && collision.contactCount > 0)
            {
                Vector3 batVelocity = Vector3.zero;
                if(collision.gameObject.CompareTag("Bat"))
                {
                    Rigidbody batRb = collision.gameObject.GetComponent<Rigidbody>();
                    batVelocity = batRb.velocity;
                }
                // Get the first contact point normal
                ContactPoint contact = collision.GetContact(0);
                Vector3 normal = contact.normal;

                // Calculate the additional force
                Vector3 extraBounceForce = normal * extraForce + batVelocity;

                // Add the force to the existing velocity
                rb.velocity += extraBounceForce;
            }

            GameEvents.OnPlayerHitBall.Invoke();
            StartCoroutine(DestroyBallAfter());
        }
        else if(collision.gameObject.CompareTag("Outside Ground"))
        {
            if(hasHitBat)
            {
                if(previousCollidedTag == "Bat")
                {
                    UIHandler.Instance.SetScoreBoard(6);
                    Destroy(gameObject);
                }
                else if(previousCollidedTag == "Ground")
                {
                    UIHandler.Instance.SetScoreBoard(4);
                    Destroy(gameObject);
                }
            }
            else
            {
                UIHandler.Instance.SetScoreBoard(0);
                GameEvents.OnBallMiss.Invoke();
                Destroy(gameObject);
            }
        }
    }

    public IEnumerator DestroyBallAfter()
    {
        yield return new WaitForSeconds(waitTime);

        if(hasHitBat)
        {
            Vector3 playerPos = GameObject.FindGameObjectWithTag("Player").gameObject.transform.position;

            float dist = Vector3.Distance(playerPos, transform.position);

            if (dist >= distOneRun)
            {
                UIHandler.Instance.SetScoreBoard(1);
            }
            else if (dist >= distTwoRun)
            {
                UIHandler.Instance.SetScoreBoard(2);
            }
            else if(dist >= distThreeRun)
            {
                UIHandler.Instance.SetScoreBoard(3);
            }
            Destroy(gameObject);
        }
    }
}
