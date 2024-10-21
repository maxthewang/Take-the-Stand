using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InterrogationLight : MonoBehaviour
{
    // Start is called before the first frame update
    int trust = 5;
    Light light;
    void Start()
    {
        light = GetComponent<Light>();
        light.color.red = 127.5;
        light.color.blue = 127.5;
    }

    // Update is called once per frame
    void Update()
    {
        trust = GameManager.instance.GetTrust();
        if (trust == 0) {
            light.color.red = 255;
            light.color.blue = 0;
        } else if (trust == 1) {
            light.color.red = 229.5;
            light.color.blue = 25.5;
        }
        else if (trust == 2) {
            light.color.red = 204;
            light.color.blue = 51;
        }
        else if (trust == 3) {
            light.color.red = 178.5;
            light.color.blue = 76.5;
        }
        else if (trust == 4) {
            light.color.red = 153;
            light.color.blue = 102;
        }
        else if (trust == 5) {
            light.color.red = 127.5;
            light.color.blue = 127.5;
        }
        else if (trust == 6) {
            light.color.red = 102;
            light.color.blue = 153;
        }
        else if (trust == 7) {
            light.color.red = 76.5;
            light.color.blue = 178.5;
        }
        else if (trust == 8) {
            light.color.red = 51;
            light.color.blue = 204;
        }
        else if (trust == 9) {
            light.color.red = 25.5;
            light.color.blue = 229.5;
        } else (trust == 10) {
            light.color.red = 0;
            light.color.blue = 255;
        }
    }
}
