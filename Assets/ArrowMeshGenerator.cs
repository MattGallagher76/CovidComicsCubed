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

    public GameObject textObj;
    public float verticalOffset;

    public int segments;
    public float outerRadius;
    public float innerRadius;
    public float minimumLengthVisible;
    public float minimumLengthFullVisibility;

    public CovidEmotionMeasurement cem;

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

        textObj.GetComponent<TMPro.TextMeshPro>().text = MetersToFeetAndInches(length);

        float stemLength = length - tipLength;

        Vector3 newUp = Vector3.Normalize(flatten(EndPoint.transform.position) - stemOrigin);

        Quaternion rightRotation = Quaternion.Euler(0, 90f, 0);
        Quaternion leftRotation = Quaternion.Euler(0, -90f, 0);

        //Stem points
        verticesList.Add(stemOrigin + (stemHalfWidth * (rightRotation * newUp)));
        verticesList.Add(stemOrigin + (stemHalfWidth * (leftRotation * newUp)));

        verticesList.Add(verticesList[0] + ((length / 2 - outerRadius + 0.01f) * newUp));
        verticesList.Add(verticesList[1] + ((length / 2 - outerRadius + 0.01f) * newUp));

        verticesList.Add(verticesList[0] + ((length / 2 + outerRadius - 0.01f) * newUp));
        verticesList.Add(verticesList[1] + ((length / 2 + outerRadius - 0.01f) * newUp));

        verticesList.Add(verticesList[0] + (stemLength * newUp));
        verticesList.Add(verticesList[1] + (stemLength * newUp));


        //Stem triangles
        trianglesList.Add(0);
        trianglesList.Add(1);
        trianglesList.Add(3);

        trianglesList.Add(0);
        trianglesList.Add(3);
        trianglesList.Add(2);

        trianglesList.Add(0 + 4);
        trianglesList.Add(1 + 4);
        trianglesList.Add(3 + 4);

        trianglesList.Add(0 + 4);
        trianglesList.Add(3 + 4);
        trianglesList.Add(2 + 4);

        //tip setup
        float tipHalfWidth = tipWidth / 2;
        Vector3 tipOrigin = stemOrigin + stemLength * newUp;

        //tip points
        verticesList.Add(tipOrigin + (tipHalfWidth * (leftRotation * newUp)));
        verticesList.Add(tipOrigin + (tipHalfWidth * (rightRotation * newUp)));
        verticesList.Add(tipOrigin + (tipLength * newUp));

        //tip triangle
        trianglesList.Add(4 + 4);
        trianglesList.Add(6 + 4);
        trianglesList.Add(5 + 4);

        Vector3[] vertices = new Vector3[segments * 2];
        int[] triangles = new int[segments * 6];

        for (int i = 0; i < segments; i++)
        {
            float angle = 2 * Mathf.PI * i / segments;

            vertices[i] = new Vector3(Mathf.Cos(angle), 0, Mathf.Sin(angle)) * outerRadius + (stemOrigin + newUp * length / 2f);
            vertices[i + segments] = new Vector3(Mathf.Cos(angle), 0, Mathf.Sin(angle)) * innerRadius + (stemOrigin + newUp * length / 2f);

            int nextIndex = (i + 1) % segments;

            // First triangle
            triangles[i * 6] = i;
            triangles[i * 6 + 1] = nextIndex;
            triangles[i * 6 + 2] = i + segments;

            // Second triangle
            triangles[i * 6 + 3] = nextIndex;
            triangles[i * 6 + 4] = nextIndex + segments;
            triangles[i * 6 + 5] = i + segments;
        }

        foreach (Vector3 v in vertices)
        {
            verticesList.Add(v);
        }

        foreach (int i in triangles)
        {
            trianglesList.Add(i + 11);
        }

        //assign lists to mesh.
        mesh.vertices = verticesList.ToArray();
        mesh.triangles = trianglesList.ToArray();

        textObj.transform.position = stemOrigin + newUp * length / 2f;


        textObj.transform.rotation = Quaternion.LookRotation(-newUp);

        // Ensure the flat object stays flat on the ground
        textObj.transform.rotation = Quaternion.Euler(90, textObj.transform.rotation.eulerAngles.y, 0);

        Color c = cem.spectrum();
        c = new Color(c.r, c.g, c.b, Interpolate(minimumLengthVisible, minimumLengthFullVisibility, length));
        GetComponent<MeshRenderer>().material.color = c;

        textObj.GetComponent<TMPro.TextMeshPro>().color = new Color(Color.black.r, Color.black.g, Color.black.b, c.a);
    }

    public float Interpolate(float a, float b, float c)
    {
        if (c > a)
            return 0f;
        if (c < b)
            return 1f;
        return (c - a) / (b - a);
    }

    public string MetersToFeetAndInches(float meters)
    {
        // Conversion factors
        const float metersToFeet = 3.28084f;
        const float feetToInches = 12f;

        // Convert meters to total feet
        float totalFeet = meters * metersToFeet;

        // Extract whole feet
        int feet = Mathf.FloorToInt(totalFeet);

        // Extract the remaining inches
        float inches = (totalFeet - feet) * feetToInches;

        // Round inches to the nearest whole number
        int roundedInches = Mathf.RoundToInt(inches);

        // Adjust if inches is exactly 12
        if (roundedInches == 12)
        {
            feet += 1;
            roundedInches = 0;
        }

        return $"{feet}' {roundedInches}''";
    }

    Vector3 flatten(Vector3 t)
    {
        return new Vector3(t.x, verticalOffset, t.z);
    }
}