using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourceControl : MonoBehaviour
{
    public float ProductWaitTime;
    public float ProductMoney;
    public GameObject Head;
    public float Degenerate;

    private float NextProduct;
    private CameraControl CC;
    private bool isdead;

    private void Start()
    {
        NextProduct = (float)(Time.time + ProductWaitTime + 0.3);
        CC = GameObject.Find("Main Camera").GetComponent<CameraControl>();
        isdead = false;
    }
    private void Update()
    {
        if (isdead)
        {
            return;
        }
        if (NextProduct <= Time.time)
        {
            CC.AddMoney((int)ProductMoney);
            NextProduct = Time.time + ProductWaitTime;
            ProductMoney -= Degenerate;
            if (ProductMoney <= 0)
            {
                Head.GetComponent<Renderer>().material.color = Color.gray;
                GetComponent<Animator>().SetTrigger("ResourceEnd");
                isdead = true;
            }
        }
    }
    public void Des() => isdead = true;
}
