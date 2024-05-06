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

        // Update is called once per frame
        void Update()
        {

        }

        public void pageCreation(Vector3[] points)
        {
            if (points.Length != 3)
                throw new System.Exception("Points input incorrect size");
            pointObjects[0].transform.position = points[0];
            pointObjects[1].transform.position = points[1];
            pointObjects[2].transform.position = points[2];
            gpe.makePath();
            pf.follow = true;
        }
    }
}