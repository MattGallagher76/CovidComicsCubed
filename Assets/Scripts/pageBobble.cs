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
        if (magnitude == 0 || frequency == 0)
            Debug.Log("WE GOT ONE");
    }

    IEnumerator startBobble()
    {
        referenceLocalLocation = transform.localPosition;
        for(float timer = 0; timer <= initialPeriodTimer; timer += Time.deltaTime)
        {
            currentMag = Mathf.Lerp(0, magnitude, timer / initialPeriodTimer);
            Vector3 temp = referenceLocalLocation + new Vector3(0,
                Mathf.Sin(start * frequency) * currentMag,
                0);
            transform.localPosition = temp;
            start += Time.deltaTime;
            yield return null;
        }
        StartCoroutine("bobble");
    }

    IEnumerator bobble()
    {
        for (; ; )
        {
            Vector3 temp = referenceLocalLocation + new Vector3(0,
                Mathf.Sin(start * frequency) * currentMag,
                0);
            transform.localPosition = temp;
            start += Time.deltaTime;
            yield return null;
        }
    }
}
