using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;

public class SectNameGenerator : MonoBehaviour
{
    private List<string> prefixes;
    [SerializeField] TextAsset txtFile; 
    public void LoadPrefixes(){
        // 將文本內容按行分割成字串陣列
        string[] lines = txtFile.text.Split('\n');
        
        // 將字串陣列轉換為List<string>
        List<string> textLines = new List<string>(lines);
        
        prefixes = textLines;
    }

    public string GenerateRandomSectName(){
        string[] availableSuffixes = { 
            "門", "派", "閣", "宗", "谷", "堂", "教", "殿", "獄", "山", "城", "國", "盟", "島", "觀", "寨", "堡", "府", "樓", "宮", "塔", "會", "幫", 
            "仙宗", "劍宗", "魔宗", "上宗", "神宗", "妖宗", "古宗", "道宗", "聖地", "天朝", "帝朝", "皇朝", "王朝", "仙朝", "聖朝", "古朝", "帝國", "上國", "天國", "聖庭", "妖庭", "天庭", "王國", "古國", "仙國", "神國", 
            "聯盟", "仙盟", "仙島", "仙山", "神山", "妖山", "聖山", "仙閣", "魔教", "邪教", "仙門", "仙城", "仙域", "魔域", "神域", "聖域", "鬼域", "仙府", "天府", "仙殿", "魔殿", "古殿", "山莊", "龍宮", "仙宮", "天宮"};

        int prefixLength = Random.Range(1, 3);
        List<string> selectedPrefixes = new List<string>();

        for (int i = 0; i < prefixLength; i++){
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