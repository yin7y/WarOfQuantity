using System.Collections;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Menu : MonoBehaviour, IPointerClickHandler
{
    byte maxFPS, modeID;
    bool canReload;
    [SerializeField] MainCity[] mainCities;
    [SerializeField] CityGenerator cityGenerator;
    [SerializeField] float timeSpeed;
    
    public static ushort playNum = 10, landNum = 10;
    
    
    [SerializeField] GameObject modePanel, cancelPanel, countryMode, landMode;
    GameObject currentActive;
    bool isModePanel;
    public InputField inputNumText, inputNum2Text, inputLandNumText, mapRangeText;    
    public static ushort playMapRange = 100;
    
    
    void Start(){
        timeSpeed = 1f;
        maxFPS = 60;
        canReload = true;
        QualitySettings.vSyncCount = 0; // 禁用垂直同步
        Application.targetFrameRate = maxFPS; // 設定目標FPS
        
        // Menu中的生成
        cityGenerator.mapRange = 55;
        cityGenerator.GenerateCities(10);
        
        // 模式選擇中的初始設定
        inputNumText.text = playNum.ToShortString();
        inputNum2Text.text = playNum.ToShortString();
        inputLandNumText.text = "40";
        mapRangeText.text = playMapRange.ToShortString();
    }

    void Update(){
        Time.timeScale = timeSpeed;
        if(AreAllMainCitiesSameTeam() && canReload){
            canReload = false;
            StartCoroutine(ReloadBackGroundGame());
        }
    }
    
    public void OnModeClick(){
        isModePanel = !isModePanel;
        modePanel.SetActive(isModePanel);
        currentActive = modePanel;
    }
    
    public void OnCountryModeClick(){
        countryMode.SetActive(true);
        landMode.SetActive(false);
        modeID = 0;
    }

    public void OnLandModeClick(){
        countryMode.SetActive(false);
        landMode.SetActive(true);
        modeID = 1;
    }
    
    public void OnStartClick(){
        playNum = 50;
        SceneManager.LoadScene("Game");
    }
    public void OnModeStartClick(){
        playMapRange = ushort.Parse(mapRangeText.text);
        switch(modeID){
            case 0:
                playNum = ushort.Parse(inputNumText.text);
                SceneManager.LoadScene("Game");
                break;
            case 1:
                playNum = ushort.Parse(inputNum2Text.text);                
                landNum = ushort.Parse(inputLandNumText.text);
                int allNum = playNum + landNum;
                SceneManager.LoadScene("Game2");
                break;
        }
        
    }
    public void ReadStringInput(){
        if(inputNumText.text != string.Empty)
            
            if(inputNumText.text.Length > 3){
                inputNumText.text = "100";
            }else if(float.Parse(inputNumText.text) < 2 || float.Parse(inputNum2Text.text) < 2){
                inputNumText.text = "2";
                inputNum2Text.text = "2";
            }else if(float.Parse(inputNumText.text) > 100){
                // inputNumText.text = "100";
            }
    }
    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.pointerCurrentRaycast.gameObject == cancelPanel && currentActive != null)
        {
            SetCurrentActiveFalse(currentActive);
            isModePanel = false;
        }
    }
    public void OnQuitClick(){
        Application.Quit();
    }
    
    public void SetCurrentActiveFalse(GameObject _current){
        _current.SetActive(false);
    }
    
    IEnumerator ReloadBackGroundGame(){
        yield return new WaitForSeconds(1);
        for(int i = 0; i < mainCities.Length; i++)
            Destroy(mainCities[i].gameObject);
        cityGenerator.GenerateCities(10);
        canReload = true;
        yield break;
    }
    
    bool AreAllMainCitiesSameTeam(){
        // 獲取所有MainCity的陣列
        mainCities = FindObjectsOfType<MainCity>();

        // 如果沒有MainCity，則返回false
        if (mainCities.Length == 0) return false;

        // 獲取第一個MainCity的teamID
        int firstTeamID = mainCities[0].GetTeamID();

        // 檢查其他MainCity的teamID是否與第一個MainCity相同
        for (int i = 1; i < mainCities.Length; i++)
        {
            if (mainCities[i].GetTeamID() != firstTeamID)
                return false;
            if(mainCities[i].GetComponentInChildren<Soldier>() != null){
                if(mainCities[i].GetComponentInChildren<Soldier>().GetTeamID() != firstTeamID)
                    return false;
            }
        }
        return true;
    }

}
