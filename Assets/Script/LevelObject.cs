using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using static UnityEngine.AudioSettings;

public class LevelObject : MonoBehaviour
{
    public string nextLevel;

    public void MoveToNextLevel()
    {
        SceneManager.LoadScene(nextLevel);
    }
}
