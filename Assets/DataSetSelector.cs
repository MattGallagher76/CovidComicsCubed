using Oculus.Interaction;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class DataSetSelector : MonoBehaviour
{
    public string countryName;
    public List<graphValue> testDataSet;
    public GameObject graphLine;
    LineRenderer lr;
    GameObject localGraphLine;
    public string handTag;

    public Vector3 offset;
    public bool graph = false;
    public float xScale;
    public float yScale;

    public dataManager dm;

    List<Vector3> graphPoints = new List<Vector3>();

    //For graph, plot all points on a spline with no shown points or lines,
    //Then have an invisable object move accross the spline with a line being generated behind it

    public void Update()
    {
        if(graph)
        {
            graph = false;
            graphData();
        }
    }

    public void addValue(float x, float y)
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
        localGraphLine = Instantiate(graphLine);
        lr = localGraphLine.GetComponent<LineRenderer>();
        foreach(graphValue gv in testDataSet)
        {
            /*
            GameObject t = Instantiate(graphPoint);
            graphPoints.Add(t);
            t.transform.parent = this.transform.parent;
            t.transform.localPosition = new Vector3(gv.getTimeSinceStart() * xScale, gv.getCases() * yScale, 0) + offset;
            */

            Vector3 t = this.transform.parent.localPosition + new Vector3(gv.getTimeSinceStart() * xScale, gv.getCases() * yScale, 0) + offset;
            graphPoints.Add(t);
        }

        //buba

        lr.positionCount = graphPoints.Count;
        int i = 0;
        foreach(Vector3 v in graphPoints)
        {
            Debug.Log("I: " + i + ", V: " + v);
            lr.SetPosition(i, v);
            i++;
        }
    }

    public void clearGraph()
    {
        Destroy(localGraphLine);
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log(other.gameObject.name);
        if (other.gameObject.CompareTag(handTag))
        {
            graphData();
        }
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
