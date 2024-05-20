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

        // Update is called once per frame
        void Update()
        {
            if(Input.GetKeyDown(KeyCode.Y))
            {
                gpe.makePath();
            }
            if (Input.GetKeyDown(KeyCode.T))
            {
                pointObjects[0].transform.localPosition = new Vector3(0, 1, 2);
                pointObjects[1].transform.localPosition = new Vector3(3, 4, 5);
                pointObjects[2].transform.localPosition = new Vector3(2, 6, 4);
            }
            if(Input.GetKeyDown(KeyCode.U))
            {
                pf.follow = true;
            }
        }

        private void Start()
        {
            disableBobble();
        }

        public void pageCreation(Vector3[] points)
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
        }

        public void setGood(bool g)
        {
            Debug.Log("SetGood being called");
            FindObjectOfType<NewPageGenerator>().setPageRenderer(g, page.GetComponent<Renderer>());
        }

        public void disableBobble()
        {
            page.GetComponent<pageBobble>().disableBobble();
        }

        public void updateLastPoint(Vector3 t)
        {
            pointObjects[2].transform.position = t + originalFinal;
            gpe.makePath();
            pf.OnPathChanged();
        }
    }
}