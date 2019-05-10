using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySmall : MonoBehaviour
{
    public float AttackRange;
    public int Damage;
    public float AttackWaitTime;
    public GameObject Shell;
    public Transform AttackPoint;

    private float NextAttack;
    private EnemyMove EM;
    private GameObject curTower;
    private bool isAttack;

    private void Start()
    {
        EM = GetComponent<EnemyMove>();
        isAttack = false;
        NextAttack = Time.time;
    }
    private void Update()
    {
        if (GetComponent<Unit>().IsDie)
        {
            return;
        }
        if (curTower == null || curTower.GetComponent<Unit>().IsDie)
        {
            isAttack = false;
            EM.SetAttack(false);
        }
        if (!isAttack)
        {
            curTower = EM.CurTower;
        }
        if (curTower != null && !curTower.GetComponent<Unit>().IsDie && Vector3.Distance(transform.position,curTower.transform.position) <= AttackRange)
        {
            isAttack = true;
            EM.SetAttack(true);
            if (NextAttack <= Time.time)
            {
                Attack();
                NextAttack = Time.time + AttackWaitTime;
            }
        }
    }
    private void Attack()
    {
        GameObject newShell = Instantiate(Shell, AttackPoint.position, AttackPoint.rotation);
        newShell.GetComponent<Shell>().SetEnemy(curTower);
    }
}
