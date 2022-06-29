using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Player))]
[RequireComponent(typeof(Shooter))]
[RequireComponent(typeof(AudioSource))]
public class PlayerSound : MonoBehaviour
{
    [SerializeField] private AudioClip _hit = null;
    [SerializeField] private AudioClip _die = null;

    private AudioSource _audioSource = null;
    private Player _player = null;
    private Shooter _shooter = null;

    private void Awake()
    {
        _audioSource = GetComponent<AudioSource>();
        _player = GetComponent<Player>();
        _shooter = GetComponent<Shooter>();
    }

    private void OnEnable()
    {
        _player.Damaged += Hit;
        _player.Died += Die;
        _shooter.StartedReload += Reload;
        _shooter.Shooted += Shoot;
        _shooter.EmptedAmmo += NeedAmmo;
    }

    private void OnDisable()
    {
        _player.Damaged -= Hit;
        _player.Died -= Die;
        _shooter.StartedReload -= Reload;
        _shooter.Shooted -= Shoot;
        _shooter.EmptedAmmo -= NeedAmmo;
    }

    private void Hit()
    {
            _audioSource.PlayOneShot(_hit);
    }

    private void Die()
    {
            _audioSource.PlayOneShot(_die);
    }

    private void Reload(Weapon weapon)
    {
            _audioSource.PlayOneShot(weapon.ReloadSound);
    }

    private void Shoot(Weapon weapon)
    {
            _audioSource.PlayOneShot(weapon.ShootSound);
    }

    private void NeedAmmo(Weapon weapon)
    {
            _audioSource.PlayOneShot(weapon.EmptySound);
    }
}
