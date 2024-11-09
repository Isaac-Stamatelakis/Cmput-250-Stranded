using UnityEngine;

public class FirelightEffect : MonoBehaviour
{
    public Light fireLight; // Assign your Directional Light here
    public float minIntensity = 0.8f;
    public float maxIntensity = 1.2f;
    public float flickerSpeed = 0.1f;

    private float targetIntensity;
    private float currentIntensity;

    void Start()
    {
        if (fireLight == null)
            fireLight = GetComponent<Light>();

        currentIntensity = fireLight.intensity;
        targetIntensity = Random.Range(minIntensity, maxIntensity);
    }

    void Update()
    {
        // Smoothly interpolate to the target intensity to create a flicker effect
        currentIntensity = Mathf.MoveTowards(currentIntensity, targetIntensity, flickerSpeed * Time.deltaTime);
        fireLight.intensity = currentIntensity;

        // Change target intensity to create the flicker effect
        if (Mathf.Approximately(currentIntensity, targetIntensity))
        {
            targetIntensity = Random.Range(minIntensity, maxIntensity);
        }
    }
}
