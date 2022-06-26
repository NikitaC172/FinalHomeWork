using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.Events;

[RequireComponent(typeof(Collider))]
public class Enemy : MonoBehaviour
{
    [SerializeField] private int _health = 100;
    [SerializeField] private float _secondsBeforeFirstShoot = 5f;
    [SerializeField] private Transform _shootPoint;
    [SerializeField] private Transform _player = null;

    private Enemy _enemy;
    private int _currentHealth = 0;
    private Collider _collider = null;
    public event UnityAction WeaponAimed;
    public event UnityAction Died;
    public event UnityAction<int, int> Damaged;
    public event UnityAction<bool> ChangedReadyToShoot;

    public Transform ShootPoint => _shootPoint;

    private void Awake()
    {
        _enemy = GetComponent<Enemy>();
        _collider = GetComponent<Collider>();
    }

    private void OnEnable()
    {
        GetComponent<Health>().enabled = true;
    }

    private void Start()
    {
        _currentHealth = _health;
        Invoke(nameof(SetReadyShoot), _secondsBeforeFirstShoot);
        WeaponAimed?.Invoke();
    }

    private void FixedUpdate()
    {
        AimAtTheTarget();
    }

    public void TakeDamage(int damage)
    {
        float verificationError = 0.1f;
        _currentHealth -= damage;
        Damaged?.Invoke(_currentHealth, _health);

        if (_currentHealth <= verificationError)
        {
            _collider.enabled = false;
            Died?.Invoke();
            ChangedReadyToShoot?.Invoke(false);
            _enemy.enabled = false;
        }
    }

    private void AimAtTheTarget()
    {
        float secondsForRotateToAim = 1.0f;
        Vector3 heading = _player.transform.position - transform.position;
        Quaternion rotation = Quaternion.LookRotation(heading);
        transform.DORotateQuaternion(rotation, secondsForRotateToAim);
        _shootPoint.LookAt(_player);
    }

    private void SetReadyShoot()
    {
        float verificationError = 0.1f;

        if (_currentHealth > verificationError)
        {
            ChangedReadyToShoot?.Invoke(true);
        }
    }
}
