using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

[RequireComponent(typeof(Player))]
public class PlayerStats : MonoBehaviour
{
    [SerializeField] private List<string> _nameWeapons;
    [SerializeField] private List<Weapon> _weapons;
    [SerializeField] private Weapon _firstWeapon;
    [SerializeField] private int _money = 0;
    [SerializeField] private int _reward;

    private Player _player;
    private string _nameSaveFile = "/PlayerStats.dat";

    public int Money => _money;
    public List<string> NameWeapons => _nameWeapons;

    private void Awake()
    {
        _player = GetComponent<Player>();
        GetSaveData();
    }

    private void OnEnable()
    {
        _player.ChangedReward += SetReward;
        _player.ChangedMoney += ChangeMoney;
        _player.BuyedWeapon += AddWeapon;
    }

    private void OnDisable()
    {
        _player.ChangedReward -= SetReward;
        _player.ChangedMoney -= ChangeMoney;
        _player.BuyedWeapon -= AddWeapon;
    }

    public void GetSaveData()
    {
        if (File.Exists(Application.persistentDataPath + _nameSaveFile))
        {
            LoadGame();
        }
        else
        {
            ResetData();
        }
    }

    public void TransferData()
    {
        _player.GetData(_money, _reward);
        TransferWeapon();
    }

    public void ResetData()
    {
        if (File.Exists(Application.persistentDataPath + _nameSaveFile))
        {
            File.Delete(Application.persistentDataPath + _nameSaveFile);
        }

        _nameWeapons.Clear();
        _nameWeapons.Add(_firstWeapon.Name);
        _money = 100;
        _reward = 0;
        SaveGame();
    }

    public void LoadGame()
    {
        if (File.Exists(Application.persistentDataPath + _nameSaveFile))
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file =
              File.Open(Application.persistentDataPath + _nameSaveFile, FileMode.Open);
            SaveData data = (SaveData)bf.Deserialize(file);
            file.Close();
            _money = data.MoneyInStore;
            _reward = data.RewardInStore;

            foreach (var weapon in data.WeaponsInStore)
            {
                _nameWeapons.Add(weapon);
            }
        }
    }

    private void TransferWeapon()
    {
        foreach (var nameWeapon in _nameWeapons)
        {
            foreach (var weapon in _weapons)
            {
                if (weapon.Name == nameWeapon)
                {
                    _player.GetWeapon(weapon);
                }
            }
        }
    }

    private void SetReward(int reward)
    {
        _reward = reward;
        SaveGame();
    }

    private void AddWeapon(string nameWeapon)
    {
        _nameWeapons.Add(nameWeapon);
        SaveGame();
    }

    private void ChangeMoney(int deltaMoney)
    {
        _money += deltaMoney;
        SaveGame();
    }

    private void SaveGame()
    {
        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Create(Application.persistentDataPath + _nameSaveFile);
        SaveData data = new SaveData();
        data.AddData(_money, _reward, _nameWeapons);
        bf.Serialize(file, data);
        file.Close();
    }
}

[Serializable]
public class SaveData
{
    private int _moneyInStore = 0;
    private List<string> _weaponsInStore = new List<string>();
    private int _rewardInStore = 0;

    public int MoneyInStore => _moneyInStore;
    public int RewardInStore => _rewardInStore;
    public List<string> WeaponsInStore => _weaponsInStore;

    public void AddData(int money, int reward, List<string> nameWeapons)
    {
        _moneyInStore = money;
        _rewardInStore = reward;

        foreach (var weapon in nameWeapons)
        {
            _weaponsInStore.Add(weapon);
        }
    }
}


