using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDoctor : MonoBehaviour
{
    [Tooltip("两次治疗间隔")]
    public float CureWaitTime;
    public int CureDamage;                          // 单次治疗量
    public float CureRange;                         // 治疗范围
    public GameObject CureEffect;                   // 治疗特效
    [Tooltip("治疗衰减时间")]
    public float Degenerate = 0.5f;

    private float NextCure;
    private EnemyMove EM;
    private float curCureWaitTime;

    private void Start()
    {
        curCureWaitTime = CureWaitTime;
        NextCure = Time.time + curCureWaitTime;
        EM = GetComponent<EnemyMove>();
    }
    private void Update()
    {
        if (GetComponent<Unit>().IsDie)
        {
            return;
        }
        if (Time.time >= NextCure)
        {
            NextCure = Time.time + curCureWaitTime;
            Cure();
        }
        if (EM.CurTower != null && Vector3.Distance(transform.position, EM.CurTower.transform.position) <= CureRange)
        {
            EM.SetAttack(true);
        }
        else
        {
            EM.SetAttack(false);
        }
    }
    private void Cure()
    {
        curCureWaitTime += Degenerate;
        GameObject newEffect = Instantiate(CureEffect, transform.position, transform.rotation);
        Destroy(newEffect, 1.5f);
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, CureRange, 1 << 13);
        foreach (var item in hitColliders)
        {
            Unit temp = item.GetComponent<Unit>();
            if (temp != null && !temp.IsDie)
            {
                temp.ApplyDamage(-CureDamage);
            }
        }
    }
    public void Die() => Cure();
}
