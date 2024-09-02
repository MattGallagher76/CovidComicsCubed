using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class handAnimation : MonoBehaviour
{
    // Start is called before the first frame update
    private Animator animator;

    public string animationKeyWord;
    public float timeBetweenAnimations;

    float timer = 0f;
    bool currentState;

    void Start()
    {
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if(timer <= 0)
        {
            Debug.Log("Set to true");
            animator.SetBool(animationKeyWord, true);
            currentState = true;
            timer = timeBetweenAnimations;
        }
        else
        {
            timer -= Time.deltaTime;
            if(currentState)
            {
                Debug.Log("Set to false");
                currentState = false;
                //animator.SetBool(animationKeyWord, false);
            }
        }

        //Debug.Log(timer);
    }
}
