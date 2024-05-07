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
    private float frequency;

    private float start = 0;

    public bool bobble = true;

    private void Start()
    {
        magnitude = UnityEngine.Random.Range(magnitudeLowerBound, magnitudeUpperBound);
        frequency = UnityEngine.Random.Range(frequencyLowerBound, frequencyUpperBound);
    }

    // Update is called once per frame
    void Update()
    {
        if(bobble)
        {
            start += Time.deltaTime;
            transform.localPosition = new Vector3(transform.localPosition.x,
                Mathf.Sin(start * frequency) * magnitude,
                transform.localPosition.z);
        }
    }

    public void disableBobble()
    {
        bobble = false;
    }
}
