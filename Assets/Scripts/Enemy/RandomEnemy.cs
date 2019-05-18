using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomEnemy : MonoBehaviour
{
    private GameObject[] EnemyList;
    public void AddGameObject(GameObject[] InputEnemy) => EnemyList = InputEnemy;

    private void Start() => StartCoroutine(EnemyBorth());

    private IEnumerator EnemyBorth()
    {
        yield return new WaitForSeconds(1f);
        int BornClass = Random.Range(0, EnemyList.Length);
        GameObject newEnemy = Instantiate(EnemyList[BornClass], transform.position, transform.rotation);
        newEnemy.GetComponent<EnemyMove>().SetHome(GameObject.Find("GameManage").GetComponent<GameDate>().MainPoint.GetTower());
        Destroy(gameObject, 1f);
    }
}
