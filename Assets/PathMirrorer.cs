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

        private float maximumPlayerDistance = 5f;

        public float maximumSpeed;
        public float speedCoefficient;

        private float currentDistance = 0f;

        [Tooltip("True if the women should mirror the jogger. False if the women should head towards the critical point to start")]
        public bool isMirroring;

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

            float target;
            if (isMirroring)
                target = (maximumPlayerDistance - dist) / maximumPlayerDistance * pathCreator.path.length * criticalPointOffset;
            else
                target = (dist < maximumPlayerDistance) ? pathCreator.path.length * criticalPointOffset : 0f;

            currentDistance += (Mathf.Min(maximumSpeed, (target - currentDistance) * speedCoefficient));

            if (pathCreator != null && follow)
            {
                transform.position = pathCreator.path.GetPointAtDistance(currentDistance, endOfPathInstruction);
                transform.rotation = pathCreator.path.GetRotationAtDistance(currentDistance, endOfPathInstruction);
                transform.localEulerAngles = new Vector3(transform.localEulerAngles.x, transform.localEulerAngles.y, 0);
            }

            animator.SetFloat("Speed", (target - currentDistance) * speedCoef);
            animator.SetFloat("AnimationSpeed", (target - currentDistance) * animationSpeedCoef);
        }
    }
}
