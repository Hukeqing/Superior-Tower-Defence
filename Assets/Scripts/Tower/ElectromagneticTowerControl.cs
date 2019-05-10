using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElectromagneticTowerControl : MonoBehaviour
{
    public GameObject AttackEffect;
    public float AttackWaitTime;
    public float AttackRange;
    public Transform AttackPoint;
    public int Damage;

    private float AttackTime;
    private Collider[] hitColliders;
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
        SeachEnemy();
        if (Time.time >= AttackTime && hitColliders.Length != 0)
        {
            Attacking();
            AttackTime = Time.time + AttackWaitTime;
        }
    }
    private void Attacking()
    {
        GameObject newEffect = Instantiate(AttackEffect, AttackPoint.position, AttackPoint.rotation);
        AS.Stop();
        AS.Play();
        Destroy(newEffect, 1.5f);
        foreach (var item in hitColliders)
        {
            Unit temp = item.GetComponent<Unit>();
            if (temp && !temp.IsDie)
            {
                temp.ApplyDamage(Damage);
            }
        }
    }
    private void SeachEnemy() => hitColliders = Physics.OverlapSphere(transform.position, AttackRange, 1 << 13);
    public void Des()
    {
        isDie = true;
        Attacking();
        return;
    }
}
