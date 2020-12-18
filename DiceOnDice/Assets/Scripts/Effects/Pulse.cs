using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pulse : MonoBehaviour
{
    public float scale = 1f;
    public float timeScale = 1f;

    private Vector3 changingVector3;
    private float minimum;
    private float maximum;
    private float t;

    private void Start()
    {
        transform.localScale = new Vector3(scale, scale, scale);
        minimum = scale - 0.05f;
        maximum = scale + 0.05f;
        t = 0f;
    }

    private void Update()
    {
        changingVector3.x = Mathf.Lerp(minimum, maximum, t);
        changingVector3.y = Mathf.Lerp(minimum, maximum, t);
        changingVector3.z = Mathf.Lerp(minimum, maximum, t);

        transform.localScale = changingVector3;
        t += timeScale * Time.deltaTime;

        if (t > 1.0f)
        {
            float temp = maximum;
            maximum = minimum;
            minimum = temp;
            t = 0.0f;
        }
    }
}