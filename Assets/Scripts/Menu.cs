using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.Mathematics;
using UnityEditor.Scripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Menu : MonoBehaviour
{
    byte maxFPS;
    bool canReload;
    [SerializeField] MainCity[] mainCities;
    [SerializeField] CityGenerator cityGenerator;
    void Start()
    {
        maxFPS = 60;
        canReload = true;
        QualitySettings.vSyncCount = 0; // 禁用垂直同步
        Application.targetFrameRate = maxFPS; // 設定目標FPS
    }

    void Update()
    {
        
        if(AreAllMainCitiesSameTeam() && canReload){
            canReload = false;
            StartCoroutine(ReloadBackGroundGame());
        }
    }
    
    public void OnStartClick(){
        SceneManager.LoadScene("Game");
    }
    
    
    IEnumerator ReloadBackGroundGame(){
        yield return new WaitForSeconds(3);
        for(int i = 0; i < mainCities.Length; i++)
            Destroy(mainCities[i].gameObject);
        cityGenerator.GenerateCities();
        canReload = true;
    }
    
    bool AreAllMainCitiesSameTeam()
    {
        // 獲取所有MainCity的陣列
        mainCities = FindObjectsOfType<MainCity>();
        
        // 如果沒有MainCity，則返回false
        if (mainCities.Length == 0)
        {
            return false;
        }

        // 獲取第一個MainCity的teamID
        int firstTeamID = mainCities[0].GetTeamID();
        
        // 檢查其他MainCity的teamID是否與第一個MainCity相同
        for (int i = 1; i < mainCities.Length; i++)
        {
            if (mainCities[i].GetTeamID() != firstTeamID)
            {
                return false;
            }
        }

        return true;
    }
}
