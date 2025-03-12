using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class PlayerController : MonoBehaviour
{
    Rigidbody2D rb;
    public float speed,rotationSpeed;
    bool leftClicked,rightClicked;
    UI_ManagerController UI_ManagerController;
    AudioSource audioSource;
    public AudioClip shooting,gameOverSound;
    //Shooting Variables
    public Transform shootingPoint;
    public GameObject bullet;
    public float timerToSet;
    float timer;
    int bulletsAvailable;
    public bool playing,pausing,offGame;
    public TextMeshProUGUI bulletsText;
    // Start is called before the first frame update
    void Start()
    {
        rb=GetComponent<Rigidbody2D>();
        audioSource=GetComponent<AudioSource>();
        UI_ManagerController = GameObject.FindGameObjectWithTag("UIManager").GetComponent<UI_ManagerController>();
        timer = 0;
        bulletsAvailable = 100;
        playing = pausing = offGame = false;
    }

    // Update is called once per frame
    void Update()
    {
        if(playing && !pausing)
        {
            rb.velocity = transform.up * speed;
            timer -= Time.deltaTime;
            if (timer < 0 && bulletsAvailable > 0)
            {
                GameObject bulletInstance = Instantiate(bullet, shootingPoint.position, shootingPoint.rotation);
                bulletInstance.GetComponent<Rigidbody2D>().velocity = shootingPoint.up * 4;
                bulletsAvailable--;
                timer = timerToSet;
                audioSource.clip = shooting;
                audioSource.Play();
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
        if (collision.gameObject.CompareTag("meteor"))
        {
            playing = false;
            audioSource.clip = gameOverSound;
            audioSource.Play();
            UI_ManagerController.GameOver();
        }
        
    }
    public void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("aidBullet"))
        {
            bulletsAvailable += 10;
            Destroy(other.gameObject);
        }
    }
    private void OnBecameInvisible()
    {
        if (!offGame)
        {
            transform.position = new Vector3(-transform.position.x, -transform.position.y, transform.position.z);
            offGame = true;
        }
    }
    private void OnBecameVisible()
    {
        offGame = false;
    }
}
