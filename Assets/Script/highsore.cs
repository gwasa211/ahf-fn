using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class highsore : MonoBehaviour
{
    public TextMeshProUGUI Level_1; 
    public TextMeshProUGUI Level_2;
    public TextMeshProUGUI Level_3;
    public TextMeshProUGUI Level_4;
    public TextMeshProUGUI Level_5;
    public TextMeshProUGUI Level_6;
    // Start is called before the first frame update
    void Start()
    {
        Level_1.text = "Level_1 :" + HighScore.Load(1).ToString();
        Level_2.text = "Level_2 :" + HighScore.Load(2).ToString();
        Level_3.text = "Level_3 :" + HighScore.Load(3).ToString();
        Level_4.text = "Level_4 :" + HighScore.Load(4).ToString();
        Level_5.text = "Level_5 :" + HighScore.Load(5).ToString();
        Level_5.text = "Level_6 :" + HighScore.Load(6).ToString();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
