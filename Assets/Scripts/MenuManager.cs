using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class MenuManager : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI highscoreTxt;

    public void HomeScreen()
    {
        SceneManager.LoadScene("Home");
    }

    public void Play()
    {
        SceneManager.LoadScene("MainScene");
    }

    private void Start()
    {
        if(highscoreTxt)
        {
            highscoreTxt.text = "Highscore: " + PlayerPrefs.GetInt("Highscore");
        }
    }
}
