using Oculus.Interaction;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

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
    public Transform handRefernce;
    public float alphaHandDistance;
    public float solidHandDistance;

    public bool isOnGlobe;

    //For graph, plot all points on a spline with no shown points or lines,
    //Then have an invisable object move accross the spline with a line being generated behind it

    public void addValue(float y, float x)
    {
        if (dm == null)
            dm = FindObjectOfType<dataManager>();
        if (testDataSet == null)
            testDataSet = new List<graphValue>();
        testDataSet.Add(new graphValue(x, y));
    }

    public List<graphValue> getList()
    {
        return testDataSet;
    }

    public void graphData()
    {
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

        List<float> graphValuesX = new List<float>();
        List<float> graphValuesY = new List<float>();

        float maxY = -1f;
        float maxX = -1f;
        List<graphValue> removedValues = new List<graphValue>();

        foreach (graphValue gv in testDataSet)
        {
            //Debug.Log("X: " + gv.getTimeSinceStart() + ", Y: " + gv.getCases());
        }

        while (testDataSet.Count != 0) {
            float smallestVal = testDataSet[0].getTimeSinceStart();
            int index = 0;
            for (int y = 0; y < testDataSet.Count; y++)
            {
                if (testDataSet[y].getTimeSinceStart() < smallestVal)
                {
                    index = y;
                    smallestVal = testDataSet[y].getTimeSinceStart();
                }
            }
            if (testDataSet[index].getCases() > maxY)
                maxY = testDataSet[index].getCases();
            if (testDataSet[index].getTimeSinceStart() > maxX)
                maxX = testDataSet[index].getTimeSinceStart();
            removedValues.Add(testDataSet[index]);
            testDataSet.RemoveAt(index);
        }
        //Now we have a sorted list named removedValues
        foreach(graphValue gv in removedValues)
        {
            //Debug.Log("X: " + gv.getTimeSinceStart() / maxX + ", Y: " + gv.getCases() / maxY);
            //Debug.Log("X: " + gv.getTimeSinceStart() + ", Y: " + gv.getCases());
            graphValuesX.Add(gv.getTimeSinceStart() / maxX);
            graphValuesY.Add(gv.getCases() / maxY);
        }

        FindObjectOfType<WindowGraph>().ShowGraph(graphValuesX, graphValuesY);
        GetComponent<Renderer>().material.color = Color.red;
    }

    public void clearGraph()
    {
        //Destroy(localGraphLine);
        FindObjectOfType<WindowGraph>().clearGraph();
        GetComponent<Renderer>().material.color = Color.white;
    }

    public void setName(string n)
    {
        countryName = n;
        nameDisplay.text = Regex.Replace(n, @"\b[a-z]", m => m.Value.ToUpper());
    }

    public void Update()
    {
        if (graph)
        {
            graph = false;
            graphData();
        }

        if (!isOnGlobe)
        {
            float dist = Vector3.Distance(handRefernce.position, transform.position);
            if (dist < solidHandDistance)
            {
                nameDisplay.color = Color.black;
            }
            else if (dist < alphaHandDistance)
            {
                Color c = Color.black;
                c = new Color(c.r, c.g, c.b, 1 - (dist - solidHandDistance) / (alphaHandDistance - solidHandDistance));
                nameDisplay.color = c;
            }
            else
            {
                nameDisplay.color = new Color(Color.black.r, Color.black.g, Color.black.b, 0f);
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Entered dss");
        if (isOnGlobe)
        {
            gst = FindObjectOfType<globeSpinTest>();
            gst.updateDss(this);
        }
        else
        {
            Debug.Log(other.gameObject.name);
            if (other.gameObject.CompareTag(handTag))
            {
                graphData();
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        gst.dssColliderExited();
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
