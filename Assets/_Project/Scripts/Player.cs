using UnityEngine;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour
{
    [Header("Script Bağlantıları")]

    public GameManager gameDirector;
    [Header("Efekt Bağlantıları")]

    public GameObject BloodExplosionEffect;

    private HealthBar _takeDamage;

    [Header("Can ve Hasar Alma")]

    public float HasarAlmaMiktari=10;
    public float CanAlmaMiktarı=30;

    [Header("Silahlar")]
    public bool tabancaObj;
    public bool tufekObj;

    void Start()
    {
        _takeDamage=GetComponent<HealthBar>();
    }

    void Update()
    {
        //if(gameDirector.healthBar.health< 0)
        //{
        //    SceneManager.LoadScene("MainSceneForrest");
        //}
    }
    void PlayerDie()
    {
        // 🔴 EFFECT OLUŞTUR
        GameObject effect = Instantiate(
            BloodExplosionEffect,
            transform.position,
            Quaternion.identity
        );

        // 🔴 1 SN SONRA EFFECT SİL
        Destroy(effect, 1f);

        // 🔴 ENEMY SİL
        Destroy(gameObject);
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Heal"))
        {
            gameDirector.healthBar.takeHeal(CanAlmaMiktarı);
        }
        

        if (other.CompareTag("Enemy"))
        {
            gameDirector.healthBar.takeDamage(HasarAlmaMiktari);
            // 🔴 EFFECT OLUŞTUR
            GameObject effect = Instantiate(
                BloodExplosionEffect,
                transform.position,
                Quaternion.identity
            );
            Destroy(effect, 0.5f);
        }
        if(other.CompareTag("Heal"))
        {
            gameDirector.healthBar.takeHeal(CanAlmaMiktarı);
        }
        if (other.CompareTag("Tufek"))
        {
            GameManager.Instance.tufekVar = true;
        }

        if (other.CompareTag("Tabanca"))
        {
            GameManager.Instance.tabancaVar = true;
        }


        if (other.CompareTag("GateDesert"))
        {
            print("triggerlandın");
            
            SceneManager.LoadScene("Desert");
        }

        
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            gameDirector.healthBar.takeDamage(HasarAlmaMiktari);

            // Çarpışma noktasında efekt oluştur
            Vector3 hitPoint = collision.contacts[0].point;
            GameObject effect = Instantiate(BloodExplosionEffect, hitPoint, Quaternion.identity);

            // 0.5 saniye sonra sil
            Destroy(effect, 0.5f);

            print("HasarAldın");
        }

    }



}
