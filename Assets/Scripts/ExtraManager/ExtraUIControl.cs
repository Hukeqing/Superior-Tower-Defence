using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ExtraUIControl : MonoBehaviour
{
    public float CDColor = 0.5f;

    public bool isCoolDownOver { get; private set; }

    private float CoolDown;
    private float curCoolDown;
    private Image SelfImage;

    private void Start()
    {
        SelfImage = GetComponent<Image>();
    }
    public void SetCoolDown(float CD)
    {
        CoolDown = CD;
        isCoolDownOver = false;
        SelfImage.fillAmount = 0;
        curCoolDown = CoolDown;
        SelfImage.color = new Color(1, 1, 1, CDColor);
    }
    private void Update()
    {
        if (isCoolDownOver) return;
        curCoolDown -= Time.deltaTime;
        SelfImage.fillAmount = 1 - curCoolDown / CoolDown;
        if (curCoolDown <= 0)
        {
            isCoolDownOver = true;
            SelfImage.color = new Color(1, 1, 1, 1);
        }
    }
}
