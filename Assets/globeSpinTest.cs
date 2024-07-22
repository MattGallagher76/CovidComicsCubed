using Oculus.Platform;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class globeSpinTest : MonoBehaviour
{
    public float spinSpeed = 10f; // Adjust spin speed as necessary
    public string handTag = "hand"; // Local variable for the hand tag
    private Rigidbody rb;
    private Dictionary<int, Vector3> handPositions = new Dictionary<int, Vector3>();
    private Dictionary<int, Vector3> lastHandPositions = new Dictionary<int, Vector3>();

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        Debug.Log("SpinGlobe script started");
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(handTag))
        {
            int handID = other.gameObject.GetInstanceID();
            if (!handPositions.ContainsKey(handID))
            {
                handPositions.Add(handID, other.transform.position);
                lastHandPositions.Add(handID, other.transform.position);
                Debug.Log("Hand entered trigger. Position: " + other.transform.position);
            }
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag(handTag))
        {
            int handID = other.gameObject.GetInstanceID();
            if (handPositions.ContainsKey(handID))
            {
                handPositions.Remove(handID);
                lastHandPositions.Remove(handID);
                Debug.Log("Hand exited trigger.");
            }
        }
    }

    void OnTriggerStay(Collider other)
    {
        if (other.CompareTag(handTag))
        {
            int handID = other.gameObject.GetInstanceID();
            Vector3 currentHandPosition = other.transform.position;

            if (handPositions.ContainsKey(handID))
            {
                Vector3 lastPosition = lastHandPositions[handID];
                Vector3 direction = currentHandPosition - lastPosition;

                // Calculate the torque based on the hand movement
                Vector3 torque = Vector3.Cross(direction, Vector3.up) * spinSpeed / handPositions.Count;
                rb.AddTorque(-torque, ForceMode.VelocityChange); // Apply negative torque to reverse the direction

                Debug.Log("Hand staying in trigger. Applying torque: " + -torque);

                // Update the hand positions
                lastHandPositions[handID] = currentHandPosition;
            }
        }
    }
}
