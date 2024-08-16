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
    }

    private void Update()
    {

        if (timer > 0)
        {
            timer -= Time.deltaTime;
        }
        else
        {
            if (isInside)
            {
                isSwipe = true;
                //Debug.Log("Timer based Swipe Start");
            }
        }
        /*
        if(transform.localEulerAngles.x != 0 || transform.localEulerAngles.z != 0)
        {
            transform.localEulerAngles = new Vector3(0f, transform.localEulerAngles.y, 0f);
        }*/
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
                //Debug.Log("Hand entered trigger. Position: " + other.transform.position);
            }
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag(handTag))
        {
            if(isSwipe)
            {
                Debug.Log("Was a swipe");
                timer = 0f;
                isInside = false;
                isSwipe = false;
            }
            else
            {
                if(timer > 0)
                {
                    Debug.Log("Was a tap");
                    timer = 0;
                    isInside = false;
                    closestDss(other.gameObject.GetInstanceID()).GetComponent<DataSetSelector>().graphData();
                }
                isInside = false;
                isSwipe = false;
            }
            int handID = other.gameObject.GetInstanceID();
            if (handPositions.ContainsKey(handID))
            {
                handPositions.Remove(handID);
                lastHandPositions.Remove(handID);
                //Debug.Log("Hand exited trigger.");
            }
        }
    }

    GameObject closestDss(int handId)
    {
        float dist = 1000f;
        GameObject closest = null;
        foreach (DataSetSelector dss in FindObjectsOfType<DataSetSelector>())
        {
            if(Vector3.Distance(dss.gameObject.transform.position, lastHandPositions[handId]) < dist)
            {
                dist = Vector3.Distance(dss.gameObject.transform.position, lastHandPositions[handId]);
                closest = dss.gameObject;
            }
        }
        return closest;
    }

    void OnTriggerStay(Collider other)
    {
        if (other.CompareTag(handTag))
        {
            int handID = other.gameObject.GetInstanceID();
            Vector3 currentHandPosition = other.transform.position;

            //Debug.Log(Vector3.Distance(enteredPosition, currentHandPosition));

            if(!isSwipe && Vector3.Distance(enteredPosition, currentHandPosition) > minimumDistanceToSwipe)
            {
                //If the distance is exceeded, just immeditely start swipe
                timer = 0f;
                isSwipe = true;
                //Debug.Log("Distance based Swipe Start");
            }

            if (isSwipe)
            {

                if (handPositions.ContainsKey(handID))
                {
                    Vector3 lastPosition = lastHandPositions[handID];
                    Vector3 direction = currentHandPosition - lastPosition;
                    direction = new Vector3(0, 0, -1f * direction.z);

                    // Calculate the torque based on the hand movement
                    Vector3 torque = Vector3.Cross(direction, Vector3.right) * spinSpeed / handPositions.Count;
                    //torque = new Vector3(0f, torque.y, 0f);
                    rb.AddTorque(torque, ForceMode.VelocityChange); // Apply negative torque to reverse the direction

                    Debug.Log(torque);

                    // Update the hand positions
                    lastHandPositions[handID] = currentHandPosition;
                }
            }
        }
    }
}
