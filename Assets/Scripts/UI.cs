using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class UI : MonoBehaviour
{
    
    [SerializeField] Text cityCountText, fpsText;
    [SerializeField] GameObject winMenu, loseMenu, pauseMenu, SelectedHint;    
    
    WaitForSeconds waitTime = new WaitForSeconds(1f);
    short maxFPS = 1000; // 最大FPS
    float fps;
    bool isPause, isFinish;
    public bool canSelect;
    private void Start()
    {
        Time.timeScale = 1f;
        canSelect = true;
        StartCoroutine(UpdateFPS());
        LimitFPS();
    }
    void Update() {
        if(!isFinish){
            if (Input.GetKeyDown(KeyCode.Escape)){
                if(!isPause){
                    // 打開暫停選單
                    pauseMenu.SetActive(true);
                    Time.timeScale = 0f;
                    isPause = true;
                    canSelect = false;
                }else{
                    // 關閉暫停選單
                    pauseMenu.SetActive(false);
                    Time.timeScale = 1f;
                    isPause = false;
                    canSelect = true;
                }
            }
        }
        // 檢查所有MainCity的teamID是否相同
        if (AreAllMainCitiesSameTeam()){
            winMenu.SetActive(true);
            SelectedHint.SetActive(false);
            canSelect = false;
            isFinish = true;
            Time.timeScale = 0f;
        }
        if (!DoesCityWithTeamIDExist(0)){
            // 顯示loseMenu並暫停遊戲
            loseMenu.SetActive(true);
            SelectedHint.SetActive(false);
            canSelect = false;
            isFinish = true;
            Time.timeScale = 0f;
        }
        UpdateCityCount(CountAllCities());
    }
    public void UpdateCityCount(int count){
        cityCountText.text = "主城: " + count.ToString();
    }
    public void OnContinueBut(){
        Time.timeScale = 1f;
        isPause = false;
        canSelect = true;;
        pauseMenu.SetActive(false);
        winMenu.SetActive(false);
        loseMenu.SetActive(false);
    }

    bool DoesCityWithTeamIDExist(int teamID)
    {
        // 獲取所有城市的陣列
        MainCity[] cities = FindObjectsOfType<MainCity>();

        // 檢查每個城市的teamID是否等於指定的teamID
        foreach (MainCity city in cities)
        {
            if (city.GetTeamID() == teamID){
                return true;
            }
            if(city.GetComponentInChildren<Soldier>() != null){
                if(city.GetComponentInChildren<Soldier>().GetTeamID() == teamID){
                    return true;
                }
            }
        }

        return false;
    }
    
    int CountAllCities(){
        int countCities = 0;
        MainCity[] cities = FindObjectsOfType<MainCity>();
        foreach (MainCity city in cities){
            if (city.GetTeamID() == 0){
                countCities++;
            }
        }
        return countCities;
    }

    bool AreAllMainCitiesSameTeam(){
        // 獲取所有MainCity的陣列
        MainCity[] mainCities = FindObjectsOfType<MainCity>();

        // 如果沒有MainCity，則返回false
        if (mainCities.Length == 0){
            return false;
        }

        // 獲取第一個MainCity的teamID
        int firstTeamID = mainCities[0].GetTeamID();

        // 檢查其他MainCity的teamID是否與第一個MainCity相同
        for (int i = 1; i < mainCities.Length; i++)
        {
            if (mainCities[i].GetTeamID() != firstTeamID)
                return false;
            if(mainCities[i].GetTeamID() != 0)
                return false;
            if(mainCities[i].GetComponentInChildren<Soldier>() != null){
                if(mainCities[i].GetComponentInChildren<Soldier>().GetTeamID() != firstTeamID)
                    return false;
                if(mainCities[i].GetComponentInChildren<Soldier>().GetTeamID() != 0)
                    return false;
            }
        }
        return true;
    }

    IEnumerator UpdateFPS(){
        while (true){
            // 計算FPS
            fps = 1f / Time.deltaTime;

            // 檢查是否超過最大FPS
            // if (fps > maxFPS)
            //     fps = maxFPS;

            // 更新顯示的FPS文本
            fpsText.text = "FPS: " + Mathf.Round(fps).ToString();

            // 等待一秒
            yield return waitTime;
        }
    }
    void LimitFPS(){
        QualitySettings.vSyncCount = 0; // 禁用垂直同步
        Application.targetFrameRate = maxFPS; // 設定目標FPS
    }
    public void OnMenuClick(){
        SceneManager.LoadScene("Menu");
    }
    public void OnReStartClick(){
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
    public void OnQuitClick(){
        Application.Quit();
    }
}