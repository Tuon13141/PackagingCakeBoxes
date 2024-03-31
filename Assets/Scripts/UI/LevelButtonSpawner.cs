using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelButtonSpawner : MonoBehaviour
{
    public int totalLevel;
    private int unlockedLevel;

    public GameObject LevelButtonPref;
    public GameObject ParentTrasform;

    private List<GameObject> levelButtonList;
    void Start()
    {
        //ResetPlayerPref();
        List<LevelSave> levelSave = LoadData();     
        unlockedLevel = PlayerPrefs.GetInt("FinishedLevel") + 1;
        if(unlockedLevel < 1) unlockedLevel = 1;
        for(int i = 1; i <= totalLevel; i++)
        {
            GameObject button = Instantiate(LevelButtonPref, ParentTrasform.transform);
            LevelButton levelButton = button.GetComponent<LevelButton>();

            LevelSave ls = levelSave[i-1];
            levelButton.SetStar(ls.stars);

            levelButton.SetLevelText(i.ToString());
            if(i <= unlockedLevel)
            {
                levelButton.LevelLockEnabled(false);
                levelButton.LevelTextEnabled(true);
            }
        }
    }

    List<LevelSave> LoadData()
    {
        List<LevelSave> levelSave = SaveSystem.LoadLevels();
        if (levelSave == null)
        {
            levelSave = new List<LevelSave>();  
            for(int i = 0; i < totalLevel; i++)
            {
                LevelSave ls = new LevelSave(i + 1, 0);
                levelSave.Add(ls);  
            }

            SaveSystem.SaveLevels(levelSave);
        }
        
        return levelSave;
    }

    void ResetPlayerPref()
    {
        PlayerPrefs.DeleteAll();
    }
}
