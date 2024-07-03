using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowPlayerNatural : MonoBehaviour
{
    public Transform playerTarget;
    public Transform centerRoom;

    private bool isInRegion;

    /**
     * True when the object has exceeded maximum difference and is working to track back
     * Once the object has tricked the minimum difference limit, isRotationTracking becomes false
     */
    private bool isRotationTracking;

    private float rotationTimer = 0;

    private bool isXTracking;
    //private bool isYTracking;
    private bool isZTracking;

    private float xTimer = 0;
    //private float yTimer = 0;
    private float zTimer = 0;

    public float timeToScaleUp;

    /**
     * The minimum angle in which the tracked object will snap to exact
     */
    [Tooltip("The minimum angle in which the tracked object will snap to exact")]
    public float minimumRotationDifference;

    /**
     * The angle that will trigger the tracking update process
     */
    [Tooltip("The angle that will trigger the tracking update process")]
    public float maximumRotationDifference;

    /**
     * Constant multiplier to adjust speed
     */
    [Tooltip("Constant multiplier to adjust speed")]
    public float rotationSpeedMultiplier;

    /**
     * The maximum magnitude for rotation transformation
     */
    [Tooltip("The maximum magnitude for rotation transformation")]
    public float maximumRotationMagnitude;


    /**
     * The head height to expect regarless of moving head
     */
    private float targetY;

    /**
     * The minimum distance in which the tracked object will be snapped to true
     */
    [Tooltip("The minimum distance in which the tracked object will be snapped to true")]
    public float minimumXDifference;

    /**
     * The minimum distances that will begin the tracking process
     */
    [Tooltip("The minimum forward distances that will begin the tracking process")]
    public float maximumForwardDifferenceX;
    [Tooltip("The minimum backward distances that will begin the tracking process")]
    public float maximumBackwardDifferenceX;


    /**
     * The minimum distance in which the tracked object will be snapped to true
     */
    [Tooltip("The minimum distance in which the tracked object will be snapped to true")]
    public float minimumZDifference;

    /**
     * The minimum distances that will begin the tracking process
     */
    [Tooltip("The minimum forward distances that will begin the tracking process")]
    public float maximumDifferenceZ;

    /**
     * The maximum magnitude for rotation transformation
     */
    [Tooltip("The maximum magnitude for rotation transformation")]
    public float maximumPositionMagnitude;

    /**
     * Constant multiplier to adjust speed
     */
    [Tooltip("Constant multiplier to adjust speed")]
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

        //Rotation tracking
        if(rotDist.magnitude > maximumRotationDifference)
        {
            isRotationTracking = true;
        }
        if(isRotationTracking)
        {
            rotationTimer += 
                
                
                Time;
            float rotationMagnitude = rotDist.magnitude;

            //Escapes tracking sequence and sets to true target
            if(rotationMagnitude < minimumRotationDifference)
            {
                isRotationTracking = false;
                rotationTimer = 0;
                transform.eulerAngles = new Vector3(0, target.eulerAngles.y, 0);
            }
            else
            {
                rotationMagnitude = (rotationMagnitude > maximumRotationMagnitude)
                    ? maximumRotationMagnitude : rotationMagnitude;

                float scale = 1;
                if (rotationTimer < timeToScaleUp)
                    scale = zCurve.Evaluate(rotationTimer / timeToScaleUp);

                transform.eulerAngles += scale * rotationMagnitude * rotationSpeedMultiplier * rotDist.normalized;
            }
        }
        //Position Tracking
        Vector3 totalPositionChange = Vector3.zero;
        
        //X
        if(locDist.x > maximumForwardDifferenceX || -locDist.x > maximumBackwardDifferenceX)
        {
            isXTracking = true;
        }
        if(isXTracking)
        {
            xTimer += Time.deltaTime;
            totalPositionChange.x = locDist.x;

            //Escapes tracking sequence and sets to true target
            if (Mathf.Abs(totalPositionChange.x) < minimumXDifference)
            {
                isXTracking = false;
                xTimer = 0;
                totalPositionChange.x = 0;
            }
            else
            {
                totalPositionChange.x = (totalPositionChange.x > maximumPositionMagnitude)
                    ? maximumPositionMagnitude : totalPositionChange.x;

                float scale = 1;
                if (xTimer < timeToScaleUp)
                    scale = zCurve.Evaluate(xTimer / timeToScaleUp);
                totalPositionChange.x *= scale;
            }
        }

        //Z
        if (Mathf.Abs(locDist.z) > maximumDifferenceZ)
        {
            isZTracking = true;
        }
        if (isZTracking)
        {
            zTimer += Time.deltaTime;
            totalPositionChange.z = locDist.z;

            //Escapes tracking sequence and sets to true target
            if (Mathf.Abs(totalPositionChange.z) < minimumZDifference)
            {
                isZTracking = false;
                zTimer = 0;
                totalPositionChange.z = 0;
            }
            else
            {
                totalPositionChange.z = (totalPositionChange.z > maximumPositionMagnitude)
                    ? maximumPositionMagnitude : totalPositionChange.z;

                float scale = 1;
                if (zTimer < timeToScaleUp)
                   scale = zCurve.Evaluate(zTimer / timeToScaleUp);
                totalPositionChange.z *= scale;
            }
        }

        transform.position += totalPositionChange * positionSpeedMultiplier;
    }

    public void setIsInRoom(bool t)
    {
        targetY = FindObjectOfType<isVisableTest>().getHeadHeight() - 0.2f;
        isInRegion = t;
        //targetY = (t ? playerTarget : centerRoom).position.y;
        transform.position = new Vector3(transform.position.x, targetY, transform.position.z);
    }
}
