using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameGo : MonoBehaviour
{
    
    public void SceneChange()
    {
        SceneManager.LoadScene("SampleScene_Door1");
    }

// Start is called before the first frame update
void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
