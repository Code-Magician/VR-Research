using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.XR;

public class UIHandler : MonoBehaviour
{
    public static UIHandler Instance;

    [SerializeField] TMP_Text feedbackTxt;
    [SerializeField] TMP_Text runsTxt, oversTxt;
    [SerializeField] GameObject menuScreen;

    private void OnEnable()
    {
        GameEvents.OnPlayerOut += OnPlayerOut;
    }

    private void OnDisable()
    {
        GameEvents.OnPlayerOut -= OnPlayerOut;
    }

    private void Awake()
    {
        Instance = this;
    }

    public void SetFeedback(string feedback)
    {
        feedbackTxt.text = feedback;
    }

    public void SetScoreBoard(int scoredRuns)
    {
        GameEvents.runs += scoredRuns;
        GameEvents.balls++;

        runsTxt.text = $"Runs : {GameEvents.runs}";
        oversTxt.text = $"Overs : {GameEvents.balls / 6}.{GameEvents.balls % 6}";

        if (scoredRuns == 0)
        {
            feedbackTxt.text = "Missed Ball";
        }
        else if(GameEvents.runs == 50)
        {
            feedbackTxt.text = "You scored a Half Century!";
        }
        else if(GameEvents.runs == 100)
        {
            feedbackTxt.text = "You scored a Century!!";
        }
        else if(GameEvents.runs/100 > 1)
        {
            feedbackTxt.text = $"{GameEvents.runs / 100} Centuries!!!";
        }
        else if(scoredRuns == 1 || scoredRuns == 2 || scoredRuns == 3)
        {
            feedbackTxt.text = $"{scoredRuns} Run" + (scoredRuns != 1 ? "s" : "");
        }
        else if(scoredRuns == 4)
        {
            feedbackTxt.text = "It's a Four";
        }
        else if(scoredRuns == 6)
        {
            feedbackTxt.text = "SIXXXXX!!!!";
        }
    }

    public void MenuButtonAction()
    {
        SceneManager.LoadScene("Menu");
    }

    public void OnPlayerOut()
    {
        menuScreen.SetActive(true);
    }

    public void SaveAndExit()
    {
        GameEvents.HandleHighScore();
        MenuButtonAction();
    }
}
