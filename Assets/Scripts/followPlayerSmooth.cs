using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class followPlayerSmooth : MonoBehaviour
{
    public Transform playerTarget;           // Target to follow (player). Set this in the inspector.
    public Transform centerRoom;
    public float maxSpeed;
    public float speedModifier;
    public float min;

    public float rotMaxSpeed;
    public float rotSpeedModifier;
    public float rotMin;

    private bool isInRoom = false;

    // Start is called before the first frame update
    void Start()
    {
        //centerRoom.localPosition = new Vector3(centerRoom.localPosition.x, playerTarget.localPosition.y, centerRoom.localPosition.z);
    }

    // Update is called once per frame
    void Update()
    {
        Transform target;
        if(isInRoom)
        {
            target = playerTarget;
        }
        else
        {
            target = centerRoom;
        }
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

        //Debug.Log("Step");
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

    public void setIsInRoom(bool t)
    {
        isInRoom = t;
    }
}
