using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.UI;

public class GameSession : MonoBehaviour
{
    [SerializeField] int playerLives;
    [SerializeField] float reLoadSceneTime = 1f;
    [SerializeField] TextMeshProUGUI scoreText;
    [SerializeField] TextMeshProUGUI livesText;
    [SerializeField] TextMeshProUGUI melonText;
    [SerializeField] TextMeshProUGUI exitText;
    [SerializeField] Image exitImage;
    int playerScore = 0;


    void Awake()
    {
        int numOfGameSession = FindObjectsOfType<GameSession>().Length;
        if (numOfGameSession > 1)
        {
            Destroy(gameObject);
        }
        else
        {
            DontDestroyOnLoad(gameObject);
        }
    }

    void Start() 
    {
        livesText.text = playerLives.ToString();
        scoreText.text = playerScore.ToString();
    }


    public void ProcessPlayerDeath()
    {
        if (playerLives > 1)
        {
            TakeLife();
        }
        else
        {
            Invoke("ResetGameSession", reLoadSceneTime);
        }
    }

    public void TakeLife()
    {
        playerLives--;
        Invoke("ReLoadScene", reLoadSceneTime);
        livesText.text = playerLives.ToString();
    }

    public void AddToScore(int points)
    {
        playerScore += points;
        scoreText.text = playerScore.ToString();
    }

    public void TurnOnExitUI()
    {
        Vector4 actualExitTextColor = exitText.color; 
        actualExitTextColor.w = 1f;
        exitText.color = actualExitTextColor;
        Vector4 actualExitImageColor = exitImage.color; 
        actualExitImageColor.w = 1f;
        exitImage.color = actualExitImageColor;
        melonText.text = "Yes";
    }

    public void TurnOffExitUI()
    {
        Vector4 actualExitTextColor = exitText.color; 
        actualExitTextColor.w = 0.4f;
        exitText.color = actualExitTextColor;
        Vector4 actualExitImageColor = exitImage.color; 
        actualExitImageColor.w = 0.2f;
        exitImage.color = actualExitImageColor;
        melonText.text = "No";
    }

    private void ResetGameSession()
    {
        FindObjectOfType<ScenePersist>().ResetScenePersist();
        SceneManager.LoadScene(0);
        Destroy(gameObject);
    }

    void ReLoadScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
