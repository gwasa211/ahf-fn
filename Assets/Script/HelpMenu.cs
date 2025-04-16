using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HelpMenu : MonoBehaviour
{
    public GameObject helpPanel;


    public void ToggleHelp()
    {

        bool isActive = helpPanel.activeSelf;
        helpPanel.SetActive(!isActive);
    }
}