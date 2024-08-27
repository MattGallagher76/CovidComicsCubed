using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class isPhoneFollowing : MonoBehaviour
{
    public GameObject phoneToHide;
    public float coroutineDuration;
    public FollowPlayerNatural fpn;

    private Vector3 initialScale;
    public AnimationCurve scaleCurve;
    // Start is called before the first frame 

    private void Start()
    {
        initialScale = phoneToHide.transform.localScale;
        phoneToHide.transform.localScale = Vector3.zero;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag.Equals("MainCamera"))
        {
            StartCoroutine(scaleUpPhone());
            fpn.setIsInRoom(true);
        }
    }

    public void hidePhone()
    {
        StartCoroutine(scaleDownPhone());
        fpn.setIsInRoom(false);
    }

    IEnumerator scaleUpPhone()
    {
        for(int i = 1; i <= coroutineDuration; i ++)
        {
            phoneToHide.transform.localScale = Vector3.Lerp(Vector3.zero, initialScale, scaleCurve.Evaluate(((float)i) / coroutineDuration));
            yield return null;
        }
    }

    IEnumerator scaleDownPhone()
    {
        for (int i = 1; i <= coroutineDuration; i++)
        {
            phoneToHide.transform.localScale = Vector3.Lerp(initialScale, Vector3.zero, scaleCurve.Evaluate(((float)i) / coroutineDuration));
            yield return null;
        }
    }
}
