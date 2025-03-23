using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameManagerScript : MonoBehaviour
{
    public float timerLeft, timerRight,timerUp,timerDown,timerAid;
    public GameObject largeMeteor, smallMeteor,bulletAid;
    public GameObject[] spawnPoints;
    PlayerController playerController;
    public float score,lastScore;
    public TextMeshProUGUI scoreText;
    AudioSource audioSource;
    public AudioClip scoreSound;
    // Start is called before the first frame update
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        timerLeft = 2;
        timerRight = 2;
        timerUp = 2;
        timerDown = 2;
        playerController = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
        lastScore = score = 0;
        timerAid = Random.RandomRange(5, 10);
    }

    // Update is called once per frame
    void Update()
    {
        if(!playerController.pausing && playerController.playing)
        {
            GenerateMeteors();
            GenerateAids();
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
            if (PlayerPrefs.GetInt("highscore") < (int)score)
            {
                PlayerPrefs.SetInt("highscore", (int)score);
            }
        }
        scoreText.text=score.ToString("0");
    }
    void GenerateAids()
    {
        timerAid -= Time.deltaTime;
        if(timerAid < 0)
        {
            int choice=Random.Range(0,4);
            GameObject aid;
            float angleChange = Random.RandomRange(-15, 15);
            float coordChange = Random.RandomRange(-6, 6);
            if (choice == 0)
            {
                aid = Instantiate(bulletAid, spawnPoints[0].transform.position + new Vector3(0, coordChange, 0), spawnPoints[0].transform.rotation);
                aid.transform.eulerAngles = new Vector3(0, 0, -90 + angleChange);
            }
            else if (choice == 1)
            {
                aid = Instantiate(bulletAid, spawnPoints[1].transform.position + new Vector3(0, coordChange, 0), spawnPoints[1].transform.rotation);
                aid.transform.eulerAngles = new Vector3(0, 0, -90 + angleChange);
            }
            else if (choice == 2)
            {
                aid = Instantiate(bulletAid, spawnPoints[2].transform.position + new Vector3(coordChange, 0, 0), spawnPoints[2].transform.rotation);
                aid.transform.eulerAngles = new Vector3(0, 0, -90 + angleChange);
            }
            else
            {
                aid = Instantiate(smallMeteor, spawnPoints[3].transform.position + new Vector3(coordChange, 0, 0), spawnPoints[3].transform.rotation);
                aid.transform.eulerAngles = new Vector3(0, 0, -90 + angleChange);
            }
            timerAid = Random.RandomRange(3, 8);
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
            int choice = Random.RandomRange(0, 2);
            GameObject meteor;
            float angleChange = Random.RandomRange(-15, 15);
            float coordChange = Random.RandomRange(-6, 6);
            if (choice == 0)
            {
                meteor = Instantiate(smallMeteor, spawnPoints[0].transform.position + new Vector3(0, coordChange, 0), spawnPoints[0].transform.rotation);
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
                meteor = Instantiate(largeMeteor, spawnPoints[0].transform.position + new Vector3(0, coordChange, 0), spawnPoints[0].transform.rotation);
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
            meteor.GetComponent<MeteorController>().rb.velocity = meteor.transform.up * 1.5f;
            timerLeft = Random.RandomRange(2, 6);
        }
        if (timerRight < 0)
        {
            int choice = Random.RandomRange(0, 2);
            GameObject meteor;
            float angleChange = Random.RandomRange(-15, 15);
            float coordChange = Random.RandomRange(-6, 6);
            if (choice == 0)
            {
                meteor = Instantiate(smallMeteor, spawnPoints[1].transform.position + new Vector3(0, coordChange, 0), spawnPoints[1].transform.rotation);
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
                meteor = Instantiate(largeMeteor, spawnPoints[1].transform.position + new Vector3(0, coordChange, 0), spawnPoints[1].transform.rotation);
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
            meteor.GetComponent<MeteorController>().rb.velocity = meteor.transform.up * 1.5f;
            timerRight = Random.RandomRange(2, 6);
        }
        if (timerUp < 0)
        {
            int choice = Random.RandomRange(0, 2);
            GameObject meteor;
            float angleChange = Random.RandomRange(-15, 15);
            float coordChange = Random.RandomRange(-12, 12);
            if (choice == 0)
            {
                meteor = Instantiate(smallMeteor, spawnPoints[2].transform.position + new Vector3(coordChange, 0, 0), spawnPoints[2].transform.rotation);
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
                meteor = Instantiate(largeMeteor, spawnPoints[2].transform.position + new Vector3(coordChange, 0, 0), spawnPoints[2].transform.rotation);
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
            meteor.GetComponent<MeteorController>().rb.velocity = meteor.transform.up * 1.5f;
            timerUp = Random.RandomRange(2, 6);
        }
        if (timerDown < 0)
        {
            int choice = Random.RandomRange(0, 2);
            GameObject meteor;
            float angleChange = Random.RandomRange(-15, 15);
            float coordChange = Random.RandomRange(-12, 12);
            if (choice == 0)
            {
                meteor = Instantiate(smallMeteor, spawnPoints[3].transform.position + new Vector3(coordChange, 0, 0), spawnPoints[3].transform.rotation);
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
                meteor = Instantiate(largeMeteor, spawnPoints[3].transform.position + new Vector3(coordChange, 0, 0), spawnPoints[3].transform.rotation);
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
            meteor.GetComponent<MeteorController>().rb.velocity = meteor.transform.up * 1.5f;
            timerDown = Random.RandomRange(2, 6);
        }
    }
}
