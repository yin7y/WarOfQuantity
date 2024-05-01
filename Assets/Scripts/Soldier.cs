using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Soldier : MonoBehaviour
{
    private ushort teamID;
    private GameObject destination;
    private float moveSpeed = 20f;
    public string belongCityName;
    MainCity mainCity;

    void Start(){
        belongCityName = GetComponentInParent<MainCity>().nameText.text;;
        mainCity = transform.parent.gameObject.GetComponentInParent<MainCity>();
        gameObject.GetComponent<SpriteRenderer>().color = mainCity.GetComponent<SpriteRenderer>().color;
    }
    void Update(){
        if (destination != null){
            // 向目的地移動
            transform.position = Vector3.MoveTowards
            (transform.position, destination.transform.position, moveSpeed * Time.deltaTime);
        }
    }
    
    public void MoveToDestination(GameObject target) => destination = target;    
    public void SetTeamID(ushort id) => teamID = id;    
    public GameObject GetTarget(){
        return destination;
    }
    public ushort GetTeamID(){
        return teamID;
    }
}