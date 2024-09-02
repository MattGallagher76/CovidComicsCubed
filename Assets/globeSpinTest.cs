using Oculus.Interaction;
using Oculus.Platform;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class globeSpinTest : MonoBehaviour
{
    //Left is 0, Right is 1
    public ActiveStateSelector[] swipePoses;
    public ActiveStateSelector[] pokePoses;

    public GameObject leftHand;
    public GameObject rightHand;

    public Material LeftHandReference;
    public Material RightHandReference;
    public Color oculusHandDefault;
    public Color oculusHandPoke;
    public Color oculusHandSwipe;

    private string OutlineColorProperty = "_OutlineColor";

    public float spinSpeed = 10f; // Adjust spin speed as necessary
    public string handTag = "hand"; // Local variable for the hand tag
    private Rigidbody rb;
    private Vector3 handPositions;
    private Vector3 lastHandPositions;

    public DataSetSelector currentDss;

    Vector3 enteredPosition;

    //0 Is open hand
    //1 Is Poke/Tap
    //2 Is Swipe
    int leftHandState = 0;
    int rightHandState = 0;

    int activeHandID = 0;
    int activeHandState = -1;

    void Start()
    {
        //hands[0].GetComponent<Renderer>().material.co
        rb = GetComponent<Rigidbody>();
        swipePoses[0].WhenSelected += () => setHandColor(LeftHandReference, oculusHandSwipe, 0, 2);
        swipePoses[1].WhenSelected += () => setHandColor(RightHandReference, oculusHandSwipe, 1, 2);
        swipePoses[0].WhenUnselected += () => setHandColor(LeftHandReference, oculusHandDefault, 0, 0);
        swipePoses[1].WhenUnselected += () => setHandColor(RightHandReference, oculusHandDefault, 1, 0);
        pokePoses[0].WhenSelected += () => setHandColor(LeftHandReference, oculusHandPoke, 0, 1);
        pokePoses[1].WhenSelected += () => setHandColor(RightHandReference, oculusHandPoke, 1, 1);
        pokePoses[0].WhenUnselected += () => setHandColor(LeftHandReference, oculusHandDefault, 0, 0);
        pokePoses[1].WhenUnselected += () => setHandColor(RightHandReference, oculusHandDefault, 1, 0);
    }

    //0 is left
    //1 is right
    public void setHandColor(Material hand, Color c, int handID, int newState)
    {
        hand.SetColor(OutlineColorProperty, c);
        if (handID == 0)
            leftHandState = newState;
        else if (handID == 1)
            rightHandState = newState;
    }


    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.J))
        {
            setHandColor(LeftHandReference, oculusHandDefault, 0, 0);
        }
        if (Input.GetKeyDown(KeyCode.K))
        {
            setHandColor(LeftHandReference, oculusHandPoke, 0, 1);
        }
        if (Input.GetKeyDown(KeyCode.L))
        {
            setHandColor(LeftHandReference, oculusHandSwipe, 0, 2);
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(handTag))
        {
            if(activeHandID == 0)
            {
                if (other.gameObject.GetInstanceID() == leftHand.GetInstanceID())
                {
                    activeHandState = leftHandState;
                    lastHandPositions = leftHand.transform.position;
                }
                else
                {
                    activeHandState = rightHandState;
                    lastHandPositions = rightHand.transform.position;
                }
                activeHandID = other.gameObject.GetInstanceID();
                if(activeHandState == 1)
                {
                    closestDss((activeHandID == leftHand.GetInstanceID() ? leftHand : rightHand).transform.position).GetComponent<DataSetSelector>().graphData();
                }
            }
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag(handTag))
        {
            if(other.gameObject.GetInstanceID() == activeHandID && activeHandState == 1)
            {
                activeHandID = 0;
                activeHandState = -1;
            }
        }
    }

    GameObject closestDss(Vector3 handPos)
    {
        float dist = 1000f;
        GameObject closest = null;
        foreach (DataSetSelector dss in FindObjectsOfType<DataSetSelector>())
        {
            if(Vector3.Distance(dss.gameObject.transform.position, handPos) < dist)
            {
                dist = Vector3.Distance(dss.gameObject.transform.position, handPos);
                closest = dss.gameObject;
            }
        }
        return closest;
    }

    void OnTriggerStay(Collider other)
    {
        /*
        if (other.CompareTag(handTag))
        {
            if(other.gameObject.GetInstanceID() == activeHandID && activeHandState == 2)
            { 
                Vector3 currentHandPosition = ((leftHand.GetInstanceID() == activeHandID) ? leftHand : rightHand).transform.position;
                Vector3 direction = currentHandPosition - lastHandPositions;

                direction = new Vector3(0, 0, -1f * direction.z);

                Vector3 torque = Vector3.Cross(direction, Vector3.right) * spinSpeed;
                rb.AddTorque(torque, ForceMode.VelocityChange); // Apply negative torque to reverse the direction

                // Update the hand positions
                lastHandPositions = currentHandPosition;
            }
        }
        */
    }
}

/*
 * int handID = other.gameObject.GetInstanceID();
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
 */
