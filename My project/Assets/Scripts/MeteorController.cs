using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;

public class MeteorController : MonoBehaviour
{
    public Rigidbody2D rb;
    public bool isLarge;
    public Animator animator;
    float timer;
    GameManagerScript gameManagerScript;
    PlayerController playerController;
    // Start is called before the first frame update
    void Start()
    {
        timer = 30;
        gameManagerScript = GameObject.FindGameObjectWithTag("Manager").GetComponent<GameManagerScript>();
        playerController = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
    }

    // Update is called once per frame
    void Update()
    {
        timer-=Time.deltaTime;
        if(timer<0)
        {
            Destroy(this.gameObject);
        }
        if (playerController.pausing)
        {
            rb.velocity = Vector3.zero;
            animator.speed = 0;
        }
        else
        {
            rb.velocity = transform.up * 1.5f;
            animator.speed = 1;
        }
    }
    public void HitMeteor(Transform currentTransform)
    {
        if (isLarge)
        {
            GameObject smallMeteor = Instantiate(gameManagerScript.smallMeteor, currentTransform.position, currentTransform.rotation);
            Vector3 newDirection = Quaternion.Euler(0, 0, Random.RandomRange(-15,15)) * smallMeteor.transform.up;
            smallMeteor.GetComponent<MeteorController>().rb.velocity = newDirection * 1.5f;
        }
        else
        {
            int choice = Random.RandomRange(0, 5);
            if (choice == 0)
            {
                GameObject aid = Instantiate(gameManagerScript.bulletAid, currentTransform.position, currentTransform.rotation);
                Vector3 newDirection = Quaternion.Euler(0, 0, Random.RandomRange(-15, 15)) * aid.transform.up;
                aid.GetComponent<Transform>().eulerAngles = newDirection;
            }
            if(choice == 1)
            {
                GameObject aid = Instantiate(gameManagerScript.fuelAid, currentTransform.position, currentTransform.rotation);
                Vector3 newDirection = Quaternion.Euler(0, 0, Random.RandomRange(-15, 15)) * aid.transform.up;
                aid.GetComponent<Transform>().eulerAngles = newDirection;
            }
            gameManagerScript.score += 10;
        }
        Destroy(this.gameObject);
    }
}
