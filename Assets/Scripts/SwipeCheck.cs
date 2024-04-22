using Oculus.Interaction;
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

    private bool isInPose = false;
    private bool isInCollider = false;
    private bool hasStartedSwiping = false;
    private Vector3 startPos;
    private GameObject currentPage;
    

    // Start is called before the first frame update
    void Start()
    {
        
        swipePose[0].WhenSelected += () => swiped();
        swipePose[0].WhenUnselected += () => unSwiped();
    }

    public void Update()
    {
        bool byPass = false;
        if(Input.GetKey(KeyCode.Space))
        {
            byPass = true;
        }
        if((isInCollider && isInPose) || byPass)
        {
            if(!hasStartedSwiping || byPass)
            {
                //Creates a new page
                GameObject temp = Instantiate(debugPrefab);
                temp.transform.position = collidedObject.transform.position;

                startPos = collidedObject.transform.position;
                hasStartedSwiping = true;
                currentPage = Instantiate(pagePrefab);
                currentPage.transform.position = center.transform.position;
                currentPage.transform.parent = transform;
            }
            float mag = (startPos - collidedObject.transform.position).magnitude;
            Vector3 movePage = (pageOutput.transform.position - center.transform.position).normalized;
            currentPage.transform.position = movePage * mag;
        }
        else
        {
            if(hasStartedSwiping)
            {
                float mag = (startPos - collidedObject.transform.position).magnitude;
                if(mag > 1) //TODO
                {
                    //Page is complete
                }
                else
                {
                    //Page should be stopped
                    Destroy(currentPage);
                    currentPage = null;
                    hasStartedSwiping = false;
                }
            }
            //Swipe is not running or has stopped
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
}
