using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CricketBall : MonoBehaviour
{
    [SerializeField]
    [Range(10, 200)] float speed;

    [SerializeField] Rigidbody rb;
    // Start is called before the first frame update
    void Start()
    {
        Throw();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Throw()
    {
        rb.AddForce(Vector3.left * speed, ForceMode.VelocityChange);
    }
}
