using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shrink : MonoBehaviour
{
    public float timeScaleUp = 1.25f;
    public float timeScaleDown = 1.25f;

    public float maximumX = 1f;
    public float maximumY = 1f;
    public float maximumZ = 1f;

    private Vector3 changingVector3;
    private float minimum;
    private float currentX;
    private float currentY;
    private float currentZ;
    private float t1;
    private float t2;

    // Start is called before the first frame update
    void Start()
    {
        transform.localScale = gameObject.transform.localScale;
        currentX = transform.localScale.x;
        currentY = transform.localScale.y;
        currentZ = transform.localScale.z;
        minimum = 0f;
        t1 = 0f;
        t2 = 0f;
    }

    // Update is called once per frame
    void Update()
    {
        if (t1 > 1f)
        {
            currentX = transform.localScale.x;
            currentY = transform.localScale.y;
            currentZ = transform.localScale.z;
            Invoke("DoShrink", 0f);
        }
        else
        {
            changingVector3.x = Mathf.Lerp(currentX, maximumX, t1);
            changingVector3.y = Mathf.Lerp(currentY, maximumY, t1);
            changingVector3.z = Mathf.Lerp(currentZ, maximumZ, t1);

            transform.localScale = changingVector3;
            t1 += timeScaleUp * Time.deltaTime;
        }

        if (transform.localScale.x <= 0f)
        {
            CancelInvoke();
            gameObject.SetActive(false);
        }
    }

    private void DoShrink()
    {
        changingVector3.x = Mathf.Lerp(currentX, minimum, t2);
        changingVector3.y = Mathf.Lerp(currentY, minimum, t2);
        changingVector3.z = Mathf.Lerp(currentZ, minimum, t2);

        transform.localScale = changingVector3;
        t2 += timeScaleDown * Time.deltaTime;
    }
}