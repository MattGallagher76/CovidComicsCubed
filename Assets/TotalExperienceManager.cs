using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TotalExperienceManager : MonoBehaviour
{
    public GuidePathManager DoomScrollingPathManager;
    public GuidePathManager JoggingPathManager;
    public GuidePathManager DataVisPathManager;

    public isVisableTest DoomScrollingComicIntro;
    public isVisableTest JoggingComicIntro;
    public isVisableTest DataVisComicIntro;

    public int state = 0;
    /*
     * 0: Waiting for START
     * Doom
     * 1: Showing comic then making prepaths appear
     * 2: Experience is running and timer until post paths is running
     * 3: Post paths had appear, if needed, urge user to move on
     */

    // Start is called before the first frame update
    void Start()
    {
        //DoomScrollingPathManager.hidePathsOnStart();
    }

    // Update is called once per frame
    void Update()
    {
        switch (state)
        {
            case 0:

                break;
            default:
                break;
        }
    }
}
