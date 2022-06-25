using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class WeaponUI : MonoBehaviour
{
    [SerializeField] private Text _label;
    [SerializeField] private Text _price;
    [SerializeField] private Image _icon;
    [SerializeField] private Button _sellButton;

    private Weapon _weapon;

    public event UnityAction<Weapon, WeaponUI> SellButtonClick;

    private void OnEnable()
    {
        _sellButton.onClick.AddListener(OnButtonClick);
        _sellButton.onClick.AddListener(TryLockItem);
    }

    private void OnDisable()
    {
        _sellButton.onClick.RemoveListener(OnButtonClick);
        _sellButton.onClick.RemoveListener(TryLockItem);
    }

    public void Render(Weapon weapon)
    {
        _weapon = weapon;
        _label.text = weapon.Name;
        _price.text = weapon.Price.ToString();

        if (_weapon.IsBuyed)
        {
            _icon.gameObject.SetActive(true);
        }
    }

    private void TryLockItem()
    {
        if (_weapon.IsBuyed)
        {
            _sellButton.interactable = false;
            _icon.gameObject.SetActive(true);
        }
    }

    private void OnButtonClick()
    {
        SellButtonClick?.Invoke(_weapon, this);
    }
}
