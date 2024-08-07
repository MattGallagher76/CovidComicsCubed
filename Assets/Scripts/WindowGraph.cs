using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class WindowGraph : MonoBehaviour
{
    [SerializeField] private Sprite circleSprite;
    [SerializeField] private RectTransform graphContainer;
    [SerializeField] private RectTransform background;
    [SerializeField] private GameObject graphLinePrefab;

    [SerializeField] private float yMultiplier;

    [SerializeField] private GameObject datePrefab;
    [SerializeField] private CoughManager cm;
    [SerializeField] private TMPro.TextMeshPro countrySlot;

    /**
     * Must me a exponential of 2
     */
    [SerializeField] private int numberOfLinesHori;
    [SerializeField] private int numberOfLinesVert;

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
        for(int i = 0; i <= numberOfLinesHori; i ++)
        {
            GameObject horz = Instantiate(graphLinePrefab);
            //GameObject vert = Instantiate(graphLinePrefab);
            /*
            if (i % 1 != 0)
            {
                Color h = horz.GetComponent<Image>().color;
                h = new Color(h.r, h.g, h.b, 0.3f);
                horz.GetComponent<Image>().color = h;
                horz.transform.localScale = new Vector3(12.75f, 0.15f, 1f);

                /*
                Color v = vert.GetComponent<Image>().color;
                v = new Color(h.r, h.g, h.b, 0.3f);
                vert.GetComponent<Image>().color = v;
                vert.transform.localScale = new Vector3(11.3f, 0.15f, 1f);
                
            }
            else
            {
                //vert.transform.localScale = new Vector3(11.3f, 0.25f, 1f);
            }
            */

            horz.transform.transform.SetParent(graphContainer, false);
            //vert.transform.transform.SetParent(graphContainer, false);
            //vert.transform.localEulerAngles = new Vector3(0, 0, 90f);

            horz.transform.localPosition = new Vector3(0, -graphContainer.rect.height/2f + (((float)i) / ((float)numberOfLinesHori)) * graphContainer.rect.height, 0);
            //vert.transform.localPosition = new Vector3(-graphContainer.rect.width / 2f + (((float)i) / ((float)numberOfLines)) * graphContainer.rect.width, 0, 0);
        }
        for (int i = 0; i <= numberOfLinesVert; i++)
        {
            GameObject vert = Instantiate(graphLinePrefab);
            float xCord = -graphContainer.rect.width / 2f + (((float)i) / ((float)numberOfLinesVert)) * graphContainer.rect.width;
            
            if (i != numberOfLinesVert)
            {
                float nextXCord = -graphContainer.rect.width / 2f + (((float)(i + 1)) / ((float)numberOfLinesVert)) * graphContainer.rect.width;

                GameObject jan = Instantiate(datePrefab);
                jan.GetComponent<TMPro.TextMeshProUGUI>().text = "Jan";
                GameObject apr = Instantiate(datePrefab);
                apr.GetComponent<TMPro.TextMeshProUGUI>().text = "Apr";
                GameObject jul = Instantiate(datePrefab);
                jul.GetComponent<TMPro.TextMeshProUGUI>().text = "Jul";
                GameObject oct = Instantiate(datePrefab);
                oct.GetComponent<TMPro.TextMeshProUGUI>().text = "Oct";
                jan.transform.transform.SetParent(background, false);
                apr.transform.transform.SetParent(background, false);
                jul.transform.transform.SetParent(background, false);
                oct.transform.transform.SetParent(background, false);


                jan.transform.localPosition = new Vector3(xCord, -400, -5);
                apr.transform.localPosition = new Vector3(xCord + (nextXCord - xCord) * 0.25f, -400, -5);
                jul.transform.localPosition = new Vector3(xCord + (nextXCord - xCord) * 0.5f, -400, -5);
                oct.transform.localPosition = new Vector3(xCord + (nextXCord - xCord) * 0.75f, -400, -5);
            }
            else
            {
                GameObject jan = Instantiate(datePrefab);
                jan.GetComponent<TMPro.TextMeshProUGUI>().text = "Jan";
                jan.transform.transform.SetParent(background, false);
                jan.transform.localPosition = new Vector3(xCord, -400, -5);
            }

            vert.transform.transform.SetParent(graphContainer, false);
            vert.transform.localEulerAngles = new Vector3(0, 0, 90f);

            vert.transform.localPosition = new Vector3(xCord, -375, 0);
            vert.transform.localScale = new Vector3(0.35f, 0.25f, 1);
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

    public void ShowGraph(List<float> xVal, List<float> yVal, string str)
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
        setCountryTitle(str);
        Debug.Log(xVal.Count + ", " + yVal.Count);
        StartCoroutine(graphDelay(xVal, yVal));
    }

    IEnumerator graphDelay(List<float> xVal, List<float> yVal)
    {
        float graphHeight = graphContainer.sizeDelta.y;
        float graphWidth = graphContainer.sizeDelta.x;

        GameObject lastCircleGameObject = null;

        for(int i = 0; i < xVal.Count * 8; i ++)
        {
            if(i % 8 == 0)
            {
                float xPos = (xVal[i/8]) * graphWidth;
                float yPos = (yVal[i/8]) * graphHeight * yMultiplier;
                GameObject circleGameObject = CreateCircle(new Vector2(xPos, yPos));
                if (lastCircleGameObject != null)
                    CreateDotConnection(lastCircleGameObject.GetComponent<RectTransform>().anchoredPosition, circleGameObject.GetComponent<RectTransform>().anchoredPosition);
                lastCircleGameObject = circleGameObject;
                cm.setIntensity(yVal[i / 8]);
            }
            yield return null;
        }
        cm.setIntensity(0);
        Debug.Log("Graph is done");
    }

    public void setCountryTitle(string str)
    {
        countrySlot.text = Regex.Replace(str, @"\b[a-z]", m => m.Value.ToUpper());
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
        gameObject.GetComponent<Image>().color = Color.white;
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
