using UnityEngine;

public class CricketBall : MonoBehaviour
{
    [SerializeField] float minSpeed = 40f, maxSpeed = 120f; // Speed range
    [SerializeField] Rigidbody rb;

    private float speed;
    private Transform startXTr, endXTr, startZTr, endZTr; // Position boundaries

    void Start()
    {
        startXTr = DataHandler.Instance.startXTr;
        endXTr = DataHandler.Instance.endXTr;
        startZTr = DataHandler.Instance.startZTr;
        endZTr = DataHandler.Instance.endZTr;

        Throw();
    }

    public void Throw()
    {
        float g = Physics.gravity.magnitude;
        float u = Random.Range(minSpeed, maxSpeed); // Get random speed

        // Pick a random landing position within the defined range
        float xPos = Random.Range(startXTr.position.x, endXTr.position.x);
        float zPos = Random.Range(startZTr.position.z, endZTr.position.z);
        Vector3 landPosition = new Vector3(xPos, 0, zPos);
        Vector3 startPosition = new Vector3(transform.position.x, 0, transform.position.z);

        float x = (landPosition - startPosition).magnitude;
        float h = transform.position.y; // Ball's initial height


        float phi = Mathf.Atan(x / h) * Mathf.Rad2Deg;
        float alpha = (g * x*x) / (u * u);
        float theta = 90- (phi + Mathf.Acos((alpha - h) / (Mathf.Sqrt(h * h + x * x))) * Mathf.Rad2Deg)/2;

        Debug.Log($"g={g}, u={u}, x={x}, h={h}, phi={phi}, theta={theta}");
        transform.rotation = Quaternion.Euler(0, 0, theta);
        rb.velocity = -transform.right * u;
    }
}
