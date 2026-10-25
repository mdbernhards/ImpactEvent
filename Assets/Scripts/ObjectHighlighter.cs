using UnityEngine;

public class ObjectHighlighter : MonoBehaviour
{
    [Header("Outline Settings")]
    public Material outlineMaterial;
    public float outlineScale = 1.03f;

    private GameObject outlineRoot;
    private bool isHighlighted;

    void Awake()
    {
        CreateOutline();
        SetHighlight(false);
    }

    void CreateOutline()
    {
        outlineRoot = new GameObject("OutlineRoot");
        outlineRoot.transform.SetParent(transform);
        outlineRoot.transform.localPosition = Vector3.zero;
        outlineRoot.transform.localRotation = Quaternion.identity;
        outlineRoot.transform.localScale = Vector3.one;

        MeshFilter[] meshFilters = GetComponentsInChildren<MeshFilter>();

        foreach (var mf in meshFilters)
        {
            if (mf.sharedMesh == null) continue;

            GameObject go = new GameObject("OutlineMesh");
            go.transform.SetParent(outlineRoot.transform);

            go.transform.position = mf.transform.position;
            go.transform.rotation = mf.transform.rotation;
            go.transform.localScale = mf.transform.lossyScale * outlineScale;

            var newMF = go.AddComponent<MeshFilter>();
            newMF.sharedMesh = mf.sharedMesh;

            var rend = go.AddComponent<MeshRenderer>();
            rend.material = outlineMaterial;
            rend.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;
            rend.receiveShadows = false;
        }
    }

    public void SetHighlight(bool state)
    {
        isHighlighted = state;

        if (outlineRoot != null)
            outlineRoot.SetActive(state);
    }

    public void Toggle()
    {
        SetHighlight(!isHighlighted);
    }
}