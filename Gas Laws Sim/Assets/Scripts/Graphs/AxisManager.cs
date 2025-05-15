using UnityEngine;
using TMPro;

public class AxisManager : MonoBehaviour
{
    [SerializeField] private TMP_Text[] PVVAxes;
    [SerializeField] private TMP_Text[] VVTAxes;
    [SerializeField] private TMP_Text[] PVTAxes;

    private void Start()
    {
        DisplayAxes();
    }

    public void DisplayAxes()
    {
        switch(GraphManager.instance.GraphType)
        {
            case GraphType.PRESSURE_VS_VOLUME:
                TMP_Text[] pressureAxes1 = PVVAxes[0..4];
                TMP_Text[] volumeAxes1 = PVVAxes[4..];
                for (int i = 0; i < pressureAxes1.Length; i++)
                {
                    pressureAxes1[i].text = (Mathf.RoundToInt(GraphManager.instance.MaxPressure / 4 * (i + 1))).ToString();
                    volumeAxes1[i].text = (Mathf.RoundToInt(GraphManager.instance.MaxVolume / 4 * (i + 1))).ToString();
                }
                break;
            case GraphType.VOLUME_VS_TEMPERATURE:
                TMP_Text[] volumeAxes2 = VVTAxes[0..4];
                TMP_Text[] tempAxes1 = VVTAxes[4..];
                for (int i = 0; i < volumeAxes2.Length; i++)
                {
                    volumeAxes2[i].text = (Mathf.RoundToInt(GraphManager.instance.MaxVolume / 4 * (i + 1))).ToString();
                    tempAxes1[i].text = (Mathf.RoundToInt(GraphManager.instance.MaxTemp / 4 * (i + 1))).ToString();
                }
                break;
            case GraphType.PRESSURE_VS_TEMPERATURE:
                TMP_Text[] pressureAxes2 = VVTAxes[0..4];
                TMP_Text[] tempAxes2 = VVTAxes[4..];
                for (int i = 0; i < pressureAxes2.Length; i++)
                {
                    pressureAxes2[i].text = (Mathf.RoundToInt(GraphManager.instance.MaxPressure / 4 * (i + 1))).ToString();
                    tempAxes2[i].text = (Mathf.RoundToInt(GraphManager.instance.MaxTemp / 4 * (i + 1))).ToString();
                }
                break;
        }
    }
}
