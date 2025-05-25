using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class asdadasd : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
        string[] array = { "사과", "배", "포도", "딸기", "바나나" };

        
        foreach (string item in array)
        {
            
            Debug.Log(item);
        }

        
        string[] array1 = { "사과", "배", "포도", "딸기", "바나나" };

        
        foreach (var item in array1)
        {
            
            Debug.Log(item);
        }

        string stars;
        for (int n = 0; n < 10; n++)
        {
            stars = "";
            for (int o = 0; o < n + 1; o++)
            {
                stars += "*";
            }
            Debug.Log(stars);
        }

        for (int n = 0; n < 10; n++)
        {
            string stars1 = new string(' ', 9 - n); 
            stars1 += new string('*', n + 1); 
            Debug.Log(stars1);
        }

        for (int j = 1; j < 10; j++)
        {
            if (j % 2 != 0) 
            {
                Debug.Log(j); 
            }
        }

        string input = "Potato Tomato";
        Debug.Log(input.ToUpper()); 
        Debug.Log(input.ToLower());

        string input3 = " test  #\n";
        Debug.Log("::" + input3.Trim() + "::");

        string input2 = "감자 고구마 토마토";
        string[] inputs = input2.Split(new char[] { ' ' }); 

        foreach (var item in inputs)
        {
            Debug.Log(item); 
        }

        
        string[] array3 = { "감자", "고구마", "토마토", "가지" };
        Debug.Log(string.Join(",", array3)); 







    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
