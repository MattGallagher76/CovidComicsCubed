using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class dataManager : MonoBehaviour
{
    public Dictionary<string, DataSetSelector> dict = new Dictionary<string, DataSetSelector>();

    public TextAsset data;

    public GameObject dssPrefab;
    // Start is called before the first frame update
    void Start()
    {
        ProcessCSV(data.text);
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
            if(fields.Length != 3)
            {
                Debug.LogError("Fields not correct length: " + fields.Length);
            }    
            if (!dict.ContainsKey(fields[0]))
            {
                dict.TryGetValue(fields[0], out DataSetSelector dss);
                dss.addValue(float.Parse(fields[2]), float.Parse(fields[1]));
            }
            else
            {
                //Eventually this will need to be replace with finding the object rather than making it
                GameObject go = Instantiate(dssPrefab);
                DataSetSelector dss = go.GetComponent<DataSetSelector>();
                dss.countryName = fields[0];
                dict.Add(fields[0], dss);
                go.name = fields[0];
                Debug.Log(fields[0]);
                dss.addValue(float.Parse(fields[2]), float.Parse(fields[1]));
            }
        }
    }
}
