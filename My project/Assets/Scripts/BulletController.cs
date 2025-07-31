using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class BulletController : MonoBehaviour
{
    PlayerController playerController;
    Rigidbody2D rb;
    GameManagerScript gameManagerScript;
    // Start is called before the first frame update
    void Start()
    {
        playerController = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
        gameManagerScript = GameObject.FindGameObjectWithTag("Manager").GetComponent<GameManagerScript>();
        rb = GetComponent<Rigidbody2D>();
        rb.linearVelocity = transform.up * 4;
    }

    // Update is called once per frame
    void Update()
    {
        if (playerController.pausing)
        {
            rb.linearVelocity=new Vector2 (0,0);
        }
        else
        {
            rb.linearVelocity = transform.up * 4;
        }
        if (gameManagerScript.AdCompleted)
        {
            Destroy(this.gameObject);
        }
    }
    private void OnBecameInvisible()
    {
        Destroy(this.gameObject);
    }
}
