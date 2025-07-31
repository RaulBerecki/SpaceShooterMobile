using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using GooglePlayGames;

public class GameManagerScript : MonoBehaviour
{
    private const string idTest = "A_21r32tef";
    public float timerLeft, timerRight,timerUp,timerDown,timerAid,timerAid2,timerPower;
    public GameObject largeMeteor, smallMeteor,bulletAid,fuelAid, resourceAid;
    public GameObject[] spawnPoints,powers;
    PlayerController playerController;
    public float score,lastScore,difficultyRate,timeRate;
    public TextMeshProUGUI scoreText;
    AudioSource audioSource;
    public AudioClip scoreSound;
    public bool AdCompleted, AdSeen;
    DatabaseController databaseController;
    UI_ManagerController uiManagerController;
    bool resourceAdded;

    [SerializeField] List<GameObject> ships, satellites;
    public GameObject currentShip;
    [Header("Camera")]
    public Camera cam;
    public BoxCollider2D[] edges;
    float camHeight;
    // Start is called before the first frame update
    void Start()
    {
        resourceAdded = false;
        databaseController = GameObject.FindGameObjectWithTag("Database").GetComponent<DatabaseController>();
        currentShip = Instantiate(ships[databaseController.currentData.lastShipPlayed],Vector3.zero,Quaternion.identity);
        uiManagerController = GameObject.FindGameObjectWithTag("UIManager").GetComponent<UI_ManagerController>();
        uiManagerController.playerController = currentShip.GetComponent<PlayerController>();
        playerController = currentShip.GetComponent<PlayerController>();
        audioSource = GetComponent<AudioSource>();
        timerLeft = 2;
        timerRight = 2;
        timerUp = 2;
        timerDown = 2;
        lastScore = score = 0;
        timerAid = Random.Range(5, 10);
        timerAid2 = Random.Range(5, 10);
        timerPower = Random.Range(10, 20);
        if (PlayerPrefs.GetInt("AdSeen") == 0)
        {
            AdCompleted = AdSeen = false;
        }
        else
        {
            AdSeen = true;
            playerController.trailAvailable = PlayerPrefs.GetInt("trail");
            playerController.bulletsAvailable = PlayerPrefs.GetInt("bullet");
            playerController.resourcesCollected = PlayerPrefs.GetInt("resources");
            databaseController.currentData.AddResources(-PlayerPrefs.GetInt("resources"));
            PlayerPrefs.SetInt("AdSeen", 0);
            PlayerPrefs.SetInt("trail", 0);
            PlayerPrefs.SetInt("bullet", 0);
            PlayerPrefs.SetInt("resources", 0);
        }
            timeRate = Random.Range(30, 60);
        difficultyRate = 1;
    }

    // Update is called once per frame
    void Update()
    {
        CameraEdges();
        SpawnPointsPosition();
        if (!playerController.pausing && playerController.playing)
        {
            GenerateEnemy();
            GenerateAids();
            GeneratePowers();
            IncreaseDificulty();
            score += Time.deltaTime;
        }
        if ((int)score / 200 > (int)lastScore / 200)
        {
            audioSource.clip = scoreSound;
            audioSource.Play();
            lastScore = (score / 500) * 500;
        }
        if (playerController.gameOver && !resourceAdded)
        {
            if (databaseController.currentData.shipInfos[databaseController.currentData.lastShipPlayed].highscoreShip < (int)score)
            {
                databaseController.currentData.shipInfos[databaseController.currentData.lastShipPlayed].highscoreShip = (int)score;
                if (databaseController.currentData.highscore < (int)score)
                {
                    databaseController.currentData.highscore = (int)score;
                }

            }
            databaseController.currentData.AddResources(playerController.resourcesCollected);
            //save local
            databaseController.UpdateData(databaseController.currentData);
            resourceAdded = true;
        }
        if (AdCompleted && !AdSeen)
        {
            ReloadGameAfterAd();
        }
        scoreText.text=score.ToString("0");
    }
    public void LeftClicked()
    {
        playerController.LeftButtonClicked();
    }
    public void RightClicked()
    {
        playerController.RightButtonClicked();
    }
    public void LeftNotClicked()
    {
        playerController.LeftButtonNotClicked();
    }
    public void RightNotClicked()
    {
        playerController.RightButtonNotClicked();
    }
    void IncreaseDificulty()
    {
        timeRate -= Time.deltaTime;
        if (timeRate < 0)
        {
            difficultyRate -= Random.Range(0, 0.05f);
            timeRate = Random.Range(30, 60);
        }
    }
    void SpawnPointsPosition()
    {
        float camHeight = 2f * cam.orthographicSize;
        float camWidth = camHeight * cam.aspect;
        // Top
        spawnPoints[2].transform.position = new Vector2(0, camHeight / 2f + 2f);
        // Bottom
        spawnPoints[3].transform.position = new Vector2(0, -camHeight / 2f - 2f);
        // Right
        spawnPoints[1].transform.position = new Vector2(camWidth / 2f + 2f, 0);
        // Left
        spawnPoints[0].transform.position = new Vector2(-camWidth / 2f - 2f, 0);
    }
    void CameraEdges()
    {
        float camHeight = 2f * cam.orthographicSize;
        float camWidth = camHeight * cam.aspect;
        // Top Edge
        edges[0].size = new Vector2(camWidth, 0.1f);
        edges[0].offset = new Vector2(0, camHeight / 2f + 0.05f);
        // Bottom Edge
        edges[1].size = new Vector2(camWidth, 0.1f);
        edges[1].offset = new Vector2(0, -camHeight / 2f - 0.05f);
        // Right Edge
        edges[2].size = new Vector2(0.1f, camHeight);
        edges[2].offset = new Vector2(camWidth / 2f + 0.05f, 0);
        // Left Edge
        edges[3].size = new Vector2(0.1f, camHeight);
        edges[3].offset = new Vector2(-camWidth / 2f - 0.05f, 0);
    }
    void ReloadGameAfterAd()
    {
        playerController.trailAvailable += 50;
        playerController.bulletsAvailable += 50;
        PlayerPrefs.SetInt("AdSeen", 1);
        PlayerPrefs.SetInt("trail", playerController.trailAvailable);
        PlayerPrefs.SetInt("bullet", playerController.bulletsAvailable);
        PlayerPrefs.SetInt("resources", playerController.resourcesCollected);
        Debug.Log("adSeen");
        Application.LoadLevel("SampleScene");
    }
    void GeneratePowers()
    {
        timerPower-=Time.deltaTime;
        if(timerPower < 0 )
        {
            float camHeight = 2f * cam.orthographicSize;
            float camWidth = camHeight * cam.aspect;
            int choice = Random.Range(0, 4);
            GameObject aid;
            float angleChange = Random.Range(-15, 15);
            float coordChangeWidth = Random.Range(-camWidth/2f, camWidth/2f);
            float coordChangeHeight = Random.Range(-camHeight/2f, camHeight / 2f);
            int choicePower = Random.Range(0, powers.Length);
            if (choice == 0)//Left
            {
                aid = Instantiate(powers[choicePower], spawnPoints[0].transform.position + new Vector3(0, coordChangeHeight, 0), spawnPoints[0].transform.rotation);
                aid.transform.eulerAngles = new Vector3(0, 0, -90 + angleChange);
            }
            else if (choice == 1)//Right
            {
                aid = Instantiate(powers[choicePower], spawnPoints[1].transform.position + new Vector3(0, coordChangeHeight, 0), spawnPoints[1].transform.rotation);
                aid.transform.eulerAngles = new Vector3(0, 0, 90 + angleChange);
            }
            else if (choice == 2)//Up
            {
                aid = Instantiate(powers[choicePower], spawnPoints[2].transform.position + new Vector3(coordChangeWidth, 0, 0), spawnPoints[2].transform.rotation);
                aid.transform.eulerAngles = new Vector3(0, 0, -180 + angleChange);
            }
            else//Down
            {
                aid = Instantiate(powers[choicePower], spawnPoints[3].transform.position + new Vector3(coordChangeWidth, 0, 0), spawnPoints[3].transform.rotation);
                aid.transform.eulerAngles = new Vector3(0, 0, 0 + angleChange);
            }
            timerPower = Random.Range(10, 20);
        }
    }
    void GenerateAids()
    {
        timerAid -= Time.deltaTime;
        timerAid2-= Time.deltaTime;
        if(timerAid < 0)
        {
            float camHeight = 2f * cam.orthographicSize;
            float camWidth = camHeight * cam.aspect;
            int choice=Random.Range(0,4);
            GameObject aid;
            float angleChange = Random.Range(-15, 15);
            float coordChangeWidth = Random.Range(-camWidth / 2f, camWidth / 2f);
            float coordChangeHeight = Random.Range(-camHeight / 2f, camHeight / 2f);
            if (choice == 0)
            {
                aid = Instantiate(bulletAid, spawnPoints[0].transform.position + new Vector3(0, coordChangeHeight, 0), spawnPoints[0].transform.rotation);
                aid.transform.eulerAngles = new Vector3(0, 0, -90 + angleChange);
            }
            else if (choice == 1)
            {
                aid = Instantiate(bulletAid, spawnPoints[1].transform.position + new Vector3(0, coordChangeHeight, 0), spawnPoints[1].transform.rotation);
                aid.transform.eulerAngles = new Vector3(0, 0, 90 + angleChange);
            }
            else if (choice == 2)
            {
                aid = Instantiate(bulletAid, spawnPoints[2].transform.position + new Vector3(coordChangeWidth, 0, 0), spawnPoints[2].transform.rotation);
                aid.transform.eulerAngles = new Vector3(0, 0, -180 + angleChange);
            }
            else
            {
                aid = Instantiate(bulletAid, spawnPoints[3].transform.position + new Vector3(coordChangeWidth, 0, 0), spawnPoints[3].transform.rotation);
                aid.transform.eulerAngles = new Vector3(0, 0, 0 + angleChange);
            }
            timerAid = difficultyRate * Random.Range(3, 8);
        }
        if (timerAid2 < 0)
        {
            float camHeight = 2f * cam.orthographicSize;
            float camWidth = camHeight * cam.aspect;
            int choice = Random.Range(0, 4);
            GameObject aid;
            float angleChange = Random.Range(-15, 15);
            float coordChangeWidth = Random.Range(-camWidth / 2f, camWidth / 2f);
            float coordChangeHeight = Random.Range(-camHeight / 2f, camHeight/2f);
            if (choice == 0)
            {
                aid = Instantiate(fuelAid, spawnPoints[0].transform.position + new Vector3(0, coordChangeHeight, 0), spawnPoints[0].transform.rotation);
                aid.transform.eulerAngles = new Vector3(0, 0, -90 + angleChange);
            }
            else if (choice == 1)
            {
                aid = Instantiate(fuelAid, spawnPoints[1].transform.position + new Vector3(0, coordChangeHeight, 0), spawnPoints[1].transform.rotation);
                aid.transform.eulerAngles = new Vector3(0, 0, 90 + angleChange);
            }
            else if (choice == 2)
            {
                aid = Instantiate(fuelAid, spawnPoints[2].transform.position + new Vector3(coordChangeWidth, 0, 0), spawnPoints[2].transform.rotation);
                aid.transform.eulerAngles = new Vector3(0, 0, -180 + angleChange);
            }
            else
            {
                aid = Instantiate(fuelAid, spawnPoints[3].transform.position + new Vector3(coordChangeWidth, 0, 0), spawnPoints[3].transform.rotation);
                aid.transform.eulerAngles = new Vector3(0, 0, 0 + angleChange);
            }
            timerAid2 = difficultyRate * Random.Range(3, 8);
        }
    }
    void GenerateEnemy()
    {
        timerLeft -= Time.deltaTime;
        timerRight -= Time.deltaTime;
        timerUp -= Time.deltaTime;
        timerDown -= Time.deltaTime;
        if (timerLeft < 0)
        {
            float camHeight = 2f * cam.orthographicSize;
            int enemey = Random.Range(0, 8);
            GameObject instance;
            float angleChange = Random.Range(-15, 15);
            float coordChangeHeight = Random.Range(-camHeight / 2f, camHeight / 2f);
            if (enemey <7)
            {
                int choice = Random.Range(0, 3);
                if (choice == 0)
                {
                    instance = Instantiate(smallMeteor, spawnPoints[0].transform.position + new Vector3(0, coordChangeHeight, 0), spawnPoints[0].transform.rotation);
                    instance.transform.eulerAngles = new Vector3(0, 0, -90 + angleChange);
                    if (angleChange > 0)
                    {
                        instance.GetComponent<MeteorController>().animator.Play("LeftRotatingSmallMeteor");
                    }
                    else
                    {
                        instance.GetComponent<MeteorController>().animator.Play("RightRotatingSmallMeteor");
                    }
                }
                else
                {
                    instance = Instantiate(largeMeteor, spawnPoints[0].transform.position + new Vector3(0, coordChangeHeight, 0), spawnPoints[0].transform.rotation);
                    instance.transform.eulerAngles = new Vector3(0, 0, -90 + angleChange);
                    if (angleChange > 0)
                    {
                        instance.GetComponent<MeteorController>().animator.Play("LeftRotatingLargeMeteor");
                    }
                    else
                    {
                        instance.GetComponent<MeteorController>().animator.Play("RightRotatingLargeMeteor");
                    }
                }
                instance.GetComponent<MeteorController>().rb.linearVelocity = instance.transform.up * 1.5f;
            }
            else if(enemey == 7)
            {
                instance = Instantiate(satellites[Random.Range(0,satellites.Count)], spawnPoints[0].transform.position + new Vector3(0, coordChangeHeight, 0), spawnPoints[0].transform.rotation);
                instance.transform.eulerAngles = new Vector3(0, 0, -90 + angleChange);
                if (angleChange > 0)
                {
                    instance.GetComponent<SatelliteController>().animator.Play("SatelliteRotateLeft");
                }
                else
                {
                    instance.GetComponent<SatelliteController>().animator.Play("SatelliteRotateRight");
                }
            }
            timerLeft = difficultyRate * Random.Range(2, 6);
        }
        if (timerRight < 0)
        {
            float camHeight = 2f * cam.orthographicSize;
            GameObject instance;
            float angleChange = Random.Range(-15, 15);
            float coordChangeHeight = Random.Range(-camHeight / 2f, camHeight / 2f);
            int enemy = Random.Range(0, 8);
            if(enemy < 7)
            {
                int choice = Random.Range(0, 2);
                if (choice == 0)
                {
                    instance = Instantiate(smallMeteor, spawnPoints[1].transform.position + new Vector3(0, coordChangeHeight, 0), spawnPoints[1].transform.rotation);
                    instance.transform.eulerAngles = new Vector3(0, 0, 90 + angleChange);
                    if (angleChange > 0)
                    {
                        instance.GetComponent<MeteorController>().animator.Play("LeftRotatingSmallMeteor");
                    }
                    else
                    {
                        instance.GetComponent<MeteorController>().animator.Play("RightRotatingSmallMeteor");
                    }
                }
                else
                {
                    instance = Instantiate(largeMeteor, spawnPoints[1].transform.position + new Vector3(0, coordChangeHeight, 0), spawnPoints[1].transform.rotation);
                    instance.transform.eulerAngles = new Vector3(0, 0, 90 + angleChange);
                    if (angleChange > 0)
                    {
                        instance.GetComponent<MeteorController>().animator.Play("LeftRotatingLargeMeteor");
                    }
                    else
                    {
                        instance.GetComponent<MeteorController>().animator.Play("RightRotatingLargeMeteor");
                    }
                }
                instance.GetComponent<MeteorController>().rb.linearVelocity = instance.transform.up * 1.5f;
            }
            else if (enemy == 7)
            {
                instance = Instantiate(satellites[Random.Range(0, satellites.Count)], spawnPoints[1].transform.position + new Vector3(0, coordChangeHeight, 0), spawnPoints[0].transform.rotation);
                instance.transform.eulerAngles = new Vector3(0, 0, 90 + angleChange);
                if (angleChange > 0)
                {
                    instance.GetComponent<SatelliteController>().animator.Play("SatelliteRotateLeft");
                }
                else
                {
                    instance.GetComponent<SatelliteController>().animator.Play("SatelliteRotateRight");
                }
            }
            timerRight = difficultyRate * Random.Range(2, 6);
        }
        if (timerUp < 0)
        {
            float camWidth = camHeight * cam.aspect;
            GameObject instance;
            float angleChange = Random.Range(-15, 15);
            float coordChangeWidth = Random.Range(-camWidth / 2f, camWidth / 2f);
            int enemy = Random.Range(0, 8);
            if (enemy < 7)
            {
                int choice = Random.Range(0, 2);
                if (choice == 0)
                {
                    instance = Instantiate(smallMeteor, spawnPoints[2].transform.position + new Vector3(coordChangeWidth, 0, 0), spawnPoints[2].transform.rotation);
                    instance.transform.eulerAngles = new Vector3(0, 0, -180 + angleChange);
                    if (angleChange > 0)
                    {
                        instance.GetComponent<MeteorController>().animator.Play("LeftRotatingSmallMeteor");
                    }
                    else
                    {
                        instance.GetComponent<MeteorController>().animator.Play("RightRotatingSmallMeteor");
                    }
                }
                else
                {
                    instance = Instantiate(largeMeteor, spawnPoints[2].transform.position + new Vector3(coordChangeWidth, 0, 0), spawnPoints[2].transform.rotation);
                    instance.transform.eulerAngles = new Vector3(0, 0, -180 + angleChange);
                    if (angleChange > 0)
                    {
                        instance.GetComponent<MeteorController>().animator.Play("LeftRotatingLargeMeteor");
                    }
                    else
                    {
                        instance.GetComponent<MeteorController>().animator.Play("RightRotatingLargeMeteor");
                    }
                }
                instance.GetComponent<MeteorController>().rb.linearVelocity = instance.transform.up * 1.5f;
            }
            else if (enemy == 7)
            {
                instance = Instantiate(satellites[Random.Range(0, satellites.Count)], spawnPoints[2].transform.position + new Vector3(0, coordChangeWidth, 0), spawnPoints[0].transform.rotation);
                instance.transform.eulerAngles = new Vector3(0, 0, -180 + angleChange);
                if (angleChange > 0)
                {
                    instance.GetComponent<SatelliteController>().animator.Play("SatelliteRotateLeft");
                }
                else
                {
                    instance.GetComponent<SatelliteController>().animator.Play("SatelliteRotateRight");
                }
            }
            timerUp = difficultyRate * Random.Range(2, 6);
        }
        if (timerDown < 0)
        {
            float camWidth = camHeight * cam.aspect;
            GameObject instance;
            float angleChange = Random.Range(-15, 15);
            float coordChangeWidth = Random.Range(-camWidth / 2f, camWidth / 2f);
            int enemy = Random.Range(0, 8);
            if (enemy < 7)
            {
                int choice = Random.Range(0, 2);
                if (choice == 0)
                {
                    instance = Instantiate(smallMeteor, spawnPoints[3].transform.position + new Vector3(coordChangeWidth, 0, 0), spawnPoints[3].transform.rotation);
                    instance.transform.eulerAngles = new Vector3(0, 0, 0 + angleChange);
                    if (angleChange > 0)
                    {
                        instance.GetComponent<MeteorController>().animator.Play("LeftRotatingSmallMeteor");
                    }
                    else
                    {
                        instance.GetComponent<MeteorController>().animator.Play("RightRotatingSmallMeteor");
                    }
                }
                else
                {
                    instance = Instantiate(largeMeteor, spawnPoints[3].transform.position + new Vector3(coordChangeWidth, 0, 0), spawnPoints[3].transform.rotation);
                    instance.transform.eulerAngles = new Vector3(0, 0, 0 + angleChange);
                    if (angleChange > 0)
                    {
                        instance.GetComponent<MeteorController>().animator.Play("LeftRotatingLargeMeteor");
                    }
                    else
                    {
                        instance.GetComponent<MeteorController>().animator.Play("RightRotatingLargeMeteor");
                    }
                }
                instance.GetComponent<MeteorController>().rb.linearVelocity = instance.transform.up * 1.5f;
            }
            else if (enemy == 7)
            {
                instance = Instantiate(satellites[Random.Range(0, satellites.Count)], spawnPoints[3].transform.position + new Vector3(0, coordChangeWidth, 0), spawnPoints[0].transform.rotation);
                instance.transform.eulerAngles = new Vector3(0, 0, 0 + angleChange);
                if (angleChange > 0)
                {
                    instance.GetComponent<SatelliteController>().animator.Play("SatelliteRotateLeft");
                }
                else
                {
                    instance.GetComponent<SatelliteController>().animator.Play("SatelliteRotateRight");
                }
            }
            timerDown = difficultyRate * Random.Range(2, 6);
        }
    }
    public void ChangeShip(int shipId)
    {
        Destroy(currentShip);
        currentShip = Instantiate(ships[shipId], Vector3.zero, Quaternion.identity);
        uiManagerController.playerController = currentShip.GetComponent<PlayerController>();
        playerController = currentShip.GetComponent<PlayerController>();
    }
}
