using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class followPlayerSmooth : MonoBehaviour
{
    public Transform target;           // Target to follow (player). Set this in the inspector.
    public float smoothSpeed; // Adjust the smoothing speed. Increase for faster following.
    public float maxSpeed;
    public float speedModifier;
    public float min;

    public float rotSmoothSpeed; // Adjust the smoothing speed. Increase for faster following.
    public float rotMaxSpeed;
    public float rotSpeedModifier;
    public float rotMin;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 rotDist = new Vector3(
            Mathf.DeltaAngle(transform.eulerAngles.x, target.eulerAngles.x),
            Mathf.DeltaAngle(transform.eulerAngles.y, target.eulerAngles.y),
            Mathf.DeltaAngle(transform.eulerAngles.z, target.eulerAngles.z)
        );

        if (rotDist.magnitude > rotMaxSpeed)
        {
            rotDist = rotDist.normalized * rotMaxSpeed;
        }

        if (rotDist.magnitude < rotMin)
        {
            transform.eulerAngles = target.eulerAngles;
        }
        else
        {
            transform.eulerAngles += rotDist * rotSpeedModifier;
        }
        Vector3 dist = target.position - transform.position;
        if(dist.magnitude > maxSpeed)
        {
            dist = dist.normalized * maxSpeed;
        }
        if (dist.magnitude < min)
        {
            transform.position = target.position;
        }
        else
        {
            transform.position += dist * speedModifier;
        }
    }
}
