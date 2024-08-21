using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ZoneCollider : MonoBehaviour
{
    public int zoneID;

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.CompareTag("MainCamera"))
        {
            Debug.Log("Player has crossed " + zoneID);
            FindObjectOfType<TotalExperienceManager>().ZoneColliderTrigger(zoneID);
        }
    }
}
