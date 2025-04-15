using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct DataPoint
{
    private readonly float volume;
    public float Volume => volume;
    private readonly float temperature;
    public float Temperature => temperature;
    private readonly float pressure;
    public float Pressure => pressure;

    public DataPoint(float _volume, float _temp, float _pressure)
    {
        volume = _volume;
        temperature = _temp;
        pressure = _pressure;
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

    private const float maxVolume = 45000;
    private const float maxTemp = 5000;
    private const float maxPressure = 200;

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);
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
        }
    }

    public void ClearGraph()
    {
        GameObject[] points = GameObject.FindGameObjectsWithTag("Data");
        foreach (GameObject point in points)
            Destroy(point);
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
        data.Add(dataPoint);
    }
    
    public void ClearData()
    {
        data.Clear();
    }
}
