using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

[RequireComponent(typeof(Collider))]
public class ActivatorEnemy : MonoBehaviour
{
    [SerializeField] private List<Enemy> _enemies = null;    

    private Mover _mover;
    private int _secondsToRemove = 10;
    private int _countDeadEnemy = 0;

    private void OnTriggerEnter(Collider collider)
    {
        _countDeadEnemy = 0;

        if (collider.TryGetComponent<Player>(out Player player))
        {
            if (_enemies.Count != 0)
            {
                foreach (var enemy in _enemies)
                {
                    enemy.gameObject.SetActive(true);
                    enemy.GetComponent<Enemy>().enabled = true;
                    enemy.GetComponent<WeaponEnemy>().enabled = true;
                    enemy.Died += CountDead;
                }
            }
        }

        if (collider.TryGetComponent<Mover>(out Mover mover))
        {
            _mover = mover;

            if (_enemies.Count == 0)
            {
                SkipStage();
            }
        }
    }

    private void OnTriggerExit(Collider collider)
    {
        if (collider.GetComponent<Player>())
        {
            foreach (var enemy in _enemies)
            {
                enemy.Died -= CountDead;
                Destroy(enemy.gameObject, _secondsToRemove);
            }
        }
    }

    private void SkipStage()
    {
        _mover.ShowMoveButton();
    }

    private void CountDead()
    {
        _countDeadEnemy++;

        if (_countDeadEnemy == _enemies.Count)
        {
            _mover.ShowMoveButton();
        }
    }
}
