using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FieldersManager : MonoBehaviour
{
    [SerializeField] List<Fielder> fielders;
    [SerializeField] float minDistanceToFollowBall;

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
    }

    private void FixedUpdate()
    {
        if(currentBallTr != null)
        {
            foreach (var fielder in fielders)
            {
                Vector3 ballPos = currentBallTr.position;
                Vector3 fielderPos = fielder.transform.position;

                ballPos.y = 0;
                fielderPos.y = 0;

                if (Vector3.Distance(ballPos, fielderPos) <= minDistanceToFollowBall)
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
