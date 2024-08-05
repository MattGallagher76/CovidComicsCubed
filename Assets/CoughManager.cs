using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoughManager : MonoBehaviour
{
    public List<AudioClip> coughClips; // List of cough audio clips
    public AudioSource audioSource; // Audio source to play the coughs

    [Range(0, 1)]
    public float intensity;
    public float threshold;

    public float minPitch = 0.8f; // Minimum pitch value
    public float maxPitch = 1.2f; // Maximum pitch value
    public float minVolume = 0.5f; // Minimum volume value
    public float maxVolume = 1.0f; // Maximum volume value

    public GameObject audioSourcePrefab;

    public void PlayCoughs(float intensity)
    {
        // Ensure intensity is clamped between 0 and 1
        intensity = Mathf.Clamp01(intensity);

        // Determine the number of coughs to play based on intensity
        int numberOfCoughs = Mathf.CeilToInt(intensity * coughClips.Count);

        audioSource.pitch = Random.Range(minPitch, maxPitch);
        audioSource.volume = Random.Range(minVolume, maxVolume) * intensity;

        // Play the determined number of cough clips
        for (int i = 0; i < numberOfCoughs; i++)
        {
            // Select a random cough clip
            int randomIndex = Random.Range(0, coughClips.Count);
            audioSource.PlayOneShot(coughClips[randomIndex]);
        }
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        float r = Random.Range(intensity, 2 * intensity);
        Debug.Log(r);
        if (r > threshold)
        {
            GameObject gb = Instantiate(audioSourcePrefab);
            gb.transform.position = new Vector3(0, 1, 0);

            gb.GetComponent<AudioSource>().pitch = Random.Range(minPitch, maxPitch);
            gb.GetComponent<AudioSource>().volume = Random.Range(minVolume, maxVolume) * intensity;

            gb.GetComponent<AudioSource>().PlayOneShot(coughClips[Random.Range(0, coughClips.Count)]);
        }
    }
}
