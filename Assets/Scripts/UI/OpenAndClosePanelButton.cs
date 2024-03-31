using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpenAndClosePanelButton : MonoBehaviour
{
    public GameObject panel;
    public List<GameObject> objectsNeedToBeUnable;

    public void SetPanel()
    {
        if (panel.activeSelf)
        {
            panel.SetActive(false);
            foreach (GameObject obj in objectsNeedToBeUnable)
            {
                obj.SetActive(true);
            }
        }
        else
        {
            panel.SetActive(true);
            foreach (GameObject obj in objectsNeedToBeUnable)
            {
                obj.SetActive(false);
            }
        }
    }
}
