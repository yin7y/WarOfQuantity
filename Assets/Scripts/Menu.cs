using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Menu : MonoBehaviour
{
    byte maxFPS;
    bool canReload;
    [SerializeField] MainCity[] mainCities;
    [SerializeField] CityGenerator cityGenerator;
    [SerializeField] float timeSpeed;
    public static short playNum;
    void Start()
    {
        timeSpeed = 1f;
        maxFPS = 60;
        canReload = true;
        QualitySettings.vSyncCount = 0; // 禁用垂直同步
        Application.targetFrameRate = maxFPS; // 設定目標FPS
        cityGenerator.GenerateCities(10);
    }

    void Update()
    {
        Time.timeScale = timeSpeed;
        if(AreAllMainCitiesSameTeam() && canReload){
            canReload = false;
            StartCoroutine(ReloadBackGroundGame());
        }
    }
    
    public void OnStartClick(){
        playNum = 50;
        SceneManager.LoadScene("Game");
    }
    public void OnQuitClick(){
        Application.Quit();
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
            if(mainCities[i].GetComponentInChildren<Soldier>() != null){
                if(mainCities[i].GetComponentInChildren<Soldier>().GetTeamID() != firstTeamID)
                    return false;
            }
        }
        return true;
    }
}
