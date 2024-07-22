using Oculus.Interaction;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class DataSetSelector : MonoBehaviour
{
    public string countryName;
    public List<graphValue> testDataSet;
    public GameObject graphPoint;
    public string handTag;

    public Vector3 offset;
    public bool graph = false;
    public float xScale;
    public float yScale;

    public dataManager dm;

    List<GameObject> graphPoints = new List<GameObject>();

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
        foreach(graphValue gv in testDataSet)
        {
            GameObject t = Instantiate(graphPoint);
            graphPoints.Add(t);
            t.transform.parent = this.transform.parent;
            t.transform.localPosition = new Vector3(gv.getTimeSinceStart() * xScale, gv.getCases() * yScale, 0) + offset;
        }
    }

    public void clearGraph()
    {
        foreach(GameObject gb in graphPoints)
        {
            Destroy(gb);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
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
