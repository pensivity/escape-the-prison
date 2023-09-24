using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class itemBounce_script : MonoBehaviour
{
    private float speed = 1f;
    private float xRotation;
    private float yRotation;
    private float zRotation;
    private float yCenter;

    // Start is called before the first frame update
    void Start()
    {
        yCenter = transform.position.y;
    }

    private void Update()
    {
        transform.position = new Vector3(transform.position.x, yCenter + Mathf.PingPong(Time.time * speed, 1f), transform.position.z);
        transform.Rotate(new Vector3(xRotation, yRotation, zRotation) * Time.deltaTime);
    }
}
