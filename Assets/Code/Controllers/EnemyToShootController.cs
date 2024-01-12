using System;
using UnityEngine;

public class EnemyToShootController : MonoBehaviour
{
    public static EnemyToShootController instance { get; private set; }
    
    [field: SerializeField] public GameObject lastEnemy { get; private set; }
    private Enemy _enemy;
    
    private void Awake()
    {
        SetSingleton();
    }

    private void Update()
    {
        LastEnemyChecker();
    }

    private void OnTriggerStay(Collider coll)
    {
        if (coll.gameObject.TryGetComponent(out Enemy _))
        {
            if (!coll.gameObject.GetComponent<Enemy>().isDead)
            {
                lastEnemy = coll.gameObject;
                _enemy = lastEnemy.GetComponent<Enemy>();
            }
        }
    }

    private void LastEnemyChecker()
    {
        if (lastEnemy != null && !lastEnemy.activeSelf && _enemy.GetHealth() <= 0)
            lastEnemy = null;
    }
    
private void SetSingleton()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
            return;
        }
        
        Destroy(gameObject);
    }
}
