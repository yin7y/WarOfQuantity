using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Soldier : MonoBehaviour
{
    private int teamID;
    private Vector3 destination;
    private float moveSpeed = 5f;

    public void SetTeamID(int id)
    {
        teamID = id;
    }

    public void MoveToDestination(Vector3 dest)
    {
        destination = dest;
    }

    private void Update()
    {
        // 向目的地移動
        transform.position = Vector3.MoveTowards(transform.position, destination, moveSpeed * Time.deltaTime);

        // 檢查是否到達目的地
        if (transform.position == destination)
        {
            // 到達目的地後執行任何必要的操作
            Debug.Log("士兵到達目的地：" + destination);

            // 銷毀士兵物件
            Destroy(gameObject);
        }
    }
    
}
