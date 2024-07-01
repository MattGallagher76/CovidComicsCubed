using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatisticVisualizer : MonoBehaviour
{
    public GameObject cubePrefab;
    public int width;
    public int depth;

    List<GameObject> visualizedObjects;

    public List<float> testDataSet;

    public bool useRandomData;
    public float minimumRandom;
    public float maximumRandom;

    public TextAsset csvFile;

    // Start is called before the first frame update
    void Start()
    {
        if (csvFile != null)
        {
            //ProcessCSV(csvFile.text);
        }
        else
        {
            Debug.LogError("CSV file not assigned!");
        }

        visualizedObjects = new List<GameObject>();
        for(int x = 0; x < width; x ++)
        {
            for(int y = 0; y < depth; y ++)
            {
                GameObject t = Instantiate(cubePrefab);
                t.transform.parent = this.transform;
                t.transform.localPosition = new Vector3(x * 1.1f, 0, y * 1.1f);
                visualizedObjects.Add(t);
            }
        }

        if(useRandomData)
        {
            float max = minimumRandom - 1f;
            testDataSet.Clear();
            for(int i = 0; i < width * depth; i ++)
            {
                float k = UnityEngine.Random.Range(minimumRandom, maximumRandom);
                testDataSet.Add(k);
                if (k > max)
                    max = k;
            }

            for(int i = 0; i < width * depth; i ++)
            {
                visualizedObjects[i].transform.localScale = new Vector3(1f, testDataSet[i] / max, 1f);
            }
        }
    }

    public Vector2 FindClosestFactors(int number)
    {
        int factor1 = 1;
        int factor2 = number;
        int minDifference = number - 1; // Initializing with the maximum possible difference

        // Iterate over possible factors
        for (int i = 1; i <= Math.Sqrt(number); i++)
        {
            if (number % i == 0)
            {
                int j = number / i;
                int difference = Math.Abs(i - j);

                // Check if the current pair has the smallest difference
                if (difference < minDifference)
                {
                    factor1 = i;
                    factor2 = j;
                    minDifference = difference;
                }
            }
        }

        return new Vector2(factor1, factor2);
    }

    private void ProcessCSV(string csvData)
    {
        string[] lines = csvData.Split('\n');

        Vector2 temp = FindClosestFactors(lines.Length);
        width = (int)temp.x;
        depth = (int)temp.y;

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

    // Update is called once per frame
    void Update()
    {
        
    }
}
