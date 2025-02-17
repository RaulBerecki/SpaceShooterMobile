using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    Rigidbody2D rb;
    public float speed,rotationSpeed;
    bool leftClicked,rightClicked;
    UI_ManagerController UI_ManagerController;
    //Shooting Variables
    public Transform shootingPoint;
    public GameObject bullet;
    public float timerToSet;
    float timer;
    int bulletsAvailable;
    public bool playing,pausing;
    // Start is called before the first frame update
    void Start()
    {
        rb=GetComponent<Rigidbody2D>();
        UI_ManagerController = GameObject.FindGameObjectWithTag("UIManager").GetComponent<UI_ManagerController>();
        timer = 0;
        bulletsAvailable = 100;
        playing = pausing = false;
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
            }
            if (leftClicked)
            {
                transform.Rotate(new Vector3(0, 0, 1) * rotationSpeed);
            }
            if (rightClicked)
            {
                transform.Rotate(-new Vector3(0, 0, 1) * rotationSpeed);
            }
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
}
