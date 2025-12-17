using System;
using UnityEngine;

public class enemyTarget : MonoBehaviour
{
    public float health = 50f;

    // 🔴 EKLE
    public GameObject plasmaExplosionEffect;

    public void takeDamage(float amount)
    {
        health -= amount;

        if (health <= 0f)
        {
            Die();
        }
    }

    void Die()
    {
        // 🔴 EFFECT OLUŞTUR
        GameObject effect = Instantiate(
            plasmaExplosionEffect,
            transform.position,
            Quaternion.identity
        );

        // 🔴 1 SN SONRA EFFECT SİL
        Destroy(effect, 1f);

        // 🔴 ENEMY SİL
        Destroy(gameObject);
    }
}
