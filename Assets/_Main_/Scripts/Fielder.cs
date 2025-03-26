using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Fielder : MonoBehaviour
{
    public NavMeshAgent agent;
    public Transform target;

    private Vector3 initialLocation;

    private void Awake()
    {
        initialLocation = transform.position;
    }

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
        target = ballTr;
    }

    void Update()
    {
        if (target != null)
        {
            Vector3 targetPosition = target.position;
            // Keep the Y position same as the agent's current Y to prevent vertical movement
            targetPosition.y = agent.transform.position.y;

            agent.SetDestination(targetPosition);
        }
        else
        {
            agent.SetDestination(initialLocation);
        }

        if (HasReachedDestination())
        {
            agent.ResetPath();
        }
    }

    bool HasReachedDestination()
    {
        return !agent.pathPending
               && agent.remainingDistance <= agent.stoppingDistance
               && (!agent.hasPath || agent.velocity.sqrMagnitude == 0f);
    }

}
