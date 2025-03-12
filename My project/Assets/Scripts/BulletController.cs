using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class BulletController : MonoBehaviour
{
    PlayerController playerController;
    Rigidbody2D rb;
    // Start is called before the first frame update
    void Start()
    {
        playerController = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
        rb = GetComponent<Rigidbody2D>();
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
        }
    }
    private void OnBecameInvisible()
    {
        Destroy(this.gameObject);
    }
}
