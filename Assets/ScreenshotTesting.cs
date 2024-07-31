using System.Collections;
using System.Collections.Generic;
using System.IO;
using TMPro;
using UnityEngine;

public class ScreenshotTesting : MonoBehaviour
{
    public string screenshotName = "AR_Screenshot";
    private int screenshotCount = 0;
    float timer = 0f;

    void Update()
    {
        timer += Time.deltaTime;
        // Replace this with your condition to trigger the screenshot
        if (timer >= 2f)
        {
            Debug.Log("Sbnap");
            timer -= 2f;
            CaptureScreenshot();
        }
    }

    public void CaptureScreenshot()
    {
        string path = Path.Combine(Application.persistentDataPath, screenshotName + screenshotCount.ToString() + ".png");
        ScreenCapture.CaptureScreenshot(path);
        screenshotCount++;
        Debug.Log("Screenshot saved to: " + path);
        GetComponentInChildren<TextMeshPro>().text = "Screenshot saved to: " + path;
    }
}
