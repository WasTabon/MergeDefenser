public class HealthSystem
{
    private int _health;
    private int _maxHealth;

    public HealthSystem(int healthToSet)
    {
        _health = healthToSet;
        _maxHealth = healthToSet;
    }
    
    public int GetHealth()
    {
        return _health;
    }

    public void Heal(int healNumber)
    {
        _health = healNumber;
        if (_health > _maxHealth)
            _health = _maxHealth;
    }
    
    public void Damage(int damageAmount)
    {
        _health -= damageAmount;
    }
}
