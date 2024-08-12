using Oculus.Platform;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GuidePathManager : MonoBehaviour
{
    public List<GameObject> preNextPaths;

    public List<GameObject> postNextPaths;

    public float durBetweenPath;
    public float durationToShow;
    public float nextDuration;

    public float debugTimer = 0f;
    // Start is called before the first frame update
    void Start()
    {
        hidePathsOnStart();
    }

    void hidePathsOnStart()
    {
        foreach(GameObject gb in preNextPaths)
        {
            Color c = gb.GetComponent<Renderer>().material.color;
            c = new Color(c.r, c.g, c.b, 0);
            gb.GetComponent<Renderer>().material.color = c;
        }
        foreach (GameObject gb in postNextPaths)
        {
            Color c = gb.GetComponent<Renderer>().material.color;
            c = new Color(c.r, c.g, c.b, 0);
            gb.GetComponent<Renderer>().material.color = c;
        }
    }

    public void showPrePaths(List<GameObject> paths)
    {
        showPaths(paths);
        StartCoroutine(waitToShowPostPaths());
    }

    IEnumerator waitToShowPostPaths()
    {
        float timer = 0f;
        while(timer <= nextDuration)
        {
            timer += Time.deltaTime;
            yield return true;
        }
        showPaths(postNextPaths);
    }

    public void showPaths(List<GameObject> paths)
    {
        for (int i = 0; i < paths.Count; i++)
        {
            StartCoroutine(showPath(paths[i], durBetweenPath * i));
        }
    }

    IEnumerator showPath(GameObject gb, float waitDur)
    {
        float timer = 0f;
        while (timer <= waitDur)
        {
            Debug.Log(timer + ", " + waitDur);
            timer += Time.deltaTime;
            yield return true;
        }
        StartCoroutine(showObject(gb));
    }

    IEnumerator showObject(GameObject gb)
    {
        float timer = 0f;
        while(timer <= durationToShow)
        {
            Color c = gb.GetComponent<Renderer>().material.color;
            c = new Color(c.r, c.g, c.b, timer / durationToShow);
            gb.GetComponent<Renderer>().material.color = c;
            timer += Time.deltaTime;
            yield return true;
        }
        Color c2 = gb.GetComponent<Renderer>().material.color;
        c2 = new Color(c2.r, c2.g, c2.b, 1f);
        gb.GetComponent<Renderer>().material.color = c2;
    }

    // Update is called once per frame
    void Update()
    {
        if (debugTimer >= 2f)
        {
            debugTimer = -1f;
            showPrePaths(preNextPaths);
        }
        if (debugTimer != -1f)
            debugTimer += Time.deltaTime;
    }
}
