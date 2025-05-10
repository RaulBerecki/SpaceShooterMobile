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
    public DatabaseController databaseController;
    public bool test;
    // Start is called before the first frame update
    void Start()
    {
        databaseController = GameObject.FindGameObjectWithTag("Database").GetComponent<DatabaseController>();
        if(test)
            databaseController.BeginDatabase("A_21r32tef", "Raul");
        else
            SignIn();
    }
    private void Update()
    {
        
    }
    public void SignIn()
    {
        PlayGamesPlatform.Activate();
        Social.localUser.Authenticate((success) =>
        {
            if (success)
            {
                databaseController.BeginDatabase(Social.localUser.id,Social.localUser.userName);
            }
            else
            {
                Debug.LogError("Failed to load player score.");
            }
            });       
    }
    public void UpdateVersion()
    {
        Application.OpenURL("https://play.google.com/store/apps/details?id=com.BeryStudio.Astroviator");
    }
}
