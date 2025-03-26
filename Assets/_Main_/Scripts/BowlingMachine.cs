using DG.Tweening;
using ProjectileCurveVisualizerSystem;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.XR;
using UnityEngine.XR.Interaction.Toolkit.AffordanceSystem.Receiver.Primitives;
using static UnityEngine.GraphicsBuffer;

public class BowlingMachine : MonoBehaviour
{
    [SerializeField] GameObject ballPrefab;
    [SerializeField] Transform spawnLocation, wicketTr;
    [SerializeField] float minSpeed = 40f, maxSpeed = 120f;
    [SerializeField] Transform startXTr, endXTr, startZTr, endZTr, spinStartZ, spinEndZ;
    [SerializeField] float timeBetweenNewBallSpawn = 1f;
    [SerializeField] GameObject playerOptionsCanvas;
    [SerializeField] TMP_Text timerTxt, ballingSpeedTxt, ballingtypeTxt;
    [SerializeField] ProjectileCurveVisualizer projectileCurveVisualizer;


    private float timer = 0.0f;
    private float speed;
    private Vector3 direction, landPosition;
    private bool isRunning = false, isInSwing;
    private Vector3 updatedProjectileStartPosition;
    private RaycastHit hit;

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

    private void Awake()
    {
        if (GameEvents.isSpin)
        {
            ballingtypeTxt.text = "Balling Spin";
            minSpeed = 22;
            maxSpeed = 30;
        }
        else
        {
            ballingtypeTxt.text = "Balling Fast";
            minSpeed = 30f;
            maxSpeed = 45;
        }
    }

    private void Update()
    {
        if (isRunning)
        {
            timer += Time.deltaTime;
            timerTxt.text = (timeBetweenNewBallSpawn - timer).ToString("0");

            if ((timer > timeBetweenNewBallSpawn))
            {
                SpawnBall();

                timer = 0;
                isRunning = false;
                timerTxt.transform.parent.gameObject.SetActive(false);
            }
        }

        InputDevice rightHand = InputDevices.GetDeviceAtXRNode(XRNode.RightHand);

        if (playerOptionsCanvas.activeInHierarchy && rightHand.TryGetFeatureValue(CommonUsages.primaryButton, out bool primaryButton) && primaryButton)
        {
            Spawn();
        }
    }

    public void OnPlayerOut()
    {
        timerTxt.text = "Out";
        isRunning = false;
        playerOptionsCanvas.SetActive(false);
    }

    public void OnPlayerHitBall(Transform ballTr)
    {
        playerOptionsCanvas.SetActive(true);
        projectileCurveVisualizer.HideProjectileCurve();
    }

    public void OnBallMiss()
    {
        playerOptionsCanvas.SetActive(true);
        projectileCurveVisualizer.HideProjectileCurve();
    }

    public void Spawn()
    {
        GameEvents.OnBallSpawn.Invoke();

        timerTxt.transform.parent.gameObject.SetActive(true);
        playerOptionsCanvas.SetActive(false);
        UIHandler.Instance.SetFeedback("");

        // Ball Speed Calculation
        float u = Random.Range(minSpeed, maxSpeed); // Get random speed
        speed = u;
        ballingSpeedTxt.text = (speed * 3600f / 1000f).ToString("F1") + " Km/h";

        // Set Balling Spawn Location.
        float spawnZPos = Random.Range(-startZTr.position.z, startZTr.position.z);
        Vector3 spawnPos = transform.position;
        spawnPos.z = spawnZPos;
        transform.DOMove(spawnPos, 1f).OnComplete(() =>
        {
            // Set Ball Direction and Land Position.
            float xPos = Random.Range(startXTr.position.x, endXTr.position.x);
            float zPos = Random.Range(startZTr.position.z, endZTr.position.z);
            landPosition = new Vector3(xPos, 0, zPos);
            Vector3 startPosition = spawnLocation.transform.position;
            direction = (landPosition - startPosition).normalized;

            if(!GameEvents.isSpin)
            {
                if (landPosition.z < wicketTr.position.z)
                {
                    isInSwing = true;
                    ballingtypeTxt.text = "Balling In Swing";
                }
                else
                {
                    isInSwing = false;
                    ballingtypeTxt.text = "Balling Out Swing";
                }
            }

            projectileCurveVisualizer.VisualizeProjectileCurve(spawnLocation.position, 0f, direction * speed, 0.05f, 0.01f, false, out updatedProjectileStartPosition, out hit);

            isRunning = true;
        });
    }


    public void SpawnBall()
    {
        GameObject ballObj = Instantiate(ballPrefab, spawnLocation.position, Quaternion.identity);

        // Set the velocity along this direction
        CricketBall cricketBall = ballObj.GetComponent<CricketBall>();
        cricketBall.rb.velocity = direction * speed;

        if(GameEvents.isSpin)
        {
            Vector3 spinTargetPos = spinStartZ.position;
            float zPos = Random.Range(spinStartZ.position.z, spinEndZ.position.z);
            spinTargetPos.z = zPos;

            cricketBall.spinTargetPos = spinTargetPos;
        }
        else
        {
            cricketBall.isInSwing = isInSwing;
        }
    }
}