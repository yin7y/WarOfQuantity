using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectSelection : MonoBehaviour
{
    GameObject selectedObject, firstSelectedObject;   // 目前被選中的物體，原先選中的物體    MainCity sameCity;
    [SerializeField] GameObject selectedHint;  // 選擇提示
    [SerializeField] bool debugMode;
    UI ui;
    SelectionState currentState = SelectionState.None;
    enum SelectionState
    {
        None,
        FirstSelected
    }
    void Start() {
        ui = FindObjectOfType<UI>();
    }
    private void Update(){
        if (Input.GetMouseButtonDown(0) && ui.canSelect){
            CheckObjectSelection();
        }
    }

    private void CheckObjectSelection(){
        Vector2 mousePosition = GetWorldMousePosition();
        RaycastHit2D hit = Physics2D.Raycast(mousePosition, Vector2.zero);

        if (hit.collider != null){
            GameObject hitObject = hit.collider.gameObject;
            if (hitObject != selectedObject && hitObject.CompareTag("City")){
                SelectObject(hitObject);
            }else{
                DeselectObject();
            }
        }else{
            DeselectObject();
        }
    }

    private void SelectObject(GameObject obj){
        selectedObject = obj;

        switch (currentState){
            case SelectionState.None:
                if(debugMode){
                    selectedHint.SetActive(true);
                    firstSelectedObject = selectedObject;
                    Debug.Log(" =========== 選中物體：" + selectedObject.name + " ===========");
                    currentState = SelectionState.FirstSelected;
                }else if(selectedObject.GetComponent<MainCity>().GetTeamID() == 0){
                    selectedHint.SetActive(true);
                    firstSelectedObject = selectedObject;
                    Debug.Log(" =========== 選中物體：" + selectedObject.name + " ===========");
                    currentState = SelectionState.FirstSelected;
                }
                break;
            case SelectionState.FirstSelected:
                selectedHint.SetActive(false);
                HandleSelection();
                DeselectObject();
                break;
        }
    }

    private void HandleSelection(){
        MainCity firstCity = firstSelectedObject.GetComponent<MainCity>();
        firstCity.SoldierGenerator(firstCity.GetNum() - 1, selectedObject);
        Debug.Log(" > 處理完畢 <");
    }

    private void DeselectObject(){
        if(selectedObject != null)
            if(debugMode){
                selectedHint.SetActive(false);
                Debug.Log("取消選中物體：" + selectedObject.name);
                
            }else if (selectedObject.GetComponent<MainCity>().GetTeamID() == 0){
                selectedHint.SetActive(false);
                Debug.Log("取消選中物體：" + selectedObject.name);
            }
        selectedObject = null;
        firstSelectedObject = null;
        currentState = SelectionState.None;        
    }

    private Vector2 GetWorldMousePosition(){
        Vector3 mousePos = Input.mousePosition;
        mousePos.z = -Camera.main.transform.position.z;
        return Camera.main.ScreenToWorldPoint(mousePos);
    }
}