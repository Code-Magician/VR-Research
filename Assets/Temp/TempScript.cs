using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TempScript : MonoBehaviour
{
    private void Awake()
    {
        Debug.Log("Awake Called");
    }

    private void OnEnable()
    {
        Debug.Log("OnEnable Called");
    }

    private void Start()
    {
        Debug.Log("Start Called");
    }

    private void FixedUpdate()
    {
        Debug.Log("FixedUpdate Called");
    }

    private void Update()
    {
        Debug.Log("Update Called");
    }

    private void LateUpdate()
    {
        Debug.Log("LateUpdate Called");
    }

    private void OnDisable()
    {
        Debug.Log("OnDisable Called");
    }

    private void OnDestroy()
    {
        Debug.Log("OnDestroy Called");
    }
}
