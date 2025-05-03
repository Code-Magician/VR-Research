using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuHandler : MonoBehaviour
{
    [SerializeField] TMP_Text highScoreTxt;
    [SerializeField] ToggleGroup toggleGroup;

    private int runs, balls;

    private void Awake()
    {
        runs = PlayerPrefs.GetInt("Runs");
        balls = PlayerPrefs.GetInt("Balls");

        highScoreTxt.text = $"Highscore is {runs} runs in {balls / 6}.{balls % 6} overs.";
    }

    private void PlayButtonAction()
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

    public void EasyButtonAction()
    {
        GameModeType(GameType.Easy);
    }

    public void FastButtonAction()
    {
        GameModeType(GameType.Fast);
    }

    public void SpinButtonAction()
    {
        GameModeType(GameType.Spin);
    }

    public void HardButtonAction()
    {
        GameModeType(GameType.Hard);
    }

    public void T20ButtonAction()
    {
        GameModeType(GameType.T20);
    }

    public void CustomButtonAction()
    {
        GameModeType(GameType.Custom);
    }

    public void GameModeType(GameType gameType)
    {
        GameEvents.gameType = gameType;
        PlayButtonAction();
    }
}
