using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AidController : MonoBehaviour
{
    public Rigidbody2D rb;
    float timer;
    PlayerController playerController;
    // Start is called before the first frame update
    void Start()
    {
        timer = 40;
        playerController = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
    }

    // Update is called once per frame
    void Update()
    {
        timer -= Time.deltaTime;
        if (timer < 0)
        {
            Destroy(this.gameObject);
        }
        if (playerController.pausing)
        {
            rb.velocity = Vector3.zero;
        }
        else
        {
            rb.velocity = transform.up * 0.5f;
        }
    }
}
