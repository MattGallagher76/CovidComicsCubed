using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class isVisableTest : MonoBehaviour
{
    public Camera cm;
    public float timer = 5;
    public bool hasDisapeared;

    private Vector3 startingScale;

    private float totalSum;
    private float totalCalls;

    public GameObject head;

    // Start is called before the first frame update
    void Start()
    {
        startingScale = transform.localScale;
    }

    // Update is called once per frame
    void Update()
    {
        if(!hasDisapeared)
        {
            Vector3 temp = cm.WorldToViewportPoint(transform.position, Camera.MonoOrStereoscopicEye.Mono);
            if (temp.x >= 0 && temp.x <= 1 && temp.y >= 0 && temp.y <= 1 && temp.z > 0)
            {
                timer -= Time.deltaTime;
                transform.localScale = startingScale * Mathf.Lerp(1f, 0.8f, (5f - timer) / 5f);
                totalSum += head.transform.position.y;
                totalCalls++;
            }
            else
            {
                totalSum = 0;
                totalCalls = 0;
                timer = 5;
            }
        }
        if(timer <= 0)
        {
            hasDisapeared = true;
            transform.localScale = Vector3.zero;
        }
    }

    public float getHeadHeight()
    {
        if (totalSum == 0 || totalCalls == 0)
            throw new System.Exception("Player has not seen comic");
        return totalSum / totalCalls;
    }
}
