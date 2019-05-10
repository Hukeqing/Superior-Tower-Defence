using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HavenControl : MonoBehaviour
{
    public float GreenRange = 5;

    private Collider[] hitColliders;
    private bool isLive;

    private void Start()
    {
        hitColliders = Physics.OverlapSphere(transform.position, GreenRange, 1 << 10);
        for (int i = 0; i < hitColliders.Length; i++)
        {
            hitColliders[i].GetComponent<PointControl>().SetGreen(1);
        }
        isLive = true;
    }
    private void Des()
    {
        if (!isLive)
        {
            return;
        }
        isLive = false;
        try
        {
            for (int i = 0; i < hitColliders.Length; i++)
            {
                hitColliders[i].GetComponent<PointControl>().SetGreen(-1);
            }
        }
        catch (System.Exception)
        {
            return;
        }
    }
}
