using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.SqlTypes;
using UnityEngine;

public class PhoneCollider : MonoBehaviour
{
    public SwipeCheck sc;
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
        Debug.Log("Something entered");
        sc.enterCollider(other.gameObject);
    }

    private void OnTriggerExit(Collider other)
    {
        Debug.Log("Something left");
        sc.exitCollider();
    }

}
