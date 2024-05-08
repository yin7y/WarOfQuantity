using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CityGenerator : MonoBehaviour
{
    ushort playingNum = 5, landNum = 10;
    public GameObject cityPrefab, landPrefab;
    public ushort numberOfCities, mapRange = 50;
    public float cityMinDistance = 20f;
    public bool watchMode, autoNextMode;
    [SerializeField] CameraMovement myCamera;
    [SerializeField] SectNameGenerator randomName;
    
    bool canGenerate;

    void Awake(){
        myCamera = GameObject.FindWithTag("MainCamera").GetComponent<CameraMovement>();
        randomName = GetComponent<SectNameGenerator>();
        randomName.LoadPrefixes();
        
        mapRange = Menu.playMapRange;
        playingNum = Menu.playNum;
        landNum = Menu.landNum;
        
        if(playingNum < 2)
            playingNum = numberOfCities;
        myCamera.boundarySize = new Vector2(mapRange*2, mapRange*2);
        if(SceneManager.GetActiveScene().name == "Game")
            GenerateCities(playingNum);
        if(SceneManager.GetActiveScene().name == "Game2")
            GenerateCities(playingNum, landNum);
        if(SceneManager.GetActiveScene().name == "Guide"){
            mapRange = 40;
            GenerateCities(2, 5);
        }
    }
    void Start(){
        if(!watchMode)
            myCamera.FindAndFocusMainCity();
    }

    public void GenerateCities(int _num){
        // 建立 Cities 物件
        GameObject citiesObject = new("MainCities");
        int setCities = watchMode? _num+1 : _num;
        
        for (ushort i = (ushort)(watchMode ? 1 : 0); i < setCities; i++){
            Vector3 randomCityPosition = GetRandomPosition(cityMinDistance);
            if(!canGenerate) continue;
            GameObject city = Instantiate(cityPrefab, randomCityPosition, Quaternion.identity);

            // 分配隊伍 teamID 給城市
            ushort teamID = (ushort)((i == 0) ? 0 : i); // 玩家陣營 teamID 為 0，其他城市隨機分配 1 到 3 之間的 teamID
            city.GetComponent<MainCity>().SetTeamID(teamID);

            // 分配隨機顏色給城市
            Color randomColor = UnityEngine.Random.ColorHSV(0f, 1f, 0.4f, 1f, 0.4f, 1f); // 控制亮色範圍
            city.GetComponent<SpriteRenderer>().color = randomColor;

            // 將城市設定為 Cities 物件的子物件
            city.transform.SetParent(citiesObject.transform);
            
            // 隨機生成城市名稱
            city.GetComponent<MainCity>().nameText.text = randomName.GenerateRandomSectName();
            city.GetComponent<MainCity>().nameText.color = randomColor;
            
            // 重新命名城市物件
            city.name = "MainCity" + teamID;
        }
    }

    public void GenerateCities(int _num, int _landnum){
        // 建立 Cities 物件
        GameObject citiesObject = new("MainCities");
        int setCities = watchMode? _num+1 : _num;
        
        for (int i = watchMode? 1 : 0; i < setCities; i++){
            Vector3 randomCityPosition = GetRandomPosition(cityMinDistance);
            if(!canGenerate) continue;
            
            GameObject city = Instantiate(cityPrefab, randomCityPosition, Quaternion.identity);
            
            // 分配隊伍 teamID 給城市
            ushort teamID = (ushort)((i == 0) ? 0 : i); // 玩家陣營 teamID 為 0，其他城市隨機分配 1 到 3 之間的 teamID
            city.GetComponent<MainCity>().SetTeamID(teamID);

            // 分配隨機顏色給城市
            Color randomColor = UnityEngine.Random.ColorHSV(0f, 1f, 0.4f, 1f, 0.4f, 1f); // 控制亮色範圍
            city.GetComponent<SpriteRenderer>().color = randomColor;

            // 將城市設定為 Cities 物件的子物件
            city.transform.SetParent(citiesObject.transform);
            
            // 隨機生成城市名稱
            city.GetComponent<MainCity>().nameText.text = randomName.GenerateRandomSectName();
            city.GetComponent<MainCity>().nameText.color = randomColor;
            
            // 重新命名城市物件
            city.name = "MainCity" + teamID;
        }
        for (int i = 0; i < _landnum; i++){
            Vector3 randomCityPosition = GetRandomPosition(cityMinDistance);
            if(!canGenerate) continue;
            
            Instantiate(landPrefab, randomCityPosition, Quaternion.identity);

            
        }
    }
    Vector3 GetRandomPosition(float minDistance){
        canGenerate = true;
        Vector3 randomPosition = Vector3.zero;
        bool isValidPosition = false;
        byte counter = 0;
        while (!isValidPosition){
            if(counter == 10){
                canGenerate = false;
                // Debug.Log("O")
                break;
            }
            randomPosition = new Vector3(UnityEngine.Random.Range(-mapRange, mapRange), UnityEngine.Random.Range(-mapRange, mapRange), 0);

            // 檢查與其他城市的距離是否足夠
            isValidPosition = true;
            GameObject[] cities = GameObject.FindGameObjectsWithTag("City");
            foreach (GameObject city in cities){
                float distance = Vector3.Distance(randomPosition, city.transform.position);
                if (distance < minDistance){
                    isValidPosition = false;
                    counter++;
                    break;
                }
            }            
        }
        return randomPosition;
    }
}