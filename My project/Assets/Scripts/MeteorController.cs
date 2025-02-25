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
    // Start is called before the first frame update
    void Start()
    {
        timer = 30;
        gameManagerScript = GameObject.FindGameObjectWithTag("Manager").GetComponent<GameManagerScript>();
    }

    // Update is called once per frame
    void Update()
    {
        timer-=Time.deltaTime;
        if(timer<0)
        {
            Destroy(this.gameObject);
        }
    }
    public void HitMeteor(Transform currentTransform)
    {
        if (isLarge)
        {
            GameObject smallMeteor = Instantiate(gameManagerScript.smallMeteor, currentTransform.position, currentTransform.rotation);
            Vector3 newDirection = Quaternion.Euler(0, 0, Random.RandomRange(-15,15)) * smallMeteor.transform.up;
            smallMeteor.GetComponent<MeteorController>().rb.velocity = newDirection * 1.5f;
            Destroy(this.gameObject);
        }
        else
        {
            gameManagerScript.score += 10;
            Debug.Log(gameManagerScript.score);
            Destroy(this.gameObject);
        }
    }
}
