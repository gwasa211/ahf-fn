using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine;

using UnityEngine.UI;


public class GameManager : MonoBehaviour
{
    public TMP_InputField inputField;
    public Button gameStartButton; // 오타 수정

    private void Start()
    {
        gameStartButton.onClick.AddListener(OnGameStartButtonClicked); // 오타 수정
    }

    private void OnGameStartButtonClicked() // 오타 수정
    {
        string playerName = inputField.text;
        if (string.IsNullOrEmpty(playerName))
        {
            Debug.Log("플레이어 이름을 입력하세요."); // 맞춤법 수정
            return;
        }

        PlayerPrefs.SetString("PlayerName", playerName); // 두 번째 인자 누락 수정
        PlayerPrefs.Save();

        Debug.Log("플레이어 이름 저장됨: " + playerName);

        SceneManager.LoadScene("Level_1");
    }
}

