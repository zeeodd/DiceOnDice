using UnityEngine;
using System.Collections;

public class Float : MonoBehaviour
{
    public float degreesPerSecond = 0f;
    public float amplitude = 0.5f;
    public float frequency = 1f;
    Vector3 posOffset = new Vector3();
    Vector3 tempPos = new Vector3();

    public bool isStatic = true;
    public bool x;
    public bool y;
    public bool z;

    void Start()
    {
        posOffset = transform.position;
    }

    void FixedUpdate()
    {

        if (isStatic)
        {
            transform.Rotate(new Vector3(0f, Time.deltaTime * degreesPerSecond, 0f), Space.World);

            tempPos = posOffset;
            if (x) tempPos.x += Mathf.Sin(Time.fixedTime * Mathf.PI * frequency) * amplitude;
            if (y) tempPos.y += Mathf.Sin(Time.fixedTime * Mathf.PI * frequency) * amplitude;
            if (z) tempPos.z += Mathf.Sin(Time.fixedTime * Mathf.PI * frequency) * amplitude;

            transform.position = tempPos;
        }
        else
        {
            transform.Rotate(new Vector3(0f, 0f, (Time.deltaTime * degreesPerSecond)), Space.World);

            var temp = transform.position;

            if (x) temp.x += Mathf.Sin(Time.fixedTime * Mathf.PI * frequency) * amplitude;
            if (y) temp.y += Mathf.Sin(Time.fixedTime * Mathf.PI * frequency) * amplitude;
            if (z) temp.z += Mathf.Sin(Time.fixedTime * Mathf.PI * frequency) * amplitude;

            transform.position = temp;
        }
    }
}