using System.Collections;
using UnityEngine;

public class CricketBall : MonoBehaviour
{
    [Header("References")]
    public Rigidbody rb;
    [SerializeField] AudioSource audioSource;

    [Header("Fields")]
    [SerializeField] int distOneRun, distTwoRun, distThreeRun;
    [SerializeField] float maxSwingForce;

    [HideInInspector] public bool isInSwing;
    [HideInInspector] public Vector3 spinTargetPos;
    private bool hasHitBat = false, hasHitGroundAfterBat = false, isSwinging = true;


    private void FixedUpdate()
    {
        if(!GameEvents.isSpin && isSwinging)
        {
            MakeBallSwing();
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        switch (collision.gameObject.tag)
        {
            case "Wickets":
                BallHitWicketAction();
                break;

            case "Bat":
                BallHitBatAction();
                break;

            case "Outside Ground":
                BallHitOutsideGroundAction();
                break;

            case "Ground":
                BallHitGroundAction();
                break;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        switch (other.gameObject.tag)
        {
            case "BallMissCollider":
                BallHitBallMissColliderAction();
                break;

            case "WideBallCollider":
                BallHitWideBallColliderAction();
                break;

            case "Fielder":
                BallHitFielderAction();
                break;
        }
    }

    public void BallHitWicketAction()
    {
        GameEvents.OnPlayerOut.Invoke();
        GameEvents.HandleHighScore();
        UIHandler.Instance.SetFeedback("Bowled");

        Destroy(gameObject);
    }

    public void BallHitBatAction()
    {
        hasHitBat = true;

        audioSource.Stop();
        audioSource.Play();

        rb.velocity *= 2.5f;

        GameEvents.OnPlayerHitBall.Invoke(transform);
    }

    public void BallHitOutsideGroundAction()
    {
        GameEvents.OnBallHitOutsideGround.Invoke();
        if (hasHitBat)
        {
            if (!hasHitGroundAfterBat)
            {
                UIHandler.Instance.SetScoreBoard(6);
            }
            else
            {
                UIHandler.Instance.SetScoreBoard(4);
            }
        }
        else
        {
            UIHandler.Instance.SetScoreBoard(0);
            UIHandler.Instance.SetFeedback("Missed Ball");
            GameEvents.OnBallMiss.Invoke();
        }

        Destroy(gameObject);
    }

    private void BallHitGroundAction()
    {
        if (!GameEvents.isSpin) isSwinging = false;
        
        if(hasHitBat)
        {
            hasHitGroundAfterBat = true;
        }
        else
        {
            if(GameEvents.isSpin)
            {
                MakeBallSpin();
            }
        }
    }

    private void BallHitBallMissColliderAction()
    {
        if (!hasHitBat)
        {
            UIHandler.Instance.SetScoreBoard(0);
            UIHandler.Instance.SetFeedback("Missed Ball");
            GameEvents.OnBallMiss.Invoke();
            Destroy(gameObject);
        }
    }

    private void BallHitWideBallColliderAction()
    {
        if (!hasHitBat)
        {
            UIHandler.Instance.SetScoreBoard(1);
            UIHandler.Instance.SetFeedback("Wide Ball");
            GameEvents.OnBallMiss.Invoke();
            Destroy(gameObject);
        }
    }

    private void BallHitFielderAction()
    {
        if (hasHitBat && !hasHitGroundAfterBat)
        {
            UIHandler.Instance.SetScoreBoard(0);
            UIHandler.Instance.SetFeedback("Catch Out");
            GameEvents.OnPlayerOut.Invoke();
        }
        else
        {
            GameEvents.OnFielderCaughtBall.Invoke();
            CalculateInBetweenWicketRuns();
        }

        Destroy(gameObject);
    }

    private void MakeBallSpin()
    {
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

    private void MakeBallSwing()
    {
        Vector3 swingDir = Vector3.Cross(rb.velocity, Vector3.up).normalized;
        float swingForce = Random.Range(0f, maxSwingForce);

        switch (isInSwing)
        {
            case true:
                rb.AddForce(-swingDir * swingForce);
                break;
            case false:
                rb.AddForce(swingDir * swingForce);
                break;
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
        }
    }
}
