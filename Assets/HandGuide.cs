using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandGuide : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject hand;
    public string animationTerm;
    Animator animator;

    public GameObject text;

    public float timeForText;

    float timer = 0f;

    public int state = 0;

    Coroutine seq;
    void Start()
    {
        animator = hand.GetComponent<Animator>();
        hand.SetActive(false);
        text.SetActive(false);
    }

    public void endSequence()
    {
        if (state == 1)
        {
            state = 2;
            StopCoroutine(seq);
            StartCoroutine(fadeTextOut());
            hand.SetActive(false);
        }
    }

    public void startSequenceExt()
    {
        if(state == 0)
        {
            state = 1;
            seq = StartCoroutine(startSequence());
            hand.SetActive(true);
            animator.SetBool(animationTerm, true);
        }
    }

    IEnumerator startSequence()
    {
        for(float t = 0; t < timeForText; t += Time.deltaTime)
        {
            yield return true;
        }
        seq = StartCoroutine(fadeTextIn());
    }

    IEnumerator fadeTextIn()
    {
        Color c = text.GetComponent<Renderer>().material.color;
        for (float t = 0; t < 0.75f; t += Time.deltaTime)
        {
            yield return true;
            c.a = t / 0.75f;
            text.GetComponent<Renderer>().material.color = c;
        }
    }

    IEnumerator fadeTextOut()
    {
        Color c = text.GetComponent<Renderer>().material.color;
        for (float t = 0; t < 0.75f; t += Time.deltaTime)
        {
            yield return true;
            c.a = (0.75f - t) / 0.75f;
            text.GetComponent<Renderer>().material.color = c;
        }
    }
}
