using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FieldersManager : MonoBehaviour
{
    [SerializeField] List<Fielder> fielders;
    [SerializeField] float minDistanceToFollowBall;
    [SerializeField] float coneAngle = 30f;

    private Transform currentBallTr;

    private void OnEnable()
    {
        GameEvents.OnPlayerHitBall += OnPlayerHitBall;
    }

    private void OnDisable()
    {
        GameEvents.OnPlayerHitBall -= OnPlayerHitBall;
    }

    private void OnPlayerHitBall(Transform ballTr)
    {
        currentBallTr = ballTr;
        ActivateFielders();
    }

    private void ActivateFielders()
    {
        if(currentBallTr != null)
        {
            foreach (var fielder in fielders)
            {
                Vector3 dirToTarget = (fielder.transform.position - currentBallTr.position).normalized;

                Vector3 ballDirection = currentBallTr.GetComponent<Rigidbody>().velocity;
                ballDirection.y = 0f;
                ballDirection.Normalize();

                float dotProduct = Vector3.Dot(ballDirection, dirToTarget);
                float angle = Mathf.Acos(dotProduct) * Mathf.Rad2Deg; // Convert to degrees

                if (angle <= coneAngle)
                {
                    fielder.targetObject = Target.Ball;
                }
                else
                {
                    fielder.targetObject = Target.InitialPosition;
                }
            }
        }
        else
        {
            foreach (var fielder in fielders)
            {
                fielder.targetObject = Target.InitialPosition;
            }
        }
    }
}
