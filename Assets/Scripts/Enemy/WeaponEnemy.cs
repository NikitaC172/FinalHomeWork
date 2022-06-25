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
    private bool _isShoot = false;
    public event UnityAction ReloadWeapon;
    public event UnityAction ShootWeapon;

    private void Awake()
    {
        _enemy = GetComponent<Enemy>();
        _shootPoint = _enemy.ShootPoint;
    }

    private void FixedUpdate()
    {
        if (_isShoot == false)
        {
            if (_enemy.IsReadyToShoot)
            {
                StartCoroutine(Shoot());
            }
        }
        else if (_isShoot)
        {
            if (_enemy.IsReadyToShoot == false)
            {
                StopCoroutine(Shoot());
            }
        }
    }

    private IEnumerator Shoot()
    {
        _isShoot = true;
        var waitSecondsBetweenShots = new WaitForSeconds(_secondsBetweenShots);
        var waitSecondsBetweenReload = new WaitForSeconds(_secondsReload);

        while (_enemy.IsReadyToShoot)
        {
            for (int i = 0; i < _numberOfShots; i++)
            {
                if (_enemy.IsReadyToShoot)
                {
                    ShootWeapon?.Invoke();
                    Instantiate(Bullet, _shootPoint.transform.position, _shootPoint.transform.rotation);
                }
                yield return waitSecondsBetweenShots;                
            }

            if (_enemy.IsReadyToShoot)
            {
                ReloadWeapon?.Invoke();
            }

            yield return waitSecondsBetweenReload;
        }

        _isShoot = false;
    }
}
