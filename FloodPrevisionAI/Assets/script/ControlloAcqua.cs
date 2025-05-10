using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class ControlloAcqua : MonoBehaviour
{
    [SerializeField] float changeDelay = 3f;
    [SerializeField] float animDuration = 1f;
    [SerializeField] float minHeightDry = 1f;
    [SerializeField] float minHeightSaturated = 1f;
    float minHeight;
    [SerializeField] float maxHeightDry = 1f;
    [SerializeField] float maxHeightSaturated = 1f;
    float maxHeight;
    [SerializeField] AnimationCurve animCurve;
    float animTimer;
    IEnumerator loop;

    [Space]

    [SerializeField] float terrainSaturation;
    
    private void Start()
    {
        loop = WaterLoop();
        StartCoroutine(loop);
    }

    private void Update()
    {

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
