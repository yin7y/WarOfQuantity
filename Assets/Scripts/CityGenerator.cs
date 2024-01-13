using UnityEngine;

public class CityGenerator : MonoBehaviour
{
    int playingNum;
    public GameObject cityPrefab;
    public int numberOfCities, mapRange;
    public float cityMinDistance = 20f;
    public bool watchMode, autoNextMode;
    [SerializeField] CameraMovement myCamera;
    [SerializeField] SectNameGenerator randomName;


    void Awake(){
        randomName = GetComponent<SectNameGenerator>();
        randomName.LoadPrefixes();
        playingNum = Menu.playNum;
        if(playingNum == 0)
            playingNum = numberOfCities;
        GenerateCities();
    }
    void Start(){
        if(!watchMode)
            myCamera.FindAndFocusMainCity();
    }

    public void GenerateCities(){
        // 建立 Cities 物件
        GameObject citiesObject = new("MainCities");
        int setCities = watchMode? playingNum+1 : playingNum;
        
        for (int i = watchMode? 1 : 0; i < setCities; i++){
            Vector3 randomCityPosition = GetRandomPosition(cityMinDistance);
            GameObject city = Instantiate(cityPrefab, randomCityPosition, Quaternion.identity);

            // 分配隊伍 teamID 給城市
            int teamID = (i == 0) ? 0 : i; // 玩家陣營 teamID 為 0，其他城市隨機分配 1 到 3 之間的 teamID
            city.GetComponent<MainCity>().SetTeamID(teamID);

            // 分配隨機顏色給城市
            Color randomColor = Random.ColorHSV(0f, 1f, 0.4f, 1f, 0.4f, 1f); // 控制亮色範圍
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

    Vector3 GetRandomPosition(float minDistance){
        Vector3 randomPosition = Vector3.zero;
        bool isValidPosition = false;

        while (!isValidPosition){
            randomPosition = new Vector3(Random.Range(-mapRange, mapRange), Random.Range(-mapRange, mapRange), 0);

            // 檢查與其他城市的距離是否足夠
            isValidPosition = true;
            GameObject[] cities = GameObject.FindGameObjectsWithTag("City");
            foreach (GameObject city in cities){
                float distance = Vector3.Distance(randomPosition, city.transform.position);
                if (distance < minDistance){
                    isValidPosition = false;
                    break;
                }
            }
        }
        return randomPosition;
    }
}