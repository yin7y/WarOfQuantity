using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectSelection : MonoBehaviour
{
    GameObject selectedObject, firstSelectedObject;   // 目前被選中的物體，原先選中的物體
    MainCity sameCity;
    [SerializeField] GameObject selectedHint;  // 選擇提示
    SelectionState currentState = SelectionState.None;
    enum SelectionState{
        None,
        FirstSelected
    }
    
    private void Update(){
        if (Input.GetMouseButtonDown(0)){
            // 射線檢測
            Vector2 mousePosition = GetWorldMousePosition();
            RaycastHit2D hit = Physics2D.Raycast(mousePosition, Vector2.zero);

            if (hit.collider != null){
                GameObject hitObject = hit.collider.gameObject;

                // 檢查是否點擊到新的物體
                if (hitObject != selectedObject){
                    // 選擇新的物體           
                    SelectObject(hitObject);
                }else{
                    // 取消選擇
                    DeselectObject();
                }
            }else{
                // 沒有點擊到物體，取消選擇
                DeselectObject();
            }
        }
    }

    private void SelectObject(GameObject obj){
        // 選擇新的物體
        selectedObject = obj;
        // 在這裡可以執行選擇物體後的操作，比如變色、顯示選中效果等
        // 取得被選中obj的MainCity腳本
        
       
        switch (currentState){
            case SelectionState.None:
                selectedHint.SetActive(true);
                firstSelectedObject = selectedObject; // 記錄第一次選中的物體
                Debug.Log("選中物體：" + selectedObject.name);
                currentState = SelectionState.FirstSelected;
                break;
            case SelectionState.FirstSelected:
                selectedHint.SetActive(false);
                
                MainCity firstCity = firstSelectedObject.GetComponent<MainCity>();
                firstCity.SoldierGenerator(99, selectedObject);
                // TDOO不可再選中同樣的城市發兵
                
                
                Debug.Log("處理完畢");
                // 處理完所有事項後取消選取
                DeselectObject();
                break;
        }
    }

    private void DeselectObject(){
        if (selectedObject != null){
            // 取消選擇物體
            // 在這裡可以撤銷選擇物體後的操作，比如恢復顏色、隱藏選中效果等
            selectedHint.SetActive(false);
            Debug.Log("取消選中物體：" + selectedObject.name);
            selectedObject = null;
        }
        currentState = SelectionState.None;
        firstSelectedObject = null; // 清空第一次選中的物體
    }
    
    
    
    private Vector2 GetWorldMousePosition(){
        // 獲取滑鼠在世界座標中的位置
        Vector3 mousePos = Input.mousePosition;
        mousePos.z = -Camera.main.transform.position.z;
        return Camera.main.ScreenToWorldPoint(mousePos);
    }
}