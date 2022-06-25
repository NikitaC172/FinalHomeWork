using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIPlayer : MonoBehaviour
{
    [SerializeField] private Text _health;
    [SerializeField] private Text _ammo;
    [SerializeField] private Text _nameWeapon;
    [SerializeField] private Button _moveButton;
    [SerializeField] private Player _player;
    [SerializeField] private Mover _mover;

    private void OnEnable()
    {
        _player.Damaged += ChangeHealth;
        _player.ChangedWeapon += ChangeNameWeapon;
        _player.ChangedWeapon += ChangedAmmo;
        _mover.DestroyedEnemies += ShowMoveButton;
        _player.Reloaded += ChangedAmmo;
        _player.Shooted += ChangedAmmo;
    }

    private void ChangeNameWeapon()
    {
        _nameWeapon.text = _player.CurrentWeapon.Name;
    }

    private void Start()
    {
        _health.text = _player.CurrentHealth.ToString();
        _ammo.text = _player.CurrentWeapon.CurrentAmmo.ToString();
    }

    private void OnDisable()
    {
        _mover.DestroyedEnemies -= ShowMoveButton;
        _player.ChangedWeapon -= ChangeNameWeapon;
        _player.Damaged -= ChangeHealth;
        _player.Reloaded -= ChangedAmmo;
        _player.Shooted -= ChangedAmmo;
        _player.ChangedWeapon -= ChangedAmmo;
    }

    public void HideMoveButton()
    {
        _moveButton.gameObject.SetActive(false);
    }

    private void ShowMoveButton()
    {
        _moveButton.gameObject.SetActive(true);
    }


    private void ChangeHealth()
    {
        _health.text = _player.CurrentHealth.ToString();
    }

    private void ChangedAmmo()
    {
        _ammo.text = _player.CurrentWeapon.CurrentAmmo.ToString();
    }
}
