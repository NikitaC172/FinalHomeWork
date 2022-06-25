using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelLoader : MonoBehaviour
{
    [SerializeField] private string _name = null;
    [SerializeField] private int _reward = 0;
    [SerializeField] private PlayerStats _playerStats;
    [SerializeField] private Sprite _label = null;
    [SerializeField] private Text _textName = null;
    [SerializeField] private Text _textReward = null;
    [SerializeField] private Image _image = null;

    private Button _buttonChoise;

    private void Awake()
    {
        _buttonChoise = GetComponent<Button>();
        _textName.text = _name;
        _textReward.text = _reward.ToString();
        _image.sprite = _label;
    }

    private void OnEnable()
    {
        _buttonChoise.onClick.AddListener(SelectLevel);
    }

    private void OnDisable()
    {
        _buttonChoise.onClick.RemoveListener(SelectLevel);
    }

    private void SelectLevel()
    {
        Time.timeScale = 1;
        _playerStats.SetReward(_reward);
        SceneManager.LoadScene(_name);
    }
}
