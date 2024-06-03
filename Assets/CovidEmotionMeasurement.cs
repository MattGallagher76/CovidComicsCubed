using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CovidEmotionMeasurement : MonoBehaviour
{
    enum emotion { HAPPY, BOTHERED, ANGRY };

    public GameObject playerRef;
    public Gradient emotionalSpectrum;
    public int checkInterval;
    public float maximumEmotionValue;

    private float emotionValue = 0;
    private float distance = 2; //6 feet
    public Renderer r;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine("checkEmotion");
    }

    public float getEV()
    {
        return emotionValue;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator checkEmotion()
    {
        int i = 0;
        for (; ; )
        {
            if (i == checkInterval)
            {
                i = 0;
                Vector3 ref1 = new Vector3(playerRef.transform.position.x, 0, playerRef.transform.position.z);
                Vector3 ref2 = new Vector3(transform.position.x, 0, transform.position.z);
                float dist = Mathf.Abs(Vector3.Distance(ref1, ref2));
                if (dist < distance)
                {
                    emotionValue = Mathf.Min(maximumEmotionValue, emotionValue + (distance - dist));
                }
                else
                {
                    emotionValue = Mathf.Max((emotionValue - 0.01f) * 0.975f, 0);
                }
            }
            else
                i++;
            r.material.color = emotionalSpectrum.Evaluate(emotionValue / maximumEmotionValue);
            yield return true;
        }
    }
}
