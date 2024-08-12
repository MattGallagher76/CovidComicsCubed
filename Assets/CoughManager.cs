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

    private int currentCoughCount = 0;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void FixedUpdate()
    {
        float r = Random.Range(0, 1f);
        if (r > ac.Evaluate(intensity) && intensity != 0 && currentCoughCount <= 3)
        {
            GameObject gb = Instantiate(audioSourcePrefab);
            gb.transform.position = new Vector3(Random.Range(-3f, 3f), Random.Range(1f, 2f), Random.Range(-3f, 3f));

            AudioSource audioSource = gb.GetComponent<AudioSource>();
            audioSource.pitch = Random.Range(minPitch, maxPitch);
            audioSource.volume = Random.Range(minVolume, maxVolume) * intensity;

            AudioClip aud = coughClips[Random.Range(0, coughClips.Count)];
            StartCoroutine(PlayCoughAndNotify(audioSource, aud));

            Destroy(gb, aud.length + 0.1f); // A small delay to ensure the object is destroyed after the clip is done playing
        }
    }

    private IEnumerator PlayCoughAndNotify(AudioSource audioSource, AudioClip clip)
    {
        audioSource.PlayOneShot(clip);
        currentCoughCount++;
        yield return new WaitForSeconds(clip.length); // Wait for the clip to finish playing
        currentCoughCount--;
    }

    public void setIntensity(float val)
    {
        if (val > 1 || val < 0)
            Debug.LogError("Intensity out of bounds");
        intensity = val / maxIntensity;
    }
}
