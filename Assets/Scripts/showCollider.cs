using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class showCollider : MonoBehaviour
{
    public bool show;
    public GameObject colliderMesh;

    // Update is called once per frame
    void Update()
    {
        colliderMesh.SetActive(show);
    }
}
