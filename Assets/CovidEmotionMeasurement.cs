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
    public TMPro.TextMeshPro textBox;

    public GameObject shocked;
    public GameObject angry;

    [Range(0, 1f)]
    public float alpha;

    public bool alphaMap;

    // Start is called before the first frame update
    void Start()
    {
        r.material.color = new Color(r.material.color.r, r.material.color.g, r.material.color.b, (alphaMap) ? r.material.color.a : alpha);
        StartCoroutine("checkEmotion");
    }

    public float getEV()
    {
        return emotionValue;
    }

    public void debugSetEv(float newEv)
    {
        if (newEv < 0)
            emotionValue = maximumEmotionValue;
        else
            emotionValue = newEv;
    }

    // Update is called once per frame
    void Update()
    {
        if(emotionValue <= maximumEmotionValue / 3f)
        {
            shocked.SetActive(false);
            angry.SetActive(false);
        }
        else if(emotionValue <= maximumEmotionValue * 2 / 3f)
        {
            shocked.SetActive(true);
            angry.SetActive(false);
        }
        else
        {
            shocked.SetActive(false);
            angry.SetActive(true);
        }
    }

    public Color spectrum()
    {
        return r.material.color;
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
            Color c = emotionalSpectrum.Evaluate(emotionValue / maximumEmotionValue);
            c = new Color(c.r, c.g, c.b, (alphaMap) ? r.material.color.a : alpha);
            r.material.color = c;
            yield return true;
        }
    }
}
