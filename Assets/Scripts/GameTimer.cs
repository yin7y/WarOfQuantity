using UnityEngine;
using UnityEngine.UI;

public class GameTimer : MonoBehaviour
{
    public Text timerText;
    private float startTime;
    private bool isTimerRunning = false;

    void Start()
    {
        // 設定計時器的起始時間
        startTime = Time.time;
        isTimerRunning = true;
    }

    void Update()
    {
        if (isTimerRunning)
        {
            // 計算經過的時間
            float elapsedTime = Time.time - startTime;

            // 計算分鐘和秒鐘
            int minutes = (int)(elapsedTime / 60);
            int seconds = (int)(elapsedTime % 60);

            // 格式化時間並顯示在UI上
            timerText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
        }
    }
}