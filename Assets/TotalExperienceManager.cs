using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TotalExperienceManager : MonoBehaviour
{
    public GuidePathManager DoomScrollingPathManager;
    //Jogging scene doesn't need a path
    //public GuidePathManager JoggingPathManager;
    public GuidePathManager DataVisPathManager;

    public GameObject DoomParent;
    public GameObject JoggingParent;
    public GameObject DataVisParent;

    public isVisableTest DoomScrollingComicIntro;
    public isVisableTest JoggingComicIntro;
    public isVisableTest DataVisComicIntro;
    public isVisableTest FinalComicIntro;

    public float DoomScrollingDuration;
    //Jogging scene is not based on timer, but rather completion of series of events
    public float DataVisDuration;

    public float urgeDuration;

    public int state = 0;

    float timer = 0f;
    /*
     * 0: Waiting for START
     * Doom
     * 1: Showing comic then making prepaths appear
     *      Experience is running and timer until post paths is running
     * 2: Post paths had appear, if needed, urge user to move on
     * 3: Scene is disabled to urge user to move 
     * 
     * Jogging:
     * 4: Zone Collider 1 is Entered and Doom Scrolling scene is hidden
     *      Showing comic then making scene appear
     * 5: Experience is running
     * 
     * Data Vis:
     * 6: Zone Collider 2 is Entered and Jogging scene is hidden
     *      Showing comic then making prepaths appear
     *      Experience is running and timer until post paths is running
     * 7: Post paths had appear, if needed, urge user to move on
     * 8: Scene is disabled to urge user to move on
     * 9: Zone Collider 3 is Entered and Data Vis scene is hidden
     * 
     * 10: Final comic is shown and message to remove headset is shown:
     */

    // Start is called before the first frame update
    void Start()
    {
        //DoomScrollingPathManager.hidePathsOnStart();
    }

    // Update is called once per frame
    void Update()
    {
        if (timer > 0f)
            timer -= Time.deltaTime;
        Debug.Log("Current State: " + state);
        switch (state)
        {
            case 0:
                //Waiting for start
                //If moved close to comic and looking at it
                if(DoomScrollingComicIntro.shouldSequenceStart())
                {
                    DoomScrollingComicIntro.startSequence();
                    DoomScrollingPathManager.showPrePaths();
                    DoomParent.SetActive(true);
                    timer = DoomScrollingDuration;
                    state = 1;
                }
                break;
            case 1:
                if (timer <= 0f)
                {
                    timer = urgeDuration;
                    DoomScrollingPathManager.showPostPaths();
                    DoomScrollingComicIntro.GetComponent<MeshRenderer>().enabled = true;
                    state = 2;
                }
                break;
            case 2:
                if(timer <= 0f)
                {
                    DoomParent.SetActive(false);
                    timer = 0f;
                    state = 3;
                }
                break;
            case 3:
                //Waiting for zone collider 1 to be entered
                break;
            //------------------------------------------------
            case 4:
                //if close enough to next comic and looking at it
                if(JoggingComicIntro.shouldSequenceStart())
                {
                    state = 5;
                    JoggingComicIntro.startSequence();
                    JoggingParent.SetActive(true);
                    JoggingComicIntro.GetComponent<MeshRenderer>().enabled = true;
                }
                break;
            case 5:
                //Waiting for zone collider 2 to be entered
                break;
            case 6:
                //if close enough to next comic and looking at it
                if (DataVisComicIntro.shouldSequenceStart())
                {
                    state = 7;
                    DataVisComicIntro.startSequence();
                    DataVisPathManager.showPrePaths();
                    timer = DataVisDuration;
                }
                break;
            case 7:
                if(timer < 0f)
                {
                    DataVisPathManager.showPostPaths();
                    timer = urgeDuration;
                    state = 8;
                    DataVisComicIntro.GetComponent<MeshRenderer>().enabled = true;
                }
                break;
            case 8:
                if(timer < 0f)
                {
                    DataVisParent.SetActive(true);
                    state = 9;
                    timer = 0f;
                }
                break;
            case 9:
                //Waiting for zone collider 3 to be entered
                break;
            case 10:
                //Show comic
                FinalComicIntro.startSequence();
                timer = 8f;
                state = 11;
                break;
            case 11:
                if(timer < 0f)
                {
                    //Show message to remove headset;
                    timer = 0;
                }
                break;
            default:
                break;
        }
    }

    public void ZoneColliderTrigger(int id)
    {
        if(id == 1)
        {
            if(state == 2 || state == 3)
            {
                //Zone 1 has been entered and the experience can move from doom to jogging
                DoomParent.SetActive(false);
                state = 4;
            }
        }
        if(id == 2)
        {
            if(state == 5)
            {
                DoomParent.SetActive(false);
                state = 6;
            }
        }
        if(id == 3)
        {
            if(state == 8 || state == 9)
            {
                DataVisParent.SetActive(false);
                state = 10;
            }
        }
    }
}
