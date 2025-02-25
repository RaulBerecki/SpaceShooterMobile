using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManagerScript : MonoBehaviour
{
    public float timerLeft, timerRight,timerUp,timerDown;
    public GameObject largeMeteor, smallMeteor;
    public GameObject[] spawnPoints;
    PlayerController playerController;
    public int score;
    // Start is called before the first frame update
    void Start()
    {
        timerLeft = Random.RandomRange(5, 11);
        timerRight = Random.RandomRange(5, 11);
        timerUp = Random.RandomRange(5, 11);
        timerDown = Random.RandomRange(5, 11);
        playerController = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
        score = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if(!playerController.pausing && playerController.playing)
        {
            GenerateMeteors();
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
            timerLeft = Random.RandomRange(5, 11);
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
            timerRight = Random.RandomRange(5, 11);
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
            timerUp = Random.RandomRange(5, 11);
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
            timerDown = Random.RandomRange(5, 11);
        }
    }
}
