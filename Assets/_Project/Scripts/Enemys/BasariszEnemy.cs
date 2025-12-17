using DG.Tweening;
using UnityEngine;
using UnityEngine.AI;

public class GiantEnemy : MonoBehaviour
{
    private Player _player;
    private Rigidbody _rb;
    private Animator _animator;
    private NavMeshAgent navMeshAgent;

    [Header("Enemy Settings")]
    public float speed = 3.5f;
    public Transform zPrefab;

    private bool _isWalking;
    private Transform _z1;
    private Transform _z2;

    [Header("Gameplay")]
    public bool isAppleCollected;

    private void Awake()
    {
        // Rigidbody fizik motorunu kapat
        _rb = GetComponent<Rigidbody>();
        if (_rb != null)
        {
            _rb.isKinematic = true;
            _rb.useGravity = false;
        }

        // Animator ve NavMeshAgent
        _animator = GetComponentInChildren<Animator>();
        navMeshAgent = GetComponent<NavMeshAgent>();

        if (navMeshAgent != null)
        {
            navMeshAgent.speed = speed;
            navMeshAgent.isStopped = true;        // Baþlangýçta duracak
            navMeshAgent.updateRotation = true;   // Agent kendi yönünü güncellesin
            navMeshAgent.updatePosition = true;   // Agent pozisyonunu güncellesin
        }

        // Sahnedeki Player'ý bul
        _player = FindFirstObjectByType<Player>();
        if (_player == null)
            Debug.LogError("Player sahnede bulunamadý!");
    }

    private void Start()
    {
        // Rasgele dönüþ
        transform.Rotate(0, Random.Range(-180, 180), 0);

        // Z simgelerini oluþtur
        CreateAndAnimateZ();
    }

    private void CreateAndAnimateZ()
    {
        if (zPrefab == null) return;

        _z1 = Instantiate(zPrefab, transform);
        _z1.position = transform.position + Vector3.up * 2;
        _z1.localScale = Vector3.zero;
        _z1.DOMoveY(_z1.position.y + 1, 1f).SetEase(Ease.Linear).SetLoops(-1, LoopType.Restart);
        _z1.DOScale(1, 1f).SetLoops(-1, LoopType.Restart);

        _z2 = Instantiate(zPrefab, transform);
        _z2.position = transform.position + Vector3.up * 2;
        _z2.localScale = Vector3.zero;
        _z2.DOMoveY(_z2.position.y + 1, 1f).SetEase(Ease.Linear).SetLoops(-1, LoopType.Restart).SetDelay(0.5f);
        _z2.DOScale(1, 1f).SetLoops(-1, LoopType.Restart).SetDelay(0.5f);
    }

    private void Update()
    {
        if (isAppleCollected && _player != null && navMeshAgent != null)
        {
            // Hareketi baþlat
            navMeshAgent.isStopped = false;
            navMeshAgent.destination = _player.transform.position;

            // Animasyonu tetikle
            if (!_isWalking)
            {
                _isWalking = true;
                if (_animator != null)
                    _animator.SetTrigger("Walk");

                // Z simgelerini yok et
                if (_z1 != null) { _z1.DOKill(); Destroy(_z1.gameObject); }
                if (_z2 != null) { _z2.DOKill(); Destroy(_z2.gameObject); }
            }
        }
        else
        {
            // Durma ve idle animasyonu
            if (_isWalking)
            {
                _isWalking = false;
                if (_animator != null)
                    _animator.SetTrigger("Idle");

                if (navMeshAgent != null)
                    navMeshAgent.isStopped = true;
            }
        }
    }

    public void Stop()
    {
        if (navMeshAgent != null)
            navMeshAgent.isStopped = true;

        if (_animator != null)
            _animator.SetTrigger("Idle");
    }
}
