using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public struct DataPoint
{
    private readonly float volume;
    public float Volume => volume;
    private readonly float temperature;
    public float Temperature => temperature;
    private readonly float pressure;
    public float Pressure => pressure;

    public DataPoint(float v, float t, float p)
    {
        volume = v;
        temperature = t;
        pressure = p;
    }

    public bool Equals(DataPoint other)
    {
        return volume == other.volume && temperature == other.temperature && pressure == other.pressure;
    }
}

public enum GraphType 
{ 
    PRESSURE_VS_VOLUME,
    VOLUME_VS_TEMPERATURE,
    PRESSURE_VS_TEMPERATURE
}

public class GraphManager : MonoBehaviour
{
    public static GraphManager instance;

    private readonly List<DataPoint> data = new List<DataPoint>();

    [SerializeField] private GameObject dataPointPrefab;
    [SerializeField] private RectTransform graphBG;
    private GraphType graphType = GraphType.PRESSURE_VS_VOLUME;

    [SerializeField] private Transform table;
    [SerializeField] private GameObject tableDataPrefab;

    private const float maxVolume = 55150;
    public float MaxVolume => maxVolume;
    private const float maxTemp = 5000;
    public float MaxTemp => maxTemp;
    private const float maxPressure = 20;
    public float MaxPressure => maxPressure;

    [SerializeField] private UILineRenderer curve;
    [SerializeField] private Button showCurveButton;
    private const int numPoints = 100;

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);
    }

    private void Start()
    {
        showCurveButton.onClick.AddListener(() => curve.gameObject.SetActive(!curve.gameObject.activeInHierarchy));
    }

    private void Update()
    {
        showCurveButton.interactable = GasManager.instance.ParticleCount != 0;
    }

    public void DisplayGraph()
    {
        foreach (DataPoint point in data)
        {
            Point dataPoint = Instantiate(dataPointPrefab, graphBG).GetComponent<Point>();
            float xPos = -(graphBG.rect.width / 2);
            float yPos = -(graphBG.rect.height / 2);
            switch (graphType)
            {
                case GraphType.PRESSURE_VS_VOLUME:
                    xPos += graphBG.rect.width * (point.Volume / maxVolume);
                    yPos += graphBG.rect.height * (point.Pressure / maxPressure);
                    break;
                case GraphType.VOLUME_VS_TEMPERATURE:
                    xPos += graphBG.rect.width * (point.Temperature / maxTemp);
                    yPos += graphBG.rect.height * (point.Volume / maxVolume);
                    break;
                case GraphType.PRESSURE_VS_TEMPERATURE:
                    xPos += graphBG.rect.width * (point.Temperature / maxTemp);
                    yPos += graphBG.rect.height * (point.Pressure / maxPressure);
                    break;
            }
            dataPoint.transform.localPosition = new Vector2(xPos, yPos);
            dataPoint.Data = point;

            GameObject tableData = Instantiate(tableDataPrefab, table);
            Button[] buttons = tableData.GetComponentsInChildren<Button>();
            buttons[0].onClick.AddListener(() => dataPoint.ToggleHighlight());
            buttons[1].onClick.AddListener(() => { DeletePoint(point, dataPoint); Destroy(tableData); });
            tableData.GetComponentInChildren<TMP_Text>().text = $"V: {point.Volume:0.00} mL\nT: {point.Temperature:0.00} K\nP: {point.Pressure:0.00} kPa";
        }
        RenderCurve();
        curve.gameObject.SetActive(false);
    }

    public void ClearGraph()
    {
        GameObject[] points = GameObject.FindGameObjectsWithTag("Data");
        foreach (GameObject point in points)
            Destroy(point);
        foreach (Transform child in table)
            Destroy(child.gameObject);
    }


    public void SwitchGraphTypeInt(int type)
    {
        ClearGraph();
        SwitchGraphType((GraphType) type);
        DisplayGraph();
    }
    private void SwitchGraphType(GraphType type)
    {
        graphType = type;
    }

    public void AddDataPoint(float currentVol, float currentTemp, float currentPressure)
    {
        DataPoint dataPoint = new DataPoint(currentVol, currentTemp, currentPressure);
        if (!data.Contains(dataPoint))
            data.Add(dataPoint);
    }
    
    private void DeletePoint(DataPoint data, Point point)
    {
        Destroy(point.gameObject);
        this.data.Remove(data);
    }

    public void ClearData()
    {
        data.Clear();
        ClearGraph();
        foreach (Transform child in table)
            Destroy(child.gameObject);
    }

    public void RenderCurve()
    {
        Vector2[] positions = new Vector2[numPoints];
        for (int i = 1; i <= numPoints; i++)
        {
            float xPos = -(graphBG.rect.width / 2);
            float yPos = -(graphBG.rect.height / 2);
            float vol, temp, pressure;
            switch (graphType)
            {
                case GraphType.PRESSURE_VS_VOLUME:
                    vol = maxVolume * i / numPoints;
                    xPos += graphBG.rect.width * (vol / maxVolume);
                    pressure = GasManager.instance.CalculatePressureVolConst(vol);
                    yPos += graphBG.rect.height * (pressure / maxPressure);
                    break;
                case GraphType.VOLUME_VS_TEMPERATURE:
                    temp = maxTemp * i / numPoints;
                    xPos += graphBG.rect.width * (temp / maxTemp);
                    vol = GasManager.instance.CalculateVolume(temp);
                    yPos += graphBG.rect.height * (vol / maxVolume);
                    break;
                case GraphType.PRESSURE_VS_TEMPERATURE:
                    temp = maxTemp * i / numPoints;
                    xPos += graphBG.rect.width * (temp / maxTemp);
                    pressure = GasManager.instance.CalculatePressureTempConst(temp);
                    yPos += graphBG.rect.height * (pressure / maxPressure);
                    break;
            }
            positions[i - 1] = new Vector3(xPos, yPos);
        }
        curve.points = positions;
    }
}