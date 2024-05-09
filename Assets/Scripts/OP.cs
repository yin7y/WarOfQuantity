using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OP : MonoBehaviour
{
    public static bool autoMode, rigMode, isMusic = true;
    [SerializeField] GameObject AutoHint;
    void Start(){
        autoMode = false;
        rigMode = false;
    }
    void Update(){
        if(Input.GetKeyDown(KeyCode.R)){
            autoMode = !autoMode;
            AutoHint.SetActive(autoMode);
            ShowConsole("自動模式", autoMode);
        }
        // else if(Input.GetKeyDown(KeyCode.O)){ // 作弊按鍵
        //     rigMode = !rigMode;
        //     ShowConsole("操縱模式", rigMode);
        // }
    }
    void ShowConsole(string _text, bool _switch){
        if(_switch)
            _text = _text + " ON";
        else
            _text = _text + " OFF";
        Debug.Log(_text);
    }
}
