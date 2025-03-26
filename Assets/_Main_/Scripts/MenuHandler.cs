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

    public void PlayButtonAction()
    {
        GetSelectedToggle();
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

    public void GetSelectedToggle()
    {
        // Returns the first toggle that is on in the group
        Toggle selectedToggle = toggleGroup.ActiveToggles().FirstOrDefault();

        if (selectedToggle != null)
        {
            if(selectedToggle.name == "Fast Ball")
            {
                GameEvents.isSpin = false;
            }
            else
            {
                GameEvents.isSpin = true;
            }
        }
        else
        {
            GameEvents.isSpin = true;
        }
    }
}
