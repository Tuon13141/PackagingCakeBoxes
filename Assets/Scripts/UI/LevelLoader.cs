using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelLoader : MonoBehaviour
{
    public Text levelName;

    public void ChangeScene()
    {
        if(levelName != null)
        {
            PlayerPrefs.SetInt("Level", int.Parse(levelName.text));
        }
        
        SceneManager.LoadScene("PlayScene");
    }
}
