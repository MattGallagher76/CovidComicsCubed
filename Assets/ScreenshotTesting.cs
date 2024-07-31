using System.Collections;
using System.Collections.Generic;
using System.IO;
using TMPro;
using UnityEngine;
using UnityEngine.Android;

public class ScreenshotTesting : MonoBehaviour
{
    public string screenshotName = "AR_Screenshot";
    private int screenshotCount = 0;

    public Material sc;

    float timer;

    void Start()
    {
        StartCoroutine(CheckAndRequestPermission());
    }

    void Update()
    {
        // Replace this with your condition to trigger the screenshot
        if (timer >= 5f && screenshotCount < 5)
        {
            timer -= 5f;
            CaptureScreenshot();
        }
        timer += Time.deltaTime;
    }

    IEnumerator CheckAndRequestPermission()
    {
        yield return new WaitForSeconds(1f); // Give some time for initialization

        if (Application.platform == RuntimePlatform.Android)
        {
            string permission = Permission.ExternalStorageWrite;
            if (!Permission.HasUserAuthorizedPermission(permission))
            {
                Permission.RequestUserPermission(permission);
                while (!Permission.HasUserAuthorizedPermission(permission))
                {
                    yield return null; // Wait until the permission is granted
                }
            }
        }
    }

    public void CaptureScreenshot()
    {
        string path = Path.Combine(Application.persistentDataPath, screenshotName + screenshotCount.ToString() + ".png");

        Texture2D ttd = ScreenCapture.CaptureScreenshotAsTexture();
        sc.mainTexture = ttd;

        screenshotCount++;
        Debug.Log("Screenshot saved to: " + path);
    }
}
