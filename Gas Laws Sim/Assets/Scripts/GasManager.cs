using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GasManager : MonoBehaviour
{
    public static GasManager instance;

    private float volume;
    private float pressure = 0;

    private float temperature = 273;
    public float Temperature => temperature;
    private bool absoluteZero;

    [SerializeField] private float gasConstant = 8.314f;
    private int particleCount = 0;

    [SerializeField] private Transform topLeftCorner;
    [SerializeField] private Transform bottomRightCorner;

    [SerializeField] private TMP_Text volumeText;
    [SerializeField] private TMP_Text pressureText;
    [SerializeField] private TMP_Text tempText;

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);
    }

    private void Update()
    {
        volume = 1000 * Mathf.Abs(topLeftCorner.position.x - bottomRightCorner.position.x) * Mathf.Abs(topLeftCorner.position.y - bottomRightCorner.position.y);
        pressure = (particleCount * gasConstant * temperature) / volume;

        volumeText.text = $"Volume: {volume:0.00} mL";
        pressureText.text = particleCount != 0 ? $"Pressure: {pressure:0.00} kPa" : "Pressure: ---";
        tempText.text = particleCount != 0 ? $"Temperature: {temperature:0.00} K" : "Temperature: ---";

        if (Input.GetKeyDown(KeyCode.Escape))
            DestroyAllParticles();
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
                GasParticle[] particles = FindObjectsByType<GasParticle>(FindObjectsSortMode.None);
                foreach (GasParticle particle in particles)
                    particle.Freeze();
                absoluteZero = true;
            }
            else if (temperature >= 0.01f && absoluteZero)
            {
                GasParticle[] particles = FindObjectsByType<GasParticle>(FindObjectsSortMode.None);
                foreach (GasParticle particle in particles)
                    particle.Unfreeze();
                absoluteZero = false;
            }
            yield return null;
        }
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
}
