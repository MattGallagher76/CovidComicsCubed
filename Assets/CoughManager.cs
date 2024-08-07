using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoughManager : MonoBehaviour
{
    public List<AudioClip> coughClips; // List of cough audio clips
    
    public float intensity;
    public float threshold;

    public float minPitch = 0.9f; // Minimum pitch value
    public float maxPitch = 1.1f; // Maximum pitch value
    public float minVolume = 0.1f; // Minimum volume value
    public float maxVolume = 0.4f; // Maximum volume value

    public GameObject audioSourcePrefab;

    public AnimationCurve ac;
    private float maxIntensity = 0.001479077f;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void FixedUpdate()
    {
        float r = Random.Range(0, 1f);
        //Debug.Log(r);
        if (r > ac.Evaluate(intensity) && intensity != 0)
        {
            Debug.Log("Playing Cough - I:" + intensity + ", R:" + r);
            GameObject gb = Instantiate(audioSourcePrefab);
            gb.transform.position = new Vector3(Random.Range(-3f, 3f), Random.Range(1f, 2f), Random.Range(-3f, 3f));

            gb.GetComponent<AudioSource>().pitch = Random.Range(minPitch, maxPitch);
            gb.GetComponent<AudioSource>().volume = Random.Range(minVolume, maxVolume) * intensity;

            AudioClip aud = coughClips[Random.Range(0, coughClips.Count)];
            gb.GetComponent<AudioSource>().PlayOneShot(aud);
            Destroy(gb, aud.length);
        }
        //else
            //Debug.Log("I: " + intensity + ", R: " + r);
    }

    public void setIntensity(float val)
    {
        if (val > 1 || val < 0)
            Debug.LogError("Intensity out of bounds");
        intensity = val / maxIntensity;
    }
}
