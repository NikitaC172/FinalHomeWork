using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Enemy))]
public class WeaponEnemy : MonoBehaviour
{
    [SerializeField] private Bullet Bullet;
    [SerializeField] private int _numberOfShots = 2;
    [SerializeField] private float _secondsBetweenShots = 1;
    [SerializeField] private float _secondsReload = 5;

    private Enemy _enemy = null;
    private Transform _shootPoint = null;
    private bool _isReadyToShoot = false;
    public event UnityAction Reloaded;
    public event UnityAction Shooted;

    private void Awake()
    {
        _enemy = GetComponent<Enemy>();
        _shootPoint = _enemy.ShootPoint;
    }

    private void OnEnable()
    {
        _enemy.ChangedReadyToShoot += ChangeReadyToShoot;
    }

    private void OnDisable()
    {
        _enemy.ChangedReadyToShoot -= ChangeReadyToShoot;
    }

    private void ChangeReadyToShoot(bool isReady)
    {
        _isReadyToShoot = isReady;

        if (_isReadyToShoot)
        {
            StartCoroutine(Shoot());
        }
        else
        {
            StopCoroutine(Shoot());
        }
    }

    private IEnumerator Shoot()
    {
        var waitSecondsBetweenShots = new WaitForSeconds(_secondsBetweenShots);
        var waitSecondsBetweenReload = new WaitForSeconds(_secondsReload);

        while (_isReadyToShoot)
        {
            for (int i = 0; i < _numberOfShots; i++)
            {
                if (_isReadyToShoot)
                {
                    Shooted?.Invoke();
                    Instantiate(Bullet, _shootPoint.transform.position, _shootPoint.transform.rotation);
                }
                yield return waitSecondsBetweenShots;                
            }

            if (_isReadyToShoot)
            {
                Reloaded?.Invoke();
            }

            yield return waitSecondsBetweenReload;
        }
    }
}
