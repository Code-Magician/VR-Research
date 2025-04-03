using System.Collections;
using UnityEngine;

public class CricketBall : MonoBehaviour
{
    public Rigidbody rb;
    [SerializeField] AudioSource audioSource;
    [SerializeField] float waitTime;
    [SerializeField] int distOneRun, distTwoRun, distThreeRun;
    [SerializeField] float maxSwingForce;

    [HideInInspector] public bool isInSwing;
    [HideInInspector] public Vector3 spinTargetPos;
    public string previousCollidedTag = "";
    private bool hasHitBat = false, isSwinging = true;

    private void Awake()
    {

    }

    private void FixedUpdate()
    {
        if(!GameEvents.isSpin && isSwinging)
        {
            Vector3 swingDir = Vector3.Cross(rb.velocity, Vector3.up).normalized;
            float swingForce = Random.Range(0f, maxSwingForce);

            switch(isInSwing)
            {
                case true:
                    rb.AddForce(-swingDir * swingForce);
                    break;
                case false:
                    rb.AddForce(swingDir * swingForce);
                    break;
            }
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Wickets"))
        {
            Debug.Log("Out");
            GameEvents.OnPlayerOut.Invoke();
            GameEvents.HandleHighScore();

            Destroy(gameObject);
        }
        else if (previousCollidedTag.Length == 0)
        {
            if (collision.gameObject.CompareTag("Bat") && !hasHitBat)
            {
                hasHitBat = true;
                previousCollidedTag = "Bat";

                audioSource.Stop();
                audioSource.Play();

                rb.velocity *= 2.5f;

                GameEvents.OnPlayerHitBall.Invoke(transform);
            }
            else if (collision.gameObject.CompareTag("Ground"))
            {
                previousCollidedTag = "Ground";

                if(GameEvents.isSpin)
                {
                    Debug.Log("Is Spinning");
                    float currentY = rb.velocity.y;

                    Vector3 toTarget = (spinTargetPos - transform.position);
                    toTarget.y = 0; // Only care about horizontal direction
                    toTarget.Normalize();

                    // Preserve horizontal speed (XZ magnitude)
                    Vector3 currentHorizontal = rb.velocity;
                    currentHorizontal.y = 0;
                    float horizontalSpeed = currentHorizontal.magnitude;

                    // Apply new horizontal direction with same speed, retain Y
                    Vector3 newVelocity = toTarget * horizontalSpeed;
                    newVelocity.y = currentY;

                    rb.velocity = newVelocity;
                }
                else
                {
                    isSwinging = false;
                }
            }
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
        else if(collision.gameObject.CompareTag("Bat") && !hasHitBat)
        {
            hasHitBat = true;
            previousCollidedTag = "Bat";

            audioSource.Stop();
            audioSource.Play();

            rb.velocity *= 2.5f;

            GameEvents.OnPlayerHitBall.Invoke(transform);
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

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.CompareTag("BallMissCollider"))
        {
            if(!hasHitBat)
            {
                UIHandler.Instance.SetScoreBoard(0);
                GameEvents.OnBallMiss.Invoke();
                Destroy(gameObject);
            }
        }

        if(other.gameObject.CompareTag("WideBallCollider"))
        {
            if(!hasHitBat)
            {
                UIHandler.Instance.SetScoreBoard(1);
                UIHandler.Instance.SetFeedback("Wide Ball");
                GameEvents.OnBallMiss.Invoke();
                Destroy(gameObject);
            }
        }

        if (other.gameObject.CompareTag("Fielder"))
        {
            if (previousCollidedTag == "Bat")
            {
                UIHandler.Instance.SetScoreBoard(0);
                UIHandler.Instance.SetFeedback("Catch Out");
                GameEvents.OnPlayerOut.Invoke();
            }
            else
            {
                CalculateInBetweenWicketRuns();
            }

            Destroy(gameObject);
        }
    }

    public void CalculateInBetweenWicketRuns()
    {
        if (hasHitBat)
        {
            Vector3 playerPos = GameObject.FindGameObjectWithTag("Player").gameObject.transform.position;

            float dist = Vector3.Distance(playerPos, transform.position);

            if (dist >= distThreeRun)
            {
                UIHandler.Instance.SetScoreBoard(3);
            }
            else if (dist >= distTwoRun)
            {
                UIHandler.Instance.SetScoreBoard(2);
            }
            else if (dist >= distOneRun)
            {
                UIHandler.Instance.SetScoreBoard(1);
            }
            else
            {
                UIHandler.Instance.SetFeedback("No Run");
            }

            Destroy(gameObject);
        }
    }
}
