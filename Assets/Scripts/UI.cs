using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Linq;

public class UI : MonoBehaviour
{
    [SerializeField] Text myCityCountText, fpsText;
    [SerializeField] GameObject winMenu, loseMenu, pauseMenu, SelectedHint, pauseMask, backGround;    
    
    public Text rankingText;
    Dictionary<ushort, ushort> teamCityCount = new Dictionary<ushort, ushort>();
    
    WaitForSeconds waitTime = new WaitForSeconds(1f);
    short maxFPS = 1000; // 最大FPS
    float fps, timer;
    bool isPause, isFinish;
    public bool canSelect;
    private void Start()
    {
        // Time.timeScale = 1f;
        canSelect = true;
        backGround.transform.localScale = new Vector3(Menu.playMapRange * 10, Menu.playMapRange * 10);
        StartCoroutine(UpdateFPS());
        LimitFPS();
        Rank();
    }
    void Update() {
        timer += Time.deltaTime;
        if(!isFinish){
            if (Input.GetKeyDown(KeyCode.Escape)){
                if(!isPause){
                    // 打開暫停選單
                    pauseMenu.SetActive(true);
                    pauseMask.SetActive(true);
                    Time.timeScale = 0f;
                    isPause = true;
                    canSelect = false;
                }else{
                    // 關閉暫停選單
                    pauseMenu.SetActive(false);
                    pauseMask.SetActive(false);
                    Time.timeScale = 1f;
                    isPause = false;
                    canSelect = true;
                }
            }
        }
        if(timer >= 1){
            Judge();
            Rank();
            UpdateMyCityCount();
            timer = 0f;
        }
        
    }
    public void UpdateMyCityCount(){
        int countCities = 0;
        MainCity[] cities = FindObjectsOfType<MainCity>();
        foreach (MainCity city in cities){
            if (city.GetTeamID() == 0){
                countCities++;
            }
        }
        myCityCountText.text = "領土: " + countCities.ToString();
    }
    public void Rank(){
         // 重置 teamCityCount 字典
        teamCityCount.Clear();

        // 獲取所有 MainCity 的陣列
        MainCity[] mainCities = FindObjectsOfType<MainCity>();
        // 計算每個 teamID 的城市數量
        foreach (MainCity city in mainCities){
            ushort teamID = city.GetTeamID();
            
            if(teamID == 500) continue;
            if (teamCityCount.ContainsKey(teamID)){
                teamCityCount[teamID]++;
            }else{
                teamCityCount[teamID] = 1;
            }
        }

        // 根據城市數量進行排名
        List<KeyValuePair<ushort, ushort>> sortedTeamCityCount = teamCityCount.ToList();
        sortedTeamCityCount.Sort((pair1, pair2) => pair2.Value.CompareTo(pair1.Value));

        // 更新 UI 文本元件
        string rankingText = string.Empty;
        if(sortedTeamCityCount.Count() > 10){
            for (int i = 0; i < 10; i++){
                int teamID = sortedTeamCityCount[i].Key;
                ushort _cityCount = sortedTeamCityCount[i].Value;
                // 獲取隊伍ID對應的城市名稱
                string cityName = GetCityName(teamID);
                
                string lineText = string.Format("<color=#{0}>{1,2}{2,12}</color>\n", GetColorHex(teamID), _cityCount, cityName);
                rankingText += lineText;
            }
        }else{
            for (int i = 0; i < sortedTeamCityCount.Count(); i++){
                int teamID = sortedTeamCityCount[i].Key;
                ushort _cityCount = sortedTeamCityCount[i].Value;
                // 獲取隊伍ID對應的城市名稱
                string cityName = GetCityName(teamID);

                string lineText = string.Format("<color=#{0}>{1,2}{2,12}</color>\n", GetColorHex(teamID), _cityCount, cityName);
                rankingText += lineText;
            }
        }
        // 顯示排名在 UI 文本元件上
        this.rankingText.text = rankingText;
    }

    string GetColorHex(int teamID)
    {
        // 自訂顏色列表
        // string[] colors = { "FF0000", "00FF00", "0000FF", "FFFF00", "FF00FF", "00FFFF" };
        MainCity[] cities = FindObjectsOfType<MainCity>();
        foreach (MainCity city in cities)
        {
            if (city.GetTeamID() == teamID)
            {
                return ColorUtility.ToHtmlStringRGB(city.nameText.color);
            }
        }
        return string.Empty;
        // 循環選擇顏色
        // return colors[index % colors.Length];
    }
    string GetCityName(int teamID){
        // 根據隊伍ID獲取城市名稱
        MainCity[] cities = FindObjectsOfType<MainCity>();
        foreach (MainCity city in cities){
            if (city.GetTeamID() == teamID){
                return city.nameText.text;
            }
        }
        return "Unknown City";
    }
    bool DoesCityWithTeamIDExist(int teamID){
        // 獲取所有城市的陣列
        MainCity[] cities = FindObjectsOfType<MainCity>();
        // 檢查每個城市的teamID是否等於指定的teamID
        foreach (MainCity city in cities){
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
        for (int i = 1; i < mainCities.Length; i++){
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
    
    public void Judge(){
        // 檢查所有MainCity的teamID是否相同
        if (AreAllMainCitiesSameTeam()){
            pauseMask.SetActive(true);
            winMenu.SetActive(true);
            SelectedHint.SetActive(false);
            
            canSelect = false;
            isFinish = true;
            Time.timeScale = 0f;
        }
        if (!DoesCityWithTeamIDExist(0)){
            // 顯示loseMenu並暫停遊戲
            pauseMask.SetActive(true);
            loseMenu.SetActive(true);
            SelectedHint.SetActive(false);
            canSelect = false;
            isFinish = true;
            Time.timeScale = 0f;
        }
    }
    
    public void OnContinueBut(){
        Time.timeScale = 1f;
        isPause = false;
        canSelect = true;;
        pauseMenu.SetActive(false);
        winMenu.SetActive(false);
        loseMenu.SetActive(false);
    }
    public void OnMenuClick(){
        Time.timeScale = 1f;
        SceneManager.LoadScene("Menu");
    }
    public void OnReStartClick(){
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
    public void OnQuitClick(){
        Application.Quit();
    }
}