using UnityEngine;

namespace PathCreation.Examples
{
    // Moves along a path at constant speed.
    // Depending on the end of path instruction, will either loop, reverse, or stop at the end of the path.
    public class PathFollower : MonoBehaviour
    {
        public PathCreator pathCreator;
        public EndOfPathInstruction endOfPathInstruction;
        public AnimationCurve curve;
        public float speed;
        public float distanceMultiplierConstant;
        float distanceTravelled;

        public bool follow = false;

        void Start() {
            if (pathCreator != null)
            {
                // Subscribed to the pathUpdated event so that we're notified if the path changes during the game
                pathCreator.pathUpdated += OnPathChanged;
            }
        }

        void Update()
        {
            if (pathCreator != null && follow)
            {
                float distanceMultiplier = (curve.Evaluate(distanceTravelled / pathCreator.path.length) * distanceMultiplierConstant) + 1;
                distanceTravelled += speed * Time.deltaTime * distanceMultiplier;
                transform.position = pathCreator.path.GetPointAtDistance(distanceTravelled, endOfPathInstruction);
            }
        }

        // If the path changes during the game, update the distance travelled so that the follower's position on the new path
        // is as close as possible to its position on the old path
        public void OnPathChanged() {
            Debug.Log("Is this being called?");
            distanceTravelled = pathCreator.path.GetClosestDistanceAlongPath(transform.position);
        }
    }
}