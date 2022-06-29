using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.Events;
using UnityEngine.EventSystems;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(Collider))]
public class Player : MonoBehaviour
{
    [SerializeField] private int _health = 100;
    [SerializeField] private int _money = 0;
    [SerializeField] private PlayerStats _playerStats;
    [SerializeField] private List<Weapon> _weapons;

    private int _rewardForLevel;
    private Collider _collider;
    private Rigidbody _rigidbody;
    private int _currentHealth = 0;

    public event UnityAction Died;
    public event UnityAction Damaged;
    public event UnityAction<string> BuyedWeapon;
    public event UnityAction<int> ChangedMoney;
    public event UnityAction<int> ChangedReward;

    public int CurrentHealth => _currentHealth;
    public int Money => _money;
    public List<Weapon> Weapons => _weapons;

    private void Awake()
    {
        _playerStats.TransferData();
        _currentHealth = _health;
        _collider = GetComponent<Collider>();
        _rigidbody = GetComponent<Rigidbody>();
    }

    public void SetReward(int reward)
    {
        _rewardForLevel = reward;
        ChangedReward?.Invoke(_rewardForLevel);
    }

    public void GetReward()
    {
        _money += _rewardForLevel;
        ChangedMoney?.Invoke(_rewardForLevel);
    }

    public void BuyWeapon(Weapon weapon)
    {
        _money -= weapon.Price;
        ChangedMoney?.Invoke(-weapon.Price);
        _weapons.Add(weapon);
        BuyedWeapon?.Invoke(weapon.Name);
    }

    public void GetData(int money, int reward)
    {
        _money = money;
        _rewardForLevel = reward;
    }

    public void GetWeapon(Weapon weapon)
    {
        _weapons.Add(weapon);
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
        Died?.Invoke();
    }     
}
