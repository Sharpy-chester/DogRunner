using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GameManager : MonoBehaviour
{
    [SerializeField] float timeScaleIncreaseMultiplier = 0.1f;
    [SerializeField] TextMeshProUGUI scoreTxt;
    [SerializeField] TextMeshProUGUI highscoreTxt;
    int currentScore = 0;
    PlayerController playerController;
    [SerializeField] Animator loseScreenAnim;
    bool playerAlive = true;
    [SerializeField] Button retryBtn;

    void Start()
    {
        highscoreTxt.text = "High Score: " + PlayerPrefs.GetInt("Highscore").ToString();
        scoreTxt.text = "Score: " + currentScore.ToString();
        playerController = FindObjectOfType<PlayerController>();
        playerController.addScore += IncreaceScore;
        playerController.playerDeath += CheckHighscore;
        playerController.playerDeath += LoseUI;
    }

    void Update()
    {
        if(playerAlive)
        {
            Time.timeScale += timeScaleIncreaseMultiplier * Time.deltaTime;
        }
    }

    void IncreaceScore()
    {
        ++currentScore;
        scoreTxt.text = "Score: " + currentScore.ToString();
    }

    void CheckHighscore()
    {
        
        playerAlive = false;
        if (currentScore > PlayerPrefs.GetInt("Highscore"))
        {
            PlayerPrefs.SetInt("Highscore", currentScore);
            highscoreTxt.text = "High Score: " + currentScore.ToString();
        }
        
    }

    void LoseUI()
    {
        
        retryBtn.interactable = true;
        loseScreenAnim.SetTrigger("PlayerDeath");
        
    }

    public void Restart()
    {
        loseScreenAnim.SetTrigger("Restart");
        retryBtn.interactable = false;
        currentScore = 0;
        scoreTxt.text = "Score: " + currentScore.ToString();
        playerAlive = true;
        playerController.Restart();
        FindObjectOfType<FenceManager>().Retry();
        foreach (GroundScroller scroller in FindObjectsOfType<GroundScroller>())
        {
            scroller.Restart();
        }
    }
}
