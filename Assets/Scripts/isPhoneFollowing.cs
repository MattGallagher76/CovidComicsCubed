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

    public HandGuide hg;
    // Start is called before the first frame 

    public GameObject sceneParent;

    public float timer = 2f;

    private void Start()
    {
        initialScale = phoneToHide.transform.localScale;
        phoneToHide.transform.localScale = Vector3.zero;
    }

    public void OnFakeEnter()
    {
        StartCoroutine(waitToShowPhone());
        StartCoroutine(waitToShowHelper());
        fpn.setIsInRoom(true);
    }

    IEnumerator waitToShowPhone()
    {
        for(float t = 0; t < timer; t ++)
        {
            yield return null;
        }
        StartCoroutine(scaleUpPhone());
    }

    public void hidePhone()
    {
        StartCoroutine(scaleDownPhone());
        fpn.setIsInRoom(false);
    }

    IEnumerator waitToShowHelper()
    {
        for(float t = 0; t < 3f; t += Time.deltaTime)
        {
            yield return null;
        }
        hg.startSequenceExt();
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
        sceneParent.SetActive(false);
    }
}
