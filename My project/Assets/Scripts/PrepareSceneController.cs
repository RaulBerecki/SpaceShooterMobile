using GooglePlayGames;
using GooglePlayGames.BasicApi;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;
using UnityEngine.SocialPlatforms.Impl;
using UnityEngine.SocialPlatforms;

public class PrepareSceneController : MonoBehaviour
{
    public TextMeshProUGUI debugText;
    public GameObject button;
    // Start is called before the first frame update
    void Start()
    {
        SignIn();
    }
    public void SignIn()
    {
        button.SetActive(false);
        PlayGamesPlatform.Activate();
        Social.localUser.Authenticate((success) =>
        {
            if (success)
            {
                PlayGamesPlatform.Instance.LoadScores(
            "CgkIsKb_vv4SEAIQAQ",
            LeaderboardStart.PlayerCentered,
            1,
            LeaderboardCollection.Public,
            LeaderboardTimeSpan.AllTime,
            (data) =>
            {
                if (data.Valid)
                {
                    IScore myScore = data.PlayerScore;
                    if (myScore != null && myScore.value > 0)
                    {
                        // Save to PlayerPrefs
                        PlayerPrefs.SetInt("highscore", (int)myScore.value);
                        PlayerPrefs.Save();
                        Application.LoadLevel("SampleScene");
                    }
                    else
                    {
                        PlayGamesPlatform.Instance.ReportScore(0, "CgkIsKb_vv4SEAIQAQ", success =>
                        {
                            Debug.Log(success ? "Initial score submitted." : "Failed to submit initial score.");
                        });
                        PlayerPrefs.SetInt("highscore", 0);
                        PlayerPrefs.Save();
                        Application.LoadLevel("DrivingTutorial");
                    }
                }
                else
                {
                    Debug.LogError("Failed to load player score.");
                }
            });       
            }
            else
            {
                debugText.text = "Not logged in";
                button.SetActive(true);
            }
        });
    }
}
