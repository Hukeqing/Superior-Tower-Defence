using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyGhost : MonoBehaviour
{
    public float AttackRange;
    public GameObject AttackShere;
    public Transform AttackPoint;

    private EnemyMove EM;
    private bool isAttack;
    private GameObject curTower;

    private void Start()
    {
        EM = GetComponent<EnemyMove>();
        isAttack = false;
    }
    private void Update()
    {
        if (GetComponent<Unit>().IsDie)
        {
            return;
        }
        if (isAttack)
        {
            return;
        }
        if (!isAttack)
        {
            curTower = EM.CurTower;
        }
        if (curTower != null && !curTower.GetComponent<Unit>().IsDie && Vector3.Distance(transform.position, curTower.transform.position) <= AttackRange)
        {
            isAttack = true;
            EM.SetAttack(true);
            gameObject.GetComponent<Unit>().ApplyDamage(gameObject.GetComponent<Unit>().maxHealth);
            Attack();
        }
    }
    private void Attack()
    {
        GameObject newShell = Instantiate(AttackShere, AttackPoint.position, AttackPoint.rotation);
        newShell.GetComponent<Shell>().SetEnemy(curTower);
    }
    public void Des() => Destroy(AttackPoint.gameObject);
}
