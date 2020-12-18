using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class CameraExtensions
{
    #region Variables
    public static Camera cam;

    public static Vector3 leftBorder;
    public static Vector3 rightBorder;
    public static Vector3 topBorder;
    public static Vector3 botBorder;

    public static Bounds cameraBounds;

    private static float cachedCameraSize;
    private static Vector3 cachedCameraPosition;

    public enum OffScreen
    {
        Right,
        Left,
        Top,
        Bottom
    }
    #endregion

    #region LifeCycle Management
    /*
     *  Initializes all of public variables and stores the scene's camera
     */
    public static void Initialize(this Camera camera)
    {
        cam = camera;
        UpdateBorders(cam);
    }

    /*
     *  Have this function running in whichever loop you're expecting a camera change
     */
    public static void Update()
    {
        if (BordersChanged(cam)) UpdateBorders(cam);
    }
    #endregion

    #region Camera Functions
    /*
     *  Updates all of the camera borders and caches the current position and orthographic size
     */
    public static void UpdateBorders(this Camera camera)
    {
        float screenAspect = (float)Screen.width / (float)Screen.height;
        float cameraHeight = camera.orthographicSize * 2f;

        cachedCameraPosition = camera.transform.position;
        cachedCameraSize = camera.orthographicSize;

        Bounds bounds = new Bounds(camera.transform.position, new Vector3(cameraHeight * screenAspect, cameraHeight, 0));

        cameraBounds = bounds;

        leftBorder = new Vector3(bounds.min.x, 0f, 0f);
        rightBorder = new Vector3(bounds.max.x, 0f, 0f);
        topBorder = new Vector3(0f, bounds.max.y, 0f);
        botBorder = new Vector3(0f, bounds.min.y, 0f);
    }

    /*
     *  Checks if either the camera's position or orthographic size has changed
     */
    public static bool BordersChanged(this Camera camera)
    {
        if (camera.transform.position != cachedCameraPosition || camera.orthographicSize != cachedCameraSize) return true;
        else return false;
    }

    public static void LerpOffScreen(GameObject gameObject, OffScreen direction, float magnitude = 1.5f)
    {
        var tempPos = gameObject.transform.position;

        switch (direction)
        {
            case OffScreen.Left:
                tempPos.x = Mathf.Lerp(tempPos.x, leftBorder.x * magnitude, Time.deltaTime);
                gameObject.transform.position = tempPos;
                break;
            case OffScreen.Right:
                tempPos.x = Mathf.Lerp(tempPos.x, rightBorder.x * magnitude, Time.deltaTime);
                gameObject.transform.position = tempPos;
                break;
            case OffScreen.Top:
                tempPos.y = Mathf.Lerp(tempPos.y, topBorder.y * magnitude, Time.deltaTime);
                gameObject.transform.position = tempPos;
                break;
            case OffScreen.Bottom:
                tempPos.y = Mathf.Lerp(tempPos.y, botBorder.y * magnitude, Time.deltaTime);
                gameObject.transform.position = tempPos;
                break;
        }
    }

    public static IEnumerator Shake(float duration, float magnitude)
    {
        Vector3 orignalPosition = cam.transform.position;
        float elapsed = 0f;

        while (elapsed < duration)
        {
            float x = Random.Range(-1f, 1f) * magnitude;
            float y = Random.Range(-1f, 1f) * magnitude;

            cam.transform.position = new Vector3(x, y, -10f);
            elapsed += Time.deltaTime;
            yield return 0;
        }
        cam.transform.position = Vector3.MoveTowards(cam.transform.position, orignalPosition, Time.deltaTime);
    }
    #endregion
}
