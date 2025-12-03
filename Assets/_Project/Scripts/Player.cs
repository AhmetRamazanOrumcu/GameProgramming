using UnityEngine;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour
{
    public GameManager gameDirector;
    private HealthBar _takeDamage;
    void Start()
    {
        _takeDamage=GetComponent<HealthBar>();
    }

    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {


        if (other.CompareTag("Enemy"))
        {
            gameDirector.healthBar.takeDamage(Random.Range(10,51));
        }
        if(other.CompareTag("Heal"))
        {
            gameDirector.healthBar.takeHeal(50);
        }

        if (other.CompareTag("GateDesert"))
        {
            print("triggerlandýn");
            SceneManager.LoadScene("Desert");
        }

    }
    


}
