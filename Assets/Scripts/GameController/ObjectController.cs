using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ObjectController : MonoBehaviour
{
    public GameObject cakePref;
    public GameObject boxPref;
    public GameObject framePref;
    public GameObject wallPref;

    public Camera mCam;

    public Sprite victoryImg;
    public Sprite faledImg;
    public Sprite starLose;
    public Sprite starWin;

    [SerializeField]
    private Text timer;
    [SerializeField]
    private Image Noti;
    [SerializeField]
    private List<Image> Stars;
    [SerializeField]
    private GameObject NextButton;
    [SerializeField]
    private GameObject NotiCanvas;

    public AudioSource faledAudio;
    public AudioSource victoryAudio;
    public AudioSource backgroundAudio;
    public AudioSource cakeCaptureAudio;

    public void CenterizeCamera(Vector2 centerPoint)
    {
        mCam.transform.position = new Vector3(centerPoint.x , centerPoint.y, -10);
    }

    public void SetTimerText(string time)
    {
        if(int.Parse(time) < 10)
        {
            timer.text = "00 : 0" + time;
        }
        else
        {
            timer.text = "00 : " + time;
        }     
    }

    public void EnableNotiCanvas()
    {
        NotiCanvas.SetActive(true);
        backgroundAudio.Stop();
    }

    public void LevelFailed()
    {
        EnableNotiCanvas();
        Noti.sprite = faledImg;
        faledAudio.Play();
    }

    public void LevelCompleted(int totalStar)
    {
        EnableNotiCanvas();
        Noti.sprite = victoryImg;
        if(PlayerPrefs.GetInt("CurrentLevel") + 1 <= PlayerPrefs.GetInt("TotalLevel"))
        {
            Debug.Log(PlayerPrefs.GetInt("Level") + 1);
            NextButton.SetActive(true);
        }
            
        else NextButton.SetActive(false);

        for(int i = 0; i < totalStar; i++)
        {
            Stars[i].sprite = starWin;
        }

        victoryAudio.Play();
    }

    public void PlayCakeCaptureAudio()
    {
        cakeCaptureAudio.Play();
    }
}
