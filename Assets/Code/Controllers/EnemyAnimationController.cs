using UnityEngine;

public class EnemyAnimationController : MonoBehaviour
{
    private Animator _animator;
    private Enemy _enemy;
    
    
    private void Start()
    {
        _animator = GetComponent<Animator>();
        _enemy = GetComponent<Enemy>();

        _enemy.died += PlayDeathAnim;
    }

    private void PlayDeathAnim()
    {
        _animator.SetBool("Died", !_animator.GetBool("Died"));
    }
}
