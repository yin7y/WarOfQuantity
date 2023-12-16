using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.UIElements;

public class MainCity : MonoBehaviour
{
    public int num, teamID;
    public float numCdTime;
    [SerializeField] float timer;
    [SerializeField] TextMeshPro numText;
    [SerializeField] GameObject soldierPrefab;
    [SerializeField] Transform soldiersParent; // 用於整理士兵的父物件

    private List<GameObject> soldiers = new List<GameObject>(); // 士兵列表

    void Start()
    {
        num = 0;
        numCdTime = 1f;
        numText.text = num.ToString();
    }

    void Update()
    {
        timer += Time.deltaTime;
        if (timer >= numCdTime)
        {
            num++;
            numText.text = num.ToString();
            timer = 0;
        }
    }

    public void SoldierGenerator(int count, float cdTime, Vector3 dest)
    {
        StartCoroutine(GenerateSoldiers(count, cdTime, dest));
    }

    private IEnumerator GenerateSoldiers(int count, float cdTime, Vector3 dest)
    {
        for (int i = 0; i < count; i++)
        {
            // 生成士兵
            GameObject soldier = Instantiate(soldierPrefab, transform.position, Quaternion.identity);

            // 將士兵設定為指定父物件的子物體
            soldier.transform.SetParent(soldiersParent);

            // 將士兵加入士兵列表
            soldiers.Add(soldier);

            // 獲取士兵的 Soldier 腳本參考
            Soldier soldierScript = soldier.GetComponent<Soldier>();
            if (soldierScript != null)
            {
                soldierScript.MoveToDestination(dest); // 移動士兵到指定的目的地
            }

            yield return new WaitForSeconds(cdTime);
        }
    }
}