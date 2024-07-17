using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataSetSelector : MonoBehaviour
{
    public string countryName;
    public List<graphValue> testDataSet;

    //For graph, plot all points on a spline with no shown points or lines,
    //Then have an invisable object move accross the spline with a line being generated behind it

    // Start is called before the first frame update
    void Start()
    {
        testDataSet = new List<graphValue>();
    }

    public void addValue(float x, float y)
    {
        testDataSet.Add(new graphValue(x, y));
    }

    public List<graphValue> getList()
    {
        return testDataSet;
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
    }
}
