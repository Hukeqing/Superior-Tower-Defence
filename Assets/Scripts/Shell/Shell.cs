using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shell : MonoBehaviour
{
    public int Damage = 5;
    public float Speed = 10;
    public float ExploredRange = 1;

    private GameObject Enemy;

    public void SetEnemy(GameObject enemy) => Enemy = enemy;

    private void Update()
    {
        if (Enemy == null)
        {
            Destroy(gameObject);
        }
        else
        {
            transform.LookAt(Enemy.transform.position);
            transform.Translate(Vector3.forward * Speed * Time.deltaTime, Space.Self);
            if (Vector3.Distance(transform.position,Enemy.transform.position) <= ExploredRange)
            {
                Enemy.GetComponent<Unit>().ApplyDamage(Damage);
                Destroy(gameObject);
            }
        }
    }
    private void OnDestroy()
    {
        //TODO...
    }
}
