using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] private int _damage;
    [SerializeField] private float _speed;

    private float _delayRemove = 20.0f;

    public int Damage => _damage;

    private void OnEnable()
    {
        Invoke(nameof(Remove), _delayRemove);
    }

    private void Update()
    {
        transform.Translate(Vector3.forward * _speed * Time.deltaTime);        
    }

    private void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.TryGetComponent(out Player player))
        {
            player.TakeDamage(_damage);
            Destroy(gameObject);
        }
        else if (collision.gameObject.TryGetComponent(out Enemy enemy))
        {
            enemy.TakeDamage(_damage);
            Destroy(gameObject);
        }
        else if ((collision.gameObject.TryGetComponent(out Block block)))
        {
            block.TakeDamage(_damage);
            Destroy(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Remove()
    {
        Destroy(gameObject);
    }
}
