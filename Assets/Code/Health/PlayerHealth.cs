using System;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    [SerializeField] private int _health;
    
    private void OnCollisionEnter(Collision coll)
    {
        if (coll.gameObject.TryGetComponent(out Enemy enemy))
        {
            enemy.Damage(1000);
            _health--;
        }
    }
}
