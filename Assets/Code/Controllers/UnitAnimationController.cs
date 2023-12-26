using System;
using UnityEngine;

public class UnitAnimationController : MonoBehaviour
{
    private Animator _animator;
    private MergeUnit _mergeUnit;
    
    private void Start()
    {
        _animator = GetComponent<Animator>();
        _mergeUnit = GetComponent<MergeUnit>();
    }

    private void Update()
    {
        SetDraggingAnim();
    }

    private void SetDraggingAnim()
    {
        _animator.SetBool("Dragging", _mergeUnit.UnitTouched);
    }
}
