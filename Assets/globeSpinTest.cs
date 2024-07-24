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

    bool isSwipe = false;
    bool isInside = false;
    public float swipeTimer; //Duration in which the hand must move before to indicate a swipe
    public float minimumDistanceToSwipe;
    public DataSetSelector currentDss;

    Vector3 enteredPosition;

    float timer = 0f;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        Debug.Log("SpinGlobe script started");
    }

    public void updateDss(DataSetSelector dss)
    {
        currentDss = dss;
    }

    public void dssColliderExited()
    {
        Debug.Log("Exited dss - Timer: " + timer);
        if(timer > 0)
        {
            //Tap
            Debug.Log("Tapped: " + currentDss.countryName);
            currentDss.graphData();
            timer = 0f;
            isInside = false;   //Forces the hand to leave before trying to spin a gain
        }
    }

    private void Update()
    {
        if(timer > 0)
        {
            timer -= Time.deltaTime;
        }
        else
        {
            if (isInside)
            {
                isSwipe = true;
                Debug.Log("Timer based Swipe Start");
            }
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(handTag))
        {
            timer = swipeTimer;
            isInside = true;
            enteredPosition = other.gameObject.transform.position;
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
            if(isSwipe)
            {
                timer = 0f;
                isInside = false;
                isSwipe = false;
            }
            else
            {
                isInside = false;
                isSwipe = false;
            }
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

            /*if(!isSwipe && Vector3.Distance(enteredPosition, currentHandPosition) > minimumDistanceToSwipe)
            {
                //If the distance is exceeded, just immeditely start swipe
                timer = 0f;
                isSwipe = true;
                Debug.Log("Distance based Swipe Start");
            }*/

            if (isSwipe)
            {

                if (handPositions.ContainsKey(handID))
                {
                    Vector3 lastPosition = lastHandPositions[handID];
                    Vector3 direction = currentHandPosition - lastPosition;

                    // Calculate the torque based on the hand movement
                    Vector3 torque = Vector3.Cross(direction, Vector3.up) * spinSpeed / handPositions.Count;
                    rb.AddTorque(-torque, ForceMode.VelocityChange); // Apply negative torque to reverse the direction

                    //Debug.Log("Hand staying in trigger. Applying torque: " + -torque);

                    // Update the hand positions
                    lastHandPositions[handID] = currentHandPosition;
                }
            }
        }
    }
}
