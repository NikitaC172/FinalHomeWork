using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Player))]
public class ChangerWeapon : MonoBehaviour
{
    [SerializeField] private List<Weapon> _weapons = null;

    private Weapon _currentWeapon;
    private Player _player;
    private int _currentWeaponNumber = 0;
    private bool _isFirstChangeWeapon = true;

    public event UnityAction<Weapon> ChangedWeapon;

    private void Awake()
    {
        _player = GetComponent<Player>();
    }

    private void Start()
    {
        GetWeapon();
        ChangeWeapon();
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

        ChangedWeapon?.Invoke(_currentWeapon);
        _currentWeapon.SetWeaponAmmoAfterChange();
    }

    private void GetWeapon()
    {
        foreach (var weapon in _player.Weapons)
        {
            weapon.SetMaxAmmoAfterStart();
            _weapons.Add(weapon);
        }
    }
}
