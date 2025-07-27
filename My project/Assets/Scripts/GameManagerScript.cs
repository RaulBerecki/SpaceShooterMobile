using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using GooglePlayGames;

public class GameManagerScript : MonoBehaviour
{
    private const string idTest = "A_21r32tef";
    public float timerLeft, timerRight,timerUp,timerDown,timerAid,timerAid2,timerPower;
    public GameObject largeMeteor, smallMeteor,bulletAid,fuelAid;
    public GameObject[] spawnPoints,powers;
    PlayerController playerController;
    public float score,lastScore,difficultyRate,timeRate;
    public TextMeshProUGUI scoreText;
    AudioSource audioSource;
    public AudioClip scoreSound;
    public bool AdCompleted, AdSeen;
    DatabaseController databaseController;
    UI_ManagerController uiManagerController;

    [SerializeField] List<GameObject> ships;
    public GameObject currentShip;
    [Header("Camera")]
    public Camera cam;
    public BoxCollider2D[] edges;
    float camHeight;
    // Start is called before the first frame update
    void Start()
    {
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
        AdCompleted = AdSeen = false;
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
            GenerateMeteors();
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
        if (playerController.gameOver)
        {
            if (databaseController.currentData.shipInfos[databaseController.currentData.lastShipPlayed].highscoreShip < (int)score)
            {
                databaseController.currentData.shipInfos[databaseController.currentData.lastShipPlayed].highscoreShip = (int)score;
                if (databaseController.currentData.highscore < (int)score)
                {
                    databaseController.currentData.highscore = (int)score;
                }
                //save local
                databaseController.UpdateData(databaseController.currentData);
            }
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
        playerController.gameOver = false;
        playerController.playing = false;
        playerController.rb.linearVelocity = Vector2.zero;
        playerController.rb.constraints = RigidbodyConstraints2D.FreezeRotation;
        playerController.rb.constraints = RigidbodyConstraints2D.None;
        playerController.transform.position = new Vector2(0,0);
        playerController.trailRenderer.Clear();
        playerController.transform.eulerAngles = new Vector3(0,0,0);
        playerController.trailAvailable += 50;
        playerController.bulletsAvailable += 50;
        playerController.LeftButtonNotClicked();
        playerController.RightButtonNotClicked();
        AdCompleted = false;
        AdSeen = true;
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
            float angleChange = Random.RandomRange(-15, 15);
            float coordChangeWidth = Random.RandomRange(-camWidth/2f, camWidth/2f);
            float coordChangeHeight = Random.RandomRange(-camHeight/2f, camHeight / 2f);
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
            timerPower = Random.RandomRange(10, 20);
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
            float angleChange = Random.RandomRange(-15, 15);
            float coordChangeWidth = Random.RandomRange(-camWidth / 2f, camWidth / 2f);
            float coordChangeHeight = Random.RandomRange(-camHeight / 2f, camHeight / 2f);
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
            timerAid = difficultyRate * Random.RandomRange(3, 8);
        }
        if (timerAid2 < 0)
        {
            float camHeight = 2f * cam.orthographicSize;
            float camWidth = camHeight * cam.aspect;
            int choice = Random.Range(0, 4);
            GameObject aid;
            float angleChange = Random.RandomRange(-15, 15);
            float coordChangeWidth = Random.RandomRange(-camWidth / 2f, camWidth / 2f);
            float coordChangeHeight = Random.RandomRange(-camHeight / 2f, camHeight/2f);
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
            timerAid2 = difficultyRate * Random.RandomRange(3, 8);
        }
    }
    void GenerateMeteors()
    {
        timerLeft -= Time.deltaTime;
        timerRight -= Time.deltaTime;
        timerUp -= Time.deltaTime;
        timerDown -= Time.deltaTime;
        if (timerLeft < 0)
        {
            float camHeight = 2f * cam.orthographicSize;
            int choice = Random.RandomRange(0, 2);
            GameObject meteor;
            float angleChange = Random.RandomRange(-15, 15);
            float coordChangeHeight = Random.RandomRange(-camHeight / 2f, camHeight/2f);
            if (choice == 0)
            {
                meteor = Instantiate(smallMeteor, spawnPoints[0].transform.position + new Vector3(0, coordChangeHeight, 0), spawnPoints[0].transform.rotation);
                meteor.transform.eulerAngles = new Vector3(0, 0, -90 + angleChange);
                if (angleChange > 0)
                {
                    meteor.GetComponent<MeteorController>().animator.Play("LeftRotatingSmallMeteor");
                }
                else
                {
                    meteor.GetComponent<MeteorController>().animator.Play("RightRotatingSmallMeteor");
                }
            }
            else
            {
                meteor = Instantiate(largeMeteor, spawnPoints[0].transform.position + new Vector3(0, coordChangeHeight, 0), spawnPoints[0].transform.rotation);
                meteor.transform.eulerAngles = new Vector3(0, 0, -90 + angleChange);
                if (angleChange > 0)
                {
                    meteor.GetComponent<MeteorController>().animator.Play("LeftRotatingLargeMeteor");
                }
                else
                {
                    meteor.GetComponent<MeteorController>().animator.Play("RightRotatingLargeMeteor");
                }
            }
            meteor.GetComponent<MeteorController>().rb.linearVelocity = meteor.transform.up * 1.5f;
            timerLeft = difficultyRate * Random.RandomRange(2, 6);
        }
        if (timerRight < 0)
        {
            float camHeight = 2f * cam.orthographicSize;
            int choice = Random.RandomRange(0, 2);
            GameObject meteor;
            float angleChange = Random.RandomRange(-15, 15);
            float coordChangeHeight = Random.RandomRange(-camHeight / 2f, camHeight/2f);
            if (choice == 0)
            {
                meteor = Instantiate(smallMeteor, spawnPoints[1].transform.position + new Vector3(0, coordChangeHeight, 0), spawnPoints[1].transform.rotation);
                meteor.transform.eulerAngles = new Vector3(0, 0, 90 + angleChange);
                if (angleChange > 0)
                {
                    meteor.GetComponent<MeteorController>().animator.Play("LeftRotatingSmallMeteor");
                }
                else
                {
                    meteor.GetComponent<MeteorController>().animator.Play("RightRotatingSmallMeteor");
                }
            }
            else
            {
                meteor = Instantiate(largeMeteor, spawnPoints[1].transform.position + new Vector3(0, coordChangeHeight, 0), spawnPoints[1].transform.rotation);
                meteor.transform.eulerAngles = new Vector3(0, 0, 90 + angleChange);
                if (angleChange > 0)
                {
                    meteor.GetComponent<MeteorController>().animator.Play("LeftRotatingLargeMeteor");
                }
                else
                {
                    meteor.GetComponent<MeteorController>().animator.Play("RightRotatingLargeMeteor");
                }
            }
            meteor.GetComponent<MeteorController>().rb.linearVelocity = meteor.transform.up * 1.5f;
            timerRight = difficultyRate * Random.RandomRange(2, 6);
        }
        if (timerUp < 0)
        {
            float camWidth = camHeight * cam.aspect;
            int choice = Random.RandomRange(0, 2);
            GameObject meteor;
            float angleChange = Random.RandomRange(-15, 15);
            float coordChangeWidth = Random.RandomRange(-camWidth / 2f, camWidth / 2f);
            if (choice == 0)
            {
                meteor = Instantiate(smallMeteor, spawnPoints[2].transform.position + new Vector3(coordChangeWidth, 0, 0), spawnPoints[2].transform.rotation);
                meteor.transform.eulerAngles = new Vector3(0, 0, -180 + angleChange);
                if (angleChange > 0)
                {
                    meteor.GetComponent<MeteorController>().animator.Play("LeftRotatingSmallMeteor");
                }
                else
                {
                    meteor.GetComponent<MeteorController>().animator.Play("RightRotatingSmallMeteor");
                }
            }
            else
            {
                meteor = Instantiate(largeMeteor, spawnPoints[2].transform.position + new Vector3(coordChangeWidth, 0, 0), spawnPoints[2].transform.rotation);
                meteor.transform.eulerAngles = new Vector3(0, 0, -180 + angleChange);
                if (angleChange > 0)
                {
                    meteor.GetComponent<MeteorController>().animator.Play("LeftRotatingLargeMeteor");
                }
                else
                {
                    meteor.GetComponent<MeteorController>().animator.Play("RightRotatingLargeMeteor");
                }
            }
            meteor.GetComponent<MeteorController>().rb.linearVelocity = meteor.transform.up * 1.5f;
            timerUp = difficultyRate * Random.RandomRange(2, 6);
        }
        if (timerDown < 0)
        {
            float camWidth = camHeight * cam.aspect;
            int choice = Random.RandomRange(0, 2);
            GameObject meteor;
            float angleChange = Random.RandomRange(-15, 15);
            float coordChangeWidth = Random.RandomRange(-camWidth / 2f, camWidth / 2f);
            if (choice == 0)
            {
                meteor = Instantiate(smallMeteor, spawnPoints[3].transform.position + new Vector3(coordChangeWidth, 0, 0), spawnPoints[3].transform.rotation);
                meteor.transform.eulerAngles = new Vector3(0, 0, 0 + angleChange);
                if (angleChange > 0)
                {
                    meteor.GetComponent<MeteorController>().animator.Play("LeftRotatingSmallMeteor");
                }
                else
                {
                    meteor.GetComponent<MeteorController>().animator.Play("RightRotatingSmallMeteor");
                }
            }
            else
            {
                meteor = Instantiate(largeMeteor, spawnPoints[3].transform.position + new Vector3(coordChangeWidth, 0, 0), spawnPoints[3].transform.rotation);
                meteor.transform.eulerAngles = new Vector3(0, 0, 0 + angleChange);
                if (angleChange > 0)
                {
                    meteor.GetComponent<MeteorController>().animator.Play("LeftRotatingLargeMeteor");
                }
                else
                {
                    meteor.GetComponent<MeteorController>().animator.Play("RightRotatingLargeMeteor");
                }
            }
            meteor.GetComponent<MeteorController>().rb.linearVelocity = meteor.transform.up * 1.5f;
            timerDown = difficultyRate * Random.RandomRange(2, 6);
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
