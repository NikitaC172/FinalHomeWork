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
    [SerializeField] private ChangerWeapon _changerWeapon;
    [SerializeField] private Shooter _shooter;
    [SerializeField] private Mover _mover;

    private void OnEnable()
    {
        _player.Damaged += ChangeHealth;
        _changerWeapon.ChangedWeapon += ChangeNameWeapon;
        _changerWeapon.ChangedWeapon += ChangedAmmo;
        _mover.ShowedButtonNewStage += ShowMoveButton;
        _shooter.Reloaded += ChangedAmmo;
        _shooter.Shooted += ChangedAmmo;
    }

    private void ChangeNameWeapon(Weapon weapon)
    {
        _nameWeapon.text = weapon.Name;
    }

    private void Start()
    {
        _health.text = _player.CurrentHealth.ToString();
    }

    private void OnDisable()
    {
        _player.Damaged -= ChangeHealth;
        _changerWeapon.ChangedWeapon -= ChangeNameWeapon;
        _changerWeapon.ChangedWeapon -= ChangedAmmo;
        _mover.ShowedButtonNewStage -= ShowMoveButton;
        _shooter.Reloaded -= ChangedAmmo;
        _shooter.Shooted -= ChangedAmmo;
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

    private void ChangedAmmo(Weapon weapon)
    {
        _ammo.text = weapon.CurrentAmmo.ToString();
    }
}
