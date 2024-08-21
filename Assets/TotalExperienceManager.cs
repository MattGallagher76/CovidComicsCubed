using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TotalExperienceManager : MonoBehaviour
{
    public GuidePathManager DoomScrollingPathManager;
    //Jogging scene doesn't need a path
    //public GuidePathManager JoggingPathManager;
    public GuidePathManager DataVisPathManager;

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
        switch (state)
        {
            case 0:
                //Waiting for start
                //If moved close to comic and looking at it
                if(false)
                {
                    DoomScrollingComicIntro.startSequence();
                    DoomScrollingPathManager.showPrePaths();
                    //DoomScoll scene visable
                    timer = DoomScrollingDuration;
                    state = 1;
                }
                break;
            case 1:
                if (timer <= 0f)
                {
                    timer = urgeDuration;
                    DoomScrollingPathManager.showPostPaths();
                    state = 2;
                }
                break;
            case 2:
                if(timer <= 0f)
                {
                    //Disable doom scene;
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
                if(false)
                {
                    state = 5;
                    JoggingComicIntro.startSequence();
                    //Jogging scene visable
                }
                break;
            case 5:
                //Waiting for zone collider 2 to be entered
                break;
            case 6:
                //if close enough to next comic and looking at it
                if(false)
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
                }
                break;
            case 8:
                if(timer < 0f)
                {
                    //Disable data vis scene
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
                //Disable doom scene if not disabled already
                state = 4;
            }
        }
        if(id == 2)
        {
            if(state == 5)
            {
                //Disable jogging scene
                state = 6;
            }
        }
        if(id == 3)
        {
            if(state == 8 || state == 9)
            {
                //Disable data vis if not disabled already
                state = 10;
            }
        }
    }
}
