using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Respawn : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Vector3 respawn_point = new Vector3 (2.52, 1.388, 2.35);
    }

    // Update is called once per frame
    void Update()
    {
        if (transform.position.y < -10) {
            transform.position = respawn_point;
        }
    }
}
