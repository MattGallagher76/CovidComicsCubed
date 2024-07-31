using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class dataManager : MonoBehaviour
{
    public Dictionary<string, DataSetSelector> dict = new Dictionary<string, DataSetSelector>();

    public Dictionary<string, int> populationDict = new Dictionary<string, int>();
    public HashSet<string> badSet = new HashSet<string>();

    public TextAsset data;
    public TextAsset populationList;

    public GameObject dssPrefab;
    public GameObject emptyPrefab;

    public Transform handReference;

    public int xSize;
    public float distScale;
    int count = 0;
    GameObject em;

    public DataSetSelector currentGraph;
    // Start is called before the first frame update
    void Start()
    {
        em = Instantiate(emptyPrefab);
        em.transform.parent = this.transform;
        em.transform.localPosition = new Vector3(-0.5f, -1.5f, 0.75f);

        ProcessPopulation(populationList.text);

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

    private void ProcessPopulation(string csvData)
    {
        string[] lines = csvData.Split('\n');

        foreach (string line in lines)
        {
            if (string.IsNullOrWhiteSpace(line)) continue; // Skip empty lines
            string[] fields = line.Split(',');

            if(fields.Length != 2)
            {
                Debug.LogError("Incorrect format in population length" + line);
            }
            else
            {
                if (populationDict.ContainsKey(fields[0].ToLower()))
                {
                    Debug.LogError(fields[0] + " already exists in dictionary");
                }
                else
                {
                    populationDict.Add(fields[0].ToLower(), int.Parse(fields[1]));
                }
            }
        }
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
            if(!badSet.Contains(fields[0].ToLower()))
            {
                if (dict.ContainsKey(fields[0].ToLower()))
                {
                    dict.TryGetValue(fields[0].ToLower(), out DataSetSelector dss);
                    try
                    {
                        dss.addValue(float.Parse(fields[2]), float.Parse(fields[1]));
                        if (fields[0].ToLower().Equals("canada") && float.Parse(fields[1]) > 700 && float.Parse(fields[1]) < 800)
                            Debug.Log(fields[1] + ", " + fields[2]);
                    }
                    catch (Exception e)
                    {
                        Debug.Log(fields[0] + ", " + fields[1] + ", " + fields[2]);
                        throw e;
                    }
                }
                else
                {
                    if (!populationDict.ContainsKey(fields[0].ToLower()))
                    {
                        Debug.LogError(fields[0] + " does not exist within the population dict");
                        badSet.Add(fields[0].ToLower());
                    }

                    //Eventually this will need to be replace with finding the object rather than making it
                    GameObject go = Instantiate(dssPrefab);
                    DataSetSelector dss = go.GetComponent<DataSetSelector>();
                    dss.handRefernce = handReference;
                    go.transform.parent = em.transform;
                    go.transform.localPosition = new Vector3(0.5f, count / xSize * distScale + 0.5f, count % xSize * distScale);
                    dss.setName(fields[0].ToLower());
                    populationDict.TryGetValue(fields[0].ToLower(), out int pop);
                    dss.population = pop;
                    dict.Add(fields[0].ToLower(), dss);
                    go.name = fields[0].ToLower();
                    dss.addValue(float.Parse(fields[2]), float.Parse(fields[1]));
                    count++;
                }
            }
        }
    }
}
