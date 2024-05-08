using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Music : MonoBehaviour
{
    public AudioClip bgm1, bgm2, bgm3;
    AudioSource musicSource;
    AudioClip currMusic;
    public GameObject musicEnObj, musicDisObj;
    void Start(){
        musicSource = GetComponent<AudioSource>();
        musicSource.loop = true;  // 設置循環播放
        if(SceneManager.GetActiveScene().name == "Menu" && OP.isMusic)
            PlayMusic(0);
        else if(OP.isMusic)
            PlayMusic();
        if(OP.isMusic)
            musicEnObj.SetActive(OP.isMusic);
        else
            musicDisObj.SetActive(!OP.isMusic);
    }
    
    public void PlayMusic(){
        ushort rd = (ushort)Random.Range(0, 3);
        currMusic = rd switch{
            0 => bgm1,
            1 => bgm2,
            2 => bgm3,
            _ => bgm1,
        };
        musicSource.clip = currMusic;
        Debug.Log(currMusic.name + " 成功播放");
        musicSource.Play();
    }
    public void PlayMusic(ushort num){
        currMusic = num switch{
            0 => bgm1,
            1 => bgm2,
            2 => bgm3,
            _ => bgm1,
        };
        musicSource.clip = currMusic;
        Debug.Log(currMusic.name + " 成功播放");
        musicSource.Play();
    }
    public void OnMusicClick(){
        OP.isMusic = !OP.isMusic;
        if(currMusic == null){
            if(SceneManager.GetActiveScene().name == "Menu" && OP.isMusic)
                PlayMusic(0);
            else if(OP.isMusic)
                PlayMusic();
        }
        if(OP.isMusic)
            musicSource.Play();
        else
            musicSource.Pause();
        musicEnObj.SetActive(OP.isMusic);
        musicDisObj.SetActive(!OP.isMusic);
    }
}
