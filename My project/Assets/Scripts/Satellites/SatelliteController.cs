using UnityEngine;

public class SatelliteController : MonoBehaviour
{
    public Rigidbody2D rb;
    public Animator animator;
    float timer;
    GameManagerScript gameManagerScript;
    PlayerController playerController;
    public GameObject explosionEffect, pointsEffect;
    public bool isVisible;

    [Header("LightAnimation")]
    public SpriteRenderer light;
    public float delay,delayStart;
    bool lightOn;
    // Start is called before the first frame update
    void Start()
    {
        timer = 100;
        gameManagerScript = GameObject.FindGameObjectWithTag("Manager").GetComponent<GameManagerScript>();
        playerController = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
        lightOn = false;
        delay = delayStart;
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
            rb.linearVelocity = Vector3.zero;
            animator.speed = 0;
        }
        else
        {
            rb.linearVelocity = transform.up * .5f;
            animator.speed = 1;
            LightAnimation();
        }
        if (gameManagerScript.AdCompleted)
        {
            Destroy(this.gameObject);
        }
    }
    public void HitSatellite(Transform currentTransform)
    {
        if (isVisible)
        {
            Instantiate(explosionEffect, currentTransform.position, currentTransform.rotation);
            int choice = Random.Range(0, 5);
            if (choice == 0)
            {
                GameObject aid = Instantiate(gameManagerScript.bulletAid, currentTransform.position, currentTransform.rotation);
                Vector3 newDirection = Quaternion.Euler(0, 0, Random.Range(-15, 15)) * aid.transform.up;
                aid.GetComponent<Transform>().eulerAngles = newDirection;
            }
            if (choice == 1)
            {
                GameObject aid = Instantiate(gameManagerScript.fuelAid, currentTransform.position, currentTransform.rotation);
                Vector3 newDirection = Quaternion.Euler(0, 0, Random.Range(-15, 15)) * aid.transform.up;
                aid.GetComponent<Transform>().eulerAngles = newDirection;
            }
            gameManagerScript.score += 10;
            Instantiate(pointsEffect, currentTransform.position, new Quaternion(0, 0, 0, 0));
            Destroy(this.gameObject);
        }
    }
    public void LightAnimation()
    {
        delay -=Time.deltaTime;
        if (delay <= 0)
        {
            Color newColor;
            if (lightOn)
            {
                newColor = light.color;
                newColor.a = 0;
                light.color = newColor;
                lightOn = false;
                delay = delayStart;
            }
            else
            {
                newColor = light.color;
                newColor.a = 1;
                light.color = newColor;
                lightOn = true;
                delay = delayStart;
            }
        }
    }
}
