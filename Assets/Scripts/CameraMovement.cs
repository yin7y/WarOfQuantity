using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float dragSpeed = 2f;
    public float edgeScrollSpeed = 10f;
    public float zoomSpeed = 5f;
    public float minZoom = -10f;
    public float maxZoom = -200f;
    public float edgeBoundary = 50f;
    bool isFullscreen = false;

    private Vector3 dragOrigin;
    private bool isDragging = false;
    [SerializeField] bool canMove;
    
    void Awake(){
        transform.position = new Vector3(-35,0,-120);
    }
    
    void Update(){
        if(canMove){
            if(Input.GetKeyDown(KeyCode.F))
                FindAndFocusMainCity();
            HandleKeyboardMovement();
            HandleMouseDrag();
            HandleEdgeScroll();
            HandleZoom();
        }
        if (Input.GetKeyDown(KeyCode.F11))
            ToggleFullscreen();
    }

    public void FindAndFocusMainCity(){
        MainCity[] mainCities = GameObject.FindObjectsOfType<MainCity>();

        foreach (MainCity city in mainCities){
            if (city.GetTeamID() == 0){
                Vector3 targetPosition = city.transform.position;
                targetPosition.z = transform.position.z; // 保持相機的 z 軸位置不變
                transform.position = targetPosition;
                break; // 找到第一個符合條件的城市後，停止迴圈
            }
        }
        transform.position = new Vector3(transform.position.x, transform.position.y, -80f);
    }
    void ToggleFullscreen(){
        isFullscreen = !isFullscreen;
        if (isFullscreen){
            //获取设置当前屏幕分辩率 
            Resolution[] resolutions = Screen.resolutions;
            //设置当前分辨率 
            Screen.SetResolution(resolutions[resolutions.Length - 1].width, resolutions[resolutions.Length - 1].height, true);
            Screen.fullScreenMode = FullScreenMode.FullScreenWindow;
        }
        else
            Screen.fullScreenMode = FullScreenMode.Windowed;
    }
    void HandleKeyboardMovement(){
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");

        Vector3 moveDirection = new Vector3(horizontalInput, verticalInput, 0f);
        transform.position += moveDirection * moveSpeed * Time.deltaTime;
    }

    void HandleMouseDrag(){
        if (Input.GetMouseButtonDown(2)){
            dragOrigin = Input.mousePosition;
            isDragging = true;
        }
        if (Input.GetMouseButtonUp(2))
            isDragging = false;
        if (isDragging){
            Vector3 currentMousePosition = Input.mousePosition;
            Vector3 dragDirection = dragOrigin - currentMousePosition;
            transform.position += dragDirection * dragSpeed * Time.deltaTime;
            dragOrigin = currentMousePosition;
        }
    }

    void HandleEdgeScroll(){
        float screenWidth = Screen.width;
        float screenHeight = Screen.height;
        Vector3 currentMousePosition = Input.mousePosition;

        if (currentMousePosition.x <= edgeBoundary)
            transform.position += Vector3.left * edgeScrollSpeed * Time.deltaTime;
        else if (currentMousePosition.x >= screenWidth - edgeBoundary)
            transform.position += Vector3.right * edgeScrollSpeed * Time.deltaTime;

        if (currentMousePosition.y <= edgeBoundary)
            transform.position += Vector3.down * edgeScrollSpeed * Time.deltaTime;
        else if (currentMousePosition.y >= screenHeight - edgeBoundary)
            transform.position += Vector3.up * edgeScrollSpeed * Time.deltaTime;
    }

    void HandleZoom(){
        float scrollInput = Input.GetAxis("Mouse ScrollWheel");
        Vector3 zoomDirection = new Vector3(0f, 0f, scrollInput);
        transform.position += zoomDirection * zoomSpeed;

        // 限制相機的 Z 座標在指定範圍內
        transform.position = new Vector3(transform.position.x, transform.position.y, Mathf.Clamp(transform.position.z, maxZoom, minZoom));
    }
}