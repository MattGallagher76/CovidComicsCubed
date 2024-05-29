using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CovidEmotionMeasurement : MonoBehaviour
{
    enum emotion { HAPPY, BOTHERED, ANGRY };

    public GameObject playerRef;
    public Color happy;
    public Color angry;
    public int checkInterval;
    public float maximumEmotionValue;

    public GameObject ringPrefab;

    private float emotionValue = 0;
    private float distance = 2; //6 feet

    // Start is called before the first frame update
    void Start()
    {
        //Create ring around gameObject with radius of distance\
        GameObject gb = Instantiate(ringPrefab);
        gb.transform.parent = transform;
        gb.transform.localScale = new Vector3(distance * 2, 0.01f, distance * 2);
        StartCoroutine("checkEmotion");
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator checkEmotion()
    {
        int i = 0;
        for(; ;)
        {
            if (i == checkInterval)
            {
                i = 0;
                Vector3 ref1 = new Vector3(playerRef.transform.position.x, 0, playerRef.transform.position.z);
                Vector3 ref2 = new Vector3(transform.position.x, 0, transform.position.z);
                float dist = Mathf.Abs(Vector3.Distance(ref1, ref2));
                if (dist < distance/2)
                {
                    emotionValue = Mathf.Min((emotionValue + 10) * 1.3f, maximumEmotionValue);
                }
                else if (dist < distance)
                {
                    emotionValue = Mathf.Min(maximumEmotionValue, emotionValue + 10);
                }
                else
                {
                    emotionValue = Mathf.Max(emotionValue - 15, 0);
                }
                Debug.Log("EV: " + emotionValue);
            }
            else
                i++;
            yield return true;
        }
    }
}
