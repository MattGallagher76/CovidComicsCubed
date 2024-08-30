using Oculus.Interaction;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwipeCheck : MonoBehaviour
{
    public ActiveStateSelector[] swipePose;
    public GameObject pagePrefab;
    public GameObject pageCurvePrefab;
    public GameObject debugPrefab;

    private GameObject collidedObject;
    public GameObject pageOutput;
    public GameObject center;

    //0 Index is right hand
    //1 Index is left hand

    public float displayBoardWidth;
    public float displayBoardHeight;

    public float pageMinimum;

    private bool isInPose = false;
    private bool isInCollider = false;
    private bool hasStartedSwiping = false;
    private Vector3 startPos;
    private GameObject currentPage;
    public float nextPageOffset;
    private GameObject nextPage;

    public NewPageGenerator pg;

    private bool isPageSuccessful = false;

    public float scaleConstant;
    public float magnitudeConstant;
    public bool useNewPage;

    public float magnitudeMinimum;

    public bool swipeStart;

    public GameObject pageParent;

    // Start is called before the first frame update
    void Start()
    {
        swipePose[0].WhenSelected += () => swiped();
        swipePose[0].WhenUnselected += () => unSwiped();
        swipePose[1].WhenSelected += () => swiped();
        swipePose[1].WhenUnselected += () => unSwiped();

        currentPage = Instantiate(pageCurvePrefab);
        currentPage.GetComponent<PathCreation.Examples.PageController>().setGood(pg.isGood());
        currentPage.transform.parent = center.transform;
        currentPage.transform.localScale = Vector3.one * scaleConstant;
        currentPage.transform.localPosition = Vector3.zero;
        currentPage.transform.localEulerAngles = Vector3.zero;

        nextPage = Instantiate(pageCurvePrefab);
        nextPage.GetComponent<PathCreation.Examples.PageController>().setGood(pg.isGood());
        nextPage.transform.parent = center.transform;
        nextPage.transform.localScale = Vector3.one * scaleConstant;
        nextPage.transform.localPosition = new Vector3(0, 0, nextPageOffset);
        nextPage.transform.localEulerAngles = Vector3.zero;
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

        if (Input.GetKeyDown(KeyCode.B) || swipeStart)
        {
            swipeStart = false;
            hasStartedSwiping = true;
            isPageSuccessful = true;
            collidedObject = currentPage;
            newPageSwipe();
            exitCollider();
        }

        if(Input.GetKeyDown(KeyCode.X))
        {
            for(int i = 0; i < 50; i ++)
            {
                hasStartedSwiping = true;
                isPageSuccessful = true;
                collidedObject = currentPage;
                newPageSwipe();
                exitCollider();
            }
        }

        newPageSwipe();
    }

    public void newPageSwipe()
    {
        if ((isInCollider && isInPose))
        {
            if (!hasStartedSwiping)
            {
                Debug.Log("Made new curve page");
                startPos = collidedObject.transform.position;
                hasStartedSwiping = true;
            }
            //While still swiping, move page with finger
            float mag = (startPos.y - collidedObject.transform.position.y);
            currentPage.transform.localPosition = new Vector3(
                currentPage.transform.localPosition.x,
                Mathf.Max(-magnitudeMinimum, -mag * magnitudeConstant),
                currentPage.transform.localPosition.z);
        }
        if (hasStartedSwiping)
        {
            float mag = (startPos.y - collidedObject.transform.position.y);
            if (-mag > pageMinimum) //TODO
            {
                isPageSuccessful = true;
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
        Debug.Log("An object has entered the collider");
        isInCollider = true;
        collidedObject = go;
    }

    public void exitCollider()
    {
        Debug.Log("An object has exited the collider");
        isInCollider = false;

        if (useNewPage)
        {
            if (hasStartedSwiping && isPageSuccessful)
            {
                isPageSuccessful = false;
                Vector3 v1 = currentPage.transform.position;
                Vector3 v3 = pg.generateEndLocation(currentPage.GetComponent<PathCreation.Examples.PageController>().isGood);
                Vector3 v2 = (v1 * 2 + v3) / 3f;
                v2 = new Vector3(v2.x, v2.y + 0.3f, v2.z);
                currentPage.GetComponent<PathCreation.Examples.PageController>().pageCreation(new Vector3[] { v1, v2, v3, v3 }, this.gameObject, pageParent);

                currentPage = nextPage;
                nextPage.transform.localPosition = Vector3.zero;

                nextPage = Instantiate(pageCurvePrefab);
                nextPage.GetComponent<PathCreation.Examples.PageController>().setGood(pg.isGood());
                nextPage.transform.parent = center.transform;
                nextPage.transform.localScale = Vector3.one * scaleConstant;
                nextPage.transform.localPosition = new Vector3(0, 0, nextPageOffset);
                nextPage.transform.localEulerAngles = Vector3.zero;
            }
            else
            {
                currentPage.transform.localPosition = Vector3.zero;
            }
        }
        else
        {
            if (hasStartedSwiping && isPageSuccessful)
            {
                isPageSuccessful = false;
                pg.makePage();
            }
            Destroy(currentPage);
        }
        hasStartedSwiping = false;
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
