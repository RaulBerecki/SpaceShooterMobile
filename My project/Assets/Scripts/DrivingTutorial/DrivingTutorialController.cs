using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DrivingTutorialController : MonoBehaviour
{
    public Animator leftButton, rightButton,finishEffect;
    public GameObject finishEffectInstance;
    string[] lines;
    public TextMeshProUGUI textTutorial;
    int stage;
    float timer;
    bool leftClicked, rightClicked,offGame;
    Rigidbody2D rb;
    TrailRenderer trailRenderer;
    // Start is called before the first frame update
    void Start()
    {
        trailRenderer = GetComponent<TrailRenderer>();
        rb=GetComponent<Rigidbody2D>();
        lines = new string[5];
        lines[0] = "Welcome to astroviator boot camp";
        lines[1]= "Hold the left side of the screen to steer left";
        lines[2] = "Hold the right side of the screen to steer right";
        lines[3] = "Need a break? Tap both the left and right sides of the screen at the same time to pause the game.";
        lines[4] = "Mission Complete, Cadet!";
        textTutorial.text = lines[0];
        stage = 0;
        timer = 3;
        trailRenderer.startWidth = .15f;
        trailRenderer.endWidth = .15f;
        trailRenderer.time = .35f;
    }

    // Update is called once per frame
    void Update()
    {
        if(stage == 0) {
            timer-=Time.deltaTime;
            if(timer < 0) {
                stage++;
            }
            rb.linearVelocity = new Vector2(0, 0);
        }
        if(stage == 1)
        {
            textTutorial.text= lines[1];
            leftButton.Play("BlinkingButton");
            if(leftClicked)
            {
                stage++;
                leftButton.Play("IdleButton");
            }
        }
        if (stage == 2)
        {
            if (leftClicked)
            {
                rb.linearVelocity = transform.up * 1.5f;
                transform.Rotate(new Vector3(0, 0, 1) * 2f);
            }
            else
            {
                rb.Sleep();
                trailRenderer.Clear();
                stage++;
            }
        }
        if (stage == 3)
        {
            textTutorial.text = lines[2];
            rightButton.Play("BlinkingButton");
            if (rightClicked)
            {
                rightButton.Play("IdleButton");
                stage++;
            }
        }
        if (stage == 4)
        {
            if (rightClicked)
            {
                rb.linearVelocity = transform.up * 1.5f;
                transform.Rotate(-new Vector3(0, 0, 1) * 2f);
            }
            else
            {
                rb.linearVelocity = new Vector2(0, 0);
                stage++;
            }
        }
        if(stage == 5)
        {
            textTutorial.text= lines[3];
            rb.linearVelocity = transform.up * 1.5f;
            rightButton.Play("BlinkingButton");
            leftButton.Play("BlinkingButton");
            if(leftClicked && rightClicked)
            {
                rightButton.Play("IdleButton");
                leftButton.Play("IdleButton");
                rb.linearVelocity = new Vector2(0, 0);
                stage++;
            }
        }
        if(stage == 6){
            textTutorial.text = lines[4];
            timer = 2f;
            stage++;
        }
        if(stage==7)
        {
            timer -= Time.deltaTime;
            if (timer < 0)
            {
                stage++;
            }
        }
        if (stage == 8)
        {
            StartCoroutine(Finished());
        }
    }
    public void LeftButtonClicked()
    {
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
    private void OnBecameInvisible()
    {
        if (!offGame)
        {
            transform.position = new Vector3(-transform.position.x, -transform.position.y, transform.position.z);
            trailRenderer.Clear();
            offGame = true;
        }
    }
    private void OnBecameVisible()
    {
        offGame = false;
    }
    IEnumerator Finished()
    {
        finishEffectInstance.SetActive(true);
        finishEffect.Play("FinishEffectPlaying");
        yield return new WaitForSeconds(1);
        Application.LoadLevel("SampleScene");
        PlayerPrefs.SetInt("started", 1);
    }
}
