using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public enum Target { Ball, InitialPosition }

public class Fielder : MonoBehaviour
{
    [Header("References")]
    public NavMeshAgent agent;
    public Transform ballingMachineTr;
    public Target targetObject;

    [Header("Fields")]
    [SerializeField] int minSpeed, maxSpeed;

    [HideInInspector] public Transform target;
    private Vector3 initialLocation;


    private void Awake()
    {
        initialLocation = transform.position;
    }

    private void Start()
    {
        int randomSpeed = Random.Range(minSpeed, maxSpeed);
        agent.speed = randomSpeed;

        LookAtTarget(ballingMachineTr.position, false);
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

    void FixedUpdate()
    {
        if (targetObject == Target.Ball)
        {
            if(target != null){
                agent.SetDestination(target.position);
                LookAtTarget(target.position);
            }
            else
            {
                targetObject = Target.InitialPosition;
            }
        }
        else if (targetObject == Target.InitialPosition)
        {
            agent.SetDestination(initialLocation);

            // If fielder reaches the initial position, look at the balling machine
            if (!agent.pathPending && agent.remainingDistance <= agent.stoppingDistance)
            {
                LookAtTarget(ballingMachineTr.position, true);
            }
        }
    }

    private void LookAtTarget(Vector3 targetPosition, bool smoothRotation = true)
    {
        // Get the direction on the horizontal plane (ignore Y-axis)
        Vector3 direction = new Vector3(targetPosition.x, transform.position.y, targetPosition.z) - transform.position;

        if (direction.magnitude > 0.1f) // Avoid jittering when very close
        {
            Quaternion targetRotation = Quaternion.LookRotation(direction);

            if(smoothRotation)
                transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime);
            else
                transform.rotation = targetRotation;
        }
    }
}
