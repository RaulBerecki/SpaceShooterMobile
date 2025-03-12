using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class VideoQualityManager : MonoBehaviour
{
    //public TextMeshProUGUI fpsText;
    private int frameCount = 0;
    private float elapsedTime = 0f;
    private float updateInterval = 0.5f; // Update the FPS every 0.5 seconds

    void Start()
    {
        Application.targetFrameRate = 60;
        QualitySettings.vSyncCount = 1;
    }
    void Update()
    {
        /*frameCount++;
        elapsedTime += Time.deltaTime;

        if (elapsedTime >= updateInterval)
        {
            // Calculate FPS
            float fps = frameCount / elapsedTime;
            fpsText.text = $"FPS: {fps:F1}"; // Display FPS rounded to 1 decimal place

            // Reset counters
            frameCount = 0;
            elapsedTime = 0f;
        }*/
    }
}
