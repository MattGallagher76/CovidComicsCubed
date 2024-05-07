using OVR.OpenVR;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewPageGenerator : MonoBehaviour
{
    public float rMinGood;
    public float rMaxGood;
    public float rMinNoise;
    public float rMaxNoise;
    public float theta; //Range will be from -t/2 to t/2 with 0 being the direction being faced
    public float phiMin;
    public float phiMax;

    public float noiseAlpha;

    public GameObject pagePrefab;
    public GameObject emptyPrefab;
    public Transform focusPoint;

    /// <summary>
    /// 0 is looking at
    /// 1 is no orientation
    /// 2 is partial looking at
    /// </summary>
    public int setOrientation;

    private List<GameObject> badEmp = new List<GameObject>();
    private List<GameObject> goodEmp = new List<GameObject>();
    
    private Texture2D[] badComics;
    private Texture2D[] goodComics;
    public float goodToBadRatio;

    public Transform target;
    private bool firstPage = false;

    // Start is called before the first frame update
    void Start()
    {
        goodComics = Resources.LoadAll<Texture2D>("GoodComics");
        badComics = Resources.LoadAll<Texture2D>("BadComics");
        Debug.Log("Good: " + goodComics.Length + ", Bad: " + badComics.Length);
    }

    public void makePage()
    {
        if (firstPage)
        {
            transform.LookAt(target);
            transform.eulerAngles = new Vector3(0, transform.eulerAngles.y, 0);
            firstPage = true;
        }

        bool isGood = UnityEngine.Random.Range(0f, 1f) > goodToBadRatio;
        float r;
        if(isGood)
            r = UnityEngine.Random.Range(rMinGood, rMaxGood);
        else
            r = UnityEngine.Random.Range(rMinNoise, rMaxNoise);
        float phi = UnityEngine.Random.Range(phiMin, phiMax);
        float th = UnityEngine.Random.Range(-theta / 2, theta / 2);
        GameObject temp = Instantiate(emptyPrefab);
        temp.transform.parent = transform;
        temp.transform.localPosition = new Vector3(r * Mathf.Sin(phi * Mathf.Deg2Rad) * Mathf.Cos(th * Mathf.Deg2Rad),
            r * Mathf.Cos(phi * Mathf.Deg2Rad),
            r * Mathf.Sin(phi * Mathf.Deg2Rad) * Mathf.Sin(th * Mathf.Deg2Rad));
        temp.transform.localScale = Vector3.one;
        GameObject newPage = Instantiate(pagePrefab);
        Vector3 scale = newPage.transform.localScale;
        newPage.transform.parent = temp.transform;
        newPage.transform.localScale = scale;
        newPage.transform.localPosition = Vector3.one;
        if (setOrientation == 0)
            newPage.transform.LookAt(focusPoint);
        if (isGood)
        {
            int index = UnityEngine.Random.Range(0, goodComics.Length); // Get a random index
            Renderer renderer = newPage.GetComponent<Renderer>(); // Access the Renderer component
            if (renderer != null)
            {
                renderer.material.mainTexture = goodComics[index]; // Set the texture randomly
            }
        }
        else
        {
            int index = UnityEngine.Random.Range(0, badComics.Length); // Get a random index
            Renderer renderer = newPage.GetComponent<Renderer>(); // Access the Renderer component
            if (renderer != null)
            {
                renderer.material.mainTexture = badComics[index]; // Set the texture 
            }

            Color c = renderer.material.color;
            c.a = noiseAlpha;
            renderer.material.color = c;
        }
    }
}
