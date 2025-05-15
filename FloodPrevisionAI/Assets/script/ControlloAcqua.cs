using System.Collections;
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
    [SerializeField] public float rainWeightPower;

    const float terrainSaturationMax = 1f;
    float lastSaturation;

    private void Start()
    {
        lastSaturation = terrainSaturation;
        minHeight = MINHEIGHT;
        maxHeight = MAXHEIGHT;

        loop = WaterLoop();
        StartCoroutine(loop);
    }

    private void Update()
    {
        // change height window based on saturation
        if(terrainSaturation != lastSaturation)
        {
            minHeight += (terrainSaturation - lastSaturation) * MINHEIGHT * 5;
            maxHeight += (terrainSaturation - lastSaturation) * MAXHEIGHT;

            lastSaturation = terrainSaturation;
        }

        // consider rain impact on saturation
        terrainSaturation = Mathf.Min(terrainSaturation + rainWeight * rainWeightPower * Time.deltaTime, terrainSaturationMax);

        // remove saturation over time
        if (terrainSaturation > 0)
        {
            terrainSaturation -= decrementSpeed * Time.deltaTime;
            if (terrainSaturation < 0)
            {
                terrainSaturation = 0;
            }
        }
    }

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
