using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MortarShell : MonoBehaviour
{
    public int Damage = 5;
    public float ExploredRange = 0.1f;
    public float DamageRange = 1f;
    public GameObject Effect;

    private GameObject Enemy;
    private float Distance;
    private float Speed;

    public void SetEnemy(GameObject enemy)
    {
        Enemy = enemy;
        Distance = Vector3.Distance(transform.position, enemy.transform.position);
        Speed = Distance / 2;
    }
    private void Update()
    {
        transform.LookAt(Enemy.transform.position);
        transform.Translate(Vector3.forward * Speed * Time.deltaTime, Space.Self);
        if (Vector3.Distance(transform.position, Enemy.transform.position) <= ExploredRange)
        {
            Destroy(gameObject);
        }
    }
    private void OnDestroy()
    {
        GameObject newEffect = Instantiate(Effect, transform.position, transform.rotation);
        Destroy(newEffect, 1.5f);
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, DamageRange, 1 << 13);
        for (int i = 0; i < hitColliders.Length; i++)
        {
            if (hitColliders[i] != null && !hitColliders[i].GetComponent<Unit>().IsDie)
            {
                hitColliders[i].GetComponent<Unit>().ApplyDamage(Damage);
            }
        }
        Destroy(Enemy);
    }
}
