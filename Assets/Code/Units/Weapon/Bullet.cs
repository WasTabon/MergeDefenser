using System;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] private int _damage;
    [SerializeField] private float _speed;
    [SerializeField] private GameObject _explosionEffect;

    private GameObject _enemy;
    
    private void Start()
    {
        Invoke("DestroyBullet", 10f);
    }

    private void Update()
    {
        _enemy = EnemyToShootController.instance.lastEnemy;
        Move();
    }

    private void OnCollisionEnter(Collision coll)
    {
        if (coll.gameObject.TryGetComponent(out Enemy _))
        {
            Instantiate(_explosionEffect, transform.position, Quaternion.identity);
            Damage(coll.gameObject);
            Destroy(gameObject);
        }
    }

    private void Move()
    {
        if (_enemy != null)
        {
            Vector3 direction = (_enemy.transform.position - transform.position);
            direction.y = 0f;
            direction.Normalize();
            
            transform.Translate(direction * _speed * Time.deltaTime);
        }
        else
        {
            Instantiate(_explosionEffect, transform.position, Quaternion.identity);
            Destroy(gameObject);
        }
    }
    private void Damage(GameObject damageTo)
    {
        damageTo.GetComponent<Enemy>().Damage(_damage);
    }

    private void DestroyBullet()
    {
        Destroy(gameObject);
    }
}
