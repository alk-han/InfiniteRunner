using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioTrigger : MonoBehaviour
{
    [SerializeField] private string triggerTag = "Player";
    [SerializeField] private AudioPlayer audioPlayerPrefab;
    [SerializeField] private AudioClip audioToPlay;
    [SerializeField] private float volume = 1f;
    [SerializeField] private float pitch = 1f;


    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(triggerTag))
        {
            PlayAudio();
        }
    }


    private void PlayAudio()
    {
        AudioPlayer newAudioPlayer = Instantiate(audioPlayerPrefab);
        newAudioPlayer.PlayAudio(audioToPlay, volume, pitch);
    }
}
