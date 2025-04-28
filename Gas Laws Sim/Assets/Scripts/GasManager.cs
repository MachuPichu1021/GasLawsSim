using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public enum SimLockState
{
    NONE,
    VOLUME,
    TEMP,
    PRESSURE_VOL,
    PRESSURE_TEMP
}

public class GasManager : MonoBehaviour
{
    public static GasManager instance;

    private SimLockState lockState;
    public SimLockState LockState => lockState;

    private float volume;
    private float pressure = 0;

    private float temperature = 273;
    public float Temperature => temperature;
    private bool absoluteZero;

    private const float gasConstant = 8.314f;
    private int particleCount = 0;

    [SerializeField] private TMP_Text volumeText;
    [SerializeField] private TMP_Text pressureText;
    [SerializeField] private TMP_Text tempText;

    [SerializeField] private GameObject handle;
    [SerializeField] private GameObject tempSlider;

    [SerializeField] private Button dataPointButton;

    [SerializeField] private Button unlockButton;
    [SerializeField] private Button lockVolumeButton;
    [SerializeField] private Button lockTempButton;
    [SerializeField] private Button lockPressureWRTVolButton;
    [SerializeField] private Button lockPressureWRTTempButton;

    private Container container;

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);
    }

    private void Start()
    {
        dataPointButton.onClick.AddListener(() => GraphManager.instance.AddDataPoint(volume, temperature, pressure));

        unlockButton.onClick.AddListener(() => SetLockState(SimLockState.NONE));
        lockVolumeButton.onClick.AddListener(() => SetLockState(SimLockState.VOLUME));
        lockTempButton.onClick.AddListener(() => SetLockState(SimLockState.TEMP));
        lockPressureWRTVolButton.onClick.AddListener(() => SetLockState(SimLockState.PRESSURE_VOL));
        lockPressureWRTTempButton.onClick.AddListener(() => SetLockState(SimLockState.PRESSURE_TEMP));

        container = FindObjectOfType<Container>();
    }

    private void Update()
    {
        lockTempButton.interactable = particleCount != 0;
        lockPressureWRTVolButton.interactable = particleCount != 0;
        lockPressureWRTTempButton.interactable = particleCount != 0;

        if (lockState == SimLockState.NONE || lockState == SimLockState.TEMP || lockState == SimLockState.VOLUME)
        {
            volume = 1000 * Mathf.Abs(container.LeftWall.transform.position.x - container.RightWall.transform.position.x) * 
                Mathf.Abs(container.TopWall.transform.position.y - container.BottomWall.transform.position.y);
            pressure = (particleCount * gasConstant * temperature) / volume;
        }
        else if (lockState == SimLockState.PRESSURE_TEMP)
        {
            volume = 1000 * Mathf.Abs(container.LeftWall.transform.position.x - container.RightWall.transform.position.x) *
                Mathf.Abs(container.TopWall.transform.position.y - container.BottomWall.transform.position.y);
            temperature = (pressure * volume) / (particleCount * gasConstant);
        }
        else if (lockState == SimLockState.PRESSURE_VOL)
        {
            volume = (particleCount * gasConstant * temperature) / pressure;
            container.Resize(volume);
        }

        volumeText.text = $"Volume: {volume:0.00} mL";
        pressureText.text = particleCount != 0 ? $"Pressure: {pressure:0.00} kPa" : "Pressure: ---";
        tempText.text = particleCount != 0 ? $"Temperature: {temperature:0.00} K" : "Temperature: ---";

        if (Input.GetKeyDown(KeyCode.Escape))
            DestroyAllParticles();
    }

    public float CalculateVolume(float temp)
    {
        return particleCount * gasConstant * temp / pressure;
    }
    public float CalculatePressureTempConst(float temp)
    {
        return particleCount * gasConstant * temp / volume;
    }
    public float CalculatePressureVolConst(float vol)
    {
        return particleCount * gasConstant * temperature / vol;
    }

    public void ChangeTemperature(float sliderVal)
    {
        if (particleCount == 0)
        {
            temperature = 273;
            return;
        }

        float deltaScroll = sliderVal - 0.5f;
        StopAllCoroutines();
        StartCoroutine(ChangeTemperatureRepeating(deltaScroll));
    }

    private IEnumerator ChangeTemperatureRepeating(float deltaScroll)
    {
        while (deltaScroll != 0)
        {
            temperature += deltaScroll * Mathf.Max(temperature, 0.0001f) * Time.deltaTime;
            temperature = Mathf.Clamp(temperature, 0, 10000);
            if (temperature < 0.01f && !absoluteZero)
            {
                FreezeAll();
                absoluteZero = true;
            }
            else if (temperature >= 0.01f && absoluteZero)
            {
                UnfreezeAll();
                absoluteZero = false;
            }
            yield return null;
        }
    }

    public void FreezeAll()
    {
        GasParticle[] particles = FindObjectsByType<GasParticle>(FindObjectsSortMode.None);
        foreach (GasParticle particle in particles)
            particle.Freeze();
    }

    public void UnfreezeAll()
    {
        GasParticle[] particles = FindObjectsByType<GasParticle>(FindObjectsSortMode.None);
        foreach (GasParticle particle in particles)
            particle.Unfreeze();
    }

    public void IncreaseParticleCount(int numParticles)
    {
        particleCount += numParticles;
    }

    public void DestroyAllParticles()
    {
        GasParticle[] particles = FindObjectsByType<GasParticle>(FindObjectsSortMode.None);
        foreach (GasParticle particle in particles)
            Destroy(particle.gameObject);
        particleCount = 0;
    }

    public void SetLockStateInt(int lockState)
    {
        SetLockState((SimLockState) lockState);
    }

    private void SetLockState(SimLockState lockState)
    {
        this.lockState = lockState;
        switch (lockState)
        {
            case SimLockState.NONE:
                handle.SetActive(true);
                tempSlider.SetActive(true);
                break;
            case SimLockState.VOLUME:
                handle.SetActive(false);
                tempSlider.SetActive(true);
                break;
            case SimLockState.TEMP:
                handle.SetActive(true);
                tempSlider.SetActive(false);
                break;
            case SimLockState.PRESSURE_VOL:
                handle.SetActive(false);
                tempSlider.SetActive(true);
                break;
            case SimLockState.PRESSURE_TEMP:
                handle.SetActive(true);
                tempSlider.SetActive(false);
                break;
        }
    }
}
