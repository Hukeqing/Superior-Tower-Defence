using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMove : MonoBehaviour
{
    public float MoveSpeed = 1;
    public float FindRange = 10;
    public float MaxIce = 3.5f;
    public float Degenerate = 0.1f;
    public GameObject CurTower { get; private set; }

    private GameObject Home;
    private float curSpeed;
    private bool isAttack = false;
    private AudioSource AS;
    private float ice;

    private void Start()
    {
        AS = GetComponent<AudioSource>();
        curSpeed = MoveSpeed;
        CurTower = null;
    }
    public void SetHome(GameObject home) => Home = home;
    public void SetAttack(bool isattack) => isAttack = isattack;
    public void GetIce(float iceTime)
    {
        ice += iceTime;
        if (ice > MaxIce)
        {
            ice = MaxIce;
        }
        curSpeed = MoveSpeed * Degenerate;
    }
    private void Update()
    {
        if (Home == null)
        {
            return;
        }
        if (!isAttack|| CurTower == null || CurTower.GetComponent<Unit>().IsDie)
        {
            SeachEnemy();
            if (CurTower == null || CurTower.GetComponent<Unit>().IsDie)
            {
                CurTower = Home;
            }
            Vector3 target = CurTower.transform.position;
            target.y = transform.position.y;
            if (ice > 0)
            {
                ice -= Time.deltaTime;
            }
            if (ice <= 0)
            {
                curSpeed = MoveSpeed;
            }
            transform.Translate((target - transform.position).normalized * curSpeed * Time.deltaTime);
        }
    }
    private void SeachEnemy()
    {
        float minRange = FindRange;
        GameObject tempEnemy = null;
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, FindRange, 1 << 12);
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
        CurTower = tempEnemy;
    }
    public void Des()
    {
        if (AS && !AS.isPlaying)
        {
            AS.Play();
        }
        return;
    }
}
