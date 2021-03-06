﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public enum ExtraClass
{
    Daodan, Ice, Cure
}

[System.Serializable]
public class ExtraObject
{
    public GameObject Prefab;
    public int Cost;
    public int Increase;
    public ExtraClass Extramode;
    public int layer = 13;
    public int curCost { get; set; }
    public float CoolDown;
    public ExtraUIControl EUC;
    public Text CostText;
}

public class ExtraControl : MonoBehaviour
{
    public CameraControl CC;
    public SuggestionControl SC;
    public ExtraObject[] Extralist;

    public void Start()
    {
        foreach (var item in Extralist)
        {
            item.curCost = item.Cost;
            item.CostText.text = "$" + item.curCost.ToString();
        }
    }

    public void Use(int mode)
    {
        if (!Extralist[mode].EUC.isCoolDownOver)
        {
            SC.Suggest("还没有冷却好呢！");
            return;
        }
        if (CC.money >= Extralist[mode].curCost)
        {
            CC.UseMoney(Extralist[mode].curCost);
            Extralist[mode].EUC.SetCoolDown(Extralist[mode].CoolDown);
            Extralist[mode].curCost += Extralist[mode].Increase;
            Extralist[mode].CostText.text = "$" + Extralist[mode].curCost.ToString();
            Collider[] hitColliders = Physics.OverlapSphere(Vector3.zero, 50, 1 << Extralist[mode].layer);
            Vector3 newVector;
            if (hitColliders.Length > 0)
            {
                int temp = Random.Range(0, hitColliders.Length);
                newVector = hitColliders[temp].transform.position;
            }
            else
            {
                newVector = new Vector3(Random.Range(-15f, 15f), 1, Random.Range(-15f, 15f));
            }
            Instantiate(Extralist[mode].Prefab, newVector, transform.rotation);
        }
        else
        {
            SC.Suggest("没有足够的金币！需要：" + Extralist[mode].curCost + "，已有：" + CC.money);
        }
    }
}
