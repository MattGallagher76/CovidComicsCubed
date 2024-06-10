using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PathCreation.Examples
{
    public class PathMirrorer : MonoBehaviour
    {
        public PathCreator pathCreator;
        public EndOfPathInstruction endOfPathInstruction;
        
        [SerializeField] private float speedCoef;
        [SerializeField] private float animationSpeedCoef;
        public Animator animator;

        public bool follow = false;
        public GameObject playerRef;
        public float criticalPointOffset;
        [Range(0,1)]
        private GameObject criticalPointReference;
        public GameObject emptyPrefab;

        private float maximumPlayerDistance = 5.15f;

        public float k;

        void Start()
        {
            criticalPointReference = Instantiate(emptyPrefab);
            criticalPointReference.transform.position = pathCreator.path.GetPointAtDistance(pathCreator.path.length * criticalPointOffset, endOfPathInstruction);
        }

        void Update()
        {
            Vector3 ref1 = playerRef.transform.position;
            ref1 = new Vector3(ref1.x, 0f, ref1.z);

            Vector3 ref2 = criticalPointReference.transform.position;
            ref2 = new Vector3(ref2.x, 0f, ref2.z);

            float dist = Mathf.Abs(Vector3.Distance(ref1, ref2));

            float targetDistanceTraveled = (maximumPlayerDistance - dist) / maximumPlayerDistance * pathCreator.path.length * criticalPointOffset;

            Vector3 ref3 = transform.position;
            ref3 = new Vector3(ref3.x, 0f, ref3.z);
            dist = Mathf.Abs(Vector3.Distance(ref3, ref2));
            float distanceTravelled = (maximumPlayerDistance - dist) / maximumPlayerDistance * pathCreator.path.length * criticalPointOffset;

            float difference = targetDistanceTraveled - distanceTravelled;

            if (pathCreator != null && follow)
            {
                Debug.Log(difference);
                transform.position = pathCreator.path.GetPointAtDistance(difference * k, endOfPathInstruction);
                transform.rotation = pathCreator.path.GetRotationAtDistance(difference * k, endOfPathInstruction);
                transform.localEulerAngles = new Vector3(transform.localEulerAngles.x, transform.localEulerAngles.y, 0);
            }

            Vector3 vel = Vector3.zero;

            animator.SetFloat("Speed", vel.z);
            animator.SetFloat("AnimationSpeed", vel.z * animationSpeedCoef);
        }
    }
}
