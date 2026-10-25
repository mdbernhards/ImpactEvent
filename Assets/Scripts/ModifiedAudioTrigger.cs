using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Yarn.Unity;

public class ModifiedAudioTrigger : PlayerActivatable
{
    public enum PlaybackMode
    {
        OneShot,
        Toggle,
        PlayOnly,
        PauseOnly
    }

    public enum AudioMode
    {
        TwoD,
        ThreeD
    }

    [Header("Audio Settings")]
    public AudioClip audioClip;
    public PlaybackMode playbackMode = PlaybackMode.OneShot;
    public bool loop = false;

    [Header("Spatial Settings")]
    public AudioMode audioMode = AudioMode.TwoD;
    public float minDistance = 2f;
    public float maxDistance = 15f;

    [Header("Fade Settings")]
    public bool useFade = false;
    public float fadeInDuration = 1f;
    public float fadeOutDuration = 1f;

    private AudioSource audioSource;

    void Awake()
    {
        if (playbackMode != PlaybackMode.OneShot)
        {
            audioSource = GetComponent<AudioSource>();

            if (audioSource == null)
            {
                audioSource = gameObject.AddComponent<AudioSource>();
            }

            audioSource.playOnAwake = false;
            audioSource.loop = loop;
            audioSource.clip = audioClip;

            ApplyAudioMode();
        }
    }

    void ApplyAudioMode()
    {
        if (audioSource == null) return;

        if (audioMode == AudioMode.TwoD)
        {
            audioSource.spatialBlend = 0f; // 2D
        }
        else
        {
            audioSource.spatialBlend = 1f; // 3D
            audioSource.minDistance = minDistance;
            audioSource.maxDistance = maxDistance;
        }
    }

    override protected void OnActivate()
    {
        if (audioClip == null) return;

        switch (playbackMode)
        {
            case PlaybackMode.OneShot:
                PlayOneShot();
                break;

            case PlaybackMode.Toggle:
                ToggleAudio();
                break;

            case PlaybackMode.PlayOnly:
                PlayAudio();
                break;

            case PlaybackMode.PauseOnly:
                PauseAudio();
                break;
        }
    }

    private void PlayOneShot()
    {
        GameObject audioObject = new GameObject("One Shot Audio");
        audioObject.transform.position = transform.position;

        AudioSource source = audioObject.AddComponent<AudioSource>();
        source.clip = audioClip;
        source.playOnAwake = false;
        source.rolloffMode = AudioRolloffMode.Linear;

        // Apply 2D / 3D
        if (audioMode == AudioMode.TwoD)
        {
            source.spatialBlend = 0f;
        }
        else
        {
            source.spatialBlend = 1f;
            source.minDistance = minDistance;
            source.maxDistance = maxDistance;
        }

        source.Play();

        Destroy(audioObject, audioClip.length);
    }

    public void PlayAudio()
    {
        if (audioSource == null) return;

        StopAllCoroutines();

        if (useFade)
        {
            StartCoroutine(FadeIn());
        }
        else
        {
            audioSource.Play();
        }
    }

    public void PauseAudio()
    {
        if (audioSource == null) return;

        StopAllCoroutines();

        if (useFade)
        {
            StartCoroutine(FadeOut());
        }
        else
        {
            audioSource.Pause();
        }
    }

    public void ToggleAudio()
    {
        if (audioSource == null) return;

        if (audioSource.isPlaying)
        {
            PauseAudio();
        }
        else
        {
            PlayAudio();
        }
    }

    IEnumerator FadeIn()
    {
        audioSource.volume = 0f;
        audioSource.Play();

        float t = 0f;

        while (t < fadeInDuration)
        {
            t += Time.deltaTime;
            audioSource.volume = Mathf.Lerp(0f, 1f, t / fadeInDuration);
            yield return null;
        }

        audioSource.volume = 1f;
    }

    IEnumerator FadeOut()
    {
        float startVolume = audioSource.volume;
        float t = 0f;

        while (t < fadeOutDuration)
        {
            t += Time.deltaTime;
            audioSource.volume = Mathf.Lerp(startVolume, 0f, t / fadeOutDuration);
            yield return null;
        }

        audioSource.volume = 0f;
        audioSource.Pause();
    }
}