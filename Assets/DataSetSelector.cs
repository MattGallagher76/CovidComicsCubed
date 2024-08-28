using Oculus.Interaction;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using static DataSetSelector;

public class DataSetSelector : MonoBehaviour
{
    public string countryName;
    public float population;
    public List<graphValue> testDataSet;
    //public GameObject graphLine;
    //LineRenderer lr;
    //GameObject localGraphLine;
    public string handTag;

    public bool graph = false;

    public dataManager dm;
    public static globeSpinTest gst;

    public TextMeshPro nameDisplay;
    public float alphaHandDistance;
    public float solidHandDistance;

    public bool isOnGlobe;

    public AnimationCurve ac;

    GameObject[] hands;

    Material defaultMaterial;

    public GameObject meshBuddy;

    public void initHands()
    {
        hands = GameObject.FindGameObjectsWithTag(handTag);
    }

    //For graph, plot all points on a spline with no shown points or lines,
    //Then have an invisable object move accross the spline with a line being generated behind it

    public void addValue(float y, float x)
    {
        if (dm == null)
            dm = FindObjectOfType<dataManager>();
        if (testDataSet == null)
            testDataSet = new List<graphValue>();
        if (population <= 0)
            Debug.LogError("Population of " + countryName + " is not set");
        testDataSet.Add(new graphValue(x, y / population));
    }

    public void GenerateCompleteGraph()
    {
        if(countryName.Equals("bulgaria"))
            Debug.Log("Before: " + testDataSet.Count);
        List<graphValue> graphValues = testDataSet;
        if (graphValues == null || graphValues.Count == 0)
            return;

        // Sort the list by X values
        graphValues = graphValues.OrderBy(gv => gv.getTimeSinceStart()).ToList();

        List<graphValue> completeGraph = new List<graphValue>();

        for (int i = 0; i < graphValues.Count - 1; i++)
        {
            graphValue current = graphValues[i];
            graphValue next = graphValues[i + 1];

            completeGraph.Add(current);

            float currentTime = current.getTimeSinceStart();
            float nextTime = next.getTimeSinceStart();

            // Fill in the missing points between current and next
            for (float time = currentTime + 1; time < nextTime; time++)
            {
                float interpolatedCases = Interpolate(currentTime, nextTime, current.getCases(), next.getCases(), time);
                graphValue interpolatedValue = new graphValue(time, interpolatedCases);
                completeGraph.Add(interpolatedValue);
            }
        }

        // Add the last element
        completeGraph.Add(graphValues.Last());

        testDataSet = completeGraph;
        if(countryName.Equals("bulgaria"))
            Debug.Log("After: " + testDataSet.Count);
    }

    private static float Interpolate(float x1, float x2, float y1, float y2, float x)
    {
        // Linear interpolation formula
        return y1 + ((y2 - y1) / (x2 - x1)) * (x - x1);
    }

    public List<graphValue> getList()
    {
        return testDataSet;
    }

    public float highestPercentage()
    {
        float highest = 0f;
        foreach(graphValue gv in testDataSet)
        {
            if(gv.getCases() / population > highest)
            {
                highest = gv.getCases() / population;
            }
        }
        return highest;
    }

    public float latestDate()
    {
        float highest = 0f;
        foreach (graphValue gv in testDataSet)
        {
            if (gv.getTimeSinceStart() > highest)
            {
                highest = gv.getTimeSinceStart();
            }
        }
        return highest;
    }

    public void graphData()
    {
        defaultMaterial = meshBuddy.GetComponent<Renderer>().material;
        meshBuddy.GetComponent<Renderer>().material = FindObjectOfType<dataManager>().selectedCountry;
        dm.graph(this);
        /*
        localGraphLine = Instantiate(graphLine);
        lr = localGraphLine.GetComponent<LineRenderer>();
        foreach (graphValue gv in testDataSet)
        {
            GameObject t = Instantiate(graphPoint);
            graphPoints.Add(t);
            t.transform.parent = this.transform.parent;
            t.transform.localPosition = new Vector3(gv.getTimeSinceStart() * xScale, gv.getCases() * yScale, 0) + offset;

            Vector3 t = this.transform.parent.localPosition + new Vector3(0, gv.getCases() * yScale, gv.getTimeSinceStart() * xScale) + offset;
            graphPoints.Add(t);
        }

        //buba

        lr.positionCount = graphPoints.Count;
        int i = 0;
        foreach (Vector3 v in graphPoints)
        {
            Debug.Log("I: " + i + ", V: " + v);
            lr.SetPosition(i, v);
            i++;
        }
        */

        Debug.Log("About to graph " + testDataSet.Count + " points");

        List<float> graphValuesX = new List<float>();
        List<float> graphValuesY = new List<float>();

        foreach(graphValue gv in testDataSet)
        {
            graphValuesX.Add(gv.getTimeSinceStart() / (4 * 365));
            graphValuesY.Add(gv.getCases());
        }

        FindObjectOfType<WindowGraph>().ShowGraph(graphValuesX, graphValuesY, countryName);
    }

    public void clearGraph()
    {
        //Destroy(localGraphLine);
        FindObjectOfType<WindowGraph>().clearGraph();
        GetComponent<Renderer>().material = defaultMaterial;
    }

    public void setName(string n)
    {
        countryName = n;
        //nameDisplay.text = Regex.Replace(n, @"\b[a-z]", m => m.Value.ToUpper());
    }

    public void Update()
    {
        if (graph)
        { 
            graph = false;
            graphData();
        }
        /*
        float dist = Mathf.Min(Vector3.Distance(hands[0].transform.position, transform.position), 
                               Vector3.Distance(hands[1].transform.position, transform.position));
        if (dist < 1f)
        {
            if (countryName.ToLower().Equals("canada"))
                Debug.Log("Dist: " + dist + " - Scale: " + ac.Evaluate(dist / 1f) * -0.0024499f + 0.018f);
            transform.parent.localScale = Vector3.one * (ac.Evaluate(dist / 1f) * -0.0024499f + 0.018f);
        }
        else
        {
            transform.parent.localScale = Vector3.one * 0.0155501f;
        }
        */

        /*
        if (!isOnGlobe)
        {
            float dist = 1000;//Vector3.Distance(handRefernce.position, transform.position);
            if (dist < solidHandDistance)
            {
                nameDisplay.color = Color.black;
            }
            else if (dist < alphaHandDistance)
            {
                Color c = Color.black;
                c = new Color(c.r, c.g, c.b, 1 - (dist - solidHandDistance) / (alphaHandDistance - solidHandDistance));
                //nameDisplay.color = c;
            }
            else
            {
                //nameDisplay.color = new Color(Color.black.r, Color.black.g, Color.black.b, 0f);
            }
        }
        */
    }

    public class graphValue
    {
        float cases;            //Y
        float timeSinceStart;   //X

        public graphValue(float x, float y)
        {
            cases = y;
            timeSinceStart = x;
        }

        public float getCases()
        {
            return cases;
        }

        public float getTimeSinceStart()
        {
            return timeSinceStart;
        }
    }
}
