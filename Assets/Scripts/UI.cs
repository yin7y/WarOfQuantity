using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI : MonoBehaviour
{
    [SerializeField] Text cityCountText;
    
    public void UpdateCityCount(int count)
    {
        cityCountText.text = "主城:" + count.ToString();
    }
}