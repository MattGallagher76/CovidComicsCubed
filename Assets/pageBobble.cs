using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class pageBobble : MonoBehaviour
{
    // Start is called before the first frame update
    public float magnitudeLowerBound;
    public float magnitudeUpperBound;
    public float frequencyLowerBound;
    public float frequencyUpperBound;

    private float magnitude;
    private float currentMag = 0;
    private float frequency;

    private float start = 0;

    private Vector3 referenceLocalLocation;
    public float initialPeriodTimer;

    private void Start()
    {
        magnitude = UnityEngine.Random.Range(magnitudeLowerBound, magnitudeUpperBound);
        frequency = UnityEngine.Random.Range(frequencyLowerBound, frequencyUpperBound);
    }

    IEnumerator startBobble()
    {
        Debug.Log("Frequency: " + frequency);
        Debug.Log("Started startBobble");
        referenceLocalLocation = transform.localPosition;
        Debug.Log("-----------------------------------------------------------------");
        for(float timer = 0; timer <= initialPeriodTimer; timer += Time.deltaTime)
        {
            Debug.Log("Time: " + timer + ", out of: " + initialPeriodTimer + ", increment: " + Time.deltaTime);
            currentMag = Mathf.Lerp(0, magnitude, timer / initialPeriodTimer);
            Vector3 temp = referenceLocalLocation + new Vector3(0,
                Mathf.Sin(start * frequency) * currentMag,
                0);
            transform.localPosition = temp;
            start += Time.deltaTime;
            //if (timer + Time.deltaTime >= initialPeriodTimer)
                //StartCoroutine("bobble");
            yield return null;
        }
    }

    IEnumerator bobble()
    {
        Debug.Log("Started bobble");
        for (; ; )
        {
            Vector3 temp = referenceLocalLocation + new Vector3(0,
                Mathf.Sin(start * frequency) * currentMag,
                0);
            Debug.Log("Constant bobble: " + temp + ", Start: " + start + ", sin: " + Mathf.Sin(start * frequency));
            transform.localPosition = temp;
            start += Time.deltaTime;
            yield return null;
        }
    }
}
