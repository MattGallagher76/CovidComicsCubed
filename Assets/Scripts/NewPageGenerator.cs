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
    public float thetaStepGood;
    public float thetaStepBad;
    public float phiMin;
    public float phiMax;
    public float phiStepGood;
    public float phiStepBad;

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
    private int createGoodPointsDepth = 0;
    private int createBadPointsDepth = 0;

    private int goodComicIndex = 0;
    private int badComicIndex = 0;

    public AnimationCurve thetaSmoothing;

    // Start is called before the first frame update
    void Start()
    {
        previousPos = transform.position;
        goodComics = Resources.LoadAll<Texture2D>("GoodComics");
        badComics = Resources.LoadAll<Texture2D>("BadComics");
        //Debug.Log("Good: " + goodComics.Length + ", Bad: " + badComics.Length);

        createGoodPoints(1);
        createBadPoints(1);

        //float r = UnityEngine.Random.Range(rMinNoise, rMaxNoise);
        //float phi = UnityEngine.Random.Range(phiMin, phiMax);
        //float th = UnityEngine.Random.Range(-theta / 2, theta / 2);
    }

    public void createBadPoints(float radiusScaleFactor)
    {
        int i = 0;
        for (float phi = phiMin; phi < phiMax; phi += phiStepBad)
        {
            for (float th = -theta / 2; th < theta / 2; th += (thetaStepBad * thetaSmoothing.Evaluate((th + (theta / 2)) / theta)))
            {
                float r = UnityEngine.Random.Range(rMinNoise, rMaxNoise) * radiusScaleFactor;
                Vector3 t = new Vector3(1.5f * r * Mathf.Sin(phi * Mathf.Deg2Rad) * Mathf.Cos((th + 180) * Mathf.Deg2Rad),
                    2 * r * Mathf.Cos(phi * Mathf.Deg2Rad),
                    2.5f * r * Mathf.Sin(phi * Mathf.Deg2Rad) * Mathf.Sin((th + 180) * Mathf.Deg2Rad));
                t *= transform.localScale.x;
                t += transform.position;
                badComicLocations.Insert(i, t);
                i++;
                /*
                GameObject gb = Instantiate(emptyPrefab);
                gb.transform.parent = transform;
                gb.transform.position = t;
                gb.transform.localScale *= 3;
                gb.GetComponent<Renderer>().material.color = Color.red;
                */
            }
        }
        createBadPointsDepth++;
    }

    public void createGoodPoints(float radiusScaleFactor)
    {
        int i = 0;
        for (float phi = phiMin; phi < phiMax; phi += phiStepGood)
        {
            for (float th = -theta / 2; th < theta / 2; th += (thetaStepGood * thetaSmoothing.Evaluate((th + (theta / 2)) / theta)))
            {
                float r = UnityEngine.Random.Range(rMinGood, rMaxGood) * radiusScaleFactor;
                Vector3 t = new Vector3(1.5f * r * Mathf.Sin(phi * Mathf.Deg2Rad) * Mathf.Cos((th + 180) * Mathf.Deg2Rad),
                    2 * r * Mathf.Cos(phi * Mathf.Deg2Rad),
                    2.5f * r * Mathf.Sin(phi * Mathf.Deg2Rad) * Mathf.Sin((th + 180) * Mathf.Deg2Rad));
                t *= transform.localScale.x;
                t += transform.position;
                goodComicLocations.Insert(i, t);
                i++;
                /*
                GameObject gb = Instantiate(emptyPrefab);
                gb.transform.parent = transform;
                gb.transform.position = t;
                gb.transform.localScale *= 3;
                gb.GetComponent<Renderer>().material.color = Color.green;
                */
            }
        }
        createGoodPointsDepth++;
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
        Debug.Log("is this being called?");
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
        if(goodComicLocations.Count == 0)
        {
            createGoodPoints(1f + createGoodPointsDepth * 0.2f);
        }
        if (badComicLocations.Count == 0)
        {
            createBadPoints(1f + createGoodPointsDepth * 0.2f);
        }

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
            if (renderer != null)
            {
                renderer.material.mainTexture = goodComics[goodComicIndex]; // Set the texture randomly
                goodComicIndex = (goodComicIndex + 1 == goodComics.Length) ? 0 : goodComicIndex + 1;
            }
        }
        else
        {
            if (renderer != null)
            {
                renderer.material.mainTexture = badComics[badComicIndex]; // Set the texture 
                badComicIndex = (badComicIndex + 1 == badComics.Length) ? 0 : badComicIndex + 1;
            }
        }
    }
}
