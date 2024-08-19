using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InvisibleWallController : MonoBehaviour
{
    public string tagToHide = "HideBehindWall";

    void OnWillRenderObject()
    {
        GameObject[] objectsToHide = GameObject.FindGameObjectsWithTag(tagToHide);
        foreach (GameObject obj in objectsToHide)
        {
            Renderer rend = obj.GetComponent<Renderer>();
            if (rend != null)
            {
                // Disable rendering of the objects behind the wall
                rend.enabled = false;
            }
        }
    }

    void OnRenderObject()
    {
        GameObject[] objectsToHide = GameObject.FindGameObjectsWithTag(tagToHide);
        foreach (GameObject obj in objectsToHide)
        {
            Renderer rend = obj.GetComponent<Renderer>();
            if (rend != null)
            {
                // Re-enable rendering of the objects after the wall has rendered
                rend.enabled = true;
            }
        }
    }
}

