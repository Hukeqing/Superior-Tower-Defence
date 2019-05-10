using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class TowerDate
{
    public GameObject TowerPrefab;
    public int cost;
    public TowerType type;
}
public enum TowerType
{
    Nothing = 0,
    Home,
    ShellTower,
    Haven,
    Resource,
    ElectromagneticTower,
    MagneticTower,
    Mortar,
    Wall,
    Ice
}
