using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using static AudioManager;

public class OnClickTrigger : MonoBehaviour
{
    Image aceImg;
    GameObject Rules;
    void Start()
    {

        aceImg = GameObject.Find("Canvas/PlayButton").GetComponent<Image>();
        Rules = GameObject.Find("Canvas/Rules");
        Rules.SetActive(false);
    }
    public void OnClickQuestionaire()
    {
        Application.OpenURL("https://forms.gle/5VvUuuYKnmxUX6ag8");
    }


    public void Quit()
    {
        Application.Quit();
    }


    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
            Application.Quit();
        aceImg.color = EffectCenter.RGB_lightEffect();
    }


}
