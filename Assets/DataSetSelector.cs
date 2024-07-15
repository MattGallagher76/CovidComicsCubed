using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataSetSelector : MonoBehaviour
{
    public List<float> testDataSet;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
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
