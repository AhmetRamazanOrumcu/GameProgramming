using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    [Header("Silah Prefab")]
    public GameObject TufekPrefab;
    public GameObject TabancaPrefab;


    [Header("Manager Glue")]
    public LevelManager LevelManager;
    [Header("Singular Glue")]
    public Player player;
    public HealthBar healthBar;

    [Header("Silah Durumlarý")]
    public bool tufekVar;
    public bool tabancaVar;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
}   
