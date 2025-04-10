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
    public GameObject pauseMenu,mainMenu,gameMenu,gameOverPanel,adButton,leaderboardPanel;
    public TextMeshProUGUI[] highscoreTexts;
    public PlayerController playerController;
    public TextMeshProUGUI finalScoreText,highscoreTextUI;
    AudioSource uiAudioSource;
    public AudioClip buttonSound;
    GameManagerScript gameManagerScript;
    public RewardedAds rewardedAds;
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
        rewardedAds = GameObject.FindAnyObjectByType<RewardedAds>();
        rewardedAds.gameManagerScript = gameManagerScript;
        rewardedAds.uiManagerController = this;
    }

    // Update is called once per frame
    void Update()
    {
        
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
        //PlayGamesPlatform.Instance.ShowLeaderboardUI();

        PlayGamesPlatform.Instance.LoadScores(
            "CgkIsKb_vv4SEAIQAQ",
            LeaderboardStart.TopScores,
            10,
            LeaderboardCollection.Public,
            LeaderboardTimeSpan.AllTime,
            (data) =>
            {
                if (data.Valid)
                {
                    IScore[] scores = data.Scores;

                    // Extract user IDs
                    string[] userIds = new string[scores.Length];
                    for (int i = 0; i < scores.Length; i++)
                    {
                        userIds[i] = scores[i].userID;
                    }

                    // Load player display names
                    Social.LoadUsers(userIds, (IUserProfile[] users) =>
                    {
                        for (int i = 0; i < highscoreTexts.Length; i++)
                        {
                            if (i < scores.Length)
                            {
                                string displayName = users[i].userName;
                                highscoreTexts[i].text = $"{i + 1}. {displayName}: {scores[i].value}";
                            }
                            else
                            {
                                highscoreTexts[i].text = $"{i + 1}. ---";
                            }
                        }
                    });
                    mainMenu.SetActive(false);
                    leaderboardPanel.SetActive(true);
                }
                else
                {
                    Debug.Log("Failed to load leaderboard.");
                }
            });
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
            adButton.SetActive(true);
        else
            adButton.SetActive(false);
    }
    public void ShowAd()
    {
        rewardedAds.ShowRewardedAd();
    }
}
