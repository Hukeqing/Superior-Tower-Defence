using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class EnemyWaveDate
{
    public float WaveBeforeTime;
    public Wave[] Enemys;
}
[System.Serializable]
public struct Wave
{
    public int BornPoint;
    public EnemyInOnce[] Enemys;
}
[System.Serializable]
public struct EnemyInOnce
{
    public float inerval;
    public EnemyType type;
    public int Number;
}
[System.Serializable]
public enum EnemyType
{
    Nothing = -1,
    Small,
    Ghost,
    UFO,
    King,
    Doctor
}