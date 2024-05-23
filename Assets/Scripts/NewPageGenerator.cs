using OVR.OpenVR;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewPageGenerator : MonoBehaviour
{
    public float rMinGood;
    public float rMaxGood;
    public float rStepGood;
    public float rMinNoise;
    public float rMaxNoise;
    public float rStepBad;
    public float theta; //Range will be from -t/2 to t/2 with 0 being the direction being faced
    public float thetaStep;
    public float phiMin;
    public float phiMax;
    public float phiStep;

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

    //Both of these will be set up with a R of the minimum, a better R will be chosen upon use
    private List<Vector3> badComicLocations = new List<Vector3>();
    private List<Vector3> goodComicLocations = new List<Vector3>();
    
    private Texture2D[] badComics;
    private Texture2D[] goodComics;
    public float goodToBadRatio;

    private Vector3 previousPos;

    // Start is called before the first frame update
    void Start()
    {
        previousPos = transform.position;
        goodComics = Resources.LoadAll<Texture2D>("GoodComics");
        badComics = Resources.LoadAll<Texture2D>("BadComics");
        //Debug.Log("Good: " + goodComics.Length + ", Bad: " + badComics.Length);

        int i = 0;
        for(float phi = phiMin; phi < phiMax; phi += phiStep)
        {
            for(float th = -theta/2; th < theta/2; th += thetaStep)
            {
                float r = UnityEngine.Random.Range(rMinNoise, rMaxNoise);
                Vector3 t = new Vector3(1.5f * r * Mathf.Sin(phi * Mathf.Deg2Rad) * Mathf.Cos((th+180) * Mathf.Deg2Rad),
                    2 * r * Mathf.Cos(phi * Mathf.Deg2Rad),
                    2.5f * r * Mathf.Sin(phi * Mathf.Deg2Rad) * Mathf.Sin((th + 180) * Mathf.Deg2Rad));
                t *= transform.localScale.x;
                t += transform.position;
                badComicLocations.Insert(i, t);
                
                GameObject gb = Instantiate(emptyPrefab);
                gb.transform.parent = transform;
                gb.transform.position = t;
                
                r = UnityEngine.Random.Range(rMinGood, rMaxGood);
                t = new Vector3(1.5f * r * Mathf.Sin(phi * Mathf.Deg2Rad) * Mathf.Cos((th + 180) * Mathf.Deg2Rad),
                    2 * r * Mathf.Cos(phi * Mathf.Deg2Rad),
                    2.5f * r * Mathf.Sin(phi * Mathf.Deg2Rad) * Mathf.Sin((th + 180) * Mathf.Deg2Rad));
                t *= transform.localScale.x;
                t += transform.position;
                goodComicLocations.Insert(i, t);
                i++;
                
                GameObject gb2 = Instantiate(emptyPrefab);
                gb2.transform.parent = transform;
                gb2.transform.position = t;
            }
        }

        //float r = UnityEngine.Random.Range(rMinNoise, rMaxNoise);
        //float phi = UnityEngine.Random.Range(phiMin, phiMax);
        //float th = UnityEngine.Random.Range(-theta / 2, theta / 2);
    }

    public void Update()
    {
        if (previousPos != null)
        {
            if(transform.position != previousPos)
            {
                //Needs to update final points
                Debug.Log("1");
                updatePageControllers();
            }
        }
        previousPos = transform.position;
    }

    private void updatePageControllers()
    {
        PathCreation.Examples.PageController[] pcs = FindObjectsOfType<PathCreation.Examples.PageController>();
        foreach(PathCreation.Examples.PageController pc in pcs)
        {
            pc.updateLastPoint(transform.localPosition);
        }
    }

    public void makePage()
    {
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
        }
    }

    //t of 0 is an alpha of 1, t of 1 is an alpha of noiseAlpha
    public void PageAlphaLerp(GameObject pg, float t)
    {
        Renderer renderer = pg.GetComponent<Renderer>(); // Access the Renderer component
        Color c = renderer.material.color;
        c.a = Mathf.Lerp(1, noiseAlpha, t);
        renderer.material.color = c;
    }

    public Vector3 generateEndLocation(bool isGood)
    {
        if (isGood)
        {
            Vector3 temp = goodComicLocations[UnityEngine.Random.Range(0, goodComicLocations.Count)];
            goodComicLocations.Remove(temp);
            return temp;
        }
        else
        {
            Vector3 temp = badComicLocations[UnityEngine.Random.Range(0, badComicLocations.Count)];
            badComicLocations.Remove(temp);
            return temp;
        }
    }

    public bool isGood()
    {
        return UnityEngine.Random.Range(0f, 1f) > goodToBadRatio;
    }

    public void setPageRenderer(bool g, Renderer renderer)
    {
        if (g)
        {
            int index = UnityEngine.Random.Range(0, goodComics.Length); // Get a random index
            if (renderer != null)
            {
                //Debug.Log("Good - " + index);
                renderer.material.mainTexture = goodComics[index]; // Set the texture randomly
            }
        }
        else
        {
            int index = UnityEngine.Random.Range(0, badComics.Length); // Get a random index
            if (renderer != null)
            {
                renderer.material.mainTexture = badComics[index]; // Set the texture 
                //Debug.Log("Bad - " + index);
            }
        }
    }
}
