using System.Collections;
using System.Collections.Generic;
using UnityEditor.Scripting;
using UnityEngine;

public class Soldier : MonoBehaviour
{
    private int teamID;
    private GameObject destination;
    private float moveSpeed = 5f;
    MainCity mainCity;

    
    private void Start() {
        mainCity = transform.parent.gameObject.GetComponentInParent<MainCity>();
    }
    public void SetTeamID(int id)
    {
        teamID = id;
    }

    public void MoveToDestination(GameObject target)
    {
        destination = target;
        
    }

    private void Update()
    {
        // 向目的地移動
        transform.position = Vector3.MoveTowards(transform.position, destination.transform.position, moveSpeed * Time.deltaTime);

        // 檢查是否到達目的地
        if (transform.position == destination.transform.position)
        {
            // 到達目的地後執行任何必要的操作
            // Debug.Log("士兵到達目的地：" + destination);
            MainCity targetCity = destination.GetComponent<MainCity>();
            targetCity.GetDamage(1);
            
            mainCity.soldiers.Remove(gameObject);
            print(mainCity.soldiers.Count);
            if(mainCity.soldiers.Count == 0){
                targetCity.isDefending = false;
            }
            // 銷毀士兵物件
            Destroy(gameObject);
        }
    }
    
}
