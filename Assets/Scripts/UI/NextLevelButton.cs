using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NextLevelButton : MonoBehaviour
{
    public void ChangeToNextLevel()
    {
        int curentLevel = PlayerPrefs.GetInt("CurrentLevel");
        
        PlayerPrefs.SetInt("CurrentLevel", curentLevel+1);
    }
}
