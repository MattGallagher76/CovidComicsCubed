using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatisticVisualizer : MonoBehaviour
{
    public GameObject cubePrefab;
    public int width;
    public int depth;

    List<GameObject> visualizedObjects;

    public List<float> testDataSet;

    public bool useRandomData;
    public float minimumRandom;
    public float maximumRandom;

    // Start is called before the first frame update
    void Start()
    {
        visualizedObjects = new List<GameObject>();
        for(int x = 0; x < width; x ++)
        {
            for(int y = 0; y < depth; y ++)
            {
                GameObject t = Instantiate(cubePrefab);
                t.transform.parent = this.transform;
                t.transform.localPosition = new Vector3(x * 1.1f, 0, y * 1.1f);
                visualizedObjects.Add(t);
            }
        }

        if(useRandomData)
        {
            float max = minimumRandom - 1f;
            testDataSet.Clear();
            for(int i = 0; i < width * depth; i ++)
            {
                float k = Random.Range(minimumRandom, maximumRandom);
                testDataSet.Add(k);
                if (k > max)
                    max = k;
            }

            for(int i = 0; i < width * depth; i ++)
            {
                visualizedObjects[i].transform.localScale = new Vector3(1f, testDataSet[i] / max, 1f);
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
