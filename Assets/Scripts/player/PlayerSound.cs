using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Player))]
[RequireComponent(typeof(AudioSource))]
public class PlayerSound : MonoBehaviour
{
    [SerializeField] private AudioClip _hit = null;
    [SerializeField] private AudioClip _die = null;

    private AudioSource _audioSource = null;
    private Player _player = null;

    private void Awake()
    {
        _audioSource = GetComponent<AudioSource>();
        _player = GetComponent<Player>();
    }

    private void OnEnable()
    {
        _player.Damaged += Hit;
        _player.Dead += Die;
        _player.StartReload += Reload;
        _player.Shooted += Shoot;
        _player.EmptyAmmo += NeedAmmo;
    }

    private void Start()
    {
        //_player.CurrentWeapon.StartReload += Reload;
        //_player.CurrentWeapon.Shooted += Shoot;
        //_player.CurrentWeapon.EmptyAmmo += NeedAmmo;
    }

    private void OnDisable()
    {
        _player.Damaged -= Hit;
        _player.Dead -= Die;
        _player.StartReload -= Reload;
        _player.Shooted -= Shoot;
        //_player.CurrentWeapon.StartReload -= Reload;
        //_player.CurrentWeapon.Shooted -= Shoot;
        //_player.CurrentWeapon.EmptyAmmo -= NeedAmmo;
        _player.EmptyAmmo -= NeedAmmo;
    }

    private void Hit()
    {
        if (_hit != null)
        {
            _audioSource.PlayOneShot(_hit);
        }
    }

    private void Die()
    {
        if (_die != null)
        {
            _audioSource.PlayOneShot(_die);
        }
    }

    private void Reload()
    {
        if (_player.CurrentWeapon.ReloadSound != null)
        {
            _audioSource.PlayOneShot(_player.CurrentWeapon.ReloadSound);
        }
    }

    private void Shoot()
    {
        if (_player.CurrentWeapon.ShootSound != null)
        {
            _audioSource.PlayOneShot(_player.CurrentWeapon.ShootSound);
        }
    }

    private void NeedAmmo()
    {
        if (_player.CurrentWeapon.EmptySound != null)
        {
            _audioSource.PlayOneShot(_player.CurrentWeapon.EmptySound);
        }
    }
}
