using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookAtTest : MonoBehaviour
{
    private Animator animator;
    public Transform lookAtTarget;  // The object the head and eyes will look at
    public float lookAtWeight = 1.0f;  // Weight for head IK
    public float bodyWeight = 0.0f;  // Weight for body IK (if needed)
    public float headWeight = 1.0f;  // Weight for head IK
    public float eyesWeight = 1.0f;  // Weight for eyes IK
    public float clampWeight = 0.5f;  // Clamping weight to limit the rotation

    void Start()
    {
        animator = GetComponent<Animator>();
        if (animator == null)
        {
            Debug.LogError("Animator component not found on this GameObject.");
        }
    }

    void Update()
    {
        if (animator != null)
        {
            AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0);
            Debug.Log("Current State: " + stateInfo.fullPathHash);
        }
    }

    void OnAnimatorIK(int layerIndex)
    {
        Debug.Log("OnAnimatorIK called.");
        if (animator)
        {
            if (lookAtTarget != null)
            {
                Debug.Log("Applying IK to look at target.");
                animator.SetLookAtWeight(lookAtWeight, bodyWeight, headWeight, eyesWeight, clampWeight);
                animator.SetLookAtPosition(lookAtTarget.position);
            }
            else
            {
                Debug.Log("No lookAtTarget assigned.");
                animator.SetLookAtWeight(0);
            }
        }
    }
}
