using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Enemy))]
[RequireComponent(typeof(WeaponEnemy))]
[RequireComponent(typeof(Animator))]
public class AnimatorEnemy : MonoBehaviour
{
    [SerializeField] private string _reloadAnimation = "Reload";
    [SerializeField] private string _idleAnimation = "Idle_Shoot";
    [SerializeField] private string _ShootAnimation = "Shoot";
    [SerializeField] private string _deadAnimation = "Die";

    private Enemy _enemy = null;
    private WeaponEnemy _enemyWeapon = null;
    private Animator _animator = null;

    private void Awake()
    {
        _animator = GetComponent<Animator>();
        _enemy = GetComponent<Enemy>();
        _enemyWeapon = GetComponent<WeaponEnemy>();
    }

    private void OnEnable()
    {
        _enemy.WeaponAimed += GetReadyShoot;
        _enemy.Died += Die;
        _enemyWeapon.Reloaded += Reload;
        _enemyWeapon.Shooted += Shoot;
    }

    private void Reload()
    {
            _animator.Play(_reloadAnimation);
    }

    private void Shoot()
    {
            _animator.Play(_ShootAnimation);
    }

    private void Die()
    {
            _animator.Play(_deadAnimation);
    }

    private void GetReadyShoot()
    {
            _animator.Play(_idleAnimation);
    }

    private void OnDisable()
    {
        _enemy.WeaponAimed -= GetReadyShoot;
        _enemy.Died -= Die;
        _enemyWeapon.Reloaded -= Reload;
        _enemyWeapon.Shooted -= Shoot;
    }
}
