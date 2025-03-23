using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.Drawing;
public class PlayerController : MonoBehaviour
{
    Rigidbody2D rb;
    public float speed,noFuelSpeed,rotationSpeed,noRotationSpeed;
    bool leftClicked,rightClicked;
    UI_ManagerController UI_ManagerController;
    public AudioSource audioSource1,audioSource2;
    public AudioClip shooting,gameOverSound,collectSound;
    //Shooting Variables
    public Transform shootingPoint;
    public GameObject bullet;
    public float timerToSet;
    float timer;
    int bulletsAvailable;
    public bool playing,pausing,offGame,gameOver;
    public TextMeshProUGUI bulletsText;
    //TrailVariables
    TrailRenderer trailRenderer;
    public float trailTimer;
    public int trailAvailable;
    public TextMeshProUGUI trailText;
    // Start is called before the first frame update
    void Start()
    {
        rb=GetComponent<Rigidbody2D>();
        trailRenderer = GetComponent<TrailRenderer>();
        UI_ManagerController = GameObject.FindGameObjectWithTag("UIManager").GetComponent<UI_ManagerController>();
        timer = 0;
        trailTimer = .5f;
        trailAvailable = bulletsAvailable = 100;
        playing = pausing = offGame = gameOver = false;
        noFuelSpeed = speed / 3;
        noRotationSpeed = rotationSpeed / 2;
        trailRenderer.startWidth = .15f;
        trailRenderer.endWidth = .15f;
    }

    // Update is called once per frame
    void Update()
    {
        if(playing && !pausing)
        {
            timer -= Time.deltaTime;
            trailTimer -= Time.deltaTime;
            if (trailTimer < 0 && trailAvailable >0)
            {
                trailAvailable--;
                trailTimer = 0.5f;
            }
            if(trailAvailable>0)
            {
                trailRenderer.time = .35f;
                rb.velocity = transform.up * speed;
            }
            else
            {
                trailRenderer.time = .2f;
                rb.velocity = transform.up * 1f;
            }
            if (timer < 0 && bulletsAvailable > 0)
            {
                GameObject bulletInstance = Instantiate(bullet, shootingPoint.position, shootingPoint.rotation);
                bulletInstance.GetComponent<Rigidbody2D>().velocity = shootingPoint.up * 4;
                bulletsAvailable--;
                timer = timerToSet;
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
            rb.velocity = new Vector2(0, 0);
            pausing = true;
            UI_ManagerController.PauseGame();
            LeftButtonNotClicked();
            RightButtonNotClicked();
        }
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
            playing = false;
            audioSource1.clip = gameOverSound;
            audioSource1.Play();
            gameOver = true;
            UI_ManagerController.GameOver();
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
