using System;
using UnityEngine;

public class Enemy : MonoBehaviour, IDamageable
{
    [SerializeField] private int _healthToSet;
    [SerializeField] private float _speed;

    public event Action died;
    
    private HealthSystem _healthSystem;
    public bool isDead { get; private set; }

    private Collider _collider;
    
    private void Start()
    {
        _healthSystem = new HealthSystem(_healthToSet);

        _collider = GetComponent<Collider>();
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
        if (_healthSystem.GetHealth() <= 0 && !isDead)
        {
            isDead = true;
            _collider.enabled = false;
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

    public int GetHealth()
    {
        return _healthSystem.GetHealth();
    }
    public void Damage(int damageAmount)
    {
        _healthSystem.Damage(damageAmount);
    }
    private void OnDisable()
    {
        isDead = false;
        _healthSystem.Heal(_healthToSet);
        _collider.enabled = true;
        died?.Invoke();
    }
}
