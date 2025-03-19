using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class BowlingMachine : MonoBehaviour
{
    [SerializeField] GameObject ballPrefab;
    [SerializeField] Transform spawnLocation;
    [SerializeField] float minSpeed = 40f, maxSpeed = 120f;
    [SerializeField] Transform startXTr, endXTr;
    [SerializeField] float timeBetweenNewBallSpawn = 1f;
    [SerializeField] GameObject landPointMarker, playerOptionsCanvas;
    [SerializeField] TMP_Text timerTxt, ballingSpeedTxt;


    private float timer = 0.0f;
    private float speed;
    private Vector3 direction;
    private bool isRunning = false;

    private float distance = 0, time = 0;
    GameObject currentBallObj;
    private bool ballThrown = false;

    private void OnEnable()
    {
        GameEvents.OnPlayerOut += OnPlayerOut;
        GameEvents.OnPlayerHitBall += OnPlayerHitBall;
        GameEvents.OnBallMiss += OnBallMiss;
    }

    private void OnDisable()
    {
        GameEvents.OnPlayerOut -= OnPlayerOut;
        GameEvents.OnPlayerHitBall -= OnPlayerHitBall;
        GameEvents.OnBallMiss -= OnBallMiss;
    }


    private void Update()
    {
        if (isRunning)
        {
            timer += Time.deltaTime;
            timerTxt.text = (timeBetweenNewBallSpawn - timer).ToString("0");

            if ((timer > timeBetweenNewBallSpawn))
            {
                StartCoroutine(SpawnBall());

                timer = 0;
                isRunning = false;
                timerTxt.transform.parent.gameObject.SetActive(false);
            }
        }

        if (ballThrown)
        {
            time += Time.deltaTime;
        }
    }

    public void OnPlayerOut()
    {
        timerTxt.text = "Out";
        isRunning = false;
        playerOptionsCanvas.SetActive(false);
    }

    public void OnPlayerHitBall()
    {
        playerOptionsCanvas.SetActive(true);

        distance = Vector3.Distance(spawnLocation.position, currentBallObj.transform.position);

        Debug.Log($"Ball Speed : {distance / time}, Distance : {distance}, Time : {time}");

        ballThrown = false;
        distance = 0;
        time = 0;
    }

    public void OnBallMiss()
    {
        playerOptionsCanvas.SetActive(true);
    }

    public void Spawn()
    {
        isRunning = true;
        timerTxt.transform.parent.gameObject.SetActive(true);
        playerOptionsCanvas.SetActive(false);
        UIHandler.Instance.SetFeedback("");

        // Calculation
        float u = Random.Range(minSpeed, maxSpeed); // Get random speed
        speed = u;
        ballingSpeedTxt.text = (speed * 3600f/1000f).ToString("F1") +" Km/h";

        // Pick a random landing position within the defined range
        float xPos = Random.Range(startXTr.position.x, endXTr.position.x);
        Vector3 landPosition = new Vector3(xPos, 0, spawnLocation.transform.position.z);
        Vector3 startPosition = spawnLocation.transform.position;
        direction = (landPosition - startPosition).normalized;

        //landPointMarker.transform.position = landPosition;
    }


    public IEnumerator SpawnBall()
    {
        GameObject ballObj = Instantiate(ballPrefab, spawnLocation.position, Quaternion.identity);
        currentBallObj = ballObj;

        ballObj.GetComponent<CricketBall>().rb.velocity = direction * speed;
        ballThrown = true;

        yield return new WaitForEndOfFrame();
    }

    public void BallDataCalculation(GameObject ballObj)
    {
        float g = Physics.gravity.magnitude;
        float u = Random.Range(minSpeed, maxSpeed); // Get random speed
        speed = u;

        // Pick a random landing position within the defined range
        float xPos = Random.Range(startXTr.position.x, endXTr.position.x);
        Vector3 landPosition = new Vector3(xPos, 0, ballObj.transform.position.z);
        Vector3 startPosition = new Vector3(ballObj.transform.position.x, 0, ballObj.transform.position.z);

        float x = (landPosition - startPosition).magnitude;
        float h = ballObj.transform.position.y; // Ball's initial height


        float phi = Mathf.Atan(x / h) * Mathf.Rad2Deg;
        float alpha = (g * x * x) / (u * u);
        float theta = 90 - (phi + Mathf.Acos((alpha - h) / (Mathf.Sqrt(h * h + x * x))) * Mathf.Rad2Deg) / 2;

        //Debug.Log($"g={g}, u={u}, x={x}, h={h}, phi={phi}, theta={theta}");

        Vector3 direction = landPosition - startPosition;
        direction.y = 0;
        ballObj.transform.rotation = Quaternion.LookRotation(direction);
        Vector3 rot = ballObj.transform.rotation.eulerAngles;
        rot.z = theta;
        ballObj.transform.rotation = Quaternion.Euler(rot);

        landPosition.y = 0.015f;
        landPointMarker.transform.position = landPosition;
    }
}
