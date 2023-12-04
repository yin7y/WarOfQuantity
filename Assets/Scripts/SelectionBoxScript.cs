using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectionBoxScript : MonoBehaviour
{
    private Vector2 startPos;
    private Rect selectionRect;
    private bool isSelecting = false;
    [SerializeField] private List<GameObject> selectedObjects;
    private GUIStyle selectionBoxStyle;
    private GUIStyle selectedObjectStyle;

    private void Start()
    {
        // 初始化樣式
        selectionBoxStyle = new GUIStyle();
        selectionBoxStyle.normal.background = MakeTexture(2, 2, new Color(0.1f, 0.1f, 0.1f, 0.2f));

        selectedObjectStyle = new GUIStyle();
        selectedObjectStyle.normal.background = MakeTexture(2, 2, new Color(0f, 0.5f, 0f, 0.2f));
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(1))
        {
            // 按下Ctrl時，保留原有選取的物體
            if (!Input.GetKey(KeyCode.LeftControl) && !Input.GetKey(KeyCode.RightControl))
            {
                selectedObjects.Clear();
            }

            // 記錄滑鼠按下的起始位置
            startPos = GetWorldMousePosition();
            isSelecting = true;
        }

        if (Input.GetMouseButtonUp(1))
        {
            // 當滑鼠右鍵釋放時，停止選取
            isSelecting = false;

            // 檢查是否有物體在選取框內
            CheckSelectedObjects();
        }

        if (isSelecting)
        {
            // 更新選取框的位置和大小
            Vector2 currentMousePos = GetWorldMousePosition();
            selectionRect = new Rect(
                Mathf.Min(startPos.x, currentMousePos.x),
                Mathf.Min(startPos.y, currentMousePos.y),
                Mathf.Abs(currentMousePos.x - startPos.x),
                Mathf.Abs(currentMousePos.y - startPos.y)
            );
        }

        // 取消選取物體
        if (Input.GetMouseButtonDown(0) && (Input.GetKey(KeyCode.LeftControl) || Input.GetKey(KeyCode.RightControl)))
        {
            GameObject clickedObject = GetClickedObject();
            if (clickedObject != null && selectedObjects.Contains(clickedObject))
            {
                selectedObjects.Remove(clickedObject);
            }
        }
        
    }

    private void OnGUI()
    {
        if (isSelecting)
        {
            // 繪製選取框
            GUI.Box(GetScreenRect(selectionRect), "", selectionBoxStyle);
        }

        // 繪製被選取物體的提示框
        foreach (GameObject obj in selectedObjects)
        {
            Rect objectRect = GetScreenRect(GetObjectBounds(obj));
            GUI.Box(objectRect, "", selectedObjectStyle);
        }
    }

    private void CheckSelectedObjects()
    {
        // 獲取所有在選取框內的物體
        Collider2D[] colliders = Physics2D.OverlapAreaAll(selectionRect.min, selectionRect.max);

        // 遍歷所有物體，找到可選擇的物體
        for (int i = 0; i < colliders.Length; i++)
        {
            GameObject obj = colliders[i].gameObject;

            // 檢查是否已經選取過，避免重複添加
            if (!selectedObjects.Contains(obj))
            {
                selectedObjects.Add(obj);
            }
        }
    }

    private Vector2 GetWorldMousePosition()
    {
        // 獲取滑鼠在世界座標中的位置
        Vector3 mousePos = Input.mousePosition;
        mousePos.z = -Camera.main.transform.position.z;
        return Camera.main.ScreenToWorldPoint(mousePos);
    }

    private Rect GetScreenRect(Rect worldRect)
    {
        // 將世界座標的矩形轉換為螢幕座標的矩形
        Vector3 min = Camera.main.WorldToScreenPoint(new Vector3(worldRect.xMin, worldRect.yMin, 0f));
        Vector3 max = Camera.main.WorldToScreenPoint(new Vector3(worldRect.xMax, worldRect.yMax, 0f));
        return new Rect(min.x, Screen.height - max.y, max.x - min.x, max.y - min.y);
    }

    private Texture2D MakeTexture(int width, int height, Color color)
    {
        // 創建純色紋理
        Color[] pixels = new Color[width * height];
        for (int i = 0; i < pixels.Length; i++)
        {
            pixels[i] = color;
        }

        Texture2D texture = new Texture2D(width, height);
        texture.SetPixels(pixels);
        texture.Apply();

        return texture;
    }

    private Rect GetObjectBounds(GameObject obj)
    {
        // 獲取物體的邊界框
        Renderer renderer = obj.GetComponent<Renderer>();
        if (renderer != null)
        {
            Bounds bounds = renderer.bounds;
            return new Rect(bounds.min.x, bounds.min.y, bounds.size.x, bounds.size.y);
        }
        else
        {
            // 如果物體沒有Renderer組件，則使用Collider的邊界框
            Collider2D collider = obj.GetComponent<Collider2D>();
            if (collider != null)
            {
                Bounds bounds = collider.bounds;
                return new Rect(bounds.min.x, bounds.min.y, bounds.size.x, bounds.size.y);
            }
        }

        // 如果物體沒有Renderer和Collider組件，則返回空的矩形
        return new Rect();
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