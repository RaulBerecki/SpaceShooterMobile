using NUnit.Framework;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TMPro;
using Newtonsoft.Json;
using UnityEngine;
using UnityEngine.Networking;

public class DatabaseController : MonoBehaviour
{
    private static DatabaseController instance;
    public UI_ManagerController uiController;
    public TextMeshProUGUI debugText;
    string supabaseUrl = "https://molvqetyggsjxjcmakzv.supabase.co";
    string supabaseKey = "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJpc3MiOiJzdXBhYmFzZSIsInJlZiI6Im1vbHZxZXR5Z2dzanhqY21ha3p2Iiwicm9sZSI6ImFub24iLCJpYXQiOjE3NDQ2MTgyNDUsImV4cCI6MjA2MDE5NDI0NX0.KngYnTcLcTjip3qi51gKUXzGaMseZZ3o4Q9T1LoSwQM";
    public GameObject updatePanel;
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void Start()
    {
        
    }
    private void Update()
    {
        
    }
    public void BeginDatabase(string userId,string username)
    {
        StartCoroutine(CheckUserExists(userId,username,exists=>
        {

        }
        ));
    }

    IEnumerator CheckUserExists(string userId, string username, Action<bool> onResult)
    {
        string url = "https://molvqetyggsjxjcmakzv.supabase.co/rest/v1/users?id=eq." + userId;

        UnityWebRequest request = UnityWebRequest.Get(url);
        request.SetRequestHeader("apikey", supabaseKey);
        request.SetRequestHeader("Authorization", "Bearer " + supabaseKey);
        request.SetRequestHeader("Accept", "application/json");

        yield return request.SendWebRequest();
        if (request.result == UnityWebRequest.Result.Success)
        {
            string jsonResponse1 = request.downloadHandler.text;
            string url2 = "https://molvqetyggsjxjcmakzv.supabase.co/rest/v1/gameDetails";

            UnityWebRequest request2 = UnityWebRequest.Get(url2);
            request2.SetRequestHeader("apikey", supabaseKey);
            request2.SetRequestHeader("Authorization", "Bearer " + supabaseKey);
            request2.SetRequestHeader("Accept", "application/json");
            
            yield return request2.SendWebRequest();
            if(request2.result == UnityWebRequest.Result.Success)
            {
                string jsonResponse2 = request2.downloadHandler.text;
                var game = JsonConvert.DeserializeObject<List<GameDetail>>(jsonResponse2);
                if (game[0].gameVersion == Application.version)
                {
                    if (jsonResponse1 == "[]")
                    {
                        StartCoroutine(SaveUserData(userId, username));
                    }
                    else
                    {
                        var users = JsonConvert.DeserializeObject<List<User>>(jsonResponse1);
                        if (!PlayerPrefs.HasKey("highscore"))
                        {
                            PlayerPrefs.SetInt("highscore", users[0].highscore);
                        }
                        Application.LoadLevel("SampleScene");
                        Debug.Log("User ID exists!");
                        Debug.Log("Response: " + users[0].highscore);
                    }
                }
                else
                {
                    updatePanel.SetActive(true);
                }
            }         
        }
        else
        {
            Debug.LogError("Error checking ID: " + request.error);
            Debug.LogError("Response: " + request.downloadHandler.text);
        }
    }

    IEnumerator SaveUserData(string userId,string username)
    {
        User newUser = new User(userId, username);
        string json = JsonUtility.ToJson(newUser);
        UnityWebRequest req = new UnityWebRequest(supabaseUrl + "/rest/v1/users", "POST");
        Debug.Log(json);
        byte[] bodyRaw = new System.Text.UTF8Encoding().GetBytes(json);
        req.uploadHandler = new UploadHandlerRaw(bodyRaw);
        req.downloadHandler = new DownloadHandlerBuffer();

        req.SetRequestHeader("Content-Type", "application/json");
        req.SetRequestHeader("apikey", supabaseKey);
        req.SetRequestHeader("Authorization", "Bearer " + supabaseKey);

        yield return req.SendWebRequest();

        if (req.result == UnityWebRequest.Result.Success)
        {
            PlayerPrefs.SetInt("highscore", 0);
            Debug.Log("User saved: " + req.downloadHandler.text);
            Application.LoadLevel("DrivingTutorial");
        }
        else
        {
            Debug.LogError("Save failed: " + req.error);
        }
    }
    
    public void UpdateHighscore(string userId, int newScore)
    {
        StartCoroutine(UpdateHighscoreDatabase(userId, newScore));
    }
    IEnumerator UpdateHighscoreDatabase(string userId, int newScore)
    {
        string url = "https://molvqetyggsjxjcmakzv.supabase.co/rest/v1/users?id=eq." + userId;

        string jsonBody = "{\"highscore\": " + newScore + "}";

        UnityWebRequest request = UnityWebRequest.Put(url, jsonBody);
        request.method = "PATCH"; // Supabase uses PATCH for updates
        request.SetRequestHeader("apikey", supabaseKey);
        request.SetRequestHeader("Authorization", "Bearer "+supabaseKey);
        request.SetRequestHeader("Content-Type", "application/json");

        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.Success)
        {
            Debug.Log("Highscore updated!");
        }
        else
        {
            Debug.LogError("Update failed: " + request.error + " | " + request.downloadHandler.text);
        }
    }
    public void GetTop10()
    {
        StartCoroutine(GetTop10FromDatabase());
    }
    IEnumerator GetTop10FromDatabase()
    {
        string url = $"{supabaseUrl}/rest/v1/users?select=*&order=highscore.desc&limit=10";
        UnityWebRequest request = UnityWebRequest.Get(url);
        request.SetRequestHeader("apikey", supabaseKey);
        request.SetRequestHeader("Authorization", "Bearer " + supabaseKey);
        request.SetRequestHeader("Content-Type", "application/json");

        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.Success)
        {
            string json = request.downloadHandler.text;
            var users = JsonConvert.DeserializeObject <List<User>>(json);
            for(int i = 0; i < users.Count; i++)
            {
                if (users[i].id==Social.localUser.id)
                {
                    uiController.highscoreTexts[i].text = (i + 1).ToString() + "." + users[i].username + " " + users[i].highscore;
                    uiController.highscoreTexts[i].color = new Color32(128, 255, 255, 255);
                }
                else
                {
                    uiController.highscoreTexts[i].text = (i + 1).ToString() + "." + users[i].username + " " + users[i].highscore;
                    uiController.highscoreTexts[i].color = Color.white;
                }
            }
            uiController.leaderboardPanel.SetActive(true);
            uiController.mainMenu.SetActive(false);
        }
        else
        {
            Debug.LogError("Failed to load leaderboard: " + request.error);
        }
    }
}
