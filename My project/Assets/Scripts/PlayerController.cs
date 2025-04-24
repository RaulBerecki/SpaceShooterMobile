using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.Drawing;
using UnityEngine.SocialPlatforms.Impl;
public class PlayerController : MonoBehaviour
{
    public Rigidbody2D rb;
    public float speed,noFuelSpeed,rotationSpeed,noRotationSpeed;
    bool leftClicked,rightClicked;
    UI_ManagerController UI_ManagerController;
    public AudioSource audioSource1,audioSource2;
    public AudioClip shooting,gameOverSound,collectSound;
    Animator animator;
    CircleCollider2D circleCollider;
    //Shooting Variables
    public Transform shootingPoint;
    public GameObject bullet;
    public float timerToSet;
    float timer;
    public int bulletsAvailable;
    public bool playing,pausing,offGame,gameOver;
    public TextMeshProUGUI bulletsText;
    //TrailVariables
    public TrailRenderer trailRenderer;
    public float trailTimer;
    public int trailAvailable;
    public TextMeshProUGUI trailText;
    //Superpowers
    public bool aidMagnet,shootingUnlimited,fuelUnlimited,shield;
    public float shootingUnlimitedTimer,doubleSpeed;
    public float timerPower;
    // Start is called before the first frame update
    void Start()
    {
        rb=GetComponent<Rigidbody2D>();
        trailRenderer = GetComponent<TrailRenderer>();
        animator = GetComponent<Animator>();
        circleCollider = GetComponent<CircleCollider2D>();
        UI_ManagerController = GameObject.FindGameObjectWithTag("UIManager").GetComponent<UI_ManagerController>();
        timer = 0;
        trailTimer = .5f;
        trailAvailable = bulletsAvailable = 100;
        playing = pausing = offGame = gameOver = false;
        noFuelSpeed = speed / 3;
        noRotationSpeed = rotationSpeed / 2;
        trailRenderer.startWidth = .15f;
        trailRenderer.endWidth = .15f;
        aidMagnet = shootingUnlimited = false;
        shootingUnlimitedTimer = timerToSet / 2f;
        doubleSpeed = speed * 1.5f;
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
                trailRenderer.startColor = new Color32(120,48,253, 255);
                trailRenderer.endColor = new Color32(120, 48, 253, 0);   
            }
            if(shield)
                circleCollider.radius = .4f;
            else
                circleCollider.radius = .12f;
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
                GameObject bulletInstance = Instantiate(bullet, shootingPoint.position, shootingPoint.rotation);
                bulletInstance.GetComponent<Rigidbody2D>().linearVelocity = shootingPoint.up * 4;
                if(!shootingUnlimited)
                {
                    bulletsAvailable--;
                    timer = timerToSet;
                }
                else
                {
                    timer = shootingUnlimitedTimer;
                }
                audioSource1.clip = shooting;
                audioSource1.Play();
            }
            if (leftClicked)
            {
                transform.Rotate(new Vector3(0, 0, 1) * rotationSpeed);
            }
            if (rightClicked)
            {
                transform.Rotate(-new Vector3(0, 0, 1) * rotationSpeed);
            }
            bulletsText.text=bulletsAvailable.ToString("0");
            trailText.text = trailAvailable.ToString("0");
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
            audioSource2.clip = collectSound;
            audioSource2.Play();
            Destroy(other.gameObject);
        }
        if (other.gameObject.CompareTag("fuelAid"))
        {
            trailAvailable += 10;
            audioSource2.clip = collectSound;
            audioSource2.Play();
            Destroy(other.gameObject);
        }
        if (other.gameObject.CompareTag("AidMagnet"))
        {
            timerPower = 30;
            aidMagnet = true;
            shootingUnlimited = false;
            fuelUnlimited = false;
            shield = false;
            Destroy(other.gameObject);
        }
        if (other.gameObject.CompareTag("ShootingPowerUnlimited"))
        {
            timerPower = 30;
            aidMagnet = false;
            shootingUnlimited = true;
            fuelUnlimited = false;
            shield = false;
            Destroy(other.gameObject);
        }
        if (other.gameObject.CompareTag("FuelPowerUnlimited"))
        {
            timerPower = 30;
            aidMagnet = false;
            shootingUnlimited = false;
            fuelUnlimited = true;
            shield = false;
            Destroy(other.gameObject);
        }
        if (other.gameObject.CompareTag("ShieldPower"))
        {
            timerPower = 30;
            aidMagnet = false;
            shootingUnlimited = false;
            fuelUnlimited = false;
            shield = true;
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
