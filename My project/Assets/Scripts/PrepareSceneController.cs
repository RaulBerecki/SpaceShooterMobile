using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PrepareSceneController : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        if (!PlayerPrefs.HasKey("highscore"))
            PlayerPrefs.SetInt("highscore", 0);
        if (!PlayerPrefs.HasKey("started"))
            PlayerPrefs.SetInt("started", 0);
        if (PlayerPrefs.GetInt("started") == 1)
            Application.LoadLevel("SampleScene");
        else
            Application.LoadLevel("DrivingTutorial");       
    }
}
