using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelButton : MonoBehaviour
{
    [SerializeField]
    private Text levelText;
    [SerializeField]
    private Image levelLock;
    [SerializeField]
    private ChangeScene changeScene;
    [SerializeField]
    private List<Image> starList;

    public Sprite starWin;
    public Sprite starLose;
    public void SetLevel()
    {
        int level = int.Parse(levelText.text);
        if(level > PlayerPrefs.GetInt("FinishedLevel") + 1) return;
        PlayerPrefs.SetInt("CurrentLevel", level);
        changeScene.changeScene();
    }

    public void SetLevelText(string levelText)
    {
        this.levelText.text = levelText;
    }

    public void LevelLockEnabled(bool b)
    {
        this.levelLock.enabled = b;
    }

    public void LevelTextEnabled(bool b)
    {
        this.levelText.enabled = b;
    }

    public void SetStar(int totalStar)
    {
        for(int i = 0; i < totalStar; i++)
        {
            starList[i].sprite = starWin;
        }
    }
}
