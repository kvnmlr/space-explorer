using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotate : MonoBehaviour
{
    public float speed = 10;
    public bool clockwise = true;

    private int time;


    // Use this for initialization
    void Start()
    {
        transform.rotation = new Quaternion(0, 0, Random.rotation.z, 1);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

