using UnityEngine;

public class Attacker : MergeUnit
{
    [SerializeField] private float _timeToNextShot;
    [SerializeField] private GameObject _bullet;
    [SerializeField] private Transform _shootPos;

    private float _timeToShot;

    protected override void Update()
    {
        base.Update();

        UpdateShotTimer();
        
        if (HaveLastEnemy())
        {
            LookAtEnemy();
            Shoot();
        }
    }

    private void Shoot()
    {
        if (_timeToShot >= _timeToNextShot)
        {
            float rotationY = transform.eulerAngles.y;
            
            Instantiate(_bullet, _shootPos.position, Quaternion.identity);
            _timeToShot = 0;
        }
    }

    private void UpdateShotTimer()
    {
        _timeToShot += Time.fixedDeltaTime;
    }
    
    private void LookAtEnemy()
    {
        GameObject enemy = EnemyToShootController.instance.lastEnemy.gameObject;
        Vector3 targetPosition = new Vector3(enemy.transform.position.x, transform.position.y, enemy.transform.position.z);
                    
        Vector3 lookAtDirection = targetPosition - transform.position;
        Quaternion rotation = Quaternion.LookRotation(lookAtDirection);
            
        rotation.x = 0;
        rotation.z = 0;
                    
        transform.rotation = rotation;
    }

    private bool HaveLastEnemy()
    {
        GameObject enemy = EnemyToShootController.instance.lastEnemy.gameObject;
        return enemy != null;
    }
}
