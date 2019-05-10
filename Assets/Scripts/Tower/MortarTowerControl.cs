using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MortarTowerControl : MonoBehaviour
{
    public GameObject Shell;
    public GameObject Target;
    public float AttackWaitTime;
    public float AttackRange;
    public Transform AttackPoint;

    private float AttackTime;
    private GameObject curEnemy;
    private AudioSource AS;
    private bool isDie;

    private void Start()
    {
        AttackTime = Time.time;
        AS = GetComponent<AudioSource>();
        isDie = false;
    }

    private void Update()
    {
        if (isDie)
        {
            return;
        }
        if (curEnemy == null || curEnemy.GetComponent<Unit>().IsDie)
        {
            SeachEnemy();
        }
        else
        {
            if (Time.time >= AttackTime)
            {
                Attacking();
                AttackTime = Time.time + AttackWaitTime;
            }
        }
    }
    private void Attacking()
    {
        AS.Stop();
        AS.Play();
        GameObject newTarget = Instantiate(Target, curEnemy.transform.position, curEnemy.transform.rotation);
        GameObject newShell = Instantiate(Shell, AttackPoint.position, AttackPoint.rotation);
        newShell.GetComponent<MortarShell>().SetEnemy(newTarget);
    }
    private void SeachEnemy()
    {
        float minRange = AttackRange;
        GameObject tempEnemy = null;
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, AttackRange, 1 << 13);
        for (int i = 0; i < hitColliders.Length; i++)
        {
            if (hitColliders[i] != null && !hitColliders[i].GetComponent<Unit>().IsDie)
            {
                if (tempEnemy == null)
                {
                    tempEnemy = hitColliders[i].gameObject;
                    minRange = Vector3.Distance(transform.position, tempEnemy.transform.position);
                }
                else
                {
                    float curRange = Vector3.Distance(transform.position, hitColliders[i].transform.position);
                    if (curRange < minRange)
                    {
                        tempEnemy = hitColliders[i].gameObject;
                        minRange = curRange;
                    }
                }
            }
        }
        curEnemy = tempEnemy;
    }
    public void Des() => isDie = true;
}
