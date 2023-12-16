using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.UIElements;

public class MainCity : MonoBehaviour
{
    public int num, teamID;
    public float numCdTime, soldierCdTime;
    bool isAtking;
    [SerializeField] float timer;
    [SerializeField] TextMeshPro numText;
    [SerializeField] GameObject soldierPrefab;
    [SerializeField] Transform soldiersParent; // 用於整理士兵的父物件

    private List<GameObject> soldiers = new List<GameObject>(); // 士兵列表

    void Start()
    {
        num = 0;
        numCdTime = 0.1f;
        soldierCdTime = 0.1f;
        numText.text = num.ToString();
    }

    void Update()
    {
        timer += Time.deltaTime;
        if(isAtking){   // 城市正在攻擊的話，數量增加速度減半
            if (timer >= numCdTime * 2){
                num++;
                numText.text = num.ToString();
                timer = 0;
            }
        }else if (timer >= numCdTime){
            num++;
            numText.text = num.ToString();
            timer = 0;
        }
       // TOOO  仲裁多方陣營領地權
    }

    public void SoldierGenerator(int count, GameObject target)
    {
        StartCoroutine(GenerateSoldiers(count, target));
    }

    private IEnumerator GenerateSoldiers(int count, GameObject target)
    {
        int setCount = count;
        if(setCount >= num)
            setCount = num - 1;
        for (int i = 0; i < setCount; i++){
            if(num > 1){
                    // 生成士兵
                    GameObject soldier = Instantiate(soldierPrefab, transform.position, Quaternion.identity);
                    num--;
                    // 將士兵設定為指定父物件的子物體
                    soldier.transform.SetParent(soldiersParent);

                    // 將士兵加入士兵列表
                    soldiers.Add(soldier);

                    // 獲取士兵的 Soldier 腳本參考
                    Soldier soldierScript = soldier.GetComponent<Soldier>();
                    if (soldierScript != null)
                    {
                        soldierScript.MoveToDestination(target); // 移動士兵到指定的目的地
                        isAtking = true;
                    }

                    yield return new WaitForSeconds(soldierCdTime);
            }
        }
        isAtking = false;
    }
    public void GetDamage(int damage){
        num -= damage;
    }
}