using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyBornControl : MonoBehaviour
{
    public float GameStartTime;
    public EnemyWaveDate[] Wave;

    public GameObject[] EnemyList;
    public Transform[] BornPoint;

    public Text text;

    public SuggestionControl SC;

    public GameObject RandomBorn;

    private Coroutine Cor;
    private Coroutine CorForRandomEnemy;
    private PointControl GD;
    private bool isEnd = false;
    private AudioSource AS;
    private Animator AT;
    private int curRound;

    private float GameHard;

    private void Start()
    {
        Cor = StartCoroutine(BornEnemy());
        GameHard = 1;
        GD = GameObject.Find("GameManage").GetComponent<GameDate>().MainPoint;
        AS = GetComponent<AudioSource>();
        AT = text.GetComponent<Animator>();
        curRound = 0;
    }
    public void GameEnd() => StopAllCoroutines();
    public void ChangeGameHard(float Number) => GameHard = Number;
    private void Update()
    {
        if (!isEnd)
        {
            return;
        }
        Collider[] hitcollider = Physics.OverlapSphere(transform.position, 50, 1 << 13);
        if (hitcollider.Length == 0)
        {
            GameObject.Find("GameManage").GetComponent<GameDate>().Win();
        }
    }
    IEnumerator BornEnemy()
    {
        SC.Suggest("敌人将在"+GameStartTime+"秒后到达右下角！");
        yield return new WaitForSeconds(5);
        SC.Suggest("游戏难度将在10秒之后锁定");
        yield return new WaitForSeconds(10);
        GetComponent<GameControl>().GameStart();
        SC.Suggest("游戏难度已经锁定");
        yield return new WaitForSeconds(GameStartTime - 15);
        SC.Suggest("做好准备！");
        yield return new WaitForSeconds(2);
        AS.Play();
        yield return new WaitForSeconds(3);
        foreach (EnemyWaveDate item in Wave)
        {
            yield return new WaitForSeconds(item.WaveBeforeTime);
            curRound++;
            text.text = "第" + curRound +"/" + Wave.Length + "波";
            SC.SiriSound("第" + curRound + "波");
            AT.SetTrigger("Round");
            if (curRound == 10)
            {
                CorForRandomEnemy = StartCoroutine(BornRandomEnemy());
            }
            foreach (Wave wave in item.Enemys)
            {
                foreach (EnemyInOnce t in wave.Enemys)
                {
                    for (int i = 0; i < t.Number * GameHard; i++)
                    {
                        GameObject newEnemy = Instantiate(EnemyList[(int)t.type], BornPoint[wave.BornPoint].position, BornPoint[wave.BornPoint].rotation);
                        newEnemy.GetComponent<EnemyMove>().SetHome(GD.GetTower());
                        yield return new WaitForSeconds(t.inerval);
                    }
                }
            }
        }
        isEnd = true;
    }

    private IEnumerator BornRandomEnemy()
    {
        float waitTime = Random.Range(1f, Wave.Length + 2 - curRound);
        yield return new WaitForSeconds(waitTime);
        GameObject newBorn = Instantiate(RandomBorn, new Vector3(Random.Range(-15, 15), 1, Random.Range(-15, 15)), transform.rotation);
        newBorn.GetComponent<RandomEnemy>().AddGameObject(EnemyList);
        CorForRandomEnemy = StartCoroutine(BornRandomEnemy());
    }
}
