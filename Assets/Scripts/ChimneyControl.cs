using UnityEngine;

public class ChimneyControl : MonoBehaviour
{
    public ParticleSystem smokeParticles;

    public void SetSmoke(bool active)
    {
        if (active) smokeParticles.Play();
        else smokeParticles.Stop();
    }
}