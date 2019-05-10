using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Unit : MonoBehaviour
{
    public int maxHealth;
    public GameObject DieEffect;
    public Slider HealthSlider;

    private int curHealth;
    private Animator animator;
    public bool IsDie { get; private set; }

    private void Awake()
    {
        curHealth = maxHealth;
        HealthSlider.value = 1;
        HealthSlider.gameObject.SetActive(false);
        animator = GetComponent<Animator>();
        IsDie = false;
    }
    public void ApplyDamage(int Damage)
    {
        if (IsDie)
        {
            return;
        }
        curHealth -= Damage;
        if (curHealth <= 0)
        {
            Die();
            IsDie = true;
            gameObject.layer = 0;
        }
        if (curHealth > maxHealth)
        {
            curHealth = maxHealth;
        }
        HealthSlider.value = (float)curHealth / maxHealth;
        if (curHealth == maxHealth)
        {
            HealthSlider.gameObject.SetActive(false);
        }
        else
        {
            HealthSlider.gameObject.SetActive(true);
        }
    }
    void Die()
    {
        Destroy(HealthSlider.gameObject);
        animator.SetTrigger("Die");
        GameObject effect = Instantiate(DieEffect, transform);
        SendMessage("Des");
        Destroy(gameObject, 2f);
    }
    public void Des() { }
}
