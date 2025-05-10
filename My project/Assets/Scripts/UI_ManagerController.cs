using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SocialPlatforms.Impl;
using GooglePlayGames;
using GooglePlayGames.BasicApi;
using UnityEngine.SocialPlatforms;

public class UI_ManagerController : MonoBehaviour
{
    public Animator playButton, pauseButton,scoreText,highscoreText;
    public GameObject pauseMenu,mainMenu,gameMenu,gameOverPanel,leaderboardPanel;
    public TextMeshProUGUI[] highscoreTexts;
    public PlayerController playerController;
    public TextMeshProUGUI finalScoreText,highscoreTextUI;
    AudioSource uiAudioSource;
    public AudioClip buttonSound;
    GameManagerScript gameManagerScript;
    public RewardedAds rewardedAds;
    DatabaseController databaseController;
    public string[] highscoreTextSaver;
    //GameOver
    public float timerShowAd;
    public bool gameOver;
    public TextMeshProUGUI timerShowAdText;
    public GameObject showAdPanel, RestartPanel;
    // Start is called before the first frame update
    void Start()
    {
        mainMenu.SetActive(true);
        gameMenu.SetActive(false);
        pauseMenu.SetActive(false);
        gameOverPanel.SetActive(false);
        uiAudioSource = GetComponent<AudioSource>();
        highscoreTextUI.text=PlayerPrefs.GetInt("highscore").ToString();
        gameManagerScript = GameObject.FindGameObjectWithTag("Manager").GetComponent<GameManagerScript>();
        databaseController = GameObject.FindGameObjectWithTag("Database").GetComponent<DatabaseController>();
        databaseController.uiController = this;
        rewardedAds = GameObject.FindGameObjectWithTag("AdManager").GetComponent<RewardedAds>();
        rewardedAds.gameManagerScript = gameManagerScript;
        rewardedAds.uiManagerController = this;
        timerShowAd = 5f;
        gameOver = false;
    }

    // Update is called once per frame
    void Update()
    {
        if(gameOver)
        {
            timerShowAd -= Time.deltaTime;
            timerShowAdText.text = timerShowAd.ToString("0");
            if(timerShowAd < 0)
            {
                showAdPanel.SetActive(false);
                RestartPanel.SetActive(true);
                gameOver = false;
            }
        }
    }
    public void PlayGame()
    {
        uiAudioSource.clip = buttonSound;
        uiAudioSource.Play();
        StartCoroutine(Play());
    }
    public void PauseGame()
    {
        StartCoroutine(Pause());
    }
    public void BackToGame()
    {
        uiAudioSource.clip = buttonSound;
        uiAudioSource.Play();
        StartCoroutine(BackToPlay());
    }
    public void MenuBack()
    {
        leaderboardPanel.SetActive(false);
        mainMenu.SetActive(true);
    }
    public void ShowLeaderboardUI()
    {
        databaseController.GetTop10();
    }

    IEnumerator Play()
    {
        playButton.Play("Outro_Play_Button");
        highscoreText.Play("Outro_Highscore");
        scoreText.Play("Start_ScoreText");
        yield return new WaitForSeconds(.5f);
        mainMenu.SetActive(false);
        gameMenu.SetActive(true);
        playerController.playing = true;
    }
    IEnumerator Pause()
    {
        pauseMenu.SetActive(true);
        gameMenu.SetActive(false);
        pauseButton.Play("Idle_Pause_Button");
        yield return new WaitForSeconds(1);
    }
    IEnumerator BackToPlay()
    {
        pauseButton.Play("Outro_Pause_Button");
        yield return new WaitForSeconds(.5f);
        gameMenu.SetActive(true);
        pauseMenu.SetActive(false);
        playerController.pausing = false;
    }
    public void RestartGame()
    {
        Application.LoadLevel("SampleScene");
    }
    public void GameOver()
    {
        gameMenu.SetActive(false);
        finalScoreText.text = GameObject.FindGameObjectWithTag("Manager").GetComponent<GameManagerScript>().score.ToString("0");
        scoreText.Play("Idle_ScoreText");
        gameOverPanel.SetActive(true);
        if (!gameManagerScript.AdSeen)
        {
            showAdPanel.SetActive(true);
            RestartPanel.SetActive(false);
            gameOver = true;
        }    
        else
        {
            showAdPanel.SetActive(false);
            RestartPanel.SetActive(true);
        }        
    }
    public void ShowAd()
    {
        rewardedAds.ShowRewardedAd();
    }
}
