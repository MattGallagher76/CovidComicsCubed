using Oculus.Interaction;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwipeCheck : MonoBehaviour
{
    public ActiveStateSelector[] swipePose;
    public GameObject pagePrefab;
    public GameObject debugPrefab;

    private GameObject collidedObject;
    public GameObject pageOutput;
    public GameObject center;

    //0 Index is right hand
    //1 Index is left hand

    public float displayBoardWidth;
    public float displayBoardHeight;

    public float magnitudeMultiplier;
    public float pageMinimum;

    private bool isInPose = false;
    private bool isInCollider = false;
    private bool hasStartedSwiping = false;
    private Vector3 startPos;
    private GameObject currentPage;

    public NewPageGenerator pg;

    private bool isPageSuccessful = false;

    // Start is called before the first frame update
    void Start()
    {
        
        swipePose[0].WhenSelected += () => swiped();
        swipePose[0].WhenUnselected += () => unSwiped();
    }

    public void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space))
        {
            Debug.Log("Space Pressed");
            isInPose = true;
        }
        if (Input.GetKeyDown(KeyCode.C))
        {
            Debug.Log("C Pressed");
            isInPose = false;
        }



        if ((isInCollider && isInPose))
        {
            if(!hasStartedSwiping)
            {
                Debug.Log("Hih");
                startPos = collidedObject.transform.position;
                hasStartedSwiping = true;
                currentPage = Instantiate(pagePrefab);
                Vector3 tempScale = currentPage.transform.localScale;
                Vector3 tempPosition = currentPage.transform.localPosition;
                currentPage.transform.parent = center.transform;
                currentPage.transform.localScale = tempScale;
                currentPage.transform.localPosition = Vector3.zero;
                currentPage.transform.localEulerAngles = Vector3.zero;
            }
            if(!isPageSuccessful)
            {
                float mag = (startPos.y - collidedObject.transform.position.y);
                Debug.Log(mag);
                currentPage.transform.localPosition = new Vector3(
                    currentPage.transform.localPosition.x,
                    -mag * magnitudeMultiplier,
                    currentPage.transform.localPosition.z);
            }
        }
        else
        {
            if(hasStartedSwiping)
            {
                Destroy(center.transform.GetChild(0));
                pg.makePage();
            }
        }
        if(hasStartedSwiping)
        {
            float mag = (startPos.y - collidedObject.transform.position.y);
            Debug.Log("Check: " + -mag + " vs " + pageMinimum);
            if (-mag > pageMinimum) //TODO
            {
                isPageSuccessful = true;
            }
            else
            {
                //Page should be stopped
                Destroy(currentPage);
                currentPage = null;
                hasStartedSwiping = false;
            }
        }
    }

    private void swiped()
    {
        isInPose = true;
    }

    private void unSwiped()
    {
        isInPose = false;
    }

    public void enterCollider(GameObject go)
    {
        isInCollider = true;
        collidedObject = go;
    }

    public void exitCollider()
    {
        isInCollider = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag.Equals("hand"))
            enterCollider(other.gameObject);
    }

    private void OnTriggerExit(Collider other)
    {
        if(other.tag.Equals("hand"))
            exitCollider();
    }
}
