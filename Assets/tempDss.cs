using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class tempDss : MonoBehaviour
{
    public float timer = 2f;
    public string countryName;

    bool done = false;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (timer > 0)
        {
            timer -= Time.deltaTime;
        }
        else
        {
            if (!done)
            {
                //Debug.Log("Data mirrored");
                DataSetSelector dss = GetComponent<DataSetSelector>();
                dataManager dm = FindObjectOfType<dataManager>();
                dm.dict.TryGetValue(countryName.ToLower(), out DataSetSelector dss2);
                dss.testDataSet = dss2.testDataSet;
                done = true;
            }
        }
    }
}
