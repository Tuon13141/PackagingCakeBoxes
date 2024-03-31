using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelController : MonoBehaviour
{
    public List<Level> Levels = new List<Level>();
    private int currentLevel = 0;

    public int MaxPlayTime = 45;
    private int timePlayed;
    public bool IsVictory { get; set; } = false;

    public ObjectController objectController;
    public MatrixController matrixController;
    public SwipeController swipeController;
    void Start()
    {
        PlayerPrefs.SetInt("TotalLevel", Levels.Count);
        currentLevel = PlayerPrefs.GetInt("CurrentLevel") - 1;
        timePlayed = MaxPlayTime;
        RenderMap();  
        StartCoroutine(CountDown());
        StartCoroutine(LevelCompleted());
    }

    void RenderMap()
    {
        GameObject cakePref = objectController.cakePref;
        GameObject boxPref = objectController.boxPref;
        GameObject framePref = objectController.framePref;
        GameObject wallPref = objectController.wallPref;

        Level level = Levels[currentLevel];

        level.SetMatrixController(matrixController);
        level.SetObjectController(objectController);

        level.RenderMap(cakePref, boxPref, wallPref, framePref);
    }

    IEnumerator CountDown()
    {
        objectController.SetTimerText(timePlayed.ToString());
        yield return new WaitForSeconds(1);

        timePlayed--;

        if(timePlayed <= 0 && !IsVictory)
        {
            objectController.SetTimerText("0");
            LevelFailed();
            yield break;
        }
        else if (IsVictory)
        {
            yield break;
        }
        else
        {         
            StartCoroutine(CountDown());
        }
    }

    void LevelFailed()
    {
        swipeController.DisableSwipe();
        objectController.LevelFailed();
    }

    IEnumerator LevelCompleted()
    {
        yield return new WaitUntil(() => IsVictory);

        swipeController.DisableSwipe();

        int totalStar = AssessmentStar();
        SaveData(totalStar);
        
        yield return new WaitForSeconds(0.2f);

        objectController.LevelCompleted(totalStar);
    }

    int AssessmentStar()
    {
        int totalStar = 0;
        int time = MaxPlayTime - timePlayed;
        if (time <= MaxPlayTime / 3)
        {
            totalStar = 3;
        }
        else if (time <= MaxPlayTime * 2 / 3 && time > MaxPlayTime / 3)
        {
            totalStar = 2;
        }
        else if (time > MaxPlayTime * 2 / 3)
        {
            totalStar = 1;
        }

        return totalStar;
    }

    void SaveData(int totalStar)
    {
        List<LevelSave> levelSaves = SaveSystem.LoadLevels();
        if(levelSaves[currentLevel].stars < totalStar)
        {
            levelSaves[currentLevel].stars = totalStar;
        }

        SaveSystem.SaveLevels(levelSaves);

        if (PlayerPrefs.GetInt("FinishedLevel") < currentLevel + 1)
        {
            PlayerPrefs.SetInt("FinishedLevel", currentLevel + 1);
        }
    }
}
