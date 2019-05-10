using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameDate : MonoBehaviour
{
    public TowerDate[] Towerdates;
    public PointControl MainPoint;
    public GameObject GameOver;
    public Text text;
    public AudioSource BackgroundAS;
    public AudioClip WinAC;
    public AudioClip OverAC;
    public AudioClip PaintEggshellAudioClip;
    public Sprite PaintEggshellSprite;
    public GameObject ProductionButton;
    public GameObject ProductionImage;

    public Font CommonFont;
    public Font PaintEggFont;
    private GameObject mainCamera;
    //public SuggestionControl SC;

    private bool isPaintEggshell = false;

    private void Start()
    {
        MainPoint.BuildTurret(Towerdates[0]);
        isPaintEggshell = false;
        ProductionButton.SetActive(false);
        ProductionImage.SetActive(false);
        GameOver.SetActive(false);
        mainCamera = Camera.main.gameObject;
    }
    public void PaintEgg() => isPaintEggshell = true;
    public void Win()
    {
        text.font = CommonFont;
        text.text = "You Win\n(" + GetComponent<GameControl>().GetHard().ToString() + "模式)";
        Time.timeScale = 1;
        BackgroundAS.Stop();
        mainCamera.GetComponent<CameraControl>().SetEnd();
        ProductionButton.SetActive(true);
        GameOver.GetComponent<AudioSource>().clip = WinAC;
        GameOver.SetActive(true);
    }
    public void End()
    {
        if (isPaintEggshell)
        {
            text.font = PaintEggFont;
            text.text = "感谢你能来玩我的游戏，也恭喜你找到了彩蛋！\n——Mauve";
            GameOver.GetComponent<AudioSource>().clip = PaintEggshellAudioClip;
            GameOver.GetComponentInChildren<Image>().sprite = PaintEggshellSprite;
            ProductionButton.SetActive(true);
        }
        else
        {
            text.font = CommonFont;
            text.text = "Game Lost";
            GameOver.GetComponent<AudioSource>().clip = OverAC;
            ProductionButton.SetActive(false);
        }
        GameOver.SetActive(true);
        Time.timeScale = 1;
        BackgroundAS.Stop();
        GetComponent<EnemyBornControl>().GameEnd();
        mainCamera.GetComponent<CameraControl>().SetEnd();
    }
    public void Production(bool isTrue) => ProductionImage.SetActive(isTrue);
    public TowerDate GetTowerDate(int towerType) => Towerdates[towerType];
    public GameObject GetCamera() => mainCamera;
    public PointControl GetMainPoint() => MainPoint;
}
