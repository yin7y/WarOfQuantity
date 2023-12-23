using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FrameRateController : MonoBehaviour
{
    public int targetFrameRate = 100;

    void Start()
    {
        // 設定目標幀數
        // Application.targetFrameRate = targetFrameRate;
    }

    void Update()
    {
        // // 檢查是否需要調整時間尺度
        // if (Time.timeScale != 1f)
        // {
        //     // 根據目標幀數計算時間尺度
        //     float targetTimeScale = targetFrameRate / (float)Time.captureFramerate;

        //     // 調整時間尺度
        //     Time.timeScale = Mathf.MoveTowards(Time.timeScale, targetTimeScale, Time.deltaTime);
        // }
    }
}