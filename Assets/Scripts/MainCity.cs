using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.UIElements;

public class MainCity : MonoBehaviour
{
    [SerializeField] int num, numMax, teamID;
    public float numCdTime, soldierCdTime, atkCdTime;
    public bool isDefending;
    int setCount;
    bool isAtking;
    
    [SerializeField] float timer, atkTimer;
    public TextMesh nameText;
    [SerializeField] TextMeshPro numText;
    [SerializeField] GameObject soldierPrefab, targetCity; 
    [SerializeField] Transform soldiersParent; // 用於整理士兵的父物件
    
    void Start(){
        num = 1;
        numCdTime = 0.4f;
        soldierCdTime = 0.15f;
        numText.text = num.ToString();
        isAtking = false;
        isDefending = false;
        numMax = 500;
        atkCdTime = UnityEngine.Random.Range(1,30);
    }
    
    void Update(){
        timer += Time.deltaTime;
        atkTimer += Time.deltaTime;
        if(num < 0)
            num += -num;
        if(num < numMax){
            if(isAtking){
                timer = 0;
            }else if(isDefending){   // 城市正在接收的話，數量產能減半
                if (timer >= numCdTime * 2){
                    num++;
                    timer = 0;
                }
            }else if (timer >= numCdTime){
                num++;
                timer = 0;
            }            
        }else{
            num = numMax;
            timer = 0;
        }
        if(atkTimer >= atkCdTime && teamID != 0){  // 敵人AI
            AutoAttackAI();            
            atkCdTime = UnityEngine.Random.Range(1, 30);
            atkTimer = 0f;
        }
        
        numText.text = num.ToString();
    }

    public void SoldierGenerator(int count, GameObject target){
        if(num >1)
            StartCoroutine(GenerateSoldiers(count, target));
    }

    IEnumerator GenerateSoldiers(int count, GameObject target){
        // print(" " + gameObject.name + " >> 發兵 >> " + target.name);
        targetCity = target;
        setCount = count;
        if(gameObject.GetComponent<MainCity>().isAtking == false){
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
                    target.GetComponent<MainCity>().isDefending = true; // 指定城正在被發兵
                    
                    yield return new WaitForSeconds(soldierCdTime);
                }
            }
            target.GetComponent<MainCity>().isDefending = false;
        }
        isAtking = false;
        yield break;
    }
    
    void AutoAttackAI(){
        MainCity[] cities = FindObjectsOfType<MainCity>();
        List<MainCity> enemyCities = new List<MainCity>();
        foreach (MainCity city in cities){
            if (city.teamID != teamID){
                enemyCities.Add(city);
            }
        }
        
        // 檢查是否有目標城市
        if (targetCity == null){
            // 從敵方城市中隨機選擇一個作為目標
            if (enemyCities.Count > 0){
                int randomIndex = UnityEngine.Random.Range(0, enemyCities.Count);
                targetCity = enemyCities[randomIndex].gameObject;
            }
        }
        if (targetCity != null){
            // 生成士兵
            SoldierGenerator(num - 1, targetCity);
        }
        targetCity = null;
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
    
    void OnTriggerEnter2D(Collider2D collision){
        if (collision.CompareTag("Soldier") ){
            Soldier soldier = collision.GetComponent<Soldier>();
            if(soldier.GetTarget() == gameObject){
                if(soldier.GetTeamID() == teamID){
                    num++;
                }else{
                    num--;
                    if(num == 0){
                        nameText.text = soldier.belongCityName;
                        gameObject.GetComponent<SpriteRenderer>().color = soldier.GetComponent<SpriteRenderer>().color;
                        nameText.color = gameObject.GetComponent<SpriteRenderer>().color;
                        
                        teamID = soldier.GetTeamID();                        
                    }
                }
                Destroy(soldier.gameObject);
            }
        }
    }
}