using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;

public class SectNameGenerator : MonoBehaviour
{
    public int numberOfNames = 5; // 生成的門派名稱數量
    public string prefixesFilePath; // 前綴文字檔的路徑
    public string[] suffixes; // 後綴字陣列

    private List<string> prefixes;

    void Start()
    {
        LoadPrefixes();

        List<string> sectNames = GenerateSectNames(numberOfNames);

        // 打印生成的門派名稱
        foreach (string name in sectNames)
        {
            Debug.Log("門派名稱：" + name);
        }
    }

    void LoadPrefixes()
    {
        prefixes = LoadWordsFromFile(prefixesFilePath);
    }

    List<string> LoadWordsFromFile(string filePath)
    {
        List<string> words = new List<string>();

        if (File.Exists(filePath))
        {
            string[] lines = File.ReadAllLines(filePath);
            words.AddRange(lines);
        }
        else
        {
            Debug.LogError("找不到文字檔：" + filePath);
        }

        return words;
    }

    List<string> GenerateSectNames(int count)
    {
        List<string> sectNames = new List<string>();

        for (int i = 0; i < count; i++)
        {
            string sectName = GenerateRandomSectName();
            sectNames.Add(sectName);
        }

        return sectNames;
    }

    string GenerateRandomSectName()
    {
        // 在這裡根據你的需求生成隨機的門派名稱
        // 可以使用各種算法、數據結構或外部資源來生成名稱

        // 這裡只是一個示例，你可以根據自己的需求進行修改
        string[] availableSuffixes = { "門", "派", "宗", "仙宗", "劍宗", "聖地", "谷", "堂", "教", "殿", "天朝", "上國", "仙朝", "聖庭", "古國", "山", "盟", "聯盟", "島"};

        int prefixLength = Random.Range(1, 4);
        List<string> selectedPrefixes = new List<string>();

        for (int i = 0; i < prefixLength; i++)
        {
            int prefixIndex = Random.Range(0, prefixes.Count);
            string selectedPrefix = prefixes[prefixIndex];
            selectedPrefixes.Add(selectedPrefix);
        }

        string prefix = string.Join("", selectedPrefixes);

        string suffix = availableSuffixes[Random.Range(0, availableSuffixes.Length)];

        string sectName = prefix + suffix;

        return sectName;
    }
}