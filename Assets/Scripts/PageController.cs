using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PathCreation;

namespace PathCreation.Examples
{

    public class PageController : MonoBehaviour
    {
        public GameObject[] pointObjects;
        public GeneratePathExample gpe;
        public PathFollower pf;
        public GameObject page;
        public bool isGood;

        private Vector3 originalFinal;

        public float transitionTime;
        private float timer;
        public float goodComicFinalSize;
        public float noiseComicFinalSize;

        private GameObject focusTarget;
        private Vector3 originalScale;
        private Vector3 originalRotation;
        private Vector3 targetedRotation;

        public IEnumerator startBobbleCoroutine;

        public void Start()
        {
            //startBobbleCoroutine = page.GetComponent<pageBobble>().
        }

        // Update is called once per frame
        void Update()
        {
            if(pf.follow)
            {
                //Over the next TIMER amount of time, bring transparency to 1 if noise, grow, and spin to face target
                if(timer <= transitionTime)
                {
                    if(!isGood)
                    {
                        FindObjectOfType<NewPageGenerator>().PageAlphaLerp(page, timer / transitionTime);
                        page.transform.localScale = originalScale * Mathf.Lerp(1, noiseComicFinalSize, timer / transitionTime);
                    }
                    else
                    {
                        page.transform.localScale = originalScale * Mathf.Lerp(1, goodComicFinalSize, timer / transitionTime);
                    }
                    page.transform.localEulerAngles = (Quaternion.Lerp(Quaternion.Euler(originalRotation), Quaternion.Euler(targetedRotation), timer / transitionTime)).eulerAngles;
                    if (timer != transitionTime)
                        timer = Mathf.Min(timer + Time.deltaTime, transitionTime);
                    if (timer == transitionTime)
                    {
                        Debug.Log("Coroutine initiated");
                        page.GetComponent<pageBobble>().StartCoroutine("startBobble");
                        timer = transitionTime + 1f;
                    }
                }
            }
        }

        public void pageCreation(Vector3[] points, GameObject lookAtTarget)
        {
            transform.parent = null;
            transform.position = Vector3.zero;
            page.transform.localEulerAngles = transform.localEulerAngles;
            transform.localEulerAngles = Vector3.zero;
            page.transform.localScale *= transform.localScale.x;
            transform.localScale = Vector3.one;
            if (points.Length != 4)
                throw new System.Exception("Points input incorrect size");
            pointObjects[0].transform.position = points[0];
            pointObjects[1].transform.position = points[1];
            pointObjects[2].transform.position = points[2];
            originalFinal = points[3];
            gpe.makePath();
            pf.follow = true;
            setOriginals(points, lookAtTarget);
        }

        public void setOriginals(Vector3[] points, GameObject lookAtTarget)
        {
            originalScale = page.transform.localScale;
            originalRotation = page.transform.localEulerAngles;
            page.transform.position = points[2];
            page.transform.LookAt(lookAtTarget.transform);
            page.transform.localEulerAngles = new Vector3(page.transform.localEulerAngles.x, page.transform.localEulerAngles.y, 0);
            page.transform.position = points[0];
            targetedRotation = page.transform.localEulerAngles;
            page.transform.localEulerAngles = originalRotation;
        }

        public void setGood(bool g)
        {
            isGood = g;
            FindObjectOfType<NewPageGenerator>().setPageRenderer(g, page.GetComponent<Renderer>());
        }

        public void updateLastPoint(Vector3 t)
        {
            pointObjects[2].transform.position = t + originalFinal;
            gpe.makePath();
            pf.OnPathChanged();
        }
    }
}