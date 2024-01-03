using System;
using UnityEngine;

public class Enemy : MonoBehaviour, IDamageable
{
    [SerializeField] private int _healthToSet;
    [SerializeField] private float _speed;

    private float spawnTime;
    
    public event Action died;
    
    private HealthSystem _healthSystem;
    private bool _isDead;

    private void Start()
    {
        _healthSystem = new HealthSystem(_healthToSet);
    }

    private void Update()
    {
        Move();
        Die();
    }

    private void Move()
    {
        transform.Translate(transform.forward * -1 * _speed * Time.deltaTime);
    }

    private void Die()
    {
        if (_healthSystem.GetHealth() <= 0 && !_isDead)
        {
            _isDead = true;
            PlayDeadAnimAndDeactivate();
        }
    }

    private void PlayDeadAnimAndDeactivate()
    {
        died?.Invoke();
        Invoke("SetActiveFalse", 3f);
    }

    private void SetActiveFalse()
    {
        gameObject.SetActive(false);
    }
    
    
    public void Damage(int damageAmount)
    {
        _healthSystem.Damage(damageAmount);
    }
    public float GetLifeTime()
    {
        return Time.time - spawnTime;
    }
    
    private void OnEnable()
    {
        spawnTime = Time.time;
    }
    private void OnDisable()
    {
        _isDead = false;
        _healthSystem.Heal(_healthToSet);
        spawnTime = 0;
        died?.Invoke();
    }
}
