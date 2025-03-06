using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BowlingMachine : MonoBehaviour
{
    [SerializeField] GameObject ballPrefab;
    [SerializeField] Transform spawnLocation;

    private void Start()
    {
        InvokeRepeating("SpawnBall", 2f, 2f);
    }

    public void SpawnBall()
    {
        Instantiate(ballPrefab, spawnLocation.position, Quaternion.identity);
    }
}
