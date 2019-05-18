using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DaodanControl : MonoBehaviour
{
    public float DamageRange;
    public int Damage;
    public Transform MainDaodan;
    public GameObject Effect;

    private bool Explore;

    private void Start() => Explore = false;

    private void Update()
    {
        if (Explore) return;
        if (MainDaodan.position.y <= 1)
        {
            Expl();
            Explore = true;
        }
    }

    private void Expl()
    {
        GameObject newEffect = Instantiate(Effect, transform.position, transform.rotation);
        Destroy(newEffect, 0.5f);
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, DamageRange, 1 << 13);
        for (int i = 0; i < hitColliders.Length; i++)
        {
            if (hitColliders[i] != null && !hitColliders[i].GetComponent<Unit>().IsDie)
            {
                hitColliders[i].GetComponent<Unit>().ApplyDamage(Damage);
            }
        }
        Destroy(gameObject);
    }
}
