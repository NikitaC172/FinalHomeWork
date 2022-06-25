using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.Events;
using UnityEngine.EventSystems;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(Collider))]
[RequireComponent(typeof(Mover))]
public class Player : MonoBehaviour
{
    [SerializeField] private int _health = 100;
    [SerializeField] private PlayerStats _playerStats;
    [SerializeField] private Transform _shootPoint;
    [SerializeField] private List<Weapon> _weapons = null;
    [SerializeField] private Mover _mover = null;

    public event UnityAction Dead;
    public event UnityAction Damaged;
    public event UnityAction ChangedWeapon;
    public event UnityAction StartReload;
    public event UnityAction Reloaded;
    public event UnityAction Shooted;
    public event UnityAction EmptyAmmo;

    private Collider _collider;
    private Rigidbody _rigidbody;
    private Weapon _currentWeapon;
    private Vector3 _cover;
    private Vector3 _pointAttack;
    private Vector3 _hitPoint;
    private int _currentWeaponNumber = 0;
    private int _currentHealth = 0;
    private bool _isCover = false;
    private bool _isFirstChangeWeapon = true;
    private bool _isBlockShoot = true;

    public Weapon CurrentWeapon => _currentWeapon;
    public int CurrentHealth => _currentHealth;


    private void Awake()
    {
        _playerStats.LoadGame();
        _playerStats.TransferData(this);
        ChangeWeapon();

        for (int i = 0; i < _weapons.Count; i++)
        {
            _weapons[i].SetMaxAmmoAfterStart();
        }

        _currentWeapon.SetReadyToShoot();
        _currentHealth = _health;
        _collider = GetComponent<Collider>();
        _rigidbody = GetComponent<Rigidbody>();
    }

    private void OnEnable()
    {
        _mover.Moved += SetBlockShoot;
    }

    private void OnDisable()
    {
        _mover.Moved -= SetBlockShoot;
    }

    private void Start()
    {
        ChangedWeapon?.Invoke();
    }

    public void GetData(string weaponName)
    {
        foreach (var weapon in _weapons)
        {
            if (weapon.Name == weaponName)
            {
                weapon.Buy();
            }
        }
    }

    public void SetOffestCover(Transform pointCover, Transform pointForShoot)
    {
        _pointAttack = pointForShoot.position;
        _cover = pointCover.position;
    }

    private void Update()
    {
        if (Input.GetMouseButton(0))//(Input.GetKeyDown(KeyCode.Q))
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

    public void TakeCover()
    {
        Debug.Log(_isCover);
        if (_isCover == false)
        {
            _isCover = true;
            transform.DOMove(_cover, 1, false);
        }
        else
        {
            _isCover = false;
            transform.DOMove(_pointAttack, 1, false);
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
        StartReload?.Invoke();
        _currentWeapon.TryReload();
        yield return new WaitWhile(() => _currentWeapon.IsReloading == true);
        Reloaded?.Invoke();
    }

    private void SetBlockShoot(bool isBlockShoot)
    {
        if (_isCover == true)
        {
            TakeCover();
        }

        _isBlockShoot = isBlockShoot;
    }

    public void Shoot()
    {
        if (_isBlockShoot == false)
        {
            if (_currentWeapon.IsReadyShoot && _currentWeapon.CurrentAmmo > 0)
            {
                _currentWeapon.Shoot(_shootPoint, _hitPoint);
                Shooted?.Invoke();
            }
            else if (_currentWeapon.CurrentAmmo == 0)
            {
                if (Input.GetMouseButtonDown(0))
                {
                    EmptyAmmo?.Invoke();
                }
            }
        }
    }

    public void TakeDamage(int damage)
    {
        _currentHealth -= damage;
        Damaged?.Invoke();

        if (_currentHealth <= 0.1)
        {
            float secondsBeforeActivateEvent = 2f;
            _collider.isTrigger = false;
            _rigidbody.isKinematic = false;
            _rigidbody.useGravity = true;
            Invoke(nameof(ActivateDeadEvent), secondsBeforeActivateEvent);
        }
    }

    private void ActivateDeadEvent()
    {
        Dead?.Invoke();
    }

    public void ChangeWeapon()
    {
        bool isChanged = false;

        if (_isFirstChangeWeapon == true)
        {
            _isFirstChangeWeapon = false;

            if (_weapons[_currentWeaponNumber].IsBuyed == true)
            {
                _currentWeapon = _weapons[_currentWeaponNumber];
                isChanged = true;
            }
        }

        while (isChanged == false)
        {
            _currentWeaponNumber++;

            if (_currentWeaponNumber < _weapons.Count)
            {
                if (_weapons[_currentWeaponNumber].IsBuyed == true)
                {
                    _currentWeapon = _weapons[_currentWeaponNumber];
                    isChanged = true;
                }
            }
            else
            {
                _currentWeaponNumber = 0;

                if (_weapons[_currentWeaponNumber].IsBuyed == true)
                {
                    _currentWeapon = _weapons[_currentWeaponNumber];
                    isChanged = true;
                }
            }
        }

        ChangedWeapon?.Invoke();
        _currentWeapon.SetWeaponAmmoAfterChange();
    }
}
