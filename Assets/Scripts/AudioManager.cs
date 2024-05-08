using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AudioManager : MonoBehaviour
{
    public AudioClip completeSound;
    AudioSource audioSource;
    void Start(){
        audioSource = GetComponent<AudioSource>();
    }
    public void PlayAudio(ushort num){
        AudioClip audioClip = num switch{
            0 => completeSound,
            _ => throw new System.NotImplementedException()
        };
        audioSource.PlayOneShot(audioClip);
    }
}
