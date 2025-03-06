using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class SpawnBallOnPress : MonoBehaviour
{
    public GameObject ballPrefab;  // Assign the Ball prefab in the Inspector
    public Transform spawnPoint;   // Assign where the ball should spawn
    private XRSimpleInteractable interactable;

    void Start()
    {
        interactable = GetComponent<XRSimpleInteractable>();
        interactable.activated.AddListener(x => SpawnBall());
    }

    void SpawnBall()
    {
        if (ballPrefab != null && spawnPoint != null)
        {
            Instantiate(ballPrefab, spawnPoint.position, spawnPoint.rotation);
        }
    }
}
