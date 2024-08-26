using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class isVisableTest : MonoBehaviour
{
    public Camera cm;
    public bool hasDisapeared;

    private Vector3 startingScale;

    private float totalSum;
    private float totalCalls;

    public GameObject head;

    public Animator zoom;

    public bool StartAni;

    public float distanceThreshold;

    // Start is called before the first frame update
    void Start()
    {
        startingScale = transform.localScale;
    }

    // Update is called once per frame
    void Update()
    {
        /*
        if(!hasDisapeared)
        {
            Vector3 temp = cm.WorldToViewportPoint(transform.position, Camera.MonoOrStereoscopicEye.Mono);
            if ((temp.x >= 0 && temp.x <= 1 && temp.y >= 0 && temp.y <= 1 && temp.z > 0) || StartAni)
            {
                timer -= Time.deltaTime;
                zoom.SetBool("zoom", true);
                totalSum += head.transform.position.y;
                totalCalls++;
            }
            else
            {
                totalSum = 0;
                totalCalls = 0;
                timer = 5;
                zoom.SetBool("zoom", false);
            }
        }*/
        if(StartAni)
        {
            StartAni = false;
            zoom.SetBool("zoom", true);
        }
    }

    public bool shouldSequenceStart()
    {
        Vector3 temp = cm.WorldToViewportPoint(transform.position, Camera.MonoOrStereoscopicEye.Mono);
        float dist = Vector3.Distance(head.transform.position, this.transform.position);

//        Debug.Log("Dist: " + dist);

        if (temp.x >= 0 && temp.x <= 1 && temp.y >= 0 && temp.y <= 1 && temp.z > 0 && dist < distanceThreshold)
        {
            totalSum = head.transform.position.y;
            totalCalls = 1;
            return true;
        }


        return false;
    }

    public float getHeadHeight()
    {
        if (totalSum == 0 || totalCalls == 0)
            throw new System.Exception("Player has not seen comic");
        return totalSum / totalCalls;
    }

    public void startSequence()
    {
        zoom.SetBool("zoom", true);
    }
}
