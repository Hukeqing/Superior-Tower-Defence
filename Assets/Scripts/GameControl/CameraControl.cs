using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CameraControl : MonoBehaviour
{
    public float movespeed;                         //摄像机上下左右移动的速度
    public float forwardspeed;                      //摄像机前进后退的速度（放大速度）
    public GameDate GD;                             //用于获取建筑物的资料
    public GameObject BuildCanvas;                  //建造选项
    public GameObject DestroyCanvas;                //拆除选项
    public int money { get; private set; }          //金钱数目
    public Text MoneyText;                          //金钱显示的UI
    public GameControl GC;                          //用来调用 Esc 键按下后的指令
    public SuggestionControl SC;                    //系统提示

    private Vector3 target;                         //摄像机的目标方向
    private PointControl curPoint;                  //保存当前点击的方块
    private bool isEnd;                             //游戏是否结束
    private AudioSource AS;                         //鼠标点击音

    private void Start()
    {
        money = 100000000;
        MoneyText.text = "$" + money;
        isEnd = false;
        AS = GetComponent<AudioSource>();
    }

    void Update()
    {
        //鼠标点击音
        if (Input.GetMouseButtonDown(0) && AS.isActiveAndEnabled)
        {
            AS.Play();
        }
        //游戏结束不再相应其他操作
        if (isEnd)
        {
            return;
        }
        //暂停或者继续的响应
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            GC.ESC();
        }
        //摄像机移动视角
        target = new Vector3(-Input.GetAxis("Vertical") * movespeed, 0, Input.GetAxis("Horizontal") * movespeed);
        transform.Translate(target * Time.deltaTime, Space.World);

        transform.Translate(new Vector3(0, 0, Input.GetAxis("Mouse ScrollWheel") * forwardspeed) * Time.deltaTime * forwardspeed, Space.Self);
        //摄像机位置框定
        Vector3 curVector = transform.position;
        curVector.x = Mathf.Clamp(curVector.x, -15, 25);
        curVector.y = Mathf.Clamp(curVector.y, 5, 20);
        curVector.z = Mathf.Clamp(curVector.z, -20, 20);
        transform.position = curVector;
        //相应点击功能
        if (Input.GetMouseButtonDown(0) && EventSystem.current.IsPointerOverGameObject() == false)
        {
            if (BuildCanvas.activeSelf || DestroyCanvas.activeSelf)
            {
                //如果有建造面板或者拆除面板
                BuildCanvas.SetActive(false);
                DestroyCanvas.SetActive(false);
                curPoint.SetSelect(false);
            }
            else
            {
                //没有正在的任务，并且点到方块
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                bool isCollider = Physics.Raycast(ray, out RaycastHit hit, 1000, LayerMask.GetMask("MapCube"));
                if (isCollider)
                {
                    curPoint = hit.collider.GetComponent<PointControl>();

                    if (curPoint.IsEmpty() && curPoint.IsGreen)
                    {
                        curPoint.SetSelect(true);
                        BuildCanvas.SetActive(true);
                        BuildCanvas.transform.localScale = Vector3.one * ((transform.position.y - 25.0f) / (transform.position.y - 50.0f));
                        BuildCanvas.transform.position = Input.mousePosition;
                    }
                    else if (curPoint.IsGreen)
                    {
                        curPoint.SetSelect(true);
                        DestroyCanvas.SetActive(true);
                        DestroyCanvas.transform.localScale = Vector3.one * ((transform.position.y - 25.0f) / (transform.position.y - 50.0f));
                        DestroyCanvas.transform.position = Input.mousePosition;
                    }
                }
            }
        }
    }
    //扣钱
    public void UseMoney(int used)
    {
        money -= used;
        MoneyText.text = "$" + money;
    }
    //赚钱
    public void AddMoney(int add)
    {
        money += add;
        MoneyText.text = "$" + money;
    }
    //建造
    public void BuildTower(int towerType)
    {
        if (towerType == -1)
        {
            curPoint.SetSelect(false);
            BuildCanvas.SetActive(false);
            return;
        }
        if (curPoint.GetComponent<PointControl>().IsGreen)
        {
            TowerDate curTower = GD.GetTowerDate(towerType);
            if (curTower.cost <= money)
            {
                curPoint.SetSelect(false);
                UseMoney(curTower.cost);
                curPoint.BuildTurret(curTower);
                BuildCanvas.SetActive(false);
            }
            else
            {
                SC.Suggest("没有足够的金币！需要：" + curTower.cost + "，已有：" + money);
            }
        }
        else
        {
            BuildCanvas.SetActive(false);
            curPoint.SetSelect(false);
        }
    }
    //拆除
    public void DestroyTower()
    {
        curPoint.SetSelect(false);
        curPoint.DestroyTurret();
        DestroyCanvas.SetActive(false);
    }
    public void SetEnd() => isEnd = true;
}
