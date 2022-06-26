using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    [SerializeField] private int _money = 0;
    [SerializeField] private List<string> _nameWeapons;
    [SerializeField] private int _reward;

    private string _nameSaveFile = "/PlayerStats.dat";

    public int Money => _money;
    public List<string> NameWeapons => _nameWeapons;

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

    public void TransferData(Player player)
    {
        foreach (var weapon in _nameWeapons)
        {
            player.GetData(weapon);
        }
    }

    public void SetReward(int reward)
    {
        _reward = reward;
        SaveGame();
    }

    public void GetReward()
    {
        _money += _reward;
        SaveGame();
    }

    public void BuyWeapon(Weapon weapon)
    {
        _money -= weapon.Price;
        _nameWeapons.Add(weapon.Name);
        SaveGame();
    }

    public void ResetData()
    {
        if (File.Exists(Application.persistentDataPath + _nameSaveFile))
        {
            File.Delete(Application.persistentDataPath + _nameSaveFile);
        }

        _nameWeapons.Clear();
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


