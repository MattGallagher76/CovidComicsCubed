using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookAtTest : MonoBehaviour
{
    public Transform HeadObject, TargetObject, HeadForward;
    public float maxAngle;
    bool isLooking;
    Quaternion LastRotation;

    //https://www.youtube.com/watch?v=ZodFnkBjSeY

    void Start()
    {
        
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
        }
    }
}
