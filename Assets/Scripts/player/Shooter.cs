using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

[RequireComponent(typeof(ChangerWeapon))]
[RequireComponent(typeof(Mover))]
public class Shooter : MonoBehaviour
{
    [SerializeField] private Transform _shootPoint;

    private ChangerWeapon _changerWeapon;
    private Mover _mover;
    private Weapon _currentWeapon;
    private Vector3 _hitPoint;
    private bool _isBlockShoot = false;

    public event UnityAction<Weapon> Shooted;
    public event UnityAction<Weapon> EmptedAmmo;
    public event UnityAction<Weapon> StartedReload;
    public event UnityAction<Weapon> Reloaded;

    public Weapon CurrentWeapon => _currentWeapon;

    private void Awake()
    {
        _changerWeapon = GetComponent<ChangerWeapon>();
        _mover = GetComponent<Mover>();
    }

    private void OnEnable()
    {
        _changerWeapon.ChangedWeapon += SetCurrentWeapon;
        _mover.MovedForward += SetBlockShoot;
    }

    private void Update()
    {
        if (Input.GetMouseButton(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit))
            {
                if (EventSystem.current.IsPointerOverGameObject())
                {
                    return;
                }
                else
                {
                    _hitPoint = hit.point;
                    Shoot();
                }
            }
        }
    }

    private void OnDisable()
    {
        _changerWeapon.ChangedWeapon -= SetCurrentWeapon;
        _mover.MovedForward += SetBlockShoot;
    }

    public void Shoot()
    {
        if (_isBlockShoot == false)
        {
            if (_currentWeapon.IsReadyShoot && _currentWeapon.CurrentAmmo > 0)
            {
                _currentWeapon.Shoot(_shootPoint, _hitPoint);
                Shooted?.Invoke(_currentWeapon);
            }
            else if (_currentWeapon.CurrentAmmo == 0)
            {
                if (Input.GetMouseButtonDown(0))
                {
                    EmptedAmmo?.Invoke(_currentWeapon);
                }
            }
        }
    }

    public void TryReload()
    {
        if (_currentWeapon.IsReloading == false)
        {
            StartCoroutine(WaitReload());
        }
    }

    private IEnumerator WaitReload()
    {
        StartedReload?.Invoke(_currentWeapon);
        _currentWeapon.TryReload();
        yield return new WaitWhile(() => _currentWeapon.IsReloading == true);
        Reloaded?.Invoke(_currentWeapon);
    }

    private void SetCurrentWeapon(Weapon weapon)
    {
        _currentWeapon = weapon;
        Reloaded?.Invoke(_currentWeapon);
    }

    private void SetBlockShoot(bool isBlockShoot)
    {
        _isBlockShoot = isBlockShoot;
    }
}
