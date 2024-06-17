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
        [Range(0,1)]
        public float criticalPointOffset;
        private GameObject criticalPointReference;
        public GameObject emptyPrefab;

        public float maximumSpeed;
        public float speedCoefficient;

        private float maximumPlayerDistance = 5f;
        private float currentDistance = 0f;
        public float distanceToTriggerAkwardShimmy;

        public float walkingMinimumThreshold;

        public enum stage {WAIT, ENTER, APPROACH, REACH, ANGER, END};
        private stage currentStage = stage.WAIT;

        public AnimationCurve ShuffleCurve;

        public GameObject[] shufflePositions;

        void Start()
        {
            criticalPointReference = Instantiate(emptyPrefab);
            criticalPointReference.transform.position = pathCreator.path.GetPointAtDistance(pathCreator.path.length * criticalPointOffset, endOfPathInstruction);
        }

        void Update()
        {
            //if (Input.GetKeyDown(KeyCode.Alpha1))
            //    currentStage = stage.ENTER;
            if (Input.GetKeyDown(KeyCode.Alpha2))
                currentStage = stage.APPROACH;
            if (Input.GetKeyDown(KeyCode.Alpha3))
                currentStage = stage.REACH;
            if (Input.GetKeyDown(KeyCode.Alpha4))
            {
                GetComponent<CovidEmotionMeasurement>().debugSetEv(-1f); //Debug set to max EV
                currentStage = stage.ANGER;
            }
            if (Input.GetKeyDown(KeyCode.Alpha5))
                currentStage = stage.END;

            Vector3 ref1 = playerRef.transform.position;
            ref1 = new Vector3(ref1.x, 0f, ref1.z);
            Vector3 ref2 = criticalPointReference.transform.position;
            ref2 = new Vector3(ref2.x, 0f, ref2.z);
            float dist = Mathf.Abs(Vector3.Distance(ref1, ref2));

            switch (currentStage)
            {
                case stage.WAIT:
                    //Waiting for user to enter region

                    if (dist < maximumPlayerDistance || Input.GetKeyDown(KeyCode.Alpha1))
                    {
                        Debug.Log("ENTER");
                        currentStage = stage.ENTER;
                    }
                    break;
                case stage.ENTER:
                    moveToLocation(criticalPointReference.transform.position);
                    if (currentDistance + walkingMinimumThreshold >= pathCreator.path.length * criticalPointOffset)
                    {
                        Debug.Log("APPROACH");
                        currentStage = stage.APPROACH;
                        animator.SetFloat("Speed", 0);
                        animator.SetFloat("AnimationSpeed", 0);
                    }
                    //Start having the woman jog to meet the user
                    break;
                case stage.APPROACH:
                    Debug.Log(dist);
                    if (dist < distanceToTriggerAkwardShimmy)
                    {
                        Debug.Log("REACH");
                        currentStage = stage.REACH;
                        walkToTargetSetup();
                    }
                    //The women has gotten to the critical point and we are waiting for the user to get close enough
                    break;
                case stage.REACH:
                    //The user has gotten close enough to the women to start the akward shimmy
                    break;
                case stage.ANGER:
                    //The woman's EV is too high and it is time for her to say something
                    break;
                case stage.END:
                    //The timer concludes and the women steps to the side
                    //Move back to pathCreator.path.GetPointAtDistance(pathCreator.path.length * criticalPointOffset, endOfPathInstruction)
                        //Do this without getting close to user
                    //Continue jogging along path
                    break;
            }
        }

        void walkToTargetSetup()
        {
            Vector3 ref1 = transform.position;
            Vector3 target = Vector3.zero;
            float minDist = 1000f;

            foreach(GameObject gb in shufflePositions)
            {
                //Might change to distance between gb and player rather than ref1
                float dist = Mathf.Abs(Vector3.Distance(gb.transform.position, playerRef.transform.position));
                if (dist <= minDist)
                {
                    target = gb.transform.position;
                    minDist = dist;
                }
            }

            target = new Vector3(target.x, ref1.y, target.z);
            StartCoroutine(walkToTarget(ref1, target));
        }

        IEnumerator walkToTarget(Vector3 a, Vector3 b)
        {
            for(int i = 0; i < 400; i ++)
            {
                Debug.Log(i);
                transform.position = Vector3.Lerp(a, b, i / 400f * ShuffleCurve.Evaluate(i / 400f));
                yield return null;
            }
            StartCoroutine(waitToWalkToTarget());
            Debug.Log("Done");
        }

        IEnumerator waitToWalkToTarget()
        {
            bool k = true;
            while(k)
            {
                Vector3 ref1 = playerRef.transform.position;
                ref1 = new Vector3(ref1.x, 0f, ref1.z);
                Vector3 ref2 = this.transform.position;
                ref2 = new Vector3(ref2.x, 0f, ref2.z);
                float dist = Mathf.Abs(Vector3.Distance(ref1, ref2));
                if (dist > 3)
                {
                    k = false;
                    walkToTargetSetup();
                }
                yield return null;
            }
        }

        void moveToLocation(Vector3 ref2)
        {
            Vector3 ref1 = playerRef.transform.position;
            ref1 = new Vector3(ref1.x, 0f, ref1.z);

            ref2 = new Vector3(ref2.x, 0f, ref2.z);

            float dist = Mathf.Abs(Vector3.Distance(ref1, ref2));

            float target = pathCreator.path.length * criticalPointOffset;

            currentDistance += (Mathf.Min(maximumSpeed, (target - currentDistance) * speedCoefficient));

            if (pathCreator != null && follow)
            {
                transform.position = pathCreator.path.GetPointAtDistance(currentDistance, endOfPathInstruction);
                transform.rotation = pathCreator.path.GetRotationAtDistance(currentDistance, endOfPathInstruction);
                transform.localEulerAngles = new Vector3(transform.localEulerAngles.x, transform.localEulerAngles.y, 0);
            }

            animator.SetFloat("Speed", (Mathf.Min(maximumSpeed, (target - currentDistance) * speedCoefficient)) * speedCoef);
            animator.SetFloat("AnimationSpeed", (Mathf.Min(maximumSpeed, (target - currentDistance) * speedCoefficient)) * animationSpeedCoef);
        }
    }
}
