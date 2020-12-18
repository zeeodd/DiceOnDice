using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldUIController
{
    public Camera cam;
    public Canvas canvas;

    public void Initialize(Canvas mainCanvas, Camera camera)
    {
        cam = camera;
        canvas = mainCanvas;
    }

    public void BuildWorldUIElement(GameObject anchor, float verticalOffset)
    {
        Vector3 v3 = anchor.transform.position;

        Vector2 ViewportPosition = cam.WorldToViewportPoint(v3);
        Vector2 WorldObject_ScreenPosition = new Vector2(((ViewportPosition.x * canvas.GetComponent<RectTransform>().sizeDelta.x) - (canvas.GetComponent<RectTransform>().sizeDelta.x * 0.5f)),
                                                         ((ViewportPosition.y * canvas.GetComponent<RectTransform>().sizeDelta.y) - (canvas.GetComponent<RectTransform>().sizeDelta.y * 0.5f)));

        var worlduielement = Object.Instantiate(Resources.Load<GameObject>("Prefabs/Image"));
        worlduielement.transform.SetParent(canvas.transform);
        worlduielement.GetComponent<RectTransform>().anchoredPosition = WorldObject_ScreenPosition;
        worlduielement.transform.localRotation = Quaternion.identity;
        worlduielement.transform.localScale = new Vector3(3f, 3f, 3f);

        Vector3 tempPos = worlduielement.transform.localPosition;
        tempPos.z = 0f;
        tempPos.y += verticalOffset;
        worlduielement.transform.localPosition = tempPos;
    }
}
