using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InterrogationLight : MonoBehaviour
{
    // Start is called before the first frame update
    int trust = 0;
    private Light lightComponent;
    Color color0 = Color.red;
    Color color1 = Color.cyan;
    void Start()
    {
        lightComponent = GetComponent<Light>();
        if (lightComponent != null)
        {
            // Set the initial light color based on the trust value
            lightComponent.color = Color.Lerp(color0, color1, (trust + 15) / 30f);
        }
        else
        {
            Debug.LogError("No Light component found on this GameObject.");
        }
    }

    // Update is called once per frame
    void Update()
    {
        trust = GameManager.instance.GetTrust();
        if (lightComponent != null)
        {
            lightComponent.color = Color.Lerp(color0, color1, (trust + 15) / 30f);
        }
    }
}
