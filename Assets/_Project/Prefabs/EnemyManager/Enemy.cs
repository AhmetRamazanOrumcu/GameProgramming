using DG.Tweening;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    public Player _player;
    public float speed = 3f;
    private Rigidbody _rb;
    public NavMeshAgent navMeshAgent;
    private Animator _animator;
    public Transform zPrefab;

    private bool _isWalking;

    private Transform _z1;
    private Transform _z2;

    private void Start()
    {
        StartEnemy(_player);
    }
    public void StartEnemy(Player player)
    {
        _player = player;
        _rb = GetComponent<Rigidbody>();
        _animator = GetComponent<Animator>();

        if (navMeshAgent == null)
            navMeshAgent = GetComponent<NavMeshAgent>();

        if (navMeshAgent != null)
            navMeshAgent.speed = speed;

        transform.Rotate(0, Random.Range(-180, 180), 0);
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
        if (_player == null || navMeshAgent == null)
            return;

        // Player'ý takip et
        navMeshAgent.destination = _player.transform.position;

        // Animasyon ve Z efektleri
        if (!_isWalking)
        {
            _isWalking = true;

            if (_animator != null)
                _animator.SetTrigger("Walk");

            if (_z1 != null) { _z1.DOKill(); Destroy(_z1.gameObject); }
            if (_z2 != null) { _z2.DOKill(); Destroy(_z2.gameObject); }
        }
    }

    public void Stop()
    {
        if (navMeshAgent != null)
            navMeshAgent.speed = 0;

        if (_animator != null)
            _animator.SetTrigger("Idle");
    }
}
