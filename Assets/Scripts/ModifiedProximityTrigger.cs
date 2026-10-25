using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ModifiedProximityTrigger : MonoBehaviour
{
    public enum ActivationMode { Hold, Instant }
    public enum RepeatMode { Once, Repeatable }

    [Header("Activation Settings")]
    public ActivationMode activationMode = ActivationMode.Hold;
    public RepeatMode repeatMode = RepeatMode.Once;
    public float longPressDuration = 2f;

    private Material material;
    private float progress = 0f;
    private bool currentActivationMode = false;
    private bool isCurrentlyVisible = false;
    private bool isCurrentlyPressed = false;
    private SpriteRenderer spriteRenderer;

    private bool isActivated = false;
    private bool hasTriggered = false;

    private Transform originalParent = null;

    public GameObject targetPlayerActivatable;
    public Vector3 offset = Vector3.zero;

    private Transform proximityRoot = null;

    // 🔥 NEW: highlight reference
    private ObjectHighlighter highlighter;

    void Start()
    {
        proximityRoot = FindFirstObjectByType<ProximityTriggerRoot>()?.transform;

        if (proximityRoot == null)
        {
            Debug.LogWarning("No ProximityTriggerRoot found in scene.");
            enabled = false;
            Destroy(gameObject);
            return;
        }

        spriteRenderer = GetComponent<SpriteRenderer>();
        material = spriteRenderer.material;

        spriteRenderer.enabled = false;

        if (targetPlayerActivatable == null)
        {
            targetPlayerActivatable = transform.parent.gameObject;
        }

        // 🔥 Get highlighter from target
        highlighter = targetPlayerActivatable.GetComponent<ObjectHighlighter>();

        originalParent = transform.parent;

        transform.SetParent(proximityRoot.transform);
        transform.localScale = Vector3.one;
    }

    public void SetVisibilityMode(bool mode)
    {
        if (activationMode == ActivationMode.Instant)
        {
            spriteRenderer.enabled = false;
            isCurrentlyVisible = false;
        }
        else
        {
            isCurrentlyVisible = mode;
            spriteRenderer.enabled = mode;
        }

        // 🔥 Highlight toggle
        if (highlighter != null)
        {
            highlighter.SetHighlight(mode);
        }
    }

    public void SetUserInteractionState(bool mode)
    {
        isCurrentlyPressed = mode;
    }

    void Update()
    {
        if (activationMode == ActivationMode.Hold)
        {
            bool mode = isCurrentlyPressed && isCurrentlyVisible;

            if (currentActivationMode != mode)
            {
                currentActivationMode = mode;
                progress = 0f;
                isActivated = false;
            }

            if (currentActivationMode && !isActivated)
            {
                progress += Time.deltaTime * 100f / longPressDuration;

                if (progress >= 100f)
                {
                    isActivated = true;
                    HandleActivation();

                    if (repeatMode == RepeatMode.Once)
                        hasTriggered = true;
                }

                progress = Mathf.Clamp(progress, 0f, 100f);
            }

            material.SetFloat("_Progress", progress);
        }
    }

    public bool IsAvailable()
    {
        if (targetPlayerActivatable)
        {
            foreach (PlayerActivatable p in targetPlayerActivatable.GetComponents<PlayerActivatable>())
            {
                if (p.IsAvailable())
                    return true;
            }
        }
        return false;
    }

    void HandleActivation()
    {
        if (hasTriggered && repeatMode == RepeatMode.Once)
            return;

        if (targetPlayerActivatable)
        {
            SetVisibilityMode(false);

            foreach (PlayerActivatable p in targetPlayerActivatable.GetComponents<PlayerActivatable>())
            {
                p.Activate();
            }
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (activationMode != ActivationMode.Instant) return;
        if (hasTriggered && repeatMode == RepeatMode.Once) return;

        if (other.CompareTag("Player"))
        {
            HandleActivation();

            if (repeatMode == RepeatMode.Once)
                hasTriggered = true;
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (activationMode != ActivationMode.Instant) return;

        if (repeatMode == RepeatMode.Repeatable && other.CompareTag("Player"))
        {
            hasTriggered = false;
        }
    }

    void LateUpdate()
    {
        if (originalParent != null)
            transform.position = originalParent.position + offset;

        if (Camera.main != null)
            transform.rotation = Camera.main.transform.rotation;
    }
}