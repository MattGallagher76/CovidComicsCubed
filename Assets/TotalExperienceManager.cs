using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TotalExperienceManager : MonoBehaviour
{
    public HandGuide hg1;
    public HandGuide hg2;

    public GuidePathManager DoomScrollingPathManager;
    //Jogging scene doesn't need a path
    //public GuidePathManager JoggingPathManager;
    public GuidePathManager DataVisPathManager;

    public GameObject[] doomRings;

    public GameObject DoomParent;
    public GameObject DoomPageParent;
    public GameObject JoggingParent;
    public GameObject DataVisParent;

    public GameObject DoomStencilMask;
    Animator doomStencilAnimator;
    public GameObject JoggerStencilMask;
    Animator joggerStencilAnimator;

    public isVisableTest DoomScrollingComicIntro;
    public isVisableTest JoggingComicIntro;
    public isVisableTest DataVisComicIntro;
    public isVisableTest FinalComicIntro;

    public GameObject bigCityDoom;
    public GameObject bigCityJogger;
    public GameObject bigCityData;
    public GameObject bigCityFinal;

    public float DoomScrollingDuration;
    //Jogging scene is not based on timer, but rather completion of series of events
    public float DataVisDuration;

    public float DoomUrgeDuration;
    public float DataUrgeDuration;

    public GameObject DoomZoneCollider;
    public GameObject DataZoneCollider;

    public int state = 0;

    float timer = 0f;

    bool doomHasHiden = false;
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
        doomStencilAnimator = DoomStencilMask.GetComponent<Animator>();
        //joggerStencilAnimator = JoggerStencilMask.GetComponent<Animator>();
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
                if(DoomScrollingComicIntro.shouldSequenceStart())
                {
                    DoomScrollingComicIntro.startSequence();
                    DoomScrollingPathManager.showPrePaths();
                    DoomParent.SetActive(true);
                    DoomPageParent.SetActive(true);
                    timer = DoomScrollingDuration;
                    state = 1;
                    Debug.Log("Current State: " + state);
                }
                break;
            case 1:
                if (timer <= 0f)
                {
                    timer = DoomUrgeDuration;
                    DoomZoneCollider.GetComponent<AudioSource>().Play();
                    DoomScrollingPathManager.showPostPaths();
                    DoomScrollingComicIntro.GetComponent<MeshRenderer>().enabled = true;
                    state = 2;
                    Debug.Log("Current State: " + state);
                }
                break;
            case 2:
                if(timer <= 0f)
                {
                    //DoomParent.SetActive(false);
                    DoomZoneCollider.GetComponent<AudioSource>().Play();
                    FindObjectOfType<isPhoneFollowing>().hidePhone();
                    
                    foreach(GameObject gb in doomRings)
                    {
                        gb.SetActive(false);
                    }
                    DoomPageParent.SetActive(false);

                    doomHasHiden = true;

                    timer = 0f;
                    state = 3;
                    Debug.Log("Current State: " + state);
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
                    Debug.Log("Current State: " + state);
                    JoggingComicIntro.startSequence();
                    JoggingParent.SetActive(true);
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
                    Debug.Log("Current State: " + state);
                    DataVisParent.SetActive(true);
                    DataVisComicIntro.startSequence();
                    DataVisPathManager.showPrePaths();
                    timer = DataVisDuration;
                }
                break;
            case 7:
                if(timer < 0f)
                {
                    DataZoneCollider.GetComponent<AudioSource>().Play();
                    DataVisPathManager.showPostPaths();
                    timer = DataUrgeDuration;
                    state = 8;
                    Debug.Log("Current State: " + state);
                    DataVisComicIntro.GetComponent<MeshRenderer>().enabled = true;
                }
                break;
            case 8:
                if(timer < 0f)
                {
                    DataZoneCollider.GetComponent<AudioSource>().Play();
                    DataVisParent.SetActive(false);
                    state = 9;
                    Debug.Log("Current State: " + state);
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
                Debug.Log("Current State: " + state);
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
                if(!doomHasHiden)
                {
                    //Zone 1 has been entered and the experience can move from doom to jogging
                    doomStencilAnimator.SetBool("SceneEnd", true);
                    FindObjectOfType<isPhoneFollowing>().hidePhone();
                    //DoomParent.SetActive(false);
                    foreach (GameObject gb in doomRings)
                    {
                        gb.SetActive(false);
                    }
                    DoomPageParent.SetActive(false);
                }
                JoggingComicIntro.GetComponent<MeshRenderer>().enabled = true;
                DoomParent.SetActive(false);
                state = 4;
                Debug.Log("Current State: " + state);
                bigCityDoom.SetActive(false);
                bigCityJogger.SetActive(true);
            }
        }
        if(id == 2)
        {
            if(state == 5)
            {
                DataVisComicIntro.GetComponent<MeshRenderer>().enabled = true;
                JoggingParent.SetActive(false);
                state = 6;
                Debug.Log("Current State: " + state);
                bigCityJogger.SetActive(false);
                bigCityData.SetActive(true);
            }
        }
        if(id == 3)
        {
            if(state == 8 || state == 9)
            {
                FinalComicIntro.GetComponent<MeshRenderer>().enabled = true;
                DataVisParent.SetActive(false);
                state = 10;
                Debug.Log("Current State: " + state);
                bigCityData.SetActive(false);
                bigCityFinal.SetActive(true);
            }
        }
    }

    public void guidePathComicDuration(int i)
    {
        if(i == 1)
        {
            FindObjectOfType<isPhoneFollowing>().OnFakeEnter();
        }
        else if(i == 2)
        {
            hg1.startSequenceExt();
            hg2.startSequenceExt();
        }
    }
}
