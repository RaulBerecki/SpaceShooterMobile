using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using TMPro;
using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;
public class PlayerController : MonoBehaviour
{
    public Rigidbody2D rb;
    public float maxFuel, maxAmmo;
    public float speed,noFuelSpeed,rotationSpeed,noRotationSpeed;
    bool leftClicked,rightClicked;
    UI_ManagerController UI_ManagerController;
    public AudioSource audioSource1,audioSource2;
    public AudioClip shooting,gameOverSound,collectSound,shootingBouncing;
    Animator animator;
    CircleCollider2D circleCollider;
    public CameraController cameraController;
    //Shooting Variables
    public Transform shootingPoint;
    public GameObject bullet;
    public float timerToSet;
    float timer;
    public int bulletsAvailable;
    public bool playing,pausing,offGame,gameOver;
    //TrailVariables
    public TrailRenderer trailRenderer;
    public float trailTimer;
    public int trailAvailable;
    //Superpowers
    public bool aidMagnet,shootingUnlimited,fuelUnlimited,shield,bouncingBullets;
    public GameObject bouncingBullet;
    public float shootingUnlimitedTimer,doubleSpeed,bouncingBulletTimerToSet;
    public float timerPower,aidMagnetSoundTimer;
    public AudioSource allDestroySound, aidMagnetSound;
    // Start is called before the first frame update
    void Start()
    {
        rb=GetComponent<Rigidbody2D>();
        trailRenderer = GetComponent<TrailRenderer>();
        animator = GetComponent<Animator>();
        circleCollider = GetComponent<CircleCollider2D>();
        UI_ManagerController = GameObject.FindGameObjectWithTag("UIManager").GetComponent<UI_ManagerController>();
        cameraController = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<CameraController>();
        timer = 0;
        trailTimer = .5f;
        trailAvailable = bulletsAvailable = 100;
        playing = pausing = offGame = gameOver = false;
        noFuelSpeed = speed / 3;
        noRotationSpeed = rotationSpeed / 2;
        trailRenderer.startWidth = .15f;
        trailRenderer.endWidth = .15f;
        aidMagnet = shootingUnlimited = fuelUnlimited= shield=bouncingBullets= false;
        shootingUnlimitedTimer = timerToSet / 2f;
        doubleSpeed = speed * 1.5f;
        aidMagnetSoundTimer = 2f;
    }

    // Update is called once per frame
    void Update()
    {
        if(playing && !pausing)
        {
            Animations();
            if (fuelUnlimited)
            {
                trailRenderer.startColor = new Color32(0, 206, 209, 255);
                trailRenderer.endColor = new Color32(0, 206, 209, 0);
            }
            else
            {
                trailRenderer.startColor = new Color32(77,42,146, 255);
                trailRenderer.endColor = new Color32(77, 42, 146, 0);   
            }
            if(shield)
                circleCollider.radius = .4f;
            else
                circleCollider.radius = .01f;
            timer -= Time.deltaTime;
            trailTimer -= Time.deltaTime;
            timerPower-= Time.deltaTime;
            if (timerPower <= 0)
            {
                aidMagnet=false;
                shootingUnlimited=false;
                fuelUnlimited=false;
                shield = false;
            }
            if (trailTimer < 0 && trailAvailable >0)
            {
                if(!fuelUnlimited)
                    trailAvailable--;
                trailTimer = 0.5f;
            }
            if(trailAvailable>0)
            {
                trailRenderer.time = .35f;
                if (fuelUnlimited)
                    rb.linearVelocity = transform.up * doubleSpeed;
                else
                    rb.linearVelocity = transform.up * speed;
            }
            else
            {
                trailRenderer.time = .2f;
                rb.linearVelocity = transform.up * 1f;
            }
            if (timer < 0 && bulletsAvailable > 0)
            {
                GameObject bulletInstance;
                if (bouncingBullets)
                {
                    bulletInstance = Instantiate(bouncingBullet, shootingPoint.position, shootingPoint.rotation);
                    audioSource1.clip = shootingBouncing;
                    audioSource1.pitch = Random.Range(.85f, 1.15f);
                    audioSource1.Play();
                }
                else
                {
                    bulletInstance = Instantiate(bullet, shootingPoint.position, shootingPoint.rotation);
                    audioSource1.clip = shooting;
                    audioSource1.pitch = Random.Range(.85f, 1.15f);
                    audioSource1.Play();
                }
                bulletInstance.GetComponent<Rigidbody2D>().linearVelocity = shootingPoint.up * 4;
                if(!shootingUnlimited && !bouncingBullets)
                {
                    bulletsAvailable--;
                    timer = timerToSet;
                }
                else
                {
                    if (shootingUnlimited)
                        timer = shootingUnlimitedTimer;
                    else
                        timer = bouncingBulletTimerToSet;
                }
            }
            if (leftClicked)
            {
                transform.Rotate(new Vector3(0, 0, 1) * rotationSpeed);
            }
            if (rightClicked)
            {
                transform.Rotate(-new Vector3(0, 0, 1) * rotationSpeed);
            }
            if (aidMagnet)
            {
                aidMagnetSoundTimer -= Time.deltaTime;
                if (aidMagnetSoundTimer <= 0)
                {
                    aidMagnetSound.pitch = Random.Range(0.85f, 1.15f);
                    aidMagnetSound.Play();
                    aidMagnetSoundTimer = 2f;
                }
            }
            else
            {
                aidMagnetSound.Stop();
                aidMagnetSoundTimer = 2f;
            }
        }
        if (rightClicked && leftClicked && !pausing)
        {
            rb.linearVelocity = new Vector2(0, 0);
            pausing = true;
            UI_ManagerController.PauseGame();
            LeftButtonNotClicked();
            RightButtonNotClicked();
        }
    }
    void Animations()
    {
        if (aidMagnet && !shield)
            animator.Play("MagnetEffect");
        else if (!aidMagnet && shield)
            animator.Play("ShieldOn");
        else
            animator.Play("Idle");
    }
    public void LeftButtonClicked() { 
        leftClicked = true;
    }
    public void RightButtonClicked()
    {
        rightClicked = true;
    }
    public void LeftButtonNotClicked()
    {
        leftClicked = false;
    }
    public void RightButtonNotClicked()
    {
        rightClicked = false;
    }
    public void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("meteor") && playing)
        {
            Debug.Log("hit");
            if(shield)
            {
                rb.linearVelocity = Vector2.zero;
                rb.angularVelocity = 0f;
                collision.gameObject.GetComponent<MeteorCollisionCatcher>().MeteorController.HitMeteor(collision.gameObject.transform);
                shield = false;
            }
            else
            {
                playing = false;
                audioSource1.clip = gameOverSound;
                audioSource1.Play();
                gameOver = true;
                UI_ManagerController.GameOver();
            }
        }
        
    }
    public void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("aidBullet"))
        {
            bulletsAvailable += 10;
            audioSource2.pitch = Random.Range(.85f, 1.15f);
            audioSource2.clip = collectSound;
            audioSource2.Play();
            Destroy(other.gameObject);
        }
        if (other.gameObject.CompareTag("fuelAid"))
        {
            audioSource2.pitch = Random.Range(.85f, 1.15f);
            trailAvailable += 10;
            audioSource2.clip = collectSound;
            audioSource2.Play();
            Destroy(other.gameObject);
        }
        if (other.gameObject.CompareTag("AidMagnet"))
        {
            aidMagnetSound.Play();
            timerPower = 30;
            aidMagnet = true;
            shootingUnlimited = false;
            fuelUnlimited = false;
            shield = false;
            bouncingBullets = false;
            Destroy(other.gameObject);
        }
        if (other.gameObject.CompareTag("ShootingPowerUnlimited"))
        {
            timerPower = 30;
            aidMagnet = false;
            shootingUnlimited = true;
            fuelUnlimited = false;
            shield = false;
            bouncingBullets = false;
            Destroy(other.gameObject);
        }
        if (other.gameObject.CompareTag("FuelPowerUnlimited"))
        {
            timerPower = 30;
            aidMagnet = false;
            shootingUnlimited = false;
            fuelUnlimited = true;
            shield = false;
            bouncingBullets = false;
            Destroy(other.gameObject);
        }
        if (other.gameObject.CompareTag("ShieldPower"))
        {
            timerPower = 30;
            aidMagnet = false;
            shootingUnlimited = false;
            fuelUnlimited = false;
            shield = true;
            bouncingBullets = false;
            Destroy(other.gameObject);
        }
        if (other.gameObject.CompareTag("DestroyAllPowerUp"))
        {
            StartCoroutine(cameraController.Shake(.15f, .2f));
            allDestroySound.pitch = Random.Range(0.85f, 1.15f);
            allDestroySound.Play();
            GameObject[] meteors = GameObject.FindGameObjectsWithTag("meteorManager");
            foreach (GameObject meteor in meteors)
            {
                meteor.GetComponent<MeteorController>().isLarge = false;
                meteor.GetComponent<MeteorController>().HitMeteor(meteor.transform.GetChild(0).transform);
            }
            Destroy(other.gameObject);
        }
        if (other.gameObject.CompareTag("BouncingBulletPowerUp"))
        {
            timerPower = 30;
            aidMagnet = false;
            shootingUnlimited = false;
            fuelUnlimited = false;
            shield = false;
            bouncingBullets = true;
            Destroy(other.gameObject);
        }
    }
    private void OnBecameInvisible()
    {
        if (!offGame)
        {
            transform.position = new Vector3(-transform.position.x, -transform.position.y, transform.position.z);
            trailRenderer.Clear();
            offGame = true;
        }
    }
    private void OnBecameVisible()
    {
        offGame = false;
    }
}
