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

    private int _currentHealth = 0;
    private bool _isReadyToShoot = false;
    private Collider _collider = null;
    public event UnityAction AimWeapon;
    public event UnityAction Die;
    public event UnityAction<int, int> Hited;

    public Transform ShootPoint => _shootPoint;
    public bool IsReadyToShoot => _isReadyToShoot;

    private void Awake()
    {
        _collider = GetComponent<Collider>();        
    }

    private void OnEnable()
    {
        GetComponent<HealthCircle>().enabled = true;
    }

    private void Start()
    {
        _currentHealth = _health;
        AimWeapon?.Invoke();
    }

    private void FixedUpdate()
    {
        if (_currentHealth > 0.1f)
        {
            AimAtTheTarget();
        }
    }

    public void TakeDamage(int damage)
    {
        _currentHealth -= damage;
        Hited?.Invoke(_currentHealth, _health);

        if (_currentHealth <= 0.1f)
        {
            _collider.enabled = false;
            _isReadyToShoot = false;
            Die?.Invoke();
        }
    }

    private void AimAtTheTarget()
    {
        float secondsForRotateToAim = 1.0f;
        Vector3 heading = _player.transform.position - transform.position;
        Quaternion rotation = Quaternion.LookRotation(heading);
        transform.DORotateQuaternion(rotation, secondsForRotateToAim);
        _shootPoint.LookAt(_player);

        if (_isReadyToShoot == false)
        {
            Invoke(nameof(SetReadyShoot), _secondsBeforeFirstShoot);
        }
    }

    private void SetReadyShoot()
    {
        if (_currentHealth > 0.1f)
        {
            _isReadyToShoot = true;
        }
    }
}
