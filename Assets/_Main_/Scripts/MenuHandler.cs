using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuHandler : MonoBehaviour
{
    [SerializeField] TMP_Text highScoreTxt;

    private int runs, balls;

    private void Awake()
    {
        runs = PlayerPrefs.GetInt("Runs");
        balls = PlayerPrefs.GetInt("Balls");

        highScoreTxt.text = $"Highscore is {runs} runs in {balls / 6}.{balls % 6} overs.";
    }

    public void PlayButtonAction()
    {
        SceneManager.LoadScene("Cricket");
    }

    public void ExitButtonAction()
    {
#if UNITY_EDITOR
        EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}
