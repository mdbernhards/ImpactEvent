using UnityEngine;

public class BeerAnimationEvents : MonoBehaviour
{
    public AudioSource splashAudio;
    public ParticleSystem splashParticles;

    public void PlaySplash()
    {
        if (splashAudio != null)
        {
            splashAudio.Play();
        }

        if (splashParticles != null)
        {
            splashParticles.Play();
        }
    }
}