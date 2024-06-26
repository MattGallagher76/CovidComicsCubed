using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MirrorTest : MonoBehaviour
{
    public Vector3 vectorToMirror;
    public Vector3 normalVector; // Example normal vector
    public Vector3 originOffset;

    public GameObject objectWithVectorToMirror;
    public GameObject normalObject;
    public GameObject finalObject;
    public GameObject mirrorOrigin;

    // Start is called before the first frame update
    void Start()
    {
        if (vectorToMirror.Equals(Vector3.zero))
            vectorToMirror = objectWithVectorToMirror.transform.position;
        if (normalVector.Equals(Vector3.zero))
            normalVector = normalObject.transform.position;
        if (originOffset.Equals(Vector3.zero))
            originOffset = mirrorOrigin.transform.position;

        objectWithVectorToMirror.transform.localScale = Vector3.one * 0.1f;
        normalObject.transform.localScale = Vector3.one * 0.1f;
        finalObject.transform.localScale = Vector3.one * 0.1f;
        mirrorOrigin.transform.localScale = Vector3.one * 0.1f;
    }

    // Update is called once per frame
    void Update()
    {
        normalObject.transform.localPosition = normalVector;
        finalObject.transform.localPosition = - MirrorVector(vectorToMirror, normalVector);
    }

    public static Vector3 MirrorVector(Vector3 vector, Vector3 normal)
    {
        // Normalize the normal vector to ensure it's a unit vector
        normal.Normalize();

        // Calculate the reflected vector using the formula
        Vector3 reflectedVector = vector - 2 * Vector3.Dot(vector, normal) * normal;

        return reflectedVector;
    }
}
