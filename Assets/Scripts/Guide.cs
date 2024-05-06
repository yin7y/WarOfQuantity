using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Guide : MonoBehaviour
{
    public GameObject step1, step2, step3;
    public GameObject pauseMask, selectedHint;
    void Start()
    {
        step1.SetActive(true);
        pauseMask.SetActive(true);
        Time.timeScale = 0f;
    }
    
    void Update()
    {
        if(step1.activeSelf && selectedHint.activeSelf){
            step1.SetActive(false);
            step2.SetActive(true);
            Time.timeScale = 1f;
        }else if(step2.activeSelf){
            
        }
    }
}
