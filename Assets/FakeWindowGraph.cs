using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class FakeWindowGraph : MonoBehaviour
{
    [SerializeField] private GameObject datePrefab;
    [SerializeField] private RectTransform background;
    [SerializeField] private RectTransform graphContainer;
    [SerializeField] private GameObject graphLinePrefab;
    [SerializeField] private int numberOfLinesHori;
    [SerializeField] private int numberOfLinesVert;

    // Start is called before the first frame update
    void Start()
    {
        drawGraphBackground();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void drawGraphBackground()
    {
        for (int i = 0; i <= numberOfLinesHori; i++)
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

            horz.transform.localPosition = new Vector3(0, -graphContainer.rect.height / 2f + (((float)i) / ((float)numberOfLinesHori)) * graphContainer.rect.height, 0);
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
}
