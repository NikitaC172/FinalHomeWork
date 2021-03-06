using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Enemy))]
public class Health : MonoBehaviour
{
    [SerializeField] private Image _image;
    [SerializeField] private Enemy _enemy;

    private void OnEnable()
    {
        _enemy.Damaged += OnValueChanged;
    }

    private void OnValueChanged(int value, int maxValue)
    {
        _image.fillAmount = (float)value / maxValue;
    }

    private void OnDisable()
    {
        _enemy.Damaged -= OnValueChanged;
    }
}
