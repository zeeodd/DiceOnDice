using UnityEngine;
using TMPro;

public class CubeFace : MonoBehaviour
{
    private TextMeshPro faceText;

    void Awake()
    {
        faceText = GetComponentInChildren<TextMeshPro>();
        gameObject.name = faceText.text;
    }
}
