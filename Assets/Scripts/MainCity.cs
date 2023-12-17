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
    public bool isDefending, canDefendingOther;
    bool isAtking;
    [SerializeField] float timer;
    [SerializeField] TextMeshPro numText;
    [SerializeField] GameObject soldierPrefab;
    [SerializeField] Transform soldiersParent; // 用於整理士兵的父物件

    public List<GameObject> soldiers = new List<GameObject>(); // 士兵列表

    void Start()
    {
        num = 0;
        numCdTime = 0.5f;
        soldierCdTime = 0.1f;
        numText.text = num.ToString();
        isAtking = false;
        isDefending = false;
    }

    void Update()
    {
        timer += Time.deltaTime;
        if(isDefending){   // 城市正在防守的話，數量產能減半
            if (timer >= numCdTime){
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
        StartCoroutine(GenerateSoldiers(count, target));
    }

    private IEnumerator GenerateSoldiers(int count, GameObject target)
    {
        int setCount = count;
        if(setCount >= num)
            setCount = num - 1;
        if(gameObject.GetComponent<MainCity>().isAtking == false && target.GetComponent<MainCity>().isDefending == false || gameObject.GetComponent<MainCity>().isAtking == false && target.GetComponent<MainCity>().isDefending == true)
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
                    target.GetComponent<MainCity>().isDefending = true; // 指定城正在被發兵
                    yield return new WaitForSeconds(soldierCdTime);
                }
            }
        isAtking = false;
    }
    public void GetDamage(int damage){
        num -= damage;
    }
}