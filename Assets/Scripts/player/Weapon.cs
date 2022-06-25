using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public abstract class Weapon : MonoBehaviour
{
    [SerializeField] private string _name;
    [SerializeField] private int _price;
    [SerializeField] protected Bullet Bullet;
    [SerializeField] protected float _delayBetweenShoot;
    [SerializeField] protected int _maxAmmo;
    [SerializeField] private bool _isBuyed;
    [SerializeField] protected float _secondsReload;
    [SerializeField] private AudioClip _shootSound = null;
    [SerializeField] private AudioClip _reloadSound = null;
    [SerializeField] private AudioClip _emptySound = null;

    protected int _currentAmmo = 0;
    protected bool _isReadyShoot = false;
    protected bool _isReloading = false;

    public string Name => _name;
    public bool IsReadyShoot => _isReadyShoot;
    public bool IsReloading => _isReloading;
    public AudioClip ShootSound => _shootSound;
    public AudioClip ReloadSound => _reloadSound;
    public AudioClip EmptySound => _emptySound;
    public int CurrentAmmo => _currentAmmo;
    public int Price => _price;
    public bool IsBuyed => _isBuyed;

    public abstract void Shoot(Transform shootPoint, Vector3 hitPoint);

    public void TryReload()
    {
        if (_isReloading == false)
        {
            _isReloading = true;
            _isReadyShoot = false;
            Invoke(nameof(Reload), _secondsReload);
        }
    }

    public void Buy()
    {
        _isBuyed = true;
    }

    public void ResetOwner()
    {
        _isBuyed = false;
    }

    public void SetMaxAmmoAfterStart()
    {
        _currentAmmo = _maxAmmo;
    }

    public void SetReadyToShoot()
    {
        _isReadyShoot = true;
        _isReloading = false;
    }

    public void SetWeaponAmmoAfterChange()
    {
        SetReadyToShoot();
    }

    private void Reload()
    {
        _currentAmmo = _maxAmmo;
        SetReadyToShoot();
    }
}
