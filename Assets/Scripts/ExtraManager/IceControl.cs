using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IceControl : MonoBehaviour
{
    public float DamageRange;

    private void Start()
    {
        Expl();
    }
    private void Expl()
    {
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, DamageRange, 1 << 13);
        for (int i = 0; i < hitColliders.Length; i++)
        {
            if (hitColliders[i] != null && !hitColliders[i].GetComponent<Unit>().IsDie)
            {
                hitColliders[i].GetComponent<EnemyMove>().MoveSpeed = 0;
            }
        }
        Destroy(gameObject, 0.5f);
    }
}
