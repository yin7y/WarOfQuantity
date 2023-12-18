using UnityEngine;

public class CityGenerator : MonoBehaviour
{
    public GameObject cityPrefab;
    public int numberOfCities = 5;
    public float cityMinDistance = 20f;

    void Start()
    {
        GenerateCities();
    }

    void GenerateCities()
    {
        for (int i = 0; i < numberOfCities; i++)
        {
            Vector3 randomCityPosition = GetRandomPosition(cityMinDistance);
            GameObject city = Instantiate(cityPrefab, randomCityPosition, Quaternion.identity);

            // 分配隊伍teamID給城市
            int teamID = (i == 0) ? 0 : Random.Range(1, 4); // 玩家陣營teamID為0，其他城市隨機分配1到3之間的teamID
            city.GetComponent<MainCity>().SetTeamID(teamID);

            // 分配隨機顏色給城市
            Color randomColor = Random.ColorHSV(0f, 1f, 0.8f, 1f, 0.8f, 1f); // 控制亮色範圍
            city.GetComponent<SpriteRenderer>().color = randomColor;
        }
    }

    Vector3 GetRandomPosition(float minDistance)
    {
        Vector3 randomPosition = Vector3.zero;
        bool isValidPosition = false;

        while (!isValidPosition)
        {
            randomPosition = new Vector3(Random.Range(-100f, 100f), Random.Range(-100f, 100f), 0);

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