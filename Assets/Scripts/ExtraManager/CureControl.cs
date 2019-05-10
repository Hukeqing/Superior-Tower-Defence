using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CureControl : MonoBehaviour
{
    public float DamageRange = 5;
    public int CureDamage = 20;

    private void Start()
    {
        Expl();
    }
    private void Expl()
    {
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, DamageRange, 1 << 12);
        for (int i = 0; i < hitColliders.Length; i++)
        {
            if (hitColliders[i] != null && !hitColliders[i].GetComponent<Unit>().IsDie)
            {
                hitColliders[i].GetComponent<Unit>().ApplyDamage(-CureDamage);
            }
        }
        Destroy(gameObject, 1f);
    }
}
