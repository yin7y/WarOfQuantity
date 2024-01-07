using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Soldier : MonoBehaviour
{
    private int teamID;
    private GameObject destination;
    private float moveSpeed = 20f;
    public string belongCityName;
    MainCity mainCity;

    void Start(){
        belongCityName = GetComponentInParent<MainCity>().nameText.text;;
        mainCity = transform.parent.gameObject.GetComponentInParent<MainCity>();
        gameObject.GetComponent<SpriteRenderer>().color = mainCity.GetComponent<SpriteRenderer>().color;
        // destination = mainCity.GetTargetCity(); 保留兵  隨時可更換目標型
    }
    void Update(){
        if (destination != null){
            // 向目的地移動
            transform.position = Vector3.MoveTowards
            (transform.position, destination.transform.position, moveSpeed * Time.deltaTime);
        }
    }
    
    public void MoveToDestination(GameObject target) => destination = target;    
    public void SetTeamID(int id) => teamID = id;    
    public GameObject GetTarget(){
        return destination;
    }
    public int GetTeamID(){
        return teamID;
    }
}