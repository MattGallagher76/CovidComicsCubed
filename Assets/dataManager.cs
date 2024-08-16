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

    public int xSize;
    public float distScale;
    int count = 0;
    GameObject em;

    public DataSetSelector currentGraph;

    private bool isGraphShowing = false;
    // Start is called before the first frame update
    void Start()
    {
        em = Instantiate(emptyPrefab);
        em.transform.parent = this.transform;
        em.transform.localPosition = new Vector3(-0.5f, -1.5f, 0.75f);

        ProcessPopulation(populationList.text);

        ProcessCSV(data.text);
        em.transform.parent = null;
        
        float highestPercentage = 0f;
        string nameOf = "";
        foreach(KeyValuePair<string, DataSetSelector> kv in dict)
        {
            if(kv.Value != null)
            {
                DataSetSelector dss = kv.Value;
                float h = dss.highestPercentage();
                //Debug.Log(kv.Key + ", " + h);
                if(h > highestPercentage)
                {
                    nameOf = kv.Key;
                    highestPercentage = h;
                }
            }
        }

        float latestDate = 0f;
        string nameOf2 = "";
        foreach (KeyValuePair<string, DataSetSelector> kv in dict)
        {
            if (kv.Value != null)
            {
                DataSetSelector dss = kv.Value;
                float h = dss.latestDate();
                //Debug.Log(kv.Key + ", " + h);
                if (h > latestDate)
                {
                    nameOf2 = kv.Key;
                    latestDate = h;
                }
            }
        }
        //Debug.Log("The latest date is: " + latestDate + " from " + nameOf2);

        cleanUpDSS();
    }

    void cleanUpDSS()
    {
        foreach(KeyValuePair<string, DataSetSelector> kv in dict)
        {
            kv.Value.GenerateCompleteGraph();
        }
    }
    //
    // Update is called once per frame
    void Update()
    {
        
    }

    public void graph(DataSetSelector dss)
    {
        if (!isGraphShowing)
        {
            FindObjectOfType<WindowGraph>().showGraph();
        }
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

    private GameObject FindChildByName(string childName)
    {
        foreach (Transform child in transform)
        {
            Debug.Log(child.gameObject.name);
            transform.Find(childName);
            if (child.gameObject.name == childName)
            {
                return child.gameObject;
            }
        }
        Debug.Log(childName);
        return null; // Return null if no child with the given name is found
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
            if (float.Parse(fields[1]) > 365 * 4)
            {
                //Parse out anything past 2023
                continue;
            }
            if(!badSet.Contains(fields[0].ToLower()))
            {
                if (dict.ContainsKey(fields[0].ToLower()))
                {
                    dict.TryGetValue(fields[0].ToLower(), out DataSetSelector dss);
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
                    if (!populationDict.ContainsKey(fields[0].ToLower()))
                    {
                        Debug.LogError(fields[0] + " does not exist within the population dict");
                        badSet.Add(fields[0].ToLower());
                    }

                    //Eventually this will need to be replace with finding the object rather than making it
                    
                    Transform temp = transform.Find(fields[0]);

                    if (temp == null)
                        Debug.Log(fields[0] + " had an issue");
                    else
                    {
                        GameObject controller = new GameObject("Controler - " + fields[0]);
                        GameObject go = temp.gameObject;
                        GameObject goDss = Instantiate(dssPrefab);
                        controller.transform.SetParent(transform, true);

                        controller.transform.position = transform.position;
                        goDss.transform.SetParent(controller.transform, true);
                        go.transform.SetParent(controller.transform, true);

                        goDss.transform.localPosition = go.transform.localPosition;

                        DataSetSelector dss = goDss.GetComponent<DataSetSelector>();

                        //go.transform.parent = em.transform;
                        //go.transform.localPosition = new Vector3(0.5f, count / xSize * distScale + 1f, -3f + (count % xSize * distScale));
                        //go.transform.localEulerAngles = new Vector3(0f, 180f, 0f);
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
}
