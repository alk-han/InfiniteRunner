using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioPlayer : MonoBehaviour
{
    [SerializeField] private AudioSource audioSource;


    public void PlayAudio(AudioClip audioToPlay, float volume, float pitch, bool destroyOnFinish = true)
    {
        audioSource.clip = audioToPlay;
        audioSource.volume = volume;
        audioSource.pitch = pitch;
        audioSource.Play();

        if (destroyOnFinish)
        {
            Invoke(nameof(DestroySelf), audioToPlay.length);
        }
    }


    private void DestroySelf()
    {
        Destroy(gameObject);
    }
}
