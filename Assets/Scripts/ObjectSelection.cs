using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectSelection : MonoBehaviour
{
    GameObject selectedObject, firstSelectedObject;   // 目前被選中的物體，原先選中的物體    MainCity sameCity;
    [SerializeField] GameObject selectedHint;  // 選擇提示
    SelectionState currentState = SelectionState.None;
    enum SelectionState
    {
        None,
        FirstSelected
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            CheckObjectSelection();
        }
    }

    private void CheckObjectSelection()
    {
        Vector2 mousePosition = GetWorldMousePosition();
        RaycastHit2D hit = Physics2D.Raycast(mousePosition, Vector2.zero);

        if (hit.collider != null)
        {
            GameObject hitObject = hit.collider.gameObject;

            if (hitObject != selectedObject)
            {
                SelectObject(hitObject);
            }
            else
            {
                DeselectObject();
            }
        }
        else
        {
            DeselectObject();
        }
    }

    private void SelectObject(GameObject obj)
    {
        selectedObject = obj;

        switch (currentState)
        {
            case SelectionState.None:
                selectedHint.SetActive(true);
                firstSelectedObject = selectedObject;
                Debug.Log("選中物體：" + selectedObject.name);
                currentState = SelectionState.FirstSelected;
                break;
            case SelectionState.FirstSelected:
                selectedHint.SetActive(false);
                HandleSelection();
                DeselectObject();
                break;
        }
    }

    private void HandleSelection()
    {
        MainCity firstCity = firstSelectedObject.GetComponent<MainCity>();
        firstCity.SoldierGenerator(1000, selectedObject);
        Debug.Log("處理完畢");
    }

    private void DeselectObject()
    {
        if (selectedObject != null)
        {
            selectedHint.SetActive(false);
            Debug.Log("取消選中物體：" + selectedObject.name);
            selectedObject = null;
        }
        currentState = SelectionState.None;
        firstSelectedObject = null;
    }

    private Vector2 GetWorldMousePosition()
    {
        Vector3 mousePos = Input.mousePosition;
        mousePos.z = -Camera.main.transform.position.z;
        return Camera.main.ScreenToWorldPoint(mousePos);
    }
}