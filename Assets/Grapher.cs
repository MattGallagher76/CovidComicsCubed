using Meta.WitAi;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class Grapher : MonoBehaviour
{
    public GameObject point;
    public float sizeWidth;
    public float sizeHeight;

    public GameObject line;

    public List<float> testDataSet;

    public bool useRandomData;
    public float randomDataSize = 500;
    public float minimumRandom;
    public float maximumRandom;

    public TextAsset csvFile;

    private int counter = 3;
    private List<GameObject> currentPoints = new List<GameObject>();

    private float timer = 0f;
    public float interval;

    public List<GameObject> dataSets;

    // Start is called before the first frame update
    void Start()
    {
        if (useRandomData)
        {
            testDataSet.Clear();
            for (int i = 0; i < randomDataSize; i ++)
            {
                testDataSet.Add(UnityEngine.Random.Range(minimumRandom, maximumRandom));
            }
        }
        else
        {
            if (csvFile != null)
            {
                ProcessCSV(csvFile.text);
            }
            else
            {
                Debug.LogError("CSV file not assigned!");
            }
        }
        //createLine(true, true, new Vector3(1, 2, 3), new Vector3(6, 7, 8));
        drawPoint(testDataSet[0], 0);
        drawPoint(testDataSet[1], 1);
        timer = interval;
    }

    // Update is called once per frame
    void Update()
    {
        if(timer <= 0 && counter != testDataSet.Count)
        {
            timer = interval;
            foreach(GameObject gb in currentPoints)
            {
                Destroy(gb);
            }
            currentPoints.Clear();
            for (int i = 0; i < counter; i ++)
            {
                drawPoint(testDataSet[i], ((float)i) / ((float)(counter - 1)) * sizeWidth);
                Debug.Log(((float)i) / ((float)(counter - 1)) * sizeWidth);
            }
            counter++;
        }
        else
        {
            timer -= Time.deltaTime;
        }
    }

    //x and y are some values between 0 and 1
    void drawPoint(float y, float x)
    {
        GameObject p = Instantiate(point);
        currentPoints.Add(p);
        p.transform.parent = this.transform;
        p.transform.localScale = Vector3.one * 0.1f;
        p.transform.localPosition = new Vector3(x, y * sizeHeight, 0f);
        if(currentPoints.Count > 1)
        {
            p.GetComponent<LineRenderer>().SetPosition(0, p.transform.position);
            p.GetComponent<LineRenderer>().SetPosition(1, currentPoints[currentPoints.Count - 2].transform.position);
        }
    }

    void createLine(bool createFirstPoint, bool createSecondPoint, Vector3 first, Vector3 second)
    {
        if (createFirstPoint)
        {
            GameObject t = Instantiate(point);
            t.transform.parent = this.transform;
            t.transform.localPosition = first;
            t.transform.localScale = Vector3.one * 0.1f;
        }
        if (createSecondPoint)
        {
            GameObject t = Instantiate(point);
            t.transform.parent = this.transform;
            t.transform.localPosition = second;
            t.transform.localScale = Vector3.one * 0.1f;
        }
        /*
        GameObject l = Instantiate(line);
        l.transform.parent = this.transform;
        l.transform.localPosition = new Vector3((first.x + second.x) / 2f, (first.y + second.y) / 2f, (first.z + second.z) / 2f);
        l.transform.localScale = new Vector3(0.1f, Vector3.Distance(first, second) / 2f, 0.1f);
        l.transform.LookAt(first);
        solvedAngles = l.transform.localEulerAngles;
        this.l = l;
        */
    }

    private void ProcessCSV(string csvData)
    {
        string[] lines = csvData.Split('\n');

        foreach (string line in lines)
        {
            if (string.IsNullOrWhiteSpace(line)) continue; // Skip empty lines

            string[] fields = line.Split(',');
            // Process each field here
            // Example: Print fields to the console
            foreach (string field in fields)
            {
                testDataSet.Add(float.Parse(line));
            }
        }
    }
}
