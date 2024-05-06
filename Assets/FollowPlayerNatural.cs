using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowPlayerNatural : MonoBehaviour
{
    public Transform playerTarget;
    public Transform centerRoom;

    private bool isInRegion;

    public float minimumRotationDifference;
    public float maximumRotationDifference;
    public float maximumRotationSpeed;
    public float rotationSpeedMultiplier;
    public float minimumRotationThreshold;

    /**
     * The head height to expect regarless of moving head
     */
    public float targetY;

    public float minimumForwardThresholdX;
    public float minimumBackwardThresholdX;
    public float minimumForwardThresholdZ;
    public float minimumBackwardThresholdZ;

    public float maximumThresholdX;
    public float maximumThresholdZ;

    public float maximumPositionSpeed;
    public float positionSpeedMultiplier;

    //Use coroutie for Z as it will move to target and then stay there
    public AnimationCurve zCurve;
    
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        Transform target = (isInRegion ? playerTarget : centerRoom);
        Vector3 rotDist = new Vector3(
            0, Mathf.DeltaAngle(transform.eulerAngles.y, target.eulerAngles.y), 0);
        Vector3 locDist = target.position - transform.position;

        //Debug.Log(rotDist.magnitude);

        if (rotDist.magnitude > minimumRotationDifference)
        {
            //If the magnitude is greater than the maximum, cap it
            float rotMagnitude = ((rotDist.magnitude > maximumRotationDifference)
                ? maximumRotationSpeed : rotDist.magnitude) * rotationSpeedMultiplier;
            if (rotMagnitude < minimumRotationThreshold)
                transform.eulerAngles = target.eulerAngles;
            else
                transform.eulerAngles += rotMagnitude * rotDist.normalized;
        }
    }

    public void setIsInRoom(bool t)
    {
        isInRegion = t;
        targetY = (t ? playerTarget : centerRoom).position.z;
    }
}
