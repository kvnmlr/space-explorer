using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotate : MonoBehaviour
{
    public float speed = 10;
    public bool clockwise = true;


    // Use this for initialization
    void Start()
    {
        if (!clockwise)
        {
            speed *= -1;
        }
        transform.localEulerAngles = new Vector3(0, 180, Random.Range(0f, 360.0f));
    }

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(new Vector3(0, 0, speed * Time.deltaTime * 5), Space.Self);
        //transform.RotateAround(transform.position, transform.up, Time.deltaTime * speed);

        //transform.Rotate((clockwise ? transform.forward * -1 : transform.forward), speed * Time.deltaTime);

    }
}

