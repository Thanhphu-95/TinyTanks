using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class MissionUI : MonoBehaviour
{
    public GameObject missionFailedUI;
    public GameObject missionSuccessUI;


    public void ShowFailed()
    {
        missionFailedUI.SetActive(true);
    }

    public void ShowSuccess()
    {
        missionSuccessUI.SetActive(true);
    }
}
