using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HomeControl : MonoBehaviour
{
    public float WarningRange = 12;
    public Light mainLight;

    private GameDate GD;
    private AudioSource AS;

    private void Start()
    {
        GD = GameObject.Find("GameManage").GetComponent<GameDate>();
        mainLight = GameObject.Find("Directional Light").GetComponent<Light>();
        AS = GetComponent<AudioSource>();
        StartCoroutine(Beat());
    }
    IEnumerator Beat()
    {
        while (true)
        {
            yield return new WaitForSeconds(0.2f);
            Collider[] hitColliders = Physics.OverlapSphere(transform.position, WarningRange, 1 << 13);
            if (hitColliders.Length > 0)
            {
                float minRange = 16;
                float curRange;
                foreach (var item in hitColliders)
                {
                    curRange = Vector3.Distance(transform.position, item.transform.position);
                    if (curRange < minRange)
                    {
                        minRange = curRange;
                    }
                }
                mainLight.color = Color.white * minRange / WarningRange;
                if (minRange < 10)
                {
                    AS.pitch = 1f;
                    AS.volume = 0.8f;
                }
                else if (minRange < 8)
                {
                    AS.pitch = 1.2f;
                    AS.volume = 1f;
                }
                else
                {
                    AS.pitch = 0.5f;
                    AS.volume = 0.5f;
                }
                if (!AS.isPlaying)
                {
                    AS.Play();
                }
            }
            else
            {
                AS.Stop();
                if (mainLight.color != Color.white)
                {
                    mainLight.color += (Color.white - mainLight.color) * 0.5f;
                }
            }
        }
    }
    private void Des()
    {
        GD.End();
        AS.Stop();
        StopAllCoroutines();
    }
}
