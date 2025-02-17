using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_ManagerController : MonoBehaviour
{
    public Animator playButton, pauseButton;
    public GameObject pauseMenu,mainMenu,gameMenu;
    public PlayerController playerController;
    // Start is called before the first frame update
    void Start()
    {
        mainMenu.SetActive(true);
        gameMenu.SetActive(false);
        pauseMenu.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {

    }
    public void PlayGame()
    {
        StartCoroutine(Play());
    }
    public void PauseGame()
    {
        StartCoroutine(Pause());
    }
    public void BackToGame()
    {
        StartCoroutine(BackToPlay());
    }
    IEnumerator Play()
    {
        playButton.Play("Outro_Play_Button");
        yield return new WaitForSeconds(1);
        mainMenu.SetActive(false);
        gameMenu.SetActive(true);
        playerController.playing = true;
    }
    IEnumerator Pause()
    {
        pauseMenu.SetActive(true);
        gameMenu.SetActive(false);
        pauseButton.Play("Idle_Pause_Button");
        yield return new WaitForSeconds(1);
    }
    IEnumerator BackToPlay()
    {
        pauseButton.Play("Outro_Pause_Button");
        yield return new WaitForSeconds(1);
        gameMenu.SetActive(true);
        pauseMenu.SetActive(false);
        playerController.pausing = false;
    }
}
