using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Guide : MonoBehaviour
{
    public GameObject step1, step2, left, mid1, mid2, right, step3, win;
    public GameObject pauseMask, selectedHint, winMenu;
    public AudioClip completeSound;
    AudioSource audioSource;
    bool a, b, c, d, end;
    Vector3 cameraOrigin;
    void Start()
    {
        step1.SetActive(true);
        pauseMask.SetActive(true);
        audioSource = GetComponent<AudioSource>();
        Time.timeScale = 0f;
    }
    
    void Update(){
        if(step1.activeSelf && selectedHint.activeSelf){
            step1.SetActive(false);
            step2.SetActive(true);
            pauseMask.SetActive(false);
            cameraOrigin = FindAnyObjectByType<Camera>().transform.position;
            Time.timeScale = 1f;
        }else if(step2.activeSelf){
            if(Left() && !a){
                audioSource.PlayOneShot(completeSound);
                left.SetActive(true);
                a = true;
            }
            if(Mid1() && !b){
                audioSource.PlayOneShot(completeSound);
                mid1.SetActive(true);
                b = true;
            }
            if(Mid2() && !c){
                audioSource.PlayOneShot(completeSound);
                mid2.SetActive(true);
                c = true;
            }
            if(Right() && !d){
                audioSource.PlayOneShot(completeSound);
                right.SetActive(true);
                d = true;
            }
            if(a&&b&&c&&d){
                step2.SetActive(false);
                step3.SetActive(true);
            }
        }else if(step3.activeSelf){
            if(winMenu.activeSelf && !end){
                audioSource.PlayOneShot(completeSound);
                win.SetActive(true);
                end = true;
            }
        }
    }
    bool Left(){
        MainCity[] myCity = FindObjectsOfType<MainCity>();
        foreach(MainCity city in myCity)
            if(city.GetTeamID() == 0 && city.isAtking)
                return true;
        return false;
    }
    bool Mid1(){
        Camera camera = FindAnyObjectByType<Camera>();
        if(camera.transform.position.x != cameraOrigin.x || camera.transform.position.y != cameraOrigin.y)
            return true;
        return false;
    }
    bool Mid2(){
        Camera camera = FindAnyObjectByType<Camera>();
        if(camera.transform.position.z != cameraOrigin.z)
            return true;
        return false;
    }
    
    bool Right(){
        MainCity[] myCity = FindObjectsOfType<MainCity>();
        foreach(MainCity city in myCity)
            if(city.selectedHint.activeSelf)
                return true;
        return false;
    }
}
