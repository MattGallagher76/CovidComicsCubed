using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewPageGenerator : MonoBehaviour
{
    public int pageCount;
    public float rMin;
    public float rMax;
    public float theta; //Range will be from -t/2 to t/2 with 0 being the direction being faced
    public float phiMin;
    public float phiMax;

    public GameObject pagePrefab;
    public GameObject emptyPrefab;
    public Transform focusPoint;

    public Boolean isRandom;

    /// <summary>
    /// 0 is looking at
    /// 1 is no orientation
    /// 2 is partial looking at
    /// </summary>
    public int setOrientation;

    public float orientationDivider;

    private Boolean isPages = false;

    private List<GameObject> badEmp = new List<GameObject>();
    private List<GameObject> goodEmp = new List<GameObject>();

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            makePages();
        }
        if (Input.GetKeyDown(KeyCode.W))
        {
            spawnPageLocations();
        }
        if (Input.GetKeyDown(KeyCode.E))
        {
            deletePages();
        }
    }

    private void spawnPageLocations()
    {
        if (!isPages)
        {
            if (isRandom)
            {
                for (int i = 0; i < pageCount; i++)
                {
                    float r = UnityEngine.Random.Range(rMin, rMax);
                    float phi = UnityEngine.Random.Range(phiMin, phiMax);
                    float th = UnityEngine.Random.Range(-theta / 2, theta / 2);
                    GameObject temp = Instantiate(emptyPrefab);
                    temp.transform.parent = transform;
                    temp.transform.localPosition = new Vector3(r * Mathf.Sin(phi * Mathf.Deg2Rad) * Mathf.Cos(th * Mathf.Deg2Rad),
                        r * Mathf.Cos(phi/2 * Mathf.Deg2Rad) * 2,
                        r * Mathf.Sin(phi * Mathf.Deg2Rad) * Mathf.Sin(th * Mathf.Deg2Rad));
                    temp.transform.localScale = Vector3.one;
                    badEmp.Add(temp);
                }
            }
            else
            {
                for (float r = rMin; r < rMax; r += 0.2f)
                {
                    for (float phi = phiMin; phi < phiMax; phi += 10)
                    {
                        for (float th = -theta / 2; th < theta / 2; th += 10)
                        {
                            /*
                            Debug.Log("RMin: " + rMin + ", R: " + r + ", RMax: " + rMax);
                            Debug.Log("PhiMin: " + phiMin + ", phi: " + phi + " PhiMax: " + phiMax);
                            Debug.Log("ThetaMin: " + -theta / 2 + ", Theta: " + th + ", ThetaMax: " + theta / 2);
                            */
                            GameObject temp = Instantiate(emptyPrefab);
                            temp.transform.parent = transform;
                            temp.transform.localPosition = new Vector3(r * Mathf.Sin(phi * Mathf.Deg2Rad) * Mathf.Cos(th * Mathf.Deg2Rad),
                                r * Mathf.Cos(phi/2 * Mathf.Deg2Rad) * 2,
                                r * Mathf.Sin(phi * Mathf.Deg2Rad) * Mathf.Sin(th * Mathf.Deg2Rad));
                            temp.transform.localScale = Vector3.one;
                            badEmp.Add(temp);
                        }
                    }
                }
            }
        }
        isPages = true;
    }

    private void makePages()
    {
        if (isPages)
        {
            foreach (GameObject e in badEmp)
            {
                GameObject p = Instantiate(pagePrefab);
                Vector3 scale = p.transform.localScale;
                p.transform.parent = e.transform;
                p.transform.localScale = scale;
                p.transform.localEulerAngles = Vector3.zero;
                p.transform.localPosition = Vector3.one;
                Debug.Log(p.transform.position);
                if (setOrientation == 0)
                    p.transform.LookAt(focusPoint);
                else if (setOrientation == 1)
                {
                    p.transform.LookAt(transform);
                    p.transform.localEulerAngles = new Vector3(0, p.transform.localEulerAngles.y, 0);
                }
            }
        }
        else
            Debug.Log("No empties");
    }

    public void makePage()
    {
        float r = UnityEngine.Random.Range(rMin, rMax);
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
    }

    private void deletePages()
    {
        isPages = false;
        foreach (GameObject e in badEmp)
        {
            Destroy(e);
        }
        badEmp.Clear();
    }
}
