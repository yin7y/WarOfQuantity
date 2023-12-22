using System.Collections;
using System.Collections.Generic;
using UnityEditor.Scripting;
using UnityEngine;

public class Soldier : MonoBehaviour
{
    private int teamID;
    private GameObject destination;
    private float moveSpeed = 20f;
    MainCity mainCity;

    
    private void Start(){
        mainCity = transform.parent.gameObject.GetComponentInParent<MainCity>();
        gameObject.GetComponent<SpriteRenderer>().color = mainCity.GetComponent<SpriteRenderer>().color;
        // destination = mainCity.GetTargetCity(); 保留兵  隨時可更換目標型
    }
    public void SetTeamID(int id){
        teamID = id;
    }

    public void MoveToDestination(GameObject target){
        destination = target;
    }

    private void Update()
    {
        if(destination != null){
            // 向目的地移動
            transform.position = Vector3.MoveTowards
            (transform.position, destination.transform.position, moveSpeed * Time.deltaTime);

            // 檢查是否到達目的地
            if (transform.position == destination.transform.position){
                // 到達目的地後執行任何必要的操作
                // Debug.Log("士兵到達目的地：" + destination);
                MainCity targetCity = destination.GetComponent<MainCity>();
                if(targetCity.GetTeamID() != mainCity.GetTeamID())
                    targetCity.GetDamage(1);
                else
                    targetCity.GetDamage(-1);
                    
                // mainCity.soldiers.Remove(gameObject);
                // Debug.Log("移除列表兵號 " + gameObject.name);
                // print(mainCity.soldiers.Count);
                if(transform.parent.childCount < 2){
                    targetCity.isDefending = false;
                }
                if(targetCity.GetNum() <= 0){
                    targetCity.SetNum(0);
                    targetCity.SetTeamID(teamID);
                    targetCity.gameObject.GetComponent<SpriteRenderer>().color = mainCity.GetComponent<SpriteRenderer>().color;
                }
                // 銷毀士兵物件
                Destroy(gameObject);
            }
        }
    }
    
}
