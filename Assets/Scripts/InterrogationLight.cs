using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InterrogationLight : MonoBehaviour
{
    // Start is called before the first frame update
    int trust = 5;
    Light light;
    Color color0 = Color.red;
    Color color1 = Color.blue;
    void Start()
    {
        light = GetComponent<Light>();
        light.color = Color.Lerp(color0, color1, trust / 10);
    }

    // Update is called once per frame
    void Update()
    {
        trust = GameManager.instance.GetTrust();
        light.color = Color.Lerp(color0, color1, trust / 10);
    }
}
