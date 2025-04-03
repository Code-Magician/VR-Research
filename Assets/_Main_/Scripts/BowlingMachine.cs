using DG.Tweening;
using ProjectileCurveVisualizerSystem;
using TMPro;
using UnityEngine;
using UnityEngine.XR;

public class BowlingMachine : MonoBehaviour
{
    [Header("References")]
    [SerializeField] GameObject ballPrefab;
    [SerializeField] Transform spawnLocation, wicketTr;
    [SerializeField] Transform startXTr, endXTr, startZTr, endZTr, spinStartZ, spinEndZ;
    [SerializeField] TMP_Text timerTxt, ballingSpeedTxt, ballingtypeTxt;
    [SerializeField] ProjectileCurveVisualizer projectileCurveVisualizer;
    [SerializeField] GameObject playerOptionsCanvas;

    [Header("Fields")]
    [SerializeField] float timeBetweenNewBallSpawn = 1f;
    [SerializeField] float minSpinBallSpeed, maxSpinBallSpeed;
    [SerializeField] float minFastBallSpeed, maxFastBallSpeed;


    private float minSpeed = 40f, maxSpeed = 120f;
    private float timer = 0.0f;
    private float speed;
    private Vector3 direction, landPosition, spinTargetPos;
    private bool isRunning = false, isInSwing, canSpawnNextBall = true;
    private Vector3 updatedProjectileStartPosition;
    private RaycastHit hit;


    private void OnEnable()
    {
        GameEvents.OnPlayerOut += OnPlayerOut;
        GameEvents.OnPlayerHitBall += OnPlayerHitBall;
        GameEvents.OnBallMiss += OnBallMiss;
        GameEvents.OnFielderCaughtBall += OnFielderCaughtBall;
        GameEvents.OnBallHitOutsideGround += OnBallHitOutsideGround;
    }

    private void OnDisable()
    {
        GameEvents.OnPlayerOut -= OnPlayerOut;
        GameEvents.OnPlayerHitBall -= OnPlayerHitBall;
        GameEvents.OnBallMiss -= OnBallMiss;
        GameEvents.OnFielderCaughtBall -= OnFielderCaughtBall;
        GameEvents.OnBallHitOutsideGround -= OnBallHitOutsideGround;
    }

    private void Awake()
    {
        Cursor.lockState = CursorLockMode.Locked;

        if (GameEvents.isSpin)
        {
            minSpeed = minSpinBallSpeed;
            maxSpeed = maxSpinBallSpeed;
        }
        else
        {
            minSpeed = minFastBallSpeed;
            maxSpeed = maxFastBallSpeed;
        }
    }

    private void Update()
    {
        if (isRunning)
        {
            timer += Time.deltaTime;
            timerTxt.text = (timeBetweenNewBallSpawn - timer).ToString("F0");

            if ((timer > timeBetweenNewBallSpawn))
            {
                isRunning = false;
                timer = 0;
                timerTxt.transform.parent.gameObject.SetActive(false);

                SpawnBall();
            }
        }

        InputDevice rightHand = InputDevices.GetDeviceAtXRNode(XRNode.RightHand);
        if (canSpawnNextBall && playerOptionsCanvas.activeInHierarchy && rightHand.TryGetFeatureValue(CommonUsages.primaryButton, out bool primaryButton) && primaryButton)
        {
            canSpawnNextBall = false;
            BallSpawnButtonAction();
        }

        if(Input.GetKey(KeyCode.M))
        {
            BallSpawnButtonAction();
        }
    }

    public void OnPlayerOut()
    {
        isRunning = false;
        projectileCurveVisualizer.HideProjectileCurve();
    }

    public void OnPlayerHitBall(Transform ballTr)
    {
        projectileCurveVisualizer.HideProjectileCurve();
    }

    public void OnBallMiss()
    {
        canSpawnNextBall = true;
        playerOptionsCanvas.SetActive(true);
        projectileCurveVisualizer.HideProjectileCurve();
    }

    private void OnFielderCaughtBall()
    {
        canSpawnNextBall = true;
        playerOptionsCanvas.SetActive(true);
    }

    private void OnBallHitOutsideGround()
    {
        canSpawnNextBall = true;
        playerOptionsCanvas.SetActive(true);
    }

    public void BallSpawnButtonAction()
    {
        GameEvents.OnBallSpawn.Invoke();

        timerTxt.transform.parent.gameObject.SetActive(true);
        playerOptionsCanvas.SetActive(false);

        UIHandler.Instance.SetFeedback("");

        // Set Balling Spawn Location.
        float spawnZPos = Random.Range(-startZTr.position.z, startZTr.position.z);
        Vector3 spawnPos = transform.position;
        spawnPos.z = spawnZPos;

        transform.DOMove(spawnPos, 1f).OnComplete(() =>
        {
            BallDataCalculation();

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
            cricketBall.spinTargetPos = spinTargetPos;
        }
        else
        {
            cricketBall.isInSwing = isInSwing;
        }
    }

    private void BallDataCalculation()
    {
        // Ball Speed Calculation
        float u = Random.Range(minSpeed, maxSpeed); // Get random speed
        speed = u;
        ballingSpeedTxt.text = (speed * 3600f / 1000f).ToString("F0") + " Km/h";

        // Set Ball Direction and Land Position.
        float xPos = Random.Range(startXTr.position.x, endXTr.position.x);
        float zPos = Random.Range(startZTr.position.z, endZTr.position.z);
        landPosition = new Vector3(xPos, 0, zPos);

        Vector3 startPosition = spawnLocation.transform.position;
        direction = (landPosition - startPosition).normalized;

        if (GameEvents.isSpin)
        {
            spinTargetPos = spinStartZ.position;
            zPos = Random.Range(spinStartZ.position.z, spinEndZ.position.z);
            spinTargetPos.z = zPos;

            if (landPosition.z < zPos)
            {
                ballingtypeTxt.text = "Balling On Spin";
            }
            else
            {
                ballingtypeTxt.text = "Balling Off Spin";
            }
        }
        else
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
    }
}