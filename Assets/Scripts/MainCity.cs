using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.UIElements;

public class MainCity : MonoBehaviour
{
    [SerializeField] int num, numMax, teamID;
    public float numCdTime, soldierCdTime;
    public bool isDefending;
    bool isAtking;
    [SerializeField] float timer;
    [SerializeField] TextMeshPro numText;
    [SerializeField] GameObject soldierPrefab, targetCity; 
    [SerializeField] Transform soldiersParent; // 用於整理士兵的父物件
    
    int myAllCityCount;
    // public List<GameObject> soldiers = new(); // 士兵列表
    
    void Start()
    {
        num = 0;
        numCdTime = 0.1f;
        soldierCdTime = 0.05f;
        numText.text = num.ToString();
        isAtking = false;
        isDefending = false;
        numMax = 500;
        StartCoroutine(AutoAttackAI());
    }

    void Update(){
        timer += Time.deltaTime;
        if(num < 0)
            num = 0;
        if(num < numMax){
            if(isDefending){   // 城市正在接收的話，數量產能減半TODO
                if (timer >= numCdTime * 2){
                    num++;
                    timer = 0;
                }
            }else if(isAtking){
                timer = 0;
            }else if (timer >= numCdTime){
                num++;
                timer = 0;
            }            
        }else{
            num = numMax;
        }
        numText.text = num.ToString();
        
        myAllCityCount = CountAllCities();
        UI ui = FindObjectOfType<UI>();
        if (ui != null){
            ui.UpdateCityCount(myAllCityCount);
        }
    }

    public void SoldierGenerator(int count, GameObject target){
        if(target != null && num >1)
            StartCoroutine(GenerateSoldiers(count, target));
    }

    private IEnumerator GenerateSoldiers(int count, GameObject target){
        print(" " + gameObject.name + " >> 發兵 >> " + target.name);
        targetCity = target;
        int setCount = count;
        if(setCount >= num)
            setCount = num - 1;
        if(gameObject.GetComponent<MainCity>().isAtking == false)
            for (int i = 0; i < setCount; i++){                 
                if(num > 1){
                    // 生成士兵
                    GameObject soldier = Instantiate(soldierPrefab, transform.position, Quaternion.identity);
                    num--;
                    soldier.name = "Soldier" + i;
                    soldier.GetComponent<Soldier>().SetTeamID(teamID);
                    // 將士兵設定為指定父物件的子物體
                    soldier.transform.SetParent(soldiersParent);

                    // 獲取士兵的 Soldier 腳本參考
                    Soldier soldierScript = soldier.GetComponent<Soldier>();
                    if (soldierScript != null)
                    {
                        soldierScript.MoveToDestination(target); // 移動士兵到指定的目的地
                        isAtking = true;
                    }
                    MainCity atkTargetCity = target.GetComponent<MainCity>();
                    atkTargetCity.isDefending = true; // 指定城正在被發兵
                    
                    yield return new WaitForSeconds(soldierCdTime);
                }
            }
        isAtking = false;
    }
    
    public void SetTeamID(int id) => teamID = id;
    public void SetNum(int _num) => num = _num;
    public void GetDamage(int damage) => num -= damage;
    public int GetTeamID(){
        return teamID;
    }
    public int GetNum(){
        return num;
    }
  
    int CountAllCities(){
        int countCities = 0;
        MainCity[] cities = FindObjectsOfType<MainCity>();
        foreach (MainCity city in cities){
            if (city.teamID == 0){
                countCities++;
            }
        }
        return countCities;
    }
    
    // public GameObject GetTargetCity(){
    //     return targetCity;
    // }
    
    IEnumerator AutoAttackAI()
    {
        while (true)
        {
            if(teamID == 0)
                break;
            // 檢查是否有目標城市
            if (targetCity == null || targetCity.GetComponent<MainCity>().teamID == teamID)
            {
                // 尋找附近的敵方城市
                MainCity[] cities = FindObjectsOfType<MainCity>();
                List<MainCity> enemyCities = new List<MainCity>();
                foreach (MainCity city in cities)
                {
                    if (city.teamID != teamID)
                    {
                        enemyCities.Add(city);
                    }
                }

                // 從敵方城市中隨機選擇一個作為目標
                if (enemyCities.Count > 0)
                {
                    int randomIndex = UnityEngine.Random.Range(0, enemyCities.Count);
                    targetCity = enemyCities[randomIndex].gameObject;
                }
            }

            // 如果有目標城市，則發兵
            if (targetCity != null)
            {
                // 計算發兵數量
                int count = Mathf.Min(num, num-1);

                // 生成士兵
                SoldierGenerator(count, targetCity);

                // 等待一段時間再進行下一次攻擊
                yield return new WaitForSeconds(UnityEngine.Random.Range(1, 60));
            }
            else
            {
                // 如果沒有目標城市，則等待一段時間再重新尋找目標
                yield return new WaitForSeconds(10f); // 每10秒重新尋找目標城市
            }
        }
    }
}