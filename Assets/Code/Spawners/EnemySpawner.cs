using System;
using UnityEngine;
using Random = UnityEngine.Random;

public class EnemySpawner : MonoBehaviour
{
    public static EnemySpawner instance { get; private set; }
    
    //public Enemy lastEnemy { get; private set; }
    public Enemy lastEnemy;
    
    [Header("POOL")]
    [SerializeField] private Enemy _prefab;
    [SerializeField] private bool _autoExpand;
    [SerializeField] private int _capacity;
    [SerializeField] private Transform _container;

    [Header("SPAWN SETTINGS")]
    [SerializeField] private float _spawnDelay;

    private PoolMono<Enemy> _pool;

    private void Awake()
    {
        SetSingleton();
    }

    private void Start()
    {
        _pool = new PoolMono<Enemy>(_prefab, _autoExpand, _container);
        _pool.CreatePool(_capacity);
        InvokeRepeating("Spawn", 0.1f, _spawnDelay);
    }

    private void Spawn()
    {
        Enemy enemy = _pool.GetFreeElement();

        if (enemy != null)
        {
            float randomX = Random.Range(-37f, -20f);

            Vector3 spawnPos = new Vector3(randomX, gameObject.transform.position.y, gameObject.transform.position.z);
            enemy.transform.position = spawnPos;
            enemy.gameObject.SetActive(true);

            UpdateLastEnemy(enemy);
        }
    }
    
    private void UpdateLastEnemy(Enemy enemy)
    {
        if (lastEnemy == null || (lastEnemy != null && lastEnemy.GetLifeTime() < enemy.GetLifeTime()))
        {
            lastEnemy = enemy;
        }
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
