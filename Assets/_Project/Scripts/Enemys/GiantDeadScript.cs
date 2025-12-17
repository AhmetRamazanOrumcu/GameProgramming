
using UnityEngine;
public class GiantDeadScript : MonoBehaviour
{
    private GiantEnemyScript enemy;

    private void Awake()
    {
        enemy = GetComponentInParent<GiantEnemyScript>();

        if (enemy == null)
            Debug.LogError("EnemyDeneme3 BULUNAMADI!", this);
    }

    // Gun BURAYI ÇAĞIRIYOR
    public void takeDamage(float amount)
    {
        if (enemy == null) return;

        Debug.Log("Giant HASAR ALDI: " + amount);
        enemy.TakeDamage(amount);
    }
}

