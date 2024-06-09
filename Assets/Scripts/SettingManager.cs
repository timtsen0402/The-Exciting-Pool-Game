using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.Audio;
using UnityEngine.EventSystems;
using static DragAndShoot;
using static GameController;
using static AudioManager;


public class SettingManager : MonoBehaviour
{

    public static Dropdown ResDropDown;

    public AudioClip[] BGMs;

    Resolution[] resolutions;
    void Start()
    {
        ResDropDown = GameObject.Find("Canvas/Settings/Resolution DD").GetComponent<Dropdown>();

        #region resolution
        resolutions = Screen.resolutions;
        ResDropDown.ClearOptions();
        List<string> options = new List<string>();
        int currentResolutionIndex = 0;

        for (int i = 0; i < resolutions.Length; i++)
        {
            string option = resolutions[i].width + " x " + resolutions[i].height;
            options.Add(option);
            if (resolutions[i].width == Screen.currentResolution.width &&
                resolutions[i].height == Screen.currentResolution.height)
            {
                currentResolutionIndex = i;
            }
        }
        ResDropDown.AddOptions(options);
        ResDropDown.value = currentResolutionIndex;
        ResDropDown.RefreshShownValue();
        #endregion
    }
    public void SetResolution(int resolutionIndex)
    {
        Resolution resolution = resolutions[resolutionIndex];
        Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreen);
    }
    public void OnToggleFullScreen(bool isFullScreen)
    {
        Screen.fullScreen = isFullScreen;
    }

    public void OnToggleMute(bool isMute)
    {
        BGM_source.enabled = !isMute;

        Sound[] sounds = Audio.GetComponent<AudioManager>().sounds;
        foreach (Sound s in sounds)
        {
            s.source.enabled = !isMute;
        }

    }
    public void SetTimeScale(float timeScale)
    {
        Time.timeScale = timeScale;
    }
    public void Load_Scene(int sceneIndex)
    {
        SceneManager.LoadScene(sceneIndex);
    }

    public void OnClickQuit()
    {
        Application.Quit();
    }
    public void SetSound(float volume)
    {
        Sound[] sounds = Audio.GetComponent<AudioManager>().sounds;
        foreach (Sound s in sounds)
        {
            s.source.volume = volume;
        }

    }

    public void SetMusic(float volume)
    {
        BGM_source.volume = volume;
    }
    public void SetPredictionLineForce(float value)
    {
        PhysicsSimulation.force = value;
    }

    public static void PartyTime()
    {
        int randomNumber = Random.Range(1, BGM_s.Length);
        BGM_now = (AudioClip)BGM_s[randomNumber];
        BGM_source.clip = BGM_now;
        BGM_source.Play();
    }
    public void ResetLineRenderers(PhysicsSimulation physicsSimulation)
    {
        if (!physicsSimulation.enabled)
        {
            foreach (var ball in balls)
            {
                ball.Line.positionCount = 0;
            }
        }

    }

}
