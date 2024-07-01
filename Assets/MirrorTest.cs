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

    public GameObject joggerReference;
    public GameObject playerReference;

    // Start is called before the first frame update
    void Start()
    {
        /*
        objectWithVectorToMirror.transform.localScale = Vector3.one * 0.1f;
        normalObject.transform.localScale = Vector3.one * 0.1f;
        finalObject.transform.localScale = Vector3.one * 0.1f;
        mirrorOrigin.transform.localScale = Vector3.one * 0.1f;
        */
    }

    // Update is called once per frame
    void Update()
    {
        objectWithVectorToMirror.transform.position = playerReference.transform.position;
        objectWithVectorToMirror.transform.position = new Vector3(objectWithVectorToMirror.transform.position.x, 0, objectWithVectorToMirror.transform.position.z);

        vectorToMirror = objectWithVectorToMirror.transform.localPosition;
        normalVector = normalObject.transform.localPosition;
        originOffset = mirrorOrigin.transform.localPosition;
        //normalObject.transform.localPosition = normalVector;
        //finalObject.transform.localPosition = - MirrorVector(vectorToMirror, normalVector);
        finalObject.transform.localPosition = MirrorVector(Vector3.zero, vectorToMirror, normalVector);
    }

    public static Vector3 MirrorVector2(Vector3 vector, Vector3 normal)
    {
        // Normalize the normal vector to ensure it's a unit vector
        normal.Normalize();

        // Calculate the reflected vector using the formula
        Vector3 reflectedVector = vector - 2 * Vector3.Dot(vector, normal) * normal;

        return reflectedVector;
    }

    Vector3 MirrorVector(Vector3 origin, Vector3 input, Vector3 direction)
    {
        // Step 1: Translate input vector to the origin
        Vector3 translatedInput = input - origin;

        // Step 2: Reflect the translated vector using the direction as the normal
        Vector3 reflectedVector = Vector3.Reflect(translatedInput, direction.normalized);

        // Step 3: Translate the reflected vector back by adding the origin vector
        Vector3 mirroredVector = reflectedVector + origin;

        return mirroredVector;
    }
}
