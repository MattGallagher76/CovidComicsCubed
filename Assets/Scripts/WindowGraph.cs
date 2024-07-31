using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WindowGraph : MonoBehaviour
{
    [SerializeField] private Sprite circleSprite;
    [SerializeField] private RectTransform graphContainer;
    [SerializeField] private GameObject graphLinePrefab;

    /**
     * Must me a exponential of 2
     */
    [SerializeField] private int numberOfLines;

    private List<GameObject> circles = new List<GameObject>();
    private List<GameObject> lines = new List<GameObject>();

    private void Awake()
    {
        List<float> valueListY = new List<float>() { 0f, 0.75f, 0.50f, 0.25f, 1.00f};
        List<float> valueListX = new List<float>() { 0f, 0.25f, 0.50f, 0.75f, 1.00f };

        //ShowGraph(valueListX, valueListY);

        drawGraphBackground();
    }

    private void drawGraphBackground()
    {
        for(int i = 0; i <= numberOfLines; i ++)
        {
            GameObject horz = Instantiate(graphLinePrefab);
            GameObject vert = Instantiate(graphLinePrefab);

            if (i % 4 != 0)
            {
                Color h = horz.GetComponent<Image>().color;
                h = new Color(h.r, h.g, h.b, 0.3f);
                horz.GetComponent<Image>().color = h;
                horz.transform.localScale = new Vector3(12.75f, 0.15f, 1f);

                Color v = vert.GetComponent<Image>().color;
                v = new Color(h.r, h.g, h.b, 0.3f);
                vert.GetComponent<Image>().color = v;
                vert.transform.localScale = new Vector3(11.3f, 0.15f, 1f);
            }
            else
            {
                vert.transform.localScale = new Vector3(11.3f, 0.25f, 1f);
            }

            horz.transform.transform.SetParent(graphContainer, false);
            vert.transform.transform.SetParent(graphContainer, false);
            vert.transform.localEulerAngles = new Vector3(0, 0, 90f);

            horz.transform.localPosition = new Vector3(0, -graphContainer.rect.height/2f + (((float)i) / ((float)numberOfLines)) * graphContainer.rect.height, 0);
            vert.transform.localPosition = new Vector3(-graphContainer.rect.width / 2f + (((float)i) / ((float)numberOfLines)) * graphContainer.rect.width, 0, 0);
        }
    }

    private GameObject CreateCircle(Vector2 anchoredPos)
    {
        GameObject gameObject = new GameObject("circle", typeof(Image));
        gameObject.transform.SetParent(graphContainer, false);
        //gameObject.GetComponent<Image>().sprite = circleSprite;
        gameObject.GetComponent<Image>().color = new Color(0, 0, 0, 0);
        RectTransform rectTransform = gameObject.GetComponent<RectTransform>();
        rectTransform.anchoredPosition = anchoredPos;
        rectTransform.sizeDelta = new Vector2(11, 11);
        rectTransform.anchorMin = new Vector2(0, 0);
        rectTransform.anchorMax = new Vector2(0, 0);
        circles.Add(gameObject);
        return gameObject;
    }

    public void ShowGraph(List<float> xVal, List<float> yVal)
    {
        /*
        float graphHeight = graphContainer.sizeDelta.y;
        float graphWidth = graphContainer.sizeDelta.x;

        GameObject lastCircleGameObject = null;
        for(int i = 0; i < xVal.Count; i ++)
        {
            float xPos = (xVal[i]) * graphWidth;
            Debug.Log("UGG" + xPos);
            float yPos = (yVal[i]) * graphHeight;
            GameObject circleGameObject = CreateCircle(new Vector2(xPos, yPos));
            if (lastCircleGameObject != null)
                CreateDotConnection(lastCircleGameObject.GetComponent<RectTransform>().anchoredPosition, circleGameObject.GetComponent<RectTransform>().anchoredPosition);
            lastCircleGameObject = circleGameObject;
        }*/

        StartCoroutine(graphDelay(xVal, yVal));
    }

    IEnumerator graphDelay(List<float> xVal, List<float> yVal)
    {
        float graphHeight = graphContainer.sizeDelta.y;
        float graphWidth = graphContainer.sizeDelta.x;

        GameObject lastCircleGameObject = null;

        int i = 0;
        for(float timer = 0f; timer < 10f; timer += Time.deltaTime)
        {
            //Debug.Log(xVal[i] + ", " + yVal[i]);

            float xPos = (xVal[i]) * graphWidth;
            float yPos = (yVal[i]) * graphHeight;
            GameObject circleGameObject = CreateCircle(new Vector2(xPos, yPos));
            if (lastCircleGameObject != null)
                CreateDotConnection(lastCircleGameObject.GetComponent<RectTransform>().anchoredPosition, circleGameObject.GetComponent<RectTransform>().anchoredPosition);
            lastCircleGameObject = circleGameObject;

            i++;
            yield return null;
        }
    }

    public void clearGraph()
    {
        foreach(GameObject gb in circles)
        {
            Destroy(gb);
        }
        foreach(GameObject gb in lines)
        {
            Destroy(gb);
        }
    }

    private void CreateDotConnection(Vector2 dotPosA, Vector2 dotPosB)
    {
        GameObject gameObject = new GameObject("dotCon", typeof(Image));
        gameObject.transform.SetParent(graphContainer, false);
        RectTransform rectTransform = gameObject.GetComponent<RectTransform>();
        Vector2 dir = (dotPosB - dotPosA).normalized;
        float dist = Vector2.Distance(dotPosA, dotPosB);

        rectTransform.anchorMin = new Vector2(0, 0);
        rectTransform.anchorMax = new Vector2(0, 0);
        rectTransform.sizeDelta = new Vector2(dist, 3f);
        rectTransform.anchoredPosition = dotPosA + dir * dist * 0.5f;
        rectTransform.localEulerAngles = new Vector3(0, 0, GetAngleFromVectorFloat(dir));
        gameObject.GetComponent<Image>().color = Color.green;
        lines.Add(gameObject);
    }

    public static float GetAngleFromVectorFloat(Vector3 dir)
    {
        // Normalize the vector to get the direction
        dir.Normalize();

        // Calculate the angle in radians between the vector and the right vector
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;

        // Return the angle in degrees
        return angle;
    }
}
