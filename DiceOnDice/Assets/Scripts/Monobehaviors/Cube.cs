using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using TMPro;

public class Cube : MonoBehaviour
{
    public enum RotationDirection
    {
        Up,
        Left,
        Right,
        Down,
        Null
    }
    private RotationDirection rotationDirection;

    public GameObject cubeFace1;
    public GameObject cubeFace2;
    public GameObject cubeFace3;
    public GameObject cubeFace4;
    public GameObject cubeFace5;
    public GameObject cubeFace6;

    private Quaternion cubeFace1TextRotation;
    private Quaternion cubeFace2TextRotation;
    private Quaternion cubeFace3TextRotation;
    private Quaternion cubeFace4TextRotation;
    private Quaternion cubeFace5TextRotation;
    private Quaternion cubeFace6TextRotation;

    private TextMeshPro cubeFace1Text;
    private TextMeshPro cubeFace2Text;
    private TextMeshPro cubeFace3Text;
    private TextMeshPro cubeFace4Text;
    private TextMeshPro cubeFace5Text;
    private TextMeshPro cubeFace6Text;

    private List<GameObject> cubeFaceOrder;

    private bool isBeingInteractedWith;
    private bool isRotating;
    private Vector2 mouseDownPos;
    private Vector2 mouseUpPos;
    private Vector3 currentRotation;
    private Vector3 newRotation;
    private float rotationDuration = 0.75f;
    private bool onRightSide;
    private bool cachedSide;

    void Start()
    {
        rotationDirection = RotationDirection.Null;
        cubeFaceOrder = new List<GameObject>();
        cubeFaceOrder.Add(cubeFace1);
        cubeFaceOrder.Add(cubeFace2);
        cubeFaceOrder.Add(cubeFace3);
        cubeFaceOrder.Add(cubeFace4);
        cubeFaceOrder.Add(cubeFace5);
        cubeFaceOrder.Add(cubeFace6);

        GetCubeFaceText();
    }

    void Update()
    {
        float distanceToPlayer = Vector3.Distance(Camera.main.transform.position, transform.position);
        Vector3 cameraPoint = Input.mousePosition;
        cameraPoint.z = distanceToPlayer;
        Vector3 worldPoint = Camera.main.ScreenToWorldPoint(cameraPoint);
        if (worldPoint.x < transform.localPosition.x)
            onRightSide = false;
        else
            onRightSide = true;
    }

    void LateUpdate()
    {
        if (isRotating && rotationDirection == RotationDirection.Right || rotationDirection == RotationDirection.Left)
        {
            cubeFace5Text.transform.rotation = cubeFace5TextRotation;
            cubeFace6Text.transform.rotation = cubeFace6TextRotation;
        }
        else if (isRotating && cachedSide && rotationDirection == RotationDirection.Up || rotationDirection == RotationDirection.Down)
        {
            cubeFace1Text.transform.rotation = cubeFace1TextRotation;
            cubeFace3Text.transform.rotation = cubeFace3TextRotation;
        } 
        else if (isRotating && !cachedSide && rotationDirection == RotationDirection.Up || rotationDirection == RotationDirection.Down)
        {
            cubeFace2Text.transform.rotation = cubeFace2TextRotation;
            cubeFace4Text.transform.rotation = cubeFace4TextRotation;
        }

        if (!isRotating)
        {
            cubeFace1Text.transform.LookAt(transform.position);
            cubeFace1Text.transform.Rotate(new Vector3(0f, 0f, 0f), 90f);

            cubeFace2Text.transform.LookAt(transform.position);
            cubeFace2Text.transform.Rotate(new Vector3(0f, 0f, 0f), 90f);

            cubeFace3Text.transform.LookAt(transform.position);
            cubeFace3Text.transform.Rotate(new Vector3(0f, 0f, 0f), 90f);

            cubeFace4Text.transform.LookAt(transform.position);
            cubeFace4Text.transform.Rotate(new Vector3(0f, 0f, 0f), 90f);

            cubeFace5Text.transform.LookAt(transform.position);
            cubeFace5Text.transform.Rotate(new Vector3(0f, 0f, 0f), 90f);

            cubeFace6Text.transform.LookAt(transform.position);
            cubeFace6Text.transform.Rotate(new Vector3(0f, 0f, 0f), 90f);
        }
    }

    private void OnMouseEnter()
    {
        isBeingInteractedWith = true;
    }

    private void OnMouseDown()
    {
        if (isBeingInteractedWith) mouseDownPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
    }

    private void OnMouseUp()
    {
        if (isBeingInteractedWith)
        {
            mouseUpPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            CalculateRotationDirection();
            if (!isRotating) RotateCube();
        }
    }

    private void OnMouseExit()
    {
        isBeingInteractedWith = false;
        mouseDownPos = Vector2.zero;
        mouseUpPos = Vector2.zero;
    }

    private void CalculateRotationDirection()
    {
        Vector2 directionVector = (mouseUpPos - mouseDownPos).normalized;

        if (Mathf.Abs(directionVector.x) >= Mathf.Abs(directionVector.y))
        {
            if (directionVector.x > 0) rotationDirection = RotationDirection.Right;
            else rotationDirection = RotationDirection.Left;
        }
        else
        {
            if (directionVector.y > 0) rotationDirection = RotationDirection.Up;
            else rotationDirection = RotationDirection.Down;
        }
    }

    private void RotateCube()
    {
        isRotating = true;

        switch (rotationDirection)
        {
            case RotationDirection.Right:
                currentRotation = transform.localEulerAngles;
                newRotation = transform.localEulerAngles;
                newRotation.y = newRotation.y - 90f;
                transform.DOLocalRotate(newRotation, rotationDuration, RotateMode.FastBeyond360).SetEase(Ease.OutCubic);
                break;
            case RotationDirection.Left:
                currentRotation = transform.localEulerAngles;
                newRotation = transform.localEulerAngles;
                newRotation.y = newRotation.y + 90f;
                transform.DOLocalRotate(newRotation, rotationDuration, RotateMode.FastBeyond360).SetEase(Ease.OutCubic);
                break;
            case RotationDirection.Up:
                if (onRightSide)
                {
                    transform.DOLocalRotate(new Vector3(90f, 0f, 0f), rotationDuration, RotateMode.WorldAxisAdd).SetEase(Ease.OutCubic);
                }
                else
                {
                    transform.DOLocalRotate(new Vector3(0f, 0f, -90f), rotationDuration, RotateMode.WorldAxisAdd).SetEase(Ease.OutCubic);
                }
                break;
            case RotationDirection.Down:
                if (onRightSide)
                {
                    transform.DOLocalRotate(new Vector3(-90f, 0f, 0f), rotationDuration, RotateMode.WorldAxisAdd).SetEase(Ease.OutCubic);
                }
                else
                {
                    transform.DOLocalRotate(new Vector3(0f, 0f, 90f), rotationDuration, RotateMode.WorldAxisAdd).SetEase(Ease.OutCubic);
                }
                break;
        }

        if (onRightSide) cachedSide = true;
        else cachedSide = false;

        UpdateFacePositions();

        GetCubeFaceText();

        StartCoroutine(ResetRotationBool());
    }

    private void UpdateFacePositions()
    {
        GameObject previous1 = cubeFaceOrder[0];
        GameObject previous2 = cubeFaceOrder[1];
        GameObject previous3 = cubeFaceOrder[2];
        GameObject previous4 = cubeFaceOrder[3];
        GameObject previous5 = cubeFaceOrder[4];
        GameObject previous6 = cubeFaceOrder[5];

        switch (rotationDirection)
        {
            case RotationDirection.Right:  
                cubeFaceOrder.Clear();
                cubeFaceOrder.Add(previous4);
                cubeFaceOrder.Add(previous1);
                cubeFaceOrder.Add(previous2);
                cubeFaceOrder.Add(previous3);
                cubeFaceOrder.Add(previous5);
                cubeFaceOrder.Add(previous6);
                break;
            case RotationDirection.Left:
                cubeFaceOrder.Clear();
                cubeFaceOrder.Add(previous2);
                cubeFaceOrder.Add(previous3);
                cubeFaceOrder.Add(previous4);
                cubeFaceOrder.Add(previous1);
                cubeFaceOrder.Add(previous5);
                cubeFaceOrder.Add(previous6);
                break;
            case RotationDirection.Up:
                if (onRightSide)
                {
                    cubeFaceOrder.Clear();
                    cubeFaceOrder.Add(previous1);
                    cubeFaceOrder.Add(previous6);
                    cubeFaceOrder.Add(previous3);
                    cubeFaceOrder.Add(previous5);
                    cubeFaceOrder.Add(previous2);
                    cubeFaceOrder.Add(previous4);
                }
                else
                {
                    cubeFaceOrder.Clear();
                    cubeFaceOrder.Add(previous6);
                    cubeFaceOrder.Add(previous2);
                    cubeFaceOrder.Add(previous5);
                    cubeFaceOrder.Add(previous4);
                    cubeFaceOrder.Add(previous1);
                    cubeFaceOrder.Add(previous3);
                }
                break;
            case RotationDirection.Down:
                if (onRightSide)
                {
                    cubeFaceOrder.Clear();
                    cubeFaceOrder.Add(previous1);
                    cubeFaceOrder.Add(previous5);
                    cubeFaceOrder.Add(previous3);
                    cubeFaceOrder.Add(previous6);
                    cubeFaceOrder.Add(previous4);
                    cubeFaceOrder.Add(previous2);
                }
                else
                {
                    cubeFaceOrder.Clear();
                    cubeFaceOrder.Add(previous5);
                    cubeFaceOrder.Add(previous2);
                    cubeFaceOrder.Add(previous6);
                    cubeFaceOrder.Add(previous4);
                    cubeFaceOrder.Add(previous3);
                    cubeFaceOrder.Add(previous1);
                }
                break;
        }
    }

    private void GetCubeFaceText()
    {
        cubeFace1Text = cubeFaceOrder[0].GetComponentInChildren<TextMeshPro>();
        cubeFace1TextRotation = cubeFace1Text.transform.rotation;

        cubeFace2Text = cubeFaceOrder[1].GetComponentInChildren<TextMeshPro>();
        cubeFace2TextRotation = cubeFace2Text.transform.rotation;

        cubeFace3Text = cubeFaceOrder[2].GetComponentInChildren<TextMeshPro>();
        cubeFace3TextRotation = cubeFace3Text.transform.rotation;

        cubeFace4Text = cubeFaceOrder[3].GetComponentInChildren<TextMeshPro>();
        cubeFace4TextRotation = cubeFace4Text.transform.rotation;

        cubeFace5Text = cubeFaceOrder[4].GetComponentInChildren<TextMeshPro>();
        cubeFace5TextRotation = cubeFace5Text.transform.rotation;

        cubeFace6Text = cubeFaceOrder[5].GetComponentInChildren<TextMeshPro>();
        cubeFace6TextRotation = cubeFace6Text.transform.rotation;
    }

    private IEnumerator ResetRotationBool()
    {
        yield return new WaitForSeconds(rotationDuration);

        isRotating = false;
        rotationDirection = RotationDirection.Null;
    }
}
