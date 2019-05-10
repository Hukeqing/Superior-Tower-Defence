using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class PointControl : MonoBehaviour
{
    public GameObject buildEffect;
    public bool IsGreen => Greens > 0;

    private TowerDate curTower;
    private GameObject curTowerPrefab;
    private Color curColor = Color.white;
    private bool isSelect = false;
    private Animator Toweranimator;

    private Renderer currenderer;
    private int Greens;

    private void Start() => currenderer = GetComponent<Renderer>();
    public bool IsEmpty()
    {
        return (curTowerPrefab == null || curTowerPrefab.GetComponent<Unit>().IsDie);
    }
    public GameObject GetTower()
    {
        return curTowerPrefab;
    }
    public void SetSelect(bool t)
    {
        isSelect = t;
        ChangeColor();
    }
    public void SetGreen(int change)
    {
        Greens += change;
        ChangeColor();
        if (Greens == 0 && curTowerPrefab != null)
        {
            DestroyTurret();
        }
    }
    public void BuildTurret(TowerDate turretData)
    {
        curTower = turretData;
        curTowerPrefab = Instantiate(curTower.TowerPrefab, transform.position, transform.rotation);
        GameObject newEffect = Instantiate(buildEffect, transform);
        Toweranimator = curTowerPrefab.GetComponent<Animator>();
        Destroy(newEffect, 1.5f);
    }
    public void DestroyTurret()
    {
        if (curTowerPrefab == null)
            return;
        Toweranimator.SetTrigger("Die");
        curTowerPrefab.SendMessage("Des");
        Destroy(curTowerPrefab, 1f);
        curTower = null;
        curTowerPrefab = null;
        GameObject newEffect = Instantiate(buildEffect, transform);
        Destroy(newEffect, 1.5f);
    }
    private void ChangeColor()
    {
        if (isSelect)
        {
            curColor = Color.blue;
        }
        else
        {
            if (Greens == 0)
            {
                curColor = Color.white;
            }
            else
            {
                curColor = Color.green;
            }
        }
        currenderer.material.color = curColor;
    }
    private void OnMouseEnter()
    {
        if (Greens > 0 && curTowerPrefab == null && !EventSystem.current.IsPointerOverGameObject())
        {
            currenderer.material.color = Color.red;
        }
    }
    private void OnMouseExit()
    {
        currenderer.material.color = curColor;
    }
}
