using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class AidController : MonoBehaviour
{
    public Rigidbody2D rb;
    float timer;
    PlayerController playerController;
    public Transform image;
    // Start is called before the first frame update
    void Start()
    {
        timer = 40;
        playerController = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        image.transform.eulerAngles = new Vector3(0,0,0);
        timer -= Time.deltaTime;
        if (timer < 0)
        {
            Destroy(this.gameObject);
        }
        if (playerController.pausing)
        {
            rb.linearVelocity = Vector3.zero;
        }
        else
        {
            if(playerController.aidMagnet)
            {
                Vector2 direction = ((Vector2)playerController.transform.position - rb.position).normalized;
                Vector2 newPosition = rb.position + direction * 1f * Time.fixedDeltaTime;

                rb.MovePosition(newPosition);
            }
            else
            {
                rb.linearVelocity = transform.up * 0.5f;
            }
        }
    }
}
