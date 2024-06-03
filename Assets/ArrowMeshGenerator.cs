using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshFilter))]
[RequireComponent(typeof(MeshRenderer))]
public class ArrowMeshGenerator : MonoBehaviour
{
    
    public float stemWidth;
    public float tipLength;
    public float tipWidth;

    public GameObject StartPoint;
    public GameObject EndPoint;
    public float endOffset;

    [System.NonSerialized]
    public List<Vector3> verticesList;
    [System.NonSerialized]
    public List<int> trianglesList;

    Mesh mesh;

    void Start()
    {
        //make sure Mesh Renderer has a material
        mesh = new Mesh();
        this.GetComponent<MeshFilter>().mesh = mesh;
    }

    void Update()
    {
        GenerateArrow();
    }

    //arrow is generated starting at Vector3.zero
    //arrow is generated facing right, towards radian 0.
    void GenerateArrow()
    {
        //setup
        verticesList = new List<Vector3>();
        trianglesList = new List<int>();

        //stem setup
        Vector3 stemOrigin = StartPoint.transform.position;
        stemOrigin = flatten(stemOrigin);

        float stemHalfWidth = stemWidth / 2f;

        float length = Mathf.Abs(Vector3.Distance(flatten(StartPoint.transform.position), flatten(EndPoint.transform.position))) - endOffset;
        float stemLength = length - tipLength;

        Vector3 newUp = Vector3.Normalize(flatten(EndPoint.transform.position) - stemOrigin);

        Quaternion rightRotation = Quaternion.Euler(0, 90f, 0);
        Quaternion leftRotation = Quaternion.Euler(0, -90f, 0);

        //Stem points
        verticesList.Add(stemOrigin + (stemHalfWidth * (rightRotation * newUp)));
        verticesList.Add(stemOrigin + (stemHalfWidth * (leftRotation * newUp)));
        verticesList.Add(verticesList[0] + (stemLength * newUp));
        verticesList.Add(verticesList[1] + (stemLength * newUp));


        //Stem triangles
        trianglesList.Add(0);
        trianglesList.Add(1);
        trianglesList.Add(3);

        trianglesList.Add(0);
        trianglesList.Add(3);
        trianglesList.Add(2);

        //tip setup
        float tipHalfWidth = tipWidth / 2;
        Vector3 tipOrigin = stemOrigin + stemLength * newUp;

        //tip points
        verticesList.Add(tipOrigin + (tipHalfWidth * (leftRotation * newUp)));
        verticesList.Add(tipOrigin + (tipHalfWidth * (rightRotation * newUp)));
        verticesList.Add(tipOrigin + (tipLength * newUp));

        //tip triangle
        trianglesList.Add(4);
        trianglesList.Add(6);
        trianglesList.Add(5);

        //assign lists to mesh.
        mesh.vertices = verticesList.ToArray();
        mesh.triangles = trianglesList.ToArray();
    }

    Vector3 flatten(Vector3 t)
    {
        return new Vector3(t.x, 0, t.z);
    }
}