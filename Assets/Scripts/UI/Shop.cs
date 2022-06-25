using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Shop : MonoBehaviour
{

    [SerializeField] private List<Weapon> _weapons;
    [SerializeField] private PlayerStats _playerStats;
    [SerializeField] private WeaponUI _template;
    [SerializeField] private GameObject _itemContainer;
    [SerializeField] private Text _money;

    private void OnEnable()
    {
        for (int i = 0; i < _weapons.Count; i++)
        {
            if (_playerStats.NameWeapons.Contains(_weapons[i].Name))
            {
                _weapons[i].Buy();
            }
            else
            {
                _weapons[i].ResetOwner();
            }

            AddItem(_weapons[i]);
        }

        _money.text = _playerStats.Money.ToString();
    }

    private void OnDisable()
    {
        foreach (Transform item in _itemContainer.transform)
        {
            Destroy(item.gameObject);
        }
    }

    private void AddItem(Weapon weapon)
    {
        var view = Instantiate(_template, _itemContainer.transform);
        view.SellButtonClick += OnSellButtonClick;
        view.Render(weapon);
    }

    private void OnSellButtonClick(Weapon weapon, WeaponUI view)
    {
        TrySellWeapon(weapon, view);
    }

    private void TrySellWeapon(Weapon weapon, WeaponUI view)
    {
        Debug.Log(_playerStats.Money);

        if (weapon.Price <= _playerStats.Money)
        {
            _playerStats.BuyWeapon(weapon);
            weapon.Buy();
            _money.text = _playerStats.Money.ToString();
            view.SellButtonClick -= OnSellButtonClick;
        }
    }
}
