using UnityEngine;
using StarterAssets;

[RequireComponent(typeof(StarterAssetsInputsAdditionalInputs))]
public class ModifiedProximityTriggerActivator : MonoBehaviour
{
    public float castRadius = 3f;

    private StarterAssetsInputsAdditionalInputs _input;

    private ProximityTrigger _currentNormalTrigger;
    private ModifiedProximityTrigger _currentModifiedTrigger;

    private void Awake()
    {
        _input = GetComponent<StarterAssetsInputsAdditionalInputs>();
    }

    private void Update()
    {
        FindClosestReachableTrigger();
        UpdateClosestTrigger();
    }

    private void FindClosestReachableTrigger()
    {
        Collider[] colliders = Physics.OverlapSphere(
            transform.position,
            castRadius,
            LayerMask.GetMask("ProximityTriggers")
        );

        float closestDistance = Mathf.Infinity;

        ProximityTrigger bestNormal = null;
        ModifiedProximityTrigger bestModified = null;

        foreach (var collider in colliders)
        {
            float distance = Vector3.Distance(transform.position, collider.transform.position);

            if (distance < closestDistance)
            {
                var mod = collider.GetComponent<ModifiedProximityTrigger>();
                if (mod != null && mod.IsAvailable())
                {
                    closestDistance = distance;
                    bestModified = mod;
                    bestNormal = null;
                    continue;
                }

                var norm = collider.GetComponent<ProximityTrigger>();
                if (norm != null && norm.IsAvailable())
                {
                    closestDistance = distance;
                    bestNormal = norm;
                    bestModified = null;
                }
            }
        }

        // 🔥 Turn OFF old highlights/UI
        if (_currentNormalTrigger != null && _currentNormalTrigger != bestNormal)
            _currentNormalTrigger.SetVisibilityMode(false);

        if (_currentModifiedTrigger != null && _currentModifiedTrigger != bestModified)
            _currentModifiedTrigger.SetVisibilityMode(false);

        // 🔥 Turn ON new
        if (bestNormal != null)
            bestNormal.SetVisibilityMode(true);

        if (bestModified != null)
            bestModified.SetVisibilityMode(true);

        _currentNormalTrigger = bestNormal;
        _currentModifiedTrigger = bestModified;
    }

    private void UpdateClosestTrigger()
    {
        bool isInteracting = _input != null && _input.interact;

        if (_currentNormalTrigger != null)
        {
            _currentNormalTrigger.SetUserInteractionState(isInteracting);
        }
        else if (_currentModifiedTrigger != null)
        {
            _currentModifiedTrigger.SetUserInteractionState(isInteracting);
        }
    }
}