using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectSelection : MonoBehaviour
{
    private GameObject selectedObject; // 目前被選中的物體
    [SerializeField] GameObject selectedHint;
    private void Start() {
        
    }
    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            // 射線檢測
            Vector2 mousePosition = GetWorldMousePosition();
            RaycastHit2D hit = Physics2D.Raycast(mousePosition, Vector2.zero);

            if (hit.collider != null)
            {
                GameObject hitObject = hit.collider.gameObject;

                // 檢查是否點擊到新的物體
                if (hitObject != selectedObject)
                {
                    // 選擇新的物體
                    
                    SelectObject(hitObject);
                }
                else
                {
                    // 取消選擇
                    DeselectObject();
                }
            }
            else
            {
                // 沒有點擊到物體，取消選擇
                DeselectObject();
            }
        }
    }

    private void SelectObject(GameObject obj)
    {
        // 取消之前選中的物體
        DeselectObject();
        // 選擇新的物體
        selectedObject = obj;
        // 在這裡可以執行選擇物體後的操作，比如變色、顯示選中效果等
        // 取得被選中obj的MainCity腳本
        MainCity mainCity = obj.GetComponent<MainCity>();
        
        if(mainCity != null){
            selectedHint.SetActive(true);   // TOOO 根據teamID是否相同 否則false
        }
        Debug.Log("選中物體：" + selectedObject.name);
    }

    private void DeselectObject()
    {
        if (selectedObject != null)
        {
            // 取消選擇物體
            // 在這裡可以撤銷選擇物體後的操作，比如恢復顏色、隱藏選中效果等
            selectedHint.SetActive(false);
            Debug.Log("取消選中物體：" + selectedObject.name);
            selectedObject = null;
        }
    }
    private Vector2 GetWorldMousePosition()
    {
        // 獲取滑鼠在世界座標中的位置
        Vector3 mousePos = Input.mousePosition;
        mousePos.z = -Camera.main.transform.position.z;
        return Camera.main.ScreenToWorldPoint(mousePos);
    }
    private GameObject GetClickedObject()
    {
        // 獲取滑鼠點擊的物體
        RaycastHit2D hit = Physics2D.Raycast(GetWorldMousePosition(), Vector2.zero);
        if (hit.collider != null)
        {
            return hit.collider.gameObject;
        }
        return null;
    }
}

