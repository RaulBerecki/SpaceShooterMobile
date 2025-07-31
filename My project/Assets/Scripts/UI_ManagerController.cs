using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SocialPlatforms.Impl;
using GooglePlayGames;
using GooglePlayGames.BasicApi;
using UnityEngine.SocialPlatforms;
using System.Linq;

public class UI_ManagerController : MonoBehaviour
{
    public Animator playButton, pauseButton,scoreText,highscoreText;
    public GameObject pauseMenu,mainMenu,gameMenu,gameOverPanel,leaderboardPanel,shopPanel;
    public TextMeshProUGUI[] highscoreTexts;
    public PlayerController playerController;
    public TextMeshProUGUI finalScoreText,highscoreTextUI;
    AudioSource uiAudioSource;
    public AudioClip buttonSound;
    GameManagerScript gameManagerScript;
    public RewardedAds rewardedAds;
    DatabaseController databaseController;
    public string[] highscoreTextSaver;
    public TextMeshProUGUI bulletsText;
    public TextMeshProUGUI trailText;
    [Header("Game Over")]
    public float timerShowAd;
    public bool gameOver;
    public TextMeshProUGUI timerShowAdText;
    public GameObject showAdPanel, RestartPanel;
    [Header("Shop")]
    public List<GameObject> shipViews,buyButtons,highscoreTextShop;
    public TextMeshProUGUI coinsText,resourceText;
    // Start is called before the first frame update
    void Start()
    {
        mainMenu.SetActive(true);
        gameMenu.SetActive(false);
        pauseMenu.SetActive(false);
        gameOverPanel.SetActive(false);
        shopPanel.SetActive(false);
        uiAudioSource = GetComponent<AudioSource>();
        gameManagerScript = GameObject.FindGameObjectWithTag("Manager").GetComponent<GameManagerScript>();
        databaseController = GameObject.FindGameObjectWithTag("Database").GetComponent<DatabaseController>();
        databaseController.uiController = this;
        rewardedAds = GameObject.FindGameObjectWithTag("AdManager").GetComponent<RewardedAds>();
        rewardedAds.gameManagerScript = gameManagerScript;
        rewardedAds.uiManagerController = this;
        timerShowAd = 5f;
        gameOver = false;
        highscoreTextUI.text = databaseController.currentData.highscore.ToString();
        for (int i = 0; i < buyButtons.Count; i++)
        {
            if (databaseController.currentData.shipInfos[i + 1].isOwned)
            {
                buyButtons[i].SetActive(false);
            }
        }
        for (int i = 0; i < shipViews.Count; i++)
        {
            if (databaseController.currentData.lastShipPlayed == i)
                shipViews[i].SetActive(true);
            else
                shipViews[i].SetActive(false);
            if (databaseController.currentData.shipInfos[i].isOwned)
            {
                highscoreTextShop[2*i].SetActive(true);
                highscoreTextShop[2*i+1].SetActive(true);
                highscoreTextShop[2 * i + 1].GetComponent<TextMeshProUGUI>().text = databaseController.currentData.shipInfos[i].highscoreShip.ToString();
            }
        }
        coinsText.text=databaseController.currentData.coins.ToString();
        resourceText.text = databaseController.currentData.resources.ToString();
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
        bulletsText.text = playerController.bulletsAvailable.ToString("0");
        trailText.text = playerController.trailAvailable.ToString("0");
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
    public void Shop()
    {
        mainMenu.SetActive(false);
        shopPanel.SetActive(true);
    }
    public void BackToMenuFromShop()
    {
        mainMenu.SetActive(true);
        shopPanel.SetActive(false);
    }
    public void GameOver()
    {
        gameMenu.SetActive(false);
        finalScoreText.text = gameManagerScript.score.ToString("0");
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
    //AD FUNCTIONS
    public void ShowAd()
    {
        rewardedAds.ShowRewardedAd();
    }
    public void SkipAd()
    {
        showAdPanel.SetActive(false);
        RestartPanel.SetActive(true);
    }
    //SHOP FUNCTIONS
    public void ShipSelect(int shipId)
    {
        for (int i = 0; i < shipViews.Count; i++)
        {
            if (shipId == i)
                shipViews[i].SetActive(true);
            else
                shipViews[i].SetActive(false);
        }
        if (databaseController.currentData.shipInfos[shipId].isOwned)
        {
            databaseController.currentData.lastShipPlayed = shipId;
            //Save Local
            databaseController.UpdateData(databaseController.currentData);
            gameManagerScript.ChangeShip(shipId);
        }
    }
    public void BuyShip(int shipId)
    {
        if (databaseController.currentData.coins >= databaseController.currentData.shipInfos[shipId].price)
        {
            databaseController.currentData.shipInfos[shipId].isOwned = true;
            databaseController.currentData.coins -= databaseController.currentData.shipInfos[shipId].price;
            databaseController.currentData.lastShipPlayed = shipId;
            buyButtons[shipId - 1].SetActive(false);
            highscoreTextShop[2 * shipId].SetActive(true);
            highscoreTextShop[2 * shipId + 1].SetActive(true);
            highscoreTextShop[2 * shipId + 1].GetComponent<TextMeshProUGUI>().text = databaseController.currentData.shipInfos[shipId].highscoreShip.ToString();
            databaseController.UpdateData(databaseController.currentData);
            gameManagerScript.ChangeShip(shipId);
            coinsText.text = databaseController.currentData.coins.ToString();
        }
    }
    public void Trade10Resources()
    {
        if (databaseController.currentData.resources >= 10)
        {
            databaseController.currentData.Trade10();
            coinsText.text = databaseController.currentData.coins.ToString();
            resourceText.text = databaseController.currentData.resources.ToString();
            databaseController.UpdateData(databaseController.currentData);
        }
    }
}
