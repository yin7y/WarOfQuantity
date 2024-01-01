using UnityEngine;

public class CityGenerator : MonoBehaviour
{
    public GameObject cityPrefab;
    public int numberOfCities = 5;
    public float cityMinDistance = 20f;
    public int mapRange;
    [SerializeField] bool watchMode, autoNextMode;
    [SerializeField] CameraMovement myCamera;

    void Awake()
    {
        GenerateCities();
        if(!watchMode)
            myCamera.FindAndFocusMainCity();
        else{
            myCamera.gameObject.transform.position = new Vector3(-35,0,-110);
        }
    }

    public void GenerateCities()
    {
        // 建立 Cities 物件
        GameObject citiesObject = new("MainCities");
        int setCities = watchMode? numberOfCities+1 : numberOfCities;
        
        for (int i = watchMode? 1 : 0; i < setCities; i++)
        {
            Vector3 randomCityPosition = GetRandomPosition(cityMinDistance);
            GameObject city = Instantiate(cityPrefab, randomCityPosition, Quaternion.identity);

            // 分配隊伍 teamID 給城市
            int teamID = (i == 0) ? 0 : i; // 玩家陣營 teamID 為 0，其他城市隨機分配 1 到 3 之間的 teamID
            city.GetComponent<MainCity>().SetTeamID(teamID);

            // 分配隨機顏色給城市
            Color randomColor = Random.ColorHSV(0f, 1f, 0.5f, 1f, 0.5f, 1f); // 控制亮色範圍
            city.GetComponent<SpriteRenderer>().color = randomColor;

            // 將城市設定為 Cities 物件的子物件
            city.transform.SetParent(citiesObject.transform);

            // 重新命名城市物件
            city.name = "MainCity" + teamID;
        }
    }

    Vector3 GetRandomPosition(float minDistance)
    {
        Vector3 randomPosition = Vector3.zero;
        bool isValidPosition = false;

        while (!isValidPosition)
        {
            randomPosition = new Vector3(Random.Range(-mapRange, mapRange), Random.Range(-mapRange, mapRange), 0);

            // 檢查與其他城市的距離是否足夠
            isValidPosition = true;
            GameObject[] cities = GameObject.FindGameObjectsWithTag("City");
            foreach (GameObject city in cities)
            {
                float distance = Vector3.Distance(randomPosition, city.transform.position);
                if (distance < minDistance)
                {
                    isValidPosition = false;
                    break;
                }
            }
        }

        return randomPosition;
    }
}