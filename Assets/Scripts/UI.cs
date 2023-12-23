using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI : MonoBehaviour
{
    float fps;
    [SerializeField] Text cityCountText, fpsText;
    WaitForSeconds waitTime = new WaitForSeconds(1f);
    int maxFPS = 144; // 最大FPS

    private void Start()
    {
        StartCoroutine(UpdateFPS());
        LimitFPS();
    }

    public void UpdateCityCount(int count)
    {
        cityCountText.text = "主城:" + count.ToString();
    }

    System.Collections.IEnumerator UpdateFPS()
    {
        while (true)
        {
            // 計算FPS
            fps = 1f / Time.deltaTime;

            // 檢查是否超過最大FPS
            if (fps > maxFPS)
            {
                fps = maxFPS;
            }

            // 更新顯示的FPS文本
            fpsText.text = "FPS: " + Mathf.Round(fps).ToString();

            // 等待一秒
            yield return waitTime;
        }
    }

    void LimitFPS()
    {
        QualitySettings.vSyncCount = 0; // 禁用垂直同步
        Application.targetFrameRate = maxFPS; // 設定目標FPS
    }
}