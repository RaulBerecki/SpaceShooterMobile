using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class BulletController : MonoBehaviour
{
    PlayerController playerController;
    Rigidbody2D rb;
    float timer;
    // Start is called before the first frame update
    void Start()
    {
        playerController = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
        rb = GetComponent<Rigidbody2D>();
        timer = 10;
    }

    // Update is called once per frame
    void Update()
    {
        if (playerController.pausing)
        {
            rb.velocity=new Vector2 (0,0);
        }
        else
        {
            rb.velocity = transform.up * 4;
            timer-=Time.deltaTime;
        }
        if (timer < 0)
            Destroy(this.gameObject);
    }
}
