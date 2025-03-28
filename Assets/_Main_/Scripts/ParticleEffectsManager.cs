using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleEffectsManager : MonoBehaviour
{
    [SerializeField] List<ParticleSystem> particles;

    private void OnEnable()
    {
        GameEvents.OnPlayerHitBall += OnPlayerHitBall;
    }

    private void OnDisable()
    {
        GameEvents.OnPlayerHitBall -= OnPlayerHitBall;
    }

    private void OnPlayerHitBall(Transform ballObj)
    {
        int randomIndex = Random.Range(0, particles.Count);
        particles[randomIndex].gameObject.transform.position = ballObj.position;
        particles[randomIndex].Play();
    }
}
