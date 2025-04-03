using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor.Rendering;
using UnityEngine;
using UnityEngine.UI;

public class Test : MonoBehaviour
{
    [SerializeField] Transform spawnLoc;
    [SerializeField] GameObject ballPrefab;
    [SerializeField] TMP_InputField speedInput;

    private int speed = 0;

    public void Spawn()
    {
        GameObject spawnedBall = Instantiate(ballPrefab, spawnLoc.position, Quaternion.identity);

        spawnedBall.GetComponent<Rigidbody>().velocity = Vector3.back * speed ;
    }

    public void SetSpeed(string input)
    {
        speed = int.Parse(input);
    }
}
