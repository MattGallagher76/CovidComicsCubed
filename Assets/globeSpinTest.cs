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
    private Dictionary<int, Vector3> handPositions = new Dictionary<int, Vector3>();
    private Dictionary<int, Vector3> lastHandPositions = new Dictionary<int, Vector3>();

    bool isSwipe = false;
    bool isInside = false;
    public float swipeTimer; //Duration in which the hand must move before to indicate a swipe
    public float minimumDistanceToSwipe;
    public DataSetSelector currentDss;

    Vector3 enteredPosition;

    float timer = 0f;

    //0 Is open hand
    //1 Is Poke/Tap
    //2 Is Swipe
    int leftHandState = 0;
    int rightHandState = 0;

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
