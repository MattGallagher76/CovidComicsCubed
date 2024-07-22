using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class dataManager : MonoBehaviour
{
    public Dictionary<string, DataSetSelector> dict = new Dictionary<string, DataSetSelector>();

    public TextAsset data;

    public GameObject dssPrefab;
    public GameObject emptyPrefab;

    public int xSize;
    public float distScale;
    int count = 0;
    GameObject em;

    public DataSetSelector currentGraph;
    // Start is called before the first frame update
    void Start()
    {
        //Debug.Log(float.Parse("3.14"));
        //Debug.Log(float.Parse("5"));
        em = Instantiate(emptyPrefab);
        em.transform.parent = this.transform;
        em.transform.localPosition = new Vector3(-0.5f, -1.5f, 0.75f);
        ProcessCSV(data.text);
        em.transform.parent = null;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void graph(DataSetSelector dss)
    {
        if (currentGraph != null)
            currentGraph.clearGraph();
        currentGraph = dss;
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
            if (dict.ContainsKey(fields[0]))
            {
                dict.TryGetValue(fields[0], out DataSetSelector dss);
                try
                {
                    dss.addValue(float.Parse(fields[2]), float.Parse(fields[1]));
                }
                catch (Exception e)
                {
                    Debug.Log(fields[0] + ", " + fields[1] + ", " + fields[2]);
                    throw e;
                }
            }
            else
            {
                //Eventually this will need to be replace with finding the object rather than making it
                GameObject go = Instantiate(dssPrefab);
                DataSetSelector dss = go.GetComponent<DataSetSelector>();
                go.transform.parent = em.transform;
                go.transform.localPosition = new Vector3(0.5f, count / xSize * distScale, count % xSize * distScale);
                dss.countryName = fields[0];
                dict.Add(fields[0], dss);
                go.name = fields[0];
                //Debug.Log(fields[0]);
                dss.addValue(float.Parse(fields[2]), float.Parse(fields[1]));
                count++;
            }
        }
    }
}
