using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UIHandler : MonoBehaviour
{
    public static UIHandler Instance;

    [SerializeField] TMP_Text scoreTxt;

    private int score = 0;

    private void Awake()
    {
        Instance = this;
    }

    public void AddScore()
    {
        score++;
        scoreTxt.text = $"Score : {score}";
    }
}
