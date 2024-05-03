using System.Collections.Generic;
using UnityEngine;

public class BoxSelection : MonoBehaviour
{
    private Vector2 startPoint;
    private Vector2 endPoint;
    private bool isSelecting = false;

    private GUIStyle boxStyle;

    void Start()
    {
        boxStyle = new GUIStyle();
        boxStyle.normal.background = MakeTexture(2, 2, new Color(1f, 1f, 1f, 0.2f));
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(1))
        {
            startPoint = Input.mousePosition;
            isSelecting = true;
        }

        if (isSelecting && Input.GetMouseButton(1))
        {
            endPoint = Input.mousePosition;
        }

        if (Input.GetMouseButtonUp(1))
        {
            isSelecting = false;
            SelectObjectsInBox(startPoint, endPoint);
        }
    }

    void OnGUI()
    {
        if (isSelecting)
        {
            Vector2 currentPoint = Event.current.mousePosition;

            Rect selectionRect = new Rect(
                Mathf.Min(startPoint.x, currentPoint.x),
                Mathf.Min(startPoint.y, currentPoint.y),
                Mathf.Abs(startPoint.y - currentPoint.x),
                Mathf.Abs(startPoint.y - currentPoint.y)
            );

            GUI.Box(selectionRect, GUIContent.none, boxStyle);
        }
    }

    void SelectObjectsInBox(Vector2 startPoint, Vector2 endPoint)
    {
        Bounds selectionBounds = new Bounds(
            new Vector2(
                (startPoint.x + endPoint.x) / 2,
                (startPoint.y + endPoint.y) / 2
            ),
            new Vector2(
                Mathf.Abs(startPoint.x - endPoint.x),
                Mathf.Abs(startPoint.y - endPoint.y)
            )
        );

        Collider2D[] colliders = Physics2D.OverlapAreaAll(startPoint, endPoint);

        List<GameObject> mainCityObjects = new List<GameObject>();
        foreach (Collider2D collider in colliders)
        {
            if (collider.GetComponent<MainCity>() != null)
            {
                mainCityObjects.Add(collider.gameObject);
            }
        }

        foreach (GameObject mainCityObject in mainCityObjects)
        {
            // 執行你需要的函式
            // mainCityObject.GetComponent<MainCity>().YourFunction();
            print("S");
        }
    }

    Texture2D MakeTexture(int width, int height, Color color)
    {
        Color[] pixels = new Color[width * height];
        for (int i = 0; i < pixels.Length; ++i)
        {
            pixels[i] = color;
        }

        Texture2D texture = new Texture2D(width, height);
        texture.SetPixels(pixels);
        texture.Apply();

        return texture;
    }
}