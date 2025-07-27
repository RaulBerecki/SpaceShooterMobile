using Newtonsoft.Json;
using NUnit.Framework;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TMPro;
using UnityEditor.PackageManager.Requests;
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
    public User currentData;
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
                        if (users[0].shipInfos == null) // UPDATE USER CREATED
                        {
                            users[0].UpdateCurrentUser(users[0].highscore);
                            string json = JsonConvert.SerializeObject(users[0]);
                            string urlUpdate = "https://molvqetyggsjxjcmakzv.supabase.co/rest/v1/users?id=eq." + users[0].id;

                            UnityWebRequest requestUpdate = new UnityWebRequest(urlUpdate, "PATCH");
                            byte[] jsonToSend = System.Text.Encoding.UTF8.GetBytes(json);
                            requestUpdate.uploadHandler = new UploadHandlerRaw(jsonToSend);
                            requestUpdate.downloadHandler = new DownloadHandlerBuffer();
                            requestUpdate.SetRequestHeader("Content-Type", "application/json");
                            requestUpdate.SetRequestHeader("apikey", supabaseKey);
                            requestUpdate.SetRequestHeader("Authorization", "Bearer " + supabaseKey);

                            yield return requestUpdate.SendWebRequest();

                            if (request.result == UnityWebRequest.Result.Success)
                            {
                                Debug.Log("Highscore updated!");
                            }
                            else
                            {
                                Debug.LogError("Update failed: " + request.error + " | " + request.downloadHandler.text);
                            }
                        }
                        currentData = users[0];
                        Application.LoadLevel("SampleScene");
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
        string json = JsonConvert.SerializeObject(newUser);
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
    
    public void UpdateData(User data)
    {
        StartCoroutine(UpdateDatabase(data));
    }
    IEnumerator UpdateDatabase(User newData)
    {
        string url = "https://molvqetyggsjxjcmakzv.supabase.co/rest/v1/users?id=eq." + newData.id;

        string json = JsonConvert.SerializeObject(newData);
        string urlUpdate = "https://molvqetyggsjxjcmakzv.supabase.co/rest/v1/users?id=eq." + newData.id;

        UnityWebRequest requestUpdate = new UnityWebRequest(urlUpdate, "PATCH");
        byte[] jsonToSend = System.Text.Encoding.UTF8.GetBytes(json);
        requestUpdate.uploadHandler = new UploadHandlerRaw(jsonToSend);
        requestUpdate.downloadHandler = new DownloadHandlerBuffer();
        requestUpdate.SetRequestHeader("Content-Type", "application/json");
        requestUpdate.SetRequestHeader("apikey", supabaseKey);
        requestUpdate.SetRequestHeader("Authorization", "Bearer " + supabaseKey);

        yield return requestUpdate.SendWebRequest();

        if (requestUpdate.result == UnityWebRequest.Result.Success)
        {
            Debug.Log("Database updated!");
        }
        else
        {
            Debug.LogError("Update failed: " + requestUpdate.error + " | " + requestUpdate.downloadHandler.text);
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
            Debug.Log(json);
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
