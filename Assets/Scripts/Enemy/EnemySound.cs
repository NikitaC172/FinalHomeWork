using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
[RequireComponent(typeof(WeaponEnemy))]
[RequireComponent(typeof(Enemy))]
public class EnemySound : MonoBehaviour
{
    [SerializeField] private AudioClip _shootSound = null;
    [SerializeField] private AudioClip _hitSound = null;
    [SerializeField] private AudioClip _dieSound = null;

    private AudioSource _audioSource = null;
    private WeaponEnemy _weaponEnemy = null;
    private Enemy _enemy = null;

    private void Awake()
    {
        _audioSource = GetComponent<AudioSource>();
        _enemy = GetComponent<Enemy>();
        _weaponEnemy = GetComponent<WeaponEnemy>();
    }

    private void OnEnable()
    {
        _enemy.Died += Die;
        _enemy.Damaged += Hit;
        _weaponEnemy.Shooted += Shoot;
    }

    private void OnDisable()
    {
        _enemy.Died -= Die;
        _enemy.Damaged -= Hit;
        _weaponEnemy.Shooted -= Shoot;
    }

    private void Hit(int currentValue, int maxValue)
    {
        _audioSource.PlayOneShot(_hitSound);
    }

    private void Die()
    {
            _audioSource.PlayOneShot(_dieSound);
    }

    private void Shoot()
    {
        _audioSource.PlayOneShot(_shootSound);
    }
}
