using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameControl : MonoBehaviour
{
    public GameObject StopUI;
    public Slider TimeControl;
    public Dropdown ScreenResolutions;
    public Dropdown ScreenFrame;
    public int[] FrameList;
    public Toggle FullScreenToggle;
    public GameObject Setting;
    public GameObject RewardImage;

    public GameObject SiriSetting;

    private float GameSpeed = 1;
    private AudioSource AS;
    private Resolution[] resolutions;
    private int lastResolution;
    private int lastFrame;
    private bool lastFullscreen;
    private float m_time;
    private EnemyBornControl EBC;

    public Slider GameHard;
    public Text HardMode;
    public Text GameHardFront;

    private void Start()
    {
        GameSpeed = 1;
        m_time = -1;
        AS = GetComponent<AudioSource>();
        EBC = GetComponent<EnemyBornControl>();
        resolutions = Screen.resolutions;
        lastFrame = -1;
        lastFullscreen = Screen.fullScreen;
        if (Setting)
        {
            Setting.SetActive(false);
        }
        if (RewardImage)
        {
            RewardImage.SetActive(false);
        }
        if (SiriSetting)
        {
            SiriSetting.SetActive(false);
        }
        if (StopUI)
        {
            StopUI.SetActive(false);
        }
        if (ScreenResolutions)
        {
            ScreenResolutions.ClearOptions();
            for (int i = 0; i < resolutions.Length; i++)
            {
                ScreenResolutions.AddOptions(new List<string> { resolutions[i].width.ToString() + "×" + resolutions[i].height.ToString() });

                if (Screen.currentResolution.width == resolutions[i].width && Screen.currentResolution.height == resolutions[i].height)
                {
                    ScreenResolutions.value = i;
                    lastResolution = i;
                }
            }
            foreach (var item in FrameList)
            {
                if (item == -1)
                {
                    ScreenFrame.AddOptions(new List<string> { "无限制" });
                }
                else
                {
                    ScreenFrame.AddOptions(new List<string> { item.ToString() });
                }
            }
            ScreenFrame.value = FrameList.Length - 1;
            FullScreenToggle.isOn = Screen.fullScreen;
        }
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.P) && !StopUI.activeInHierarchy)
        {
            if (Time.timeScale == 0)
            {
                Time.timeScale = GameSpeed;
            }
            else
            {
                Time.timeScale = 0;
            }
        }
    }
    public void LoadLevel(int x)
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(x);
    }
    public GameObject HelpCanvas;
    public GameObject MenuCanvas;
    public void Help()
    {
        if (HelpCanvas.activeSelf)
        {
            HelpCanvas.SetActive(false);
            MenuCanvas.SetActive(true);
            HelpCanvas.GetComponent<CanvasScaler>().scaleFactor = 0;
        }
        else
        {
            HelpCanvas.SetActive(true);
            MenuCanvas.SetActive(false);
        }
    }
    public void SetSpeed(float x)
    {
        GameSpeed = x;
        TimeControl.value = (int)x;
        AS.pitch = GameSpeed;
#if UNITY_EDITOR
        Time.timeScale = GameSpeed * 2;
#else
        Time.timeScale = GameSpeed;
#endif
    }
    public void Quit()
    {
        Time.timeScale = 1;
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
    public void ESC()
    {
        if (Time.timeScale > 0)
        {
            Stop();
        }
        else
        {
            if (SiriSetting.activeSelf)
            {
                SiriSet(false);
            }
            else if (Setting.activeSelf)
            {
                setting(false);
                SaveSetting();
            }
            else if (RewardImage.activeSelf)
            {
                SetReward(false);
            }
            else
            {
                Continue();
            }
        }
    }
    public void Stop()
    {
        StopUI.SetActive(true);
        Setting.SetActive(false);
        if (AS.isPlaying)
        {
            m_time = AS.time;
            AS.Stop();
        }
        else
        {
            m_time = -1;
        }
        Time.timeScale = 0;
    }
    public void Continue()
    {
        StopUI.SetActive(false);
        if (m_time > 0)
        {
            AS.Play();
            AS.time = m_time;
        }
        Time.timeScale = GameSpeed;
    }
    public void setting(bool istrue) => Setting.SetActive(istrue);
    public void SetReward(bool isTrue) => RewardImage.SetActive(isTrue);
    public void SetScreen()
    {
        if (Screen.currentResolution.width == resolutions[ScreenResolutions.value].width && Screen.currentResolution.height == resolutions[ScreenResolutions.value].height)
        {
            return;
        }
        Screen.SetResolution(resolutions[ScreenResolutions.value].width, resolutions[ScreenResolutions.value].height, Screen.fullScreen);
    }
    public void SetFrame()
    {
        Application.targetFrameRate = FrameList[ScreenFrame.value];
    }
    public void SetFullScreen()
    {
        if (FullScreenToggle.isOn == Screen.fullScreen)
        {
            return;
        }
        Screen.SetResolution(Screen.currentResolution.width, Screen.currentResolution.height, FullScreenToggle.isOn);
    }
    public void SaveSetting()
    {
        lastFrame = FrameList[ScreenFrame.value];
        lastResolution = ScreenResolutions.value;
        lastFullscreen = Screen.fullScreen;
    }
    public void EraseSetting()
    {
        ScreenFrame.value = FrameList.Length - 1;
        ScreenResolutions.value = lastResolution;
        FullScreenToggle.isOn = lastFullscreen;
        Screen.SetResolution(resolutions[ScreenResolutions.value].width, resolutions[ScreenResolutions.value].height, lastFullscreen);
        Application.targetFrameRate = lastFrame;
    }
    public void ChangeGameHard()
    {
        EBC.ChangeGameHard(GameHard.value);
        if (GameHard.value <= 0.5f)
        {
            HardMode.text = "极其简单";
        }
        else if (GameHard.value <= 0.8f)
        {
            HardMode.text = "简单";
        }
        else if (GameHard.value <= 1.2f)
        {
            HardMode.text = "标准";
        }
        else if (GameHard.value <= 1.5f)
        {
            HardMode.text = "困难";
        }
        else
        {
            HardMode.text = "极其困难";
        }
    }
    public string GetHard() => HardMode.text;
    public void GameStart()
    {
        GameHard.interactable = false;
        GameHardFront.text = "已锁定";
    }
    public void SiriSet(bool is_On)
    {
        SiriSetting.SetActive(is_On);
    }
}
