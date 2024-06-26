using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookAtTest : MonoBehaviour
{
    public Transform HeadObject, TargetObject, HeadForward;
    public float LookSpeed;
    public float maxAngle;
    bool isLooking;
    Quaternion LastRotation;
    float HeadResetTimer;

    //https://www.youtube.com/watch?v=ZodFnkBjSeY

    void Start()
    {
        isLooking = false;
    }

    void LateUpdate()
    {
        Vector3 dir = (TargetObject.position - HeadObject.position).normalized;
        float angle = Vector3.SignedAngle(dir, HeadForward.forward, HeadForward.up);
        if(Mathf.Abs(angle) < maxAngle)
        {
            if(!isLooking)
            {
                isLooking = true;
                LastRotation = HeadObject.rotation;
            }
            Quaternion TargetRotation = Quaternion.LookRotation(TargetObject.position - HeadObject.position);
            LastRotation = Quaternion.Slerp(LastRotation, TargetRotation, LookSpeed * Time.deltaTime);

            HeadObject.rotation = LastRotation;
            HeadResetTimer = 0.5f;
        }
        else if(isLooking)
        {
            LastRotation = Quaternion.Slerp(LastRotation, HeadForward.rotation, LookSpeed * Time.deltaTime);
            HeadObject.rotation = LastRotation;
            HeadResetTimer -= Time.deltaTime;
            if(HeadResetTimer <= 0)
            {
                HeadObject.rotation = HeadForward.rotation;
                isLooking = false;
            }
        }
    }
}
