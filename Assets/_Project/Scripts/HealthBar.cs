using System;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    public Slider healthSlider;
    public Slider eastHealthSlider;
    public float maxHealth = 100f;
    public float health;
    private float lerpSpeed = 0.05f;//0.05f default

    private void Start()
    {
        health = maxHealth;
        //healthSlider.maxValue = maxHealth;
        //eastHealthSlider.maxValue = maxHealth;
        //healthSlider.value = maxHealth;
        //eastHealthSlider.value = maxHealth;
    }
    private void Update()
    {
        if(healthSlider.value != health)
        {
            healthSlider.value = health;
        }

        if(Input.GetKeyDown(KeyCode.E))
        {
            takeDamage(10);
        }
        if(Input.GetKeyDown(KeyCode.Q))
        {
            takeHeal(10);
        }

        if (healthSlider.value != eastHealthSlider.value)
        {
            eastHealthSlider.value = Mathf.Lerp(eastHealthSlider.value,health,lerpSpeed);
        }
    }

    private void takeDamage(float damage)
    {
        health -= damage;
    }
    private void takeHeal(float heal)
    {
        health += heal;
    }
}
