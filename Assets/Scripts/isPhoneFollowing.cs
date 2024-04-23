using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class isPhoneFollowing : MonoBehaviour
{
    public followPlayerSmooth fps;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag.Equals("MainCamera"))
        {
            fps.setIsInRoom(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag.Equals("MainCamera"))
        {
            fps.setIsInRoom(false);
        }
    }
}
