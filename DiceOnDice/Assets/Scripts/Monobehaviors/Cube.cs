﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

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

    private List<GameObject> cubeFaceOrder;

    private bool isBeingInteractedWith;
    private bool isRotating;
    private Vector2 mouseDownPos;
    private Vector2 mouseUpPos;
    private Vector3 currentRotation;
    private Vector3 newRotation;
    private float rotationDuration = 0.75f;

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

        Debug.Log("Initial Order:");
        foreach(GameObject cube in cubeFaceOrder)
        {
            Debug.Log(cube.name);
        }
    }

    void Update()
    {
        
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
            rotationDirection = RotationDirection.Null;
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

        Debug.Log(rotationDirection);
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
        }

        UpdateFacePositions();

        StartCoroutine(ResetRotationBool());
    }

    private void UpdateFacePositions()
    {
        switch (rotationDirection)
        {
            case RotationDirection.Right:
                GameObject previous1 = cubeFaceOrder[0];
                GameObject previous2 = cubeFaceOrder[1];
                GameObject previous3 = cubeFaceOrder[2];
                GameObject previous4 = cubeFaceOrder[3];
                cubeFaceOrder[3] = previous3;
                cubeFaceOrder[2] = previous2;
                cubeFaceOrder[1] = previous1;
                cubeFaceOrder[0] = previous4;
                break;
            case RotationDirection.Left:
                
                break;
        }

        Debug.Log("New Order:");
        foreach (GameObject cube in cubeFaceOrder)
        {
            Debug.Log(cube.name);
        }
    }

    private IEnumerator ResetRotationBool()
    {
        yield return new WaitForSeconds(rotationDuration);

        isRotating = false;
    }
}
