using UnityEngine;
using TMPro;

public class AddressSign : MonoBehaviour
{
    [Header("Address Settings")]
    [TextArea]
    public string addressText;

    [Header("References")]
    public TextMeshPro textMesh;

    void Start()
    {
        UpdateText();
    }

    void OnValidate()
    {
        UpdateText();
    }

    void UpdateText()
    {
        if (textMesh != null)
        {
            textMesh.text = addressText;
        }
    }
}