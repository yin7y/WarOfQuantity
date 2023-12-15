using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UIElements;

public class MainCity : MonoBehaviour
{
    public int numSoldier;
    [SerializeField] int teamID;    // TODO
    public float cdTime;
    [SerializeField] float timer;
    [SerializeField] TextMeshPro numText;
    void Start()
    {
        numSoldier = 0;
        cdTime = 1f;
        numText.text = numSoldier.ToString();
    }

    void Update()
    {
        timer += Time.deltaTime;
        if(timer >= cdTime){
            numSoldier++;
            numText.text = numSoldier.ToString();
            timer = 0;
        }
    }
    
}
