using UnityEngine;

public class PowerController : MonoBehaviour
{
    public Rigidbody2D rb;
    float timer;
    PlayerController playerController;
    public Transform image;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        timer = 40;
        playerController = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        image.transform.eulerAngles = new Vector3(0, 0, 0);
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
            rb.linearVelocity = transform.up * 0.5f;
        }
    }
}
