using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class ControlloAcqua : MonoBehaviour
{
    [SerializeField] float changeDelay = 3f;
    [SerializeField] float animDuration = 1f;
    [SerializeField] float MINHEIGHT;
    [SerializeField] float MAXHEIGHT;

    [Space]
    [SerializeField] AnimationCurve animCurve;
    [SerializeField] float animTimer;
    IEnumerator loop;

    [Space]
    [SerializeField] public float terrainSaturation;
    [SerializeField] float decrementSpeed = 2.5f;
    [SerializeField] float minHeight;
    [SerializeField] float maxHeight;

    [Space]
    [SerializeField] public float rainWeight;
    [Range(0.0f, 1.0f)]
    [SerializeField] public float rainWeightPower;

    const float terrainSaturationMax = 1f;
    float lastSaturation;

    [Space]
    [Header("Rain Effect")]
    [SerializeField] ParticleSystem rainEffect;
    [SerializeField] float rainIncrement;

    [Space]
    [Header("UI")]
    [SerializeField] TMP_Text saturationPercent;

    
    [Space]
    [Header("Weather Simulation")]
    [SerializeField] float hourDuration = 1f;
    [SerializeField] AnimationCurve rainCurve;
    [SerializeField] List<float> rainBuffer;
    [SerializeField] int bufferSize = 5;
    [Range(0.0f, 1.0f)]
    [SerializeField] float rainThreshold = 0.5f; //il valore sotto il quale il tempo è considerato senza pioggia


    private void Start()
    {
        lastSaturation = terrainSaturation;
        minHeight = MINHEIGHT;
        maxHeight = MAXHEIGHT;

        loop = RainLoop();

        FillBuffer();
        StartCoroutine(loop);
    }

    private void Update()
    {
        // change height window based on saturation
        if(true)
        {
            /*
            minHeight += (terrainSaturation - lastSaturation) * MINHEIGHT * 5;
            maxHeight += (terrainSaturation - lastSaturation) * MAXHEIGHT;
            */
            float targetY = Mathf.Lerp(minHeight, maxHeight, terrainSaturation);
            transform.position = new Vector3(transform.position.x, Mathf.Lerp(transform.position.y, targetY, Time.deltaTime), transform.position.z);

            lastSaturation = terrainSaturation;
        }

        // consider rain impact on saturation
        terrainSaturation = Mathf.Min(terrainSaturation + rainWeight * rainWeightPower * Time.deltaTime, terrainSaturationMax);
        terrainSaturation = Mathf.Max(terrainSaturation, 0f);

        // remove saturation over time
        if (terrainSaturation > 0)
        {
            terrainSaturation -= decrementSpeed * Time.deltaTime;
            if (terrainSaturation < 0)
            {
                terrainSaturation = 0;
            }
        }


        //effetti grafici
        rainEffect.emissionRate = rainIncrement * rainWeightPower;
        saturationPercent.text = Mathf.Round(terrainSaturation * 100).ToString() + "%";
    }

    IEnumerator RainLoop()
    {
        //loop che determina la variazione di potenza della pioggia nel tempo
        while (true)
        {
            float timer = hourDuration;
            while (timer > 0f)
            {
                timer-=Time.deltaTime;
                //la potenza è interpolata tra i due valori nel buffer utilizzando la curva di animazione
                rainWeightPower = rainBuffer[0] + (rainBuffer[1] - rainBuffer[0]) * rainCurve.Evaluate((hourDuration-timer)/hourDuration);
                yield return null;
            }
            rainBuffer.RemoveAt(0);
            if( rainBuffer.Count < bufferSize) FillBuffer();
        }
    }

    void FillBuffer()
    {
        //riempie il buffer delle previsioni con piogge casuali

        for (int i = 0; i < bufferSize; i++)
        {
            float res = Random.Range(0f, 1f); //rain weight power casuale tra 0 e 1
            //il perlin noise non funzionava ma può essere riprovato per valori più coerenti
            rainBuffer.Add(res > rainThreshold ? res : 0);
        }
    }


    //inutilizzato
    IEnumerator WaterLoop()
    {
        while (true)
        {

            float startHeight = transform.position.y;
            float newHeight = Random.Range(minHeight, maxHeight);
            Debug.Log(newHeight);
            float diff = newHeight - startHeight;
            animTimer = 0f;
            while (animTimer < animDuration)
            {
                animTimer += Time.deltaTime;
                transform.position = new Vector3 (transform.position.x, startHeight + diff*animCurve.Evaluate(animTimer/animDuration), transform.position.z);
                yield return null;
            }
            yield return new WaitForSeconds(changeDelay);

        }
    }
}