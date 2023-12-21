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
    
    // public List<GameObject> soldiers = new(); // 士兵列表
    
    void Start()
    {
        num = 0;
        numCdTime = 0.01f;
        soldierCdTime = 0.05f;
        numText.text = num.ToString();
        isAtking = false;
        isDefending = false;
        numMax = 500;
    }

    void Update()
    {
        timer += Time.deltaTime;
        if(num < numMax)
        if(isDefending){   // 城市正在防守的話，數量產能減半TODO
            if (timer >= numCdTime * 1){
                num++;
                numText.text = num.ToString();
                timer = 0;
            }
        }else if(isAtking){
            numText.text = num.ToString();
            timer = 0;
        }else if (timer >= numCdTime){
            num++;
            numText.text = num.ToString();
            timer = 0;
        }
       // TOOO  仲裁多方陣營領地權
    }

    public void SoldierGenerator(int count, GameObject target)
    {
        if(target != null && num >1)
            StartCoroutine(GenerateSoldiers(count, target));
    }

    private IEnumerator GenerateSoldiers(int count, GameObject target)
    {
        print(" ======== " + gameObject.name + " 發兵 to " + target.name + " ========");
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
                    // 將士兵設定為指定父物件的子物體
                    soldier.transform.SetParent(soldiersParent);

                    // 將士兵加入士兵列表
                    // soldiers.Add(soldier);

                    // 獲取士兵的 Soldier 腳本參考
                    Soldier soldierScript = soldier.GetComponent<Soldier>();
                    if (soldierScript != null)
                    {
                        // soldierScript.MoveToDestination(target); // 移動士兵到指定的目的地
                        isAtking = true;
                    }
                    MainCity targetCity = GetComponent<MainCity>();
                    targetCity.isDefending = true; // 指定城正在被發兵
                    
                    yield return new WaitForSeconds(soldierCdTime);
                }
            }
        isAtking = false;
    }
    
    public void SetTeamID(int id)
    {
        teamID = id;
        // Debug.Log(gameObject.name + "'s teamID set to: " + teamID);
    }
    public int GetTeamID(){
        return teamID;
    }
    public GameObject GetTargetCity(){
        return targetCity;
    }
    public void GetDamage(int damage){
        num -= damage;
    }
}