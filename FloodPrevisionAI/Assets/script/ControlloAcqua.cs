using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class ControlloAcqua : MonoBehaviour
{
    IEnumerator loop;

    [Header("State")]
    [Range(0.0f, 1.0f)]
    [SerializeField] public float terrainSaturation; // how much water is in the terrain, high = can't absorb more water
    [Range(0.0f, 1.0f)]
    [SerializeField] public float rainIntensity; // velocity for saturation increase when raining

    [Space]
    [Header("Saturation Variables")]
    [Range(0.0f, 1.0f)]
    [SerializeField] float saturationIncreaseVelocity; // multiplicative factor for saturation increase, 1 = 100% increase, 0.5 = 50% increase
    [SerializeField] float saturationDecrease; // water lowering per update

    [Space]
    [Header("Water Heights")]
    [SerializeField] float minHeight;
    [SerializeField] float maxHeight;

    [Space]
    [Header("Weather Simulation")]
    [SerializeField] int bufferSize; // how many weather events the buffer can hold
    [SerializeField] float bias; // how much the weather events are biased towards 0 (1 = uniform distribution, >1 = more events with low values)
    [SerializeField] float hourDuration; // how long a weather event lasts (1 hour = 1 second)
    [SerializeField] public List<float> rainBuffer; // buffer that contains all the weather events

    [SerializeField] AnimationCurve rainCurve;

    [Space]
    [Header("Rain Graphic Effect")]
    [SerializeField] ParticleSystem rainEffect;
    [SerializeField] float rainIncrement; // how much water spawns, only graphic effect

    [Space]
    [Header("UI")]
    [SerializeField] float maxMmPerHour = 60f; // strongest rain = 60 mm/h
    [SerializeField] TMP_Text saturationPercent;
    [SerializeField] TMP_Text rainIntensityValue;
    [SerializeField] TMP_Text rainIntensityWord;
    [SerializeField] TMP_Text previsionValues;
    [SerializeField] TMP_Text secondsPerHour;
    [SerializeField] TMP_Text hoursPassed;


    private void Start()
    {
        secondsPerHour.text = $"1 ora = {hourDuration} secondi";
        loop = RainLoop();

        FillBuffer();
        StartCoroutine(loop);
    }

    private void Update()
    {
        float targetY = Mathf.Lerp(minHeight, maxHeight, terrainSaturation);
        transform.position = new Vector3(transform.position.x, Mathf.Lerp(transform.position.y, targetY, Time.deltaTime), transform.position.z);

        // consider rain impact on saturation
        terrainSaturation = Mathf.Min(terrainSaturation + rainIntensity * saturationIncreaseVelocity * Time.deltaTime, 1f);
        terrainSaturation = Mathf.Max(terrainSaturation, 0f);

        // remove saturation over time
        if (terrainSaturation > 0)
        {
            terrainSaturation -= saturationDecrease * Time.deltaTime;
            if (terrainSaturation < 0)
            {
                terrainSaturation = 0;
            }
        }

        // graphic
        rainEffect.emissionRate = rainIncrement * rainIntensity;
        saturationPercent.text = Mathf.Round(terrainSaturation * 100).ToString() + "%";

        float mmPerHour = rainIntensity * maxMmPerHour;
        rainIntensityValue.text = $"{mmPerHour:0.##} mm/h";

        if(rainIntensity < 0.1f)
        {
            rainIntensityWord.text = "Nulla";
            rainIntensityWord.color = Color.white;
        }
        else if (rainIntensity < 0.5f)
        {
            rainIntensityWord.text = "Leggera";
            rainIntensityWord.color = Color.darkCyan;
        }
        else if (rainIntensity < 0.8f)
        {
            rainIntensityWord.text = "Moderata";
            rainIntensityWord.color = Color.yellow;
        }
        else
        {
            rainIntensityWord.text = "Pesante";
            rainIntensityWord.color = Color.red;
        }
    }

    IEnumerator RainLoop()
    {
        // loop che determina la variazione di potenza della pioggia nel tempo

        float hours = 0;
        while (true)
        {
            hoursPassed.text = $"Ore passate: {hours}";
            UpdatePrevisionUI();

            float timer = hourDuration;
            while (timer > 0f)
            {
                timer-=Time.deltaTime;
                // la potenza è interpolata tra i due valori nel buffer utilizzando la curva di animazione
                rainIntensity = rainBuffer[0] + (rainBuffer[1] - rainBuffer[0]) * rainCurve.Evaluate((hourDuration-timer)/hourDuration);
                yield return null;
            }
            rainBuffer.RemoveAt(0);
            if(rainBuffer.Count < bufferSize) FillBuffer();

            hours += 1f;
        }
    }

    void UpdatePrevisionUI()
    {
        // update the prevision UI with the next 5 values in the buffer
        previsionValues.text = "";
        for (int i = 0; i < rainBuffer.Count; i++)
        {
            float mmPerHour = rainBuffer[i] * maxMmPerHour;
            previsionValues.text += $"~{mmPerHour:0.##} mm, ";
        }
        previsionValues.text = previsionValues.text.Remove(previsionValues.text.Length - 2);
    }    

    void FillBuffer()
    {
        // fill the buffer with random values between 0 and 1, biased towards 0

        for (int i = rainBuffer.Count; i <= bufferSize; i++)
        {
            double uniform = UnityEngine.Random.value;
            double biased = System.Math.Pow(uniform, bias); // bias towards 0
            biased = (biased < 0) ? 0 : biased;
            rainBuffer.Add((float)biased);
        }
    }
}